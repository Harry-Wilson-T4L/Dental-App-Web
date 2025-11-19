using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Ephemeral;
using DentalDrill.CRM.Models.QueryModels;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Corporate;
using DentalDrill.CRM.Permissions.Validators;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    public partial class CorporateSurgeriesController : Controller
    {
        private readonly IImageUploadService imageUploadService;
        private readonly ApplicationDbContext dbContext;

        private CorporateSurgeryMaintenanceHandpieceFilter surgeryMaintenanceHandpieceFilter;

        public CorporateSurgeriesController(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, ApplicationDbContext dbContext)
        {
            this.ControllerServices = controllerServices;
            this.imageUploadService = imageUploadService;
            this.dbContext = dbContext;

            var userEntityResolver = this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>();
            this.CorporatePermissionsValidator = new CorporatePermissionsValidator(this.ControllerServices.PermissionsHub, userEntityResolver);

            this.IndexHandler = new BasicCrudDetailsActionHandler<String, Corporate, CorporateSurgeryViewModel>(this, controllerServices, this.CorporatePermissionsValidator);
            this.ReportsHandler = new BasicCrudDetailsActionHandler<String, Corporate, CorporateSurgeryReportsViewModel>(this, controllerServices, this.CorporatePermissionsValidator);

            this.MaintenanceHandpiecesReadHandler = new TelerikCrudAjaxDependentReadActionHandler<String, CorporateMaintenanceHandpiece, String, Corporate, CorporateSurgeryMaintenanceHandpieceViewModel>(
                this,
                this.ControllerServices,
                new CorporateDependentPermissionsValidator<CorporateMaintenanceHandpiece>(this.ControllerServices.PermissionsHub, userEntityResolver, x => throw new NotSupportedException()));

            this.MaintenanceHistoryReadHandler = new TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, CorporateSurgeryMaintenanceHandpieceId, CorporateHandpiece, SurgeryMaintenanceHistoryItemViewModel>(
                this,
                this.ControllerServices,
                new CorporateDependent2PermissionsValidator<Handpiece, CorporateHandpiece>(this.ControllerServices.PermissionsHub, userEntityResolver, x => x.CorporateId, x => x.Job.Client.CorporateId.Value));

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

        public IEntityControllerServices ControllerServices { get; }

        public IRepository Repository => this.ControllerServices.Repository;

        protected IEntityPermissionsValidator<Corporate> CorporatePermissionsValidator { get; }

        protected BasicCrudDetailsActionHandler<String, Corporate, CorporateSurgeryViewModel> IndexHandler { get; }

        protected BasicCrudDetailsActionHandler<String, Corporate, CorporateSurgeryReportsViewModel> ReportsHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<String, CorporateMaintenanceHandpiece, String, Corporate, CorporateSurgeryMaintenanceHandpieceViewModel> MaintenanceHandpiecesReadHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, CorporateSurgeryMaintenanceHandpieceId, CorporateHandpiece, SurgeryMaintenanceHistoryItemViewModel> MaintenanceHistoryReadHandler { get; }

        public Task<IActionResult> Index(String id) => this.IndexHandler.Details(id);

        public Task<IActionResult> Reports(String id) => this.ReportsHandler.Details(id);

        public Task<IActionResult> ReadMaintenanceHandpieces(String parentId, [DataSourceRequest] DataSourceRequest request, CorporateSurgeryMaintenanceHandpieceFilter filterModel)
        {
            this.surgeryMaintenanceHandpieceFilter = filterModel;
            return this.MaintenanceHandpiecesReadHandler.Read(parentId, request);
        }

        public Task<IActionResult> ReadMaintenanceHistory(String parentId, String clientId, String brand, String modelName, String serialNo, [DataSourceRequest] DataSourceRequest request) => this.MaintenanceHistoryReadHandler.Read(
            new CorporateSurgeryMaintenanceHandpieceId { CorporateId = parentId, ClientId = clientId, Brand = brand, MakeAndModel = modelName, Serial = serialNo }, request);

        [AjaxPost]
        public async Task<IActionResult> ReadHandpieces(String parentId, [DataSourceRequest] DataSourceRequest request)
        {
            var corporate = await this.QuerySingleEntity(parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate);

            var queryText = new StringBuilder();
            queryText.AppendLine(@"select
  h.[Id],
  j.[ClientId],
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
inner join [Clients] c on j.[ClientId] = c.[Id]
outer apply (
  select top 1
    ui.[Id], ui.[Container], ui.[ContainerPrefix], ui.[Variations]
  from [HandpieceImages] hi
  inner join [UploadedImages] ui on hi.[ImageId] = ui.[Id]
  where hi.[HandpieceId] = h.[Id] and hi.[Display] = 1
  order by hi.[Date] desc
) img
where c.[CorporateId] = @corporateId");
            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("corporateId", SqlDbType.UniqueIdentifier) { Value = corporate.Id });

            var query = this.dbContext.Set<CorporateActiveHandpiece>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return this.Json(await query.ToDataSourceResultAsync(request, x => new SurgeryHandpieceViewModel
            {
                Id = x.Id,
                ClientId = x.ClientId,
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
            var corporate = await this.QuerySingleEntity(parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate);

            var jobNumbers = this.Repository.QueryWithoutTracking<Job>().Where(x => x.Client.CorporateId == corporate.Id).Select(x => new { JobNumber = x.JobNumber.ToString() });
            return this.Json(await jobNumbers.ToDataSourceResultAsync(request));
        }

        [AjaxPost]
        public async Task<IActionResult> ReadSerials(String parentId, [DataSourceRequest] DataSourceRequest request)
        {
            var corporate = await this.QuerySingleEntity(parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate);
            var serials = this.Repository.QueryWithoutTracking<Handpiece>().Where(x => x.Job.Client.CorporateId == corporate.Id).Select(x => new { Serial = x.Serial }).Distinct();
            return this.Json(await serials.ToDataSourceResultAsync(request));
        }

        [AjaxGet]
        public async Task<IActionResult> Handpiece(String parentId, Guid id)
        {
            var corporate = await this.QuerySingleEntity(parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate);

            var handpiece = await this.Repository.QueryWithoutTracking<Handpiece>("Job", "Job.Client", "Components", "ServiceLevel", "ReturnBy", "Images", "Images.Image", "Images.Video")
                .SingleOrDefaultAsync(x => x.Id == id && x.Job.Client.CorporateId == corporate.Id);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            return this.View(handpiece);
        }

        [AjaxGet]
        public async Task<IActionResult> HandpieceImages(String parentId, Guid id, Guid? image)
        {
            var corporate = await this.QuerySingleEntity(parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate);

            var handpiece = await this.Repository.QueryWithoutTracking<Handpiece>("Images", "Images.Image", "Images.Video")
                .SingleOrDefaultAsync(x => x.Id == id && x.Job.Client.CorporateId == corporate.Id);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            this.ViewBag.SelectedImage = image;
            return this.View(handpiece);
        }

        private Task<Corporate> QuerySingleEntity(String id)
        {
            return this.Repository.Query<Corporate>().SingleOrDefaultAsync(x => x.UrlPath == id);
        }

        private async Task<CorporateSurgeryViewModel> ConvertToViewModel(Corporate entity)
        {
            var appearances = await this.Repository.Query<CorporateAppearance>("BackgroundImage", "Logo").SingleOrDefaultAsync(x => x.CorporateId == entity.Id);

            // TODO: Refactor this bit in a separate query, selecting and SQL-grouping each status
            var handpieces = await this.Repository.QueryWithoutTracking<Handpiece>()
                .Where(x => x.Job.Client.CorporateId == entity.Id)
                .OrderBy(x => x.MakeAndModel)
                .ToListAsync();
            var models = handpieces.Select(x => new HandpieceModelInfo(x.Brand, x.MakeAndModel))
                .Distinct()
                .ToList();

            return new CorporateSurgeryViewModel()
            {
                Id = entity.Id,
                Corporate = entity,
                Appearance = appearances ?? CorporateAppearance.Default(),
                Models = models,
                Clients = await this.Repository.Query<Client>().Where(x => x.CorporateId == entity.Id ).ToListAsync(),
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

        private async Task<CorporateSurgeryReportsViewModel> ConvertToReportsViewModel(Corporate entity)
        {
            var appearance = await this.Repository.Query<CorporateAppearance>("BackgroundImage", "Logo").SingleOrDefaultAsync(x => x.CorporateId == entity.Id);
            var clients = await this.Repository.Query<Client>().Where(x => x.CorporateId == entity.Id).OrderBy(x => x.Name).ToListAsync();
            var statesIds = clients.Select(x => x.StateId).Distinct().ToList();
            var states = await this.Repository.Query<State>().Where(x => statesIds.Contains(x.Id)).OrderBy(x => x.Name).ToListAsync();

            return new CorporateSurgeryReportsViewModel
            {
                Corporate = entity,
                Appearance = appearance ?? CorporateAppearance.Default(),
                DateRanges = this.CreateReportDateRanges(),
                DefaultDateRange = ReportDateRange.YearUpTo(DateTime.UtcNow.Date, "Year"),
                Clients = clients,
                States = states,
            };
        }

        private Task<IQueryable<CorporateMaintenanceHandpiece>> PrepareReadMaintenanceHandpieces(Corporate parent)
        {
            var queryText = new StringBuilder();
            var queryParameters = new List<Object>();
            var queryFilter = this.surgeryMaintenanceHandpieceFilter?.GenerateWhereCondition("jh", "jh", true) ?? (String.Empty, new Object[0]);

            queryText.AppendLine($@"select 
  q.[ClientId],
  clt.[UrlPath] as [ClientUrlPath],
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
    select jh.[CorporateId], jh.[ClientId], jh.[JobNumber], jh.[Received], jh.[Brand], jh.[MakeAndModel], jh.[Serial], jh.[SpeedType] from
    (select
      c.[CorporateId], j.[ClientId], j.[JobNumber], j.[Received], h.[Brand], h.[MakeAndModel], h.[Serial], h.[SpeedType], h.[HandpieceStatus], row_number() over (partition by j.[ClientId], h.[Brand], h.[MakeAndModel], h.[Serial] order by j.[Received] desc) as [Rank]
    from [Handpieces] h inner join [Jobs] j on h.[JobId] = j.[Id] inner join [Clients] c on j.[ClientId] = c.[Id]) jh
    where jh.[Rank] = 1 {queryFilter.QueryText}
) q
inner join [Clients] clt on q.[ClientId] = clt.[Id]
outer apply (
    select top 1 hi.[ImageId]
	from [HandpieceImages] hi
	  inner join [Handpieces] hih on hi.[HandpieceId] = hih.[Id]
	  inner join [Jobs] hij on hih.[JobId] = hij.[Id]
	where hih.[Brand] = q.[Brand]
	  and hih.[MakeAndModel] = q.[MakeAndModel]
	  and hih.[Serial] = q.[Serial]
	  and q.[ClientId] = hij.[ClientId]
	  and hi.[Display] = 1
      and hi.[ImageId] is not null
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
where q.[CorporateId] = @corporateId");

            queryParameters.AddRange(queryFilter.QueryParameters);
            queryParameters.Add(new SqlParameter("corporateId", SqlDbType.UniqueIdentifier) { Value = parent.Id });

            var query = this.dbContext.Set<CorporateMaintenanceHandpiece>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            query = query.OrderBy(x => x.Brand).ThenBy(x => x.MakeAndModel).ThenBy(x => x.Serial);
            return Task.FromResult(query);
        }

        private CorporateSurgeryMaintenanceHandpieceViewModel ConvertMaintenanceHandpieceToViewModel(Corporate parent, CorporateMaintenanceHandpiece entity, String[] allowedProperties)
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

            return new CorporateSurgeryMaintenanceHandpieceViewModel
            {
                Id = $"{entity.MakeAndModel}/{entity.Serial}",
                ClientId = entity.ClientUrlPath,
                Brand = entity.Brand,
                MakeAndModel = entity.MakeAndModel,
                Serial = entity.Serial,
                ImageUrl = this.imageUploadService.GetImageUrl(mostRecentImage, "300"),
                OverYearAgo = entity.LatestJobReceived < DateTime.UtcNow.AddYears(-1),
            };
        }

        private async Task<CorporateHandpiece> QuerySingleMaintenanceHistoryParent(CorporateSurgeryMaintenanceHandpieceId id)
        {
            var corporate = await this.Repository.Query<Corporate>().SingleOrDefaultAsync(x => x.UrlPath == id.CorporateId);
            if (corporate == null)
            {
                return null;
            }

            var client = await this.Repository.Query<Client>().SingleOrDefaultAsync(x => x.CorporateId == corporate.Id && x.UrlPath == id.ClientId);
            if (client == null)
            {
                return null;
            }

            return new CorporateHandpiece
            {
                Id = $"{id.Brand}/{id.MakeAndModel}/{id.Serial}",
                CorporateId = corporate.Id,
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

        private Task<IQueryable<Handpiece>> PrepareMaintenanceHistoryRead(CorporateHandpiece parent)
        {
            var query = this.Repository.Query<Handpiece>()
                .Include(x => x.Job).ThenInclude(x => x.Client)
                .Include(x => x.Job).ThenInclude(x => x.JobType)
                .Include(x => x.ServiceLevel)
                .Include(x => x.Images).ThenInclude(x => x.Image)
                .Where(x => x.Job.Client.CorporateId == parent.CorporateId && x.Job.ClientId == parent.ClientId && x.Brand == parent.Brand && x.MakeAndModel == parent.MakeAndModel && x.Serial == parent.Serial);

            return Task.FromResult(query);
        }

        private SurgeryMaintenanceHistoryItemViewModel ConvertMaintenanceHistoryToViewModel(CorporateHandpiece parent, Handpiece entity, String[] allowedProperties)
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

        private Task<IQueryable<CorporateSurgeryReportSurgeryItem>> PrepareCorporateReport(Corporate corporate, DateTime from, DateTime to, Guid[] clients)
        {
            var queryText = new StringBuilder();
            queryText.Append($@"select
  [Jobs].[ClientId] as [ClientId],
  [Clients].[Name] as [ClientName],
  [Handpieces].[Brand] as [Brand],
  [Handpieces].[MakeAndModel] as [Model],
  datepart(year, [Jobs].[Received]) as [Year],
  datepart(quarter, [Jobs].[Received]) as [Quarter],
  datepart(month, [Jobs].[Received]) as [Month],
  datepart(week, [Jobs].[Received]) as [Week],
  [Jobs].[Received] as [Date],
  COALESCE([Handpieces].[CostOfRepair], 0.00) as [Cost],
  CAST([Handpieces].[Rating] as decimal(18,2)) as [Rating],
  IIF ([Handpieces].[HandpieceStatus] = 71, 1, 0) as [Unrepaired],
  1 as [Handpieces]
from [Handpieces]
  inner join [Jobs] on [Handpieces].[JobId] = [Jobs].[Id]
  inner join [Clients] on [Jobs].[ClientId] = [Clients].[Id]
where [Clients].[CorporateId] = @corporateId
  and [Jobs].[Received] between @from and @to
  and [Jobs].[ClientId] in ({String.Join(", ", clients.Select((x, i) => $"@client{i}"))})");

            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("corporateId", SqlDbType.UniqueIdentifier) { Value = corporate.Id });
            queryParameters.Add(new SqlParameter("from", SqlDbType.Date) { Value = from });
            queryParameters.Add(new SqlParameter("to", SqlDbType.Date) { Value = to });
            for (var i = 0; i < clients.Length; i++)
            {
                queryParameters.Add(new SqlParameter($"client{i}", SqlDbType.UniqueIdentifier) { Value = clients[i] });
            }

            var query = this.dbContext.Set<CorporateSurgeryReportSurgeryItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());

            return Task.FromResult(query);
        }
    }
}
