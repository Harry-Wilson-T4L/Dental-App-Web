using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.QueryModels;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Permissions.Validators;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClientHandpiece = DentalDrill.CRM.Models.Ephemeral.ClientHandpiece;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/Client")]
    [PermissionsManager("Entity", "/Domain/Client/Entities/{entity:Client}")]
    public partial class SurgeriesController : Controller
    {
        private readonly IImageUploadService imageUploadService;
        private readonly ApplicationDbContext dbContext;

        private SurgeryMaintenanceHandpieceFilter surgeryMaintenanceHandpieceFilter;

        public SurgeriesController(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, ApplicationDbContext dbContext)
        {
            this.ControllerServices = controllerServices;
            this.Repository = this.ControllerServices.Repository;
            this.imageUploadService = imageUploadService;
            this.dbContext = dbContext;

            var userEntityResolver = this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>();
            this.ClientPermissionsValidator = new ClientPermissionsValidator(this.ControllerServices.PermissionsHub, userEntityResolver);

            this.IndexHandler = new BasicCrudDetailsActionHandler<String, Client, SurgeryViewModel>(this, controllerServices, this.ClientPermissionsValidator);
            this.ReportsHandler = new BasicCrudDetailsActionHandler<String, Client, SurgeryReportsViewModel>(this, controllerServices, this.ClientPermissionsValidator);

            this.MaintenanceHandpiecesReadHandler = new TelerikCrudAjaxDependentReadActionHandler<String, ClientMaintenanceHandpiece, String, Client, SurgeryMaintenanceHandpieceViewModel>(
                this,
                this.ControllerServices,
                new ClientDependentPermissionsValidator<ClientMaintenanceHandpiece>(this.ControllerServices.PermissionsHub, userEntityResolver, x => throw new NotSupportedException()));

            this.MaintenanceHistoryReadHandler = new TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, SurgeryMaintenanceHandpieceId, ClientHandpiece, SurgeryMaintenanceHistoryItemViewModel>(
                this,
                this.ControllerServices,
                new ClientDependent2PermissionsValidator<Handpiece, ClientHandpiece>(this.ControllerServices.PermissionsHub, userEntityResolver, x => x.ClientId, x => x.Job.ClientId));

            this.IndexHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.IndexHandler.Overrides.ConvertToDetailsModel = this.ConvertToViewModel;

            this.ReportsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ReportsHandler.Overrides.ConvertToDetailsModel = this.ConvertToReportsViewModel;

            this.MaintenanceHandpiecesReadHandler.Overrides.QuerySingleParentEntity = this.QuerySingleEntity;
            this.MaintenanceHandpiecesReadHandler.Overrides.PrepareReadQuery = this.PrepareReadMaintenanceHandpieces;
            this.MaintenanceHandpiecesReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertMaintenanceHandpieceToViewModel;

            this.MaintenanceHistoryReadHandler.Overrides.QuerySingleParentEntity = this.QuerySingleMaintenanceHistoryParent;
            this.MaintenanceHistoryReadHandler.Overrides.PreprocessRequest = this.PreprocessMaintenanceHistoryRequest;
            this.MaintenanceHistoryReadHandler.Overrides.PrepareReadQuery = this.PrepareMaintenanceHistoryRead;
            this.MaintenanceHistoryReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertMaintenanceHistoryToViewModel;
        }

        protected IEntityControllerServices ControllerServices { get; }

        protected IRepository Repository { get; }

        protected IEntityPermissionsValidator<Client> ClientPermissionsValidator { get; }

        protected BasicCrudDetailsActionHandler<String, Client, SurgeryViewModel> IndexHandler { get; }

        protected BasicCrudDetailsActionHandler<String, Client, SurgeryReportsViewModel> ReportsHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<String, ClientMaintenanceHandpiece, String, Client, SurgeryMaintenanceHandpieceViewModel> MaintenanceHandpiecesReadHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, SurgeryMaintenanceHandpieceId, ClientHandpiece, SurgeryMaintenanceHistoryItemViewModel> MaintenanceHistoryReadHandler { get; }

        public Task<IActionResult> Index(String id) => this.IndexHandler.Details(id);

        public Task<IActionResult> Reports(String id) => this.ReportsHandler.Details(id);

        public Task<IActionResult> ReadMaintenanceHandpieces(String parentId, [DataSourceRequest] DataSourceRequest request, SurgeryMaintenanceHandpieceFilter filterModel)
        {
            this.surgeryMaintenanceHandpieceFilter = filterModel;
            return this.MaintenanceHandpiecesReadHandler.Read(parentId, request);
        }

        public Task<IActionResult> ReadMaintenanceHistory(String parentId, String brand, String modelName, String serialNo, [DataSourceRequest] DataSourceRequest request) => this.MaintenanceHistoryReadHandler.Read(
            new SurgeryMaintenanceHandpieceId { ClientId = parentId, Brand = brand, MakeAndModel = modelName, Serial = serialNo }, request);

        [AjaxPost]
        public async Task<IActionResult> ReadHandpieces(String parentId, [DataSourceRequest] DataSourceRequest request)
        {
            var client = await this.QuerySingleEntity(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(client);

            var queryText = new StringBuilder();
            queryText.AppendLine(@"select
  h.[Id],
  cast(j.[JobNumber] as nvarchar(20)) as [JobNumber],
  h.[Brand],
  h.[MakeAndModel],
  h.[Serial],
  h.[HandpieceStatus] as [Status],
  h.[Rating],
  h.[SpeedType],
  j.[Received],
  img.[Id] as [LatestImageId],
  img.[Container] as [LatestImageContainer],
  img.[ContainerPrefix] as [LatestImageContainerPrefix],
  img.[Variations] as [LatestImageVariations]
from [Handpieces] h
inner join [Jobs] j on h.[JobId] = j.[Id]
outer apply (
  select top 1
    ui.[Id], ui.[Container], ui.[ContainerPrefix], ui.[Variations]
  from [HandpieceImages] hi
  inner join [UploadedImages] ui on hi.[ImageId] = ui.[Id]
  where hi.[HandpieceId] = h.[Id] and hi.[Display] = 1
  order by hi.[Date] desc
) img
where j.[ClientId] = @clientId");
            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("clientId", SqlDbType.UniqueIdentifier) { Value = client.Id });

            var query = this.dbContext.Set<ClientActiveHandpiece>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return this.Json(await query.ToDataSourceResultAsync(request, x => new SurgeryHandpieceViewModel
            {
                Id = x.Id,
                JobNumber = x.JobNumber,
                Brand = x.Brand,
                MakeAndModel = x.MakeAndModel,
                Serial = x.Serial,
                Status = x.Status.ToExternal(),
                Rating = x.Rating,
                SpeedType = x.SpeedType,
                Received = x.Received,
                ImageUrl = this.imageUploadService.GetImageUrl(
                    x.LatestImageId.HasValue
                        ? new UploadedImage
                        {
                            Id = x.LatestImageId.Value,
                            Container = x.LatestImageContainer,
                            ContainerPrefix = x.LatestImageContainerPrefix,
                            VariationsContent = x.LatestImageVariations,
                        }
                        : null, "600"),
            }));
        }

        [AjaxPost]
        public async Task<IActionResult> ReadJobNumbers(String parentId, [DataSourceRequest] DataSourceRequest request)
        {
            var client = await this.QuerySingleEntity(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(client);

            var jobNumbers = this.Repository.QueryWithoutTracking<Job>().Where(x => x.ClientId == client.Id).Select(x => new { JobNumber = x.JobNumber.ToString() });
            return this.Json(await jobNumbers.ToDataSourceResultAsync(request));
        }

        [AjaxPost]
        public async Task<IActionResult> ReadSerials(String parentId, [DataSourceRequest] DataSourceRequest request)
        {
            var client = await this.QuerySingleEntity(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(client);
            var serials = this.Repository.QueryWithoutTracking<Handpiece>().Where(x => x.Job.Client.Id == client.Id).Select(x => new { Serial = x.Serial }).Distinct();
            return this.Json(await serials.ToDataSourceResultAsync(request));
        }

        [AjaxGet]
        public async Task<IActionResult> Handpiece(String parentId, Guid id)
        {
            var client = await this.QuerySingleEntity(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(client);

            var handpiece = await this.Repository.QueryWithoutTracking<Handpiece>("Job", "Components", "ServiceLevel", "ReturnBy", "Images", "Images.Image", "Images.Video")
                .SingleOrDefaultAsync(x => x.Id == id && x.Job.ClientId == client.Id);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            return this.View(handpiece);
        }

        public async Task<IActionResult> HandpieceImages(String parentId, Guid id, Guid? image)
        {
            var client = await this.QuerySingleEntity(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(client);

            var handpiece = await this.Repository.QueryWithoutTracking<Handpiece>("Images", "Images.Image", "Images.Video")
                .SingleOrDefaultAsync(x => x.Id == id && x.Job.ClientId == client.Id);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            this.ViewBag.SelectedImage = image;
            return this.View(handpiece);
        }

        private Task<Client> QuerySingleEntity(String id)
        {
            return this.Repository.Query<Client>().SingleOrDefaultAsync(x => x.UrlPath == id);
        }

        private async Task<SurgeryViewModel> ConvertToViewModel(Client entity)
        {
            var appearances = await this.Repository.Query<ClientAppearance>("BackgroundImage", "Logo").SingleOrDefaultAsync(x => x.ClientId == entity.Id);

            // TODO: Refactor this bit in a separate query, selecting and SQL-grouping each status
            var handpieces = await this.Repository.QueryWithoutTracking<Handpiece>()
                .Where(x => x.Job.ClientId == entity.Id)
                .OrderBy(x => x.MakeAndModel)
                .ToListAsync();

            var models = handpieces.Select(x => new HandpieceModelInfo(x.Brand, x.MakeAndModel)).Distinct().ToList();

            return new SurgeryViewModel()
            {
                Id = entity.Id,
                Client = entity,
                Appearance = appearances ?? ClientAppearance.Default(),
                Models = models,
                ReceivedItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.Received),
                BeingEstimatedItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.BeingEstimated),
                WaitingForApprovalItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.WaitingForApproval ||
                                                                x.HandpieceStatus == HandpieceStatus.TbcHoldOn),
                EstimateSentItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.EstimateSent),
                BeingRepairedItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.BeingRepaired ||
                                                           x.HandpieceStatus == HandpieceStatus.WaitingForParts ||
                                                           x.HandpieceStatus == HandpieceStatus.NeedsReApproval),
                ReadyForReturnItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.ReadyToReturn),
                UnrepairedItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.Unrepairable ||
                                                        x.HandpieceStatus == HandpieceStatus.Cancelled ||
                                                        x.HandpieceStatus == HandpieceStatus.ReturnUnrepaired ||
                                                        x.HandpieceStatus == HandpieceStatus.TradeIn),
                SentCompleteItems = handpieces.Count(x => x.HandpieceStatus == HandpieceStatus.SentComplete),
            };
        }

        private async Task<SurgeryReportsViewModel> ConvertToReportsViewModel(Client entity)
        {
            var appearance = await this.Repository.Query<ClientAppearance>("BackgroundImage", "Logo").SingleOrDefaultAsync(x => x.ClientId == entity.Id);

            return new SurgeryReportsViewModel
            {
                Surgery = entity,
                Appearance = appearance ?? ClientAppearance.Default(),
                DateRanges = this.CreateReportDateRanges(),
                DefaultDateRange = ReportDateRange.YearUpTo(DateTime.UtcNow.Date, "Year"),
            };
        }

        private List<List<ReportDateRange>> CreateReportDateRanges()
        {
            var today = DateTime.UtcNow.Date;
            return new List<List<ReportDateRange>>
            {
                new List<ReportDateRange>
                {
                    ReportDateRange.SingleDay(today, "Today"),
                    ReportDateRange.SingleWeekUpTo(today, "Week"),
                },
                new List<ReportDateRange>
                {
                    ReportDateRange.SpecificNumberOfPreviousDays(today, 30, "30 days"),
                    ReportDateRange.SingleMonthUpTo(today, today.ToString("MMMM")),
                    ReportDateRange.SingleMonth(today.Year, today.Month - 1, today.AddMonths(-1).ToString("MMMM")),
                    ReportDateRange.SingleMonth(today.Year, today.Month - 2, today.AddMonths(-2).ToString("MMMM")),
                },
                new List<ReportDateRange>
                {
                    ReportDateRange.QuarterUpTo(today, "Quarter"),
                    ReportDateRange.YearUpTo(today, "Year"),
                },
            };
        }

        private Task<IQueryable<ClientMaintenanceHandpiece>> PrepareReadMaintenanceHandpieces(Client parent)
        {
            var queryText = new StringBuilder();
            var queryParameters = new List<Object>();
            var queryFilter = this.surgeryMaintenanceHandpieceFilter?.GenerateWhereCondition("jh", "jh", true) ?? (String.Empty, new Object[0]);
            queryText.AppendLine($@"select 
  q.[Brand],
  q.[MakeAndModel],
  q.[Serial],
  latestJob.[Received] as [LatestJobReceived],
  latestImageRef.[ImageId] as [LatestImageId],
  latestImage.[Container] as [LatestImageContainer],
  latestImage.[ContainerPrefix] as [LatestImageContainerPrefix],
  latestImage.[Variations] as [LatestImageVariations]
from
(
    select jh.[ClientId], jh.[JobNumber], jh.[Received], jh.[Brand], jh.[MakeAndModel], jh.[Serial], jh.[SpeedType] from
    (select
      j.[ClientId], j.[JobNumber], j.[Received], h.[Brand], h.[MakeAndModel], h.[Serial], h.[SpeedType], h.[HandpieceStatus], row_number() over (partition by j.[ClientId], h.[Brand], h.[MakeAndModel], h.[Serial] order by j.[Received] desc) as [Rank]
    from [Handpieces] h inner join [Jobs] j on h.[JobId] = j.[Id]) jh
    where jh.[Rank] = 1 {queryFilter.QueryText}
) q
outer apply (
    select top 1 hi.[ImageId]
	from [HandpieceImages] hi
	  inner join [Handpieces] hih on hi.[HandpieceId] = hih.[Id]
	  inner join [Jobs] hij on hih.[JobId] = hij.[Id]
	where hih.[Brand] = q.[Brand]
	  and hih.[MakeAndModel] = q.[MakeAndModel]
	  and hih.[Serial] = q.[Serial]
	  and q.[ClientId] = hij.[ClientId]
      and hi.[ImageId] is not null
	  and hi.[Display] = 1
	order by hij.[Received] desc, hi.[Date]
) latestImageRef
left join [UploadedImages] latestImage on latestImageRef.[ImageId] = latestImage.[Id]
outer apply (
    select top 1 hjj.[Received]
	from [Handpieces] hjh
	  inner join [Jobs] hjj on hjh.[JobId] = hjj.[Id]
	where hjh.[Brand] = q.[Brand]
	  and hjh.[MakeAndModel] = q.[MakeAndModel]
	  and hjh.[Serial] = q.[Serial]
	  and q.[ClientId] = hjj.[ClientId]
	order by hjj.[Received] desc
) latestJob
where q.[ClientId] = @clientId");

            queryParameters.AddRange(queryFilter.QueryParameters);
            queryParameters.Add(new SqlParameter("clientId", SqlDbType.UniqueIdentifier) { Value = parent.Id });

            var query = this.dbContext.Set<ClientMaintenanceHandpiece>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            query = query.OrderBy(x => x.Brand).ThenBy(x => x.MakeAndModel).ThenBy(x => x.Serial);
            return Task.FromResult(query);
        }

        private SurgeryMaintenanceHandpieceViewModel ConvertMaintenanceHandpieceToViewModel(Client parent, ClientMaintenanceHandpiece entity, String[] allowedProperties)
        {
            var mostRecentImage = entity.LatestImageId.HasValue
                ? new UploadedImage
                {
                    Id = entity.LatestImageId.Value,
                    Container = entity.LatestImageContainer,
                    ContainerPrefix = entity.LatestImageContainerPrefix,
                    VariationsContent = entity.LatestImageVariations,
                }
                : null;

            return new SurgeryMaintenanceHandpieceViewModel
            {
                Id = $"{entity.MakeAndModel}/{entity.Serial}",
                Brand = entity.Brand,
                MakeAndModel = entity.MakeAndModel,
                Serial = entity.Serial,
                ImageUrl = this.imageUploadService.GetImageUrl(mostRecentImage, "300"),
                OverYearAgo = entity.LatestJobReceived < DateTime.UtcNow.AddYears(-1),
            };
        }

        private async Task<ClientHandpiece> QuerySingleMaintenanceHistoryParent(SurgeryMaintenanceHandpieceId id)
        {
            var client = await this.Repository.Query<Client>().SingleOrDefaultAsync(x => x.UrlPath == id.ClientId);
            if (client == null)
            {
                return null;
            }

            return new ClientHandpiece
            {
                Id = $"{id.Brand}/{id.MakeAndModel}/{id.Serial}",
                ClientId = client.Id,
                Brand = id.Brand,
                MakeAndModel = id.MakeAndModel,
                Serial = id.Serial,
            };
        }

        private Task PreprocessMaintenanceHistoryRequest(DataSourceRequest request)
        {
            request.RenameFields(new Dictionary<String, String>
            {
                ["JobNumber"] = "Job.Number",
                ["JobReceived"] = "Job.Received",
            });

            return Task.CompletedTask;
        }

        private Task<IQueryable<Handpiece>> PrepareMaintenanceHistoryRead(ClientHandpiece parent)
        {
            var query = this.Repository.Query<Handpiece>()
                .Include(x => x.Job).ThenInclude(x => x.JobType)
                .Include(x => x.ServiceLevel)
                .Include(x => x.Images).ThenInclude(x => x.Image)
                .Where(x => x.Job.ClientId == parent.ClientId && x.Brand == parent.Brand && x.MakeAndModel == parent.MakeAndModel && x.Serial == parent.Serial);

            return Task.FromResult(query);
        }

        private SurgeryMaintenanceHistoryItemViewModel ConvertMaintenanceHistoryToViewModel(ClientHandpiece parent, Handpiece entity, String[] allowedProperties)
        {
            return new SurgeryMaintenanceHistoryItemViewModel
            {
                Id = entity.Id,
                ImageUrl = this.imageUploadService.GetImageUrl(entity.Images.OrderBy(x => x.Date).FirstOrDefault(x => x.Display)?.Image, "300"),
                JobType = entity.Job.JobType.Name,
                JobNumber = entity.Job.JobNumber,
                JobReceived = entity.Job.Received,
                CompletedOn = entity.CompletedOn,
                DiagnosticReport = entity.DiagnosticReport,
                ServiceLevel = entity.ServiceLevel?.Name,
                Rating = entity.Rating,
                CostOfRepair = entity.CostOfRepair ?? 0m,
                PublicComment = entity.PublicComment,
            };
        }
    }
}