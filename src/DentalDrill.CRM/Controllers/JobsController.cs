using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Data;
using DentalDrill.CRM.Services.Generation;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.EntitySequences;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Int32 = System.Int32;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class JobsController : BaseTelerikIndexBasicCrudController<Guid, Job, JobsIndexViewModel, JobReadModel, JobDetailsModel, JobCreateModel, JobEditModel, Job>
    {
        private readonly IEntitySequenceService sequenceService;
        private readonly IDataTransactionService transactionService;
        private readonly RepairWorkflowService repairWorkflowService;
        private readonly UserEntityResolver userEntityResolver;
        private readonly ApplicationDbContext dbContext;
        private readonly IChangeTrackingService<Job, JobChange> jobChangeTrackingService;
        private readonly IChangeTrackingService<Handpiece, HandpieceChange> handpieceChangeTrackingService;
        private readonly EntitiesGenerationService entitiesGenerationService;

        public JobsController(
            IEntityControllerServices controllerServices,
            IEntitySequenceService sequenceService,
            IDataTransactionService transactionService,
            RepairWorkflowService repairWorkflowService,
            UserEntityResolver userEntityResolver,
            ApplicationDbContext dbContext,
            IChangeTrackingService<Job, JobChange> jobChangeTrackingService,
            IChangeTrackingService<Handpiece, HandpieceChange> handpieceChangeTrackingService, EntitiesGenerationService entitiesGenerationService)
            : base(controllerServices)
        {
            this.sequenceService = sequenceService;
            this.transactionService = transactionService;
            this.repairWorkflowService = repairWorkflowService;
            this.userEntityResolver = userEntityResolver;
            this.dbContext = dbContext;
            this.jobChangeTrackingService = jobChangeTrackingService;
            this.handpieceChangeTrackingService = handpieceChangeTrackingService;
            this.entitiesGenerationService = entitiesGenerationService;

            var permissionsValidator = new DefaultEntityPermissionsValidator<Job>(this.ControllerServices.PermissionsHub, null, null, null);
            var handpiecesPermissionsValidator = new DefaultEntityPermissionsValidator<Handpiece>(this.ControllerServices.PermissionsHub, null, null, null);
            var typeJobsPermissionsValidator = new DefaultDependentEntityPermissionsValidator<Job, JobType>(this.ControllerServices.PermissionsHub, null, null, null, null);
            var clientJobsPermissionsValidator = new DefaultDependentEntityPermissionsValidator<Job, Client>(this.ControllerServices.PermissionsHub, null, null, null, null);

            this.CreateJobHandler = new BasicCrudDependentCreateActionHandler<Guid, Job, String, JobType, JobCreateModel>(this, this.ControllerServices, typeJobsPermissionsValidator);
            this.ReadClientJobsHandler = new TelerikCrudAjaxDependentReadActionHandler<Guid, Job, Guid, Client, JobReadModel>(this, this.ControllerServices, clientJobsPermissionsValidator);
            this.IndexItemTabHandpiecesHandler = new BasicCrudDetailsActionHandler<Guid, Job, Job>(this, this.ControllerServices, permissionsValidator);
            this.IndexItemTabInvoicesHandler = new BasicCrudDetailsActionHandler<Guid, Job, Job>(this, this.ControllerServices, permissionsValidator);
            this.DetailsTabHandpiecesHandler = new BasicCrudDetailsActionHandler<Guid, Job, JobDetailsModel>(this, this.ControllerServices, permissionsValidator);
            this.DetailsTabInvoicesHandler = new BasicCrudDetailsActionHandler<Guid, Job, JobDetailsModel>(this, this.ControllerServices, permissionsValidator);
            this.ReadJobsHandler = new TelerikCrudAjaxReadIntermediateActionHandler<Guid, Job, JobReadModel, JobReadModel>(this, this.ControllerServices, permissionsValidator);
            this.ReadHandpiecesHandler = new TelerikCrudAjaxReadIntermediateActionHandler<Guid, Handpiece, JobHandpieceFullReadModel, JobHandpieceFullReadModel>(this, this.ControllerServices, handpiecesPermissionsValidator);
            this.UpdateHandpiecesHandler = new BasicCrudCustomOperationActionHandler<Guid, Job, JobStatusChangeModel>(this, this.ControllerServices, permissionsValidator);
            this.SetStatusHandler = new BasicCrudCustomOperationActionHandler<Guid, Job, JobSetStatusModel>(this, this.ControllerServices, permissionsValidator);
            this.ChangeClientHandler = new BasicCrudCustomOperationActionHandler<Guid, Job, JobChangeClientModel>(this, this.ControllerServices, permissionsValidator);
            this.ChangeWorkshopHandler = new BasicCrudCustomOperationActionHandler<Guid, Job, JobChangeWorkshopModel>(this, this.ControllerServices, permissionsValidator);

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadJobsHandler.Overrides.PreprocessRequest = this.PreprocessReadJobsRequest;
            this.ReadJobsHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadJobsHandler.Overrides.FinalizeQueryPreparation = this.FinalizeReadJobsQueryPreparation;
            this.ReadJobsHandler.Overrides.ConvertEntityToViewModel = this.ConvertJobEntityToViewModel;

            this.ReadClientJobsHandler.Overrides.PrepareReadQuery = this.PrepareReadClientJobsQuery;
            this.ReadClientJobsHandler.Overrides.ConvertEntityToViewModel = (client, job, properties) => this.ConvertEntityToViewModel(job, properties);

            this.ReadHandpiecesHandler.Overrides.PreprocessRequest = this.PreprocessReadHandpiecesRequest;
            this.ReadHandpiecesHandler.Overrides.PrepareReadQuery = this.PrepareReadHandpiecesQuery;
            this.ReadHandpiecesHandler.Overrides.FinalizeQueryPreparation = this.FinalizeReadHandpiecesQueryPreparation;
            this.ReadHandpiecesHandler.Overrides.ConvertEntityToViewModel = this.ConvertHandpieceEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModelAndOpen;

            this.DetailsTabHandpiecesHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsTabHandpiecesHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabInvoicesHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsTabInvoicesHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateJobHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateJobHandler.Overrides.ValidateCreateModel = this.ValidateCreateModel;
            this.CreateJobHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateJobHandler.Overrides.AfterEntityCreated = this.AfterEntityCreated;
            this.CreateJobHandler.Overrides.GetCreateSuccessResult = this.CreateSuccessResult;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.BeforeEntityUpdated = this.BeforeEntityUpdated;
            this.EditHandler.Overrides.AfterEntityUpdated = this.AfterEntityUpdated;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.AfterEntityDeleted = this.AfterEntityDeleted;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;

            this.UpdateHandpiecesHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.UpdateHandpiecesHandler.Overrides.ExecuteOperation = this.ExecuteUpdateHandpiecesOperation;
            this.UpdateHandpiecesHandler.Overrides.GetOperationSuccessResult = this.GetUpdateHandpiecesOperationSuccessResult;

            this.SetStatusHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.SetStatusHandler.Overrides.ExecuteOperation = this.ExecuteSetStatusOperation;
            this.SetStatusHandler.Overrides.GetOperationSuccessResult = this.GetSetStatusOperationSuccessResult;

            this.ChangeClientHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ChangeClientHandler.Overrides.InitializeOperationModel = this.InitializeChangeClientModel;
            this.ChangeClientHandler.Overrides.ExecuteOperation = this.ExecuteChangeClient;
            this.ChangeClientHandler.Overrides.GetOperationSuccessResult = this.GetChangeClientSuccessResult;

            this.ChangeWorkshopHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ChangeWorkshopHandler.Overrides.InitializeOperationModel = this.InitializeChangeWorkshopModel;
            this.ChangeWorkshopHandler.Overrides.ExecuteOperation = this.ExecuteChangeWorkshop;
            this.ChangeWorkshopHandler.Overrides.GetOperationSuccessResult = this.GetChangeWorkshopSuccessResult;
        }

        protected BasicCrudDependentCreateActionHandler<Guid, Job, String, JobType, JobCreateModel> CreateJobHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Job, Job> IndexItemTabHandpiecesHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Job, Job> IndexItemTabInvoicesHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Job, JobDetailsModel> DetailsTabHandpiecesHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Job, JobDetailsModel> DetailsTabInvoicesHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<Guid, Job, Guid, Client, JobReadModel> ReadClientJobsHandler { get; }

        protected TelerikCrudAjaxReadIntermediateActionHandler<Guid, Job, JobReadModel, JobReadModel> ReadJobsHandler { get; }

        protected TelerikCrudAjaxReadIntermediateActionHandler<Guid, Handpiece, JobHandpieceFullReadModel, JobHandpieceFullReadModel> ReadHandpiecesHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Job, JobStatusChangeModel> UpdateHandpiecesHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Job, JobSetStatusModel> SetStatusHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Job, JobChangeClientModel> ChangeClientHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Job, JobChangeWorkshopModel> ChangeWorkshopHandler { get; }

        [NonAction]
        public override Task<IActionResult> Create()
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Create(JobCreateModel model)
        {
            throw new NotSupportedException();
        }

        public Task<IActionResult> Create(String type)
        {
            return this.CreateJobHandler.Create(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Create(String type, JobCreateModel model)
        {
            return this.CreateJobHandler.Create(type, model);
        }

        [AjaxPost]
        public Task<IActionResult> ReadClientJobs(Guid parentId, [DataSourceRequest] DataSourceRequest request) => this.ReadClientJobsHandler.Read(parentId, request);

        public Task<IActionResult> IndexItemTabHandpieces(Guid id) => this.IndexItemTabHandpiecesHandler.Details(id);

        public Task<IActionResult> IndexItemTabInvoices(Guid id) => this.IndexItemTabInvoicesHandler.Details(id);

        public Task<IActionResult> DetailsTabHandpieces(Guid id) => this.DetailsTabHandpiecesHandler.Details(id);

        public Task<IActionResult> DetailsTabInvoices(Guid id) => this.DetailsTabInvoicesHandler.Details(id);

        [AjaxPost]
        public Task<IActionResult> ReadJobs([DataSourceRequest] DataSourceRequest request) => this.ReadJobsHandler.Read(request);

        [AjaxPost]
        public Task<IActionResult> ReadHandpieces([DataSourceRequest] DataSourceRequest request) => this.ReadHandpiecesHandler.Read(request);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> UpdateHandpieces(Guid id, JobStatusChangeModel model) => this.UpdateHandpiecesHandler.Execute(id, model);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SetStatus(Guid id, JobStatus status) => this.SetStatusHandler.Execute(id, new JobSetStatusModel { Status = status });

        public Task<IActionResult> ChangeClient(Guid id) => this.ChangeClientHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ChangeClient(Guid id, JobChangeClientModel model) => this.ChangeClientHandler.Execute(id, model);

        public Task<IActionResult> ChangeWorkshop(Guid id) => this.ChangeWorkshopHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ChangeWorkshop(Guid id, JobChangeWorkshopModel model) => this.ChangeWorkshopHandler.Execute(id, model);

        [AjaxPost]
        public async Task<IActionResult> ClientsAutoComplete([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.Query<Client>().Select(x => new { Id = x.Id, FullName = x.FullName });
            var result = await query.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ClientsResolve(Guid[] ids)
        {
            var items = await this.Repository.Query<Client>()
                .Where(x => ids.Contains(x.Id))
                .Select(x => new { Id = x.Id, FullName = x.FullName })
                .ToListAsync();

            return this.Json(items);
        }

        [AjaxPost]
        public async Task<IActionResult> BrandsAutoComplete([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.Query<Handpiece>().Select(x => new { Name = x.Brand }).Distinct();
            var result = await query.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ModelsAutoComplete([DataSourceRequest] DataSourceRequest request, String brand)
        {
            var query = this.Repository.Query<Handpiece>();

            if (!String.IsNullOrEmpty(brand))
            {
                query = query.Where(x => x.Brand == brand);
            }

            var result = await query.Select(x => new { Name = x.MakeAndModel }).Distinct().ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ProblemsAutoComplete([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.Query<ProblemDescriptionOption>();
            var result = await query.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [Authorize(Roles = ApplicationRoles.CompanyAdministrator)]
        public async Task<IActionResult> DownloadImages()
        {
            var handpieceImageService = this.ControllerServices.ServiceProvider.GetService<HandpieceImagesService>();
            var archiveBytes = await handpieceImageService.CreateArchive();
            return this.File(archiveBytes, "application/zip", "Images.zip");
        }

        [Authorize(Roles = ApplicationRoles.CompanyAdministrator)]
        public async Task<IActionResult> Generate()
        {
            var corporates = await this.Repository.Query<Corporate>().ToListAsync();
            var clients = await this.Repository.Query<Client>().ToListAsync();
            var employees = await this.Repository.Query<Employee>().ToListAsync();

            var model = new JobsGenerateViewModel
            {
                Corporates = corporates,
                Clients = clients,
                Employees = employees,

                Quantity = 1000,
                Year = DateTime.UtcNow.Year,
            };

            var defaultSettings = EntitiesGenerationSettings.CreateDefault();
            model.BrandsConfig = defaultSettings.SerializeBrandsConfig();
            model.CostsConfig = defaultSettings.SerializeCostsConfig();
            model.RatingsConfig = defaultSettings.SerializeRatingsConfig();
            model.StatusesConfig = defaultSettings.SerializeStatusesConfig();
            model.DatesConfig = defaultSettings.SerializeDatesConfig();

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.CompanyAdministrator)]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 512 * 1024 * 1024, ValueLengthLimit = 512 * 1024 * 1024)]
        public async Task<IActionResult> Generate(JobsGenerateViewModel model)
        {
            var corporates = await this.Repository.Query<Corporate>().ToListAsync();
            var clients = await this.Repository.Query<Client>().ToListAsync();
            var employees = await this.Repository.Query<Employee>().ToListAsync();

            model.Corporates = corporates;
            model.Clients = clients;
            model.Employees = employees;

            (Boolean Success, String Error) result;
            var settings = new EntitiesGenerationSettings();
            if (!(result = settings.TryParseBrands(model.BrandsConfig)).Success)
            {
                this.ModelState.AddModelError(nameof(model.BrandsConfig), result.Error);
                return this.View(model);
            }

            if (!(result = settings.TryParseCosts(model.CostsConfig)).Success)
            {
                this.ModelState.AddModelError(nameof(model.CostsConfig), result.Error);
                return this.View(model);
            }

            if (!(result = settings.TryParseRatings(model.RatingsConfig)).Success)
            {
                this.ModelState.AddModelError(nameof(model.RatingsConfig), result.Error);
                return this.View(model);
            }

            if (!(result = settings.TryParseStatuses(model.StatusesConfig)).Success)
            {
                this.ModelState.AddModelError(nameof(model.StatusesConfig), result.Error);
                return this.View(model);
            }

            if (!(result = settings.TryParseDates(model.DatesConfig)).Success)
            {
                this.ModelState.AddModelError(nameof(model.DatesConfig), result.Error);
                return this.View(model);
            }

            var selectedEmployee = employees.SingleOrDefault(x => x.Id == model.SelectedEmployee);
            if (selectedEmployee == null)
            {
                this.ModelState.AddModelError(nameof(model.SelectedEmployee), "Invalid employee");
                return this.View(model);
            }

            var selectedClients = clients.Where(x => model.SelectedClients.Contains(x.Id) || (x.CorporateId.HasValue && model.SelectedCorporates.Contains(x.CorporateId.Value))).ToList();
            if (selectedClients.Count == 0)
            {
                this.ModelState.AddModelError(nameof(model.SelectedEmployee), "At least one client is required");
                return this.View(model);
            }

            if (this.ModelState.IsValid)
            {
                var handpieceImageService = this.ControllerServices.ServiceProvider.GetService<HandpieceImagesService>();
                var runtimeSettings = new EntitiesGenerationRuntimeSettings
                {
                    Year = model.Year,
                    NumberOfHandpieces = model.Quantity,
                    Employee = selectedEmployee,
                    Clients = selectedClients,
                };

                var jobs = await this.entitiesGenerationService.GenerateJobs(settings, runtimeSettings, handpieceImageService.CreateLoader(model.ImagesArchive));
                foreach (var job in jobs)
                {
                    await this.Repository.InsertAsync(job);
                }

                await this.Repository.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        private async Task InitializeIndexViewModel(JobsIndexViewModel model)
        {
            var filtersMetadata = this.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>().GetMetadataForType(typeof(JobsIndexFilterModel));
            var filtersBinder = this.ModelBinderFactory.CreateBinder(new ModelBinderFactoryContext { Metadata = filtersMetadata, });
            var filtersBindingContext = DefaultModelBindingContext.CreateBindingContext(
                this.HttpContext.RequestServices.GetRequiredService<IActionContextAccessor>().ActionContext ?? throw new InvalidOperationException("Unable to retrieve action context"),
                await CompositeValueProvider.CreateAsync(this.ControllerContext),
                filtersMetadata,
                null,
                String.Empty);
            await filtersBinder.BindModelAsync(filtersBindingContext);
            model.Filters = filtersBindingContext.Model as JobsIndexFilterModel ?? new JobsIndexFilterModel();

            var userAccess = await this.userEntityResolver.GetEmployeeAccessAsync();

            model.Clients = await this.Repository.QueryWithoutTracking<Client>()
                .OrderBy(x => x.Name)
                .ToListAsync();

            var accessibleWorkshops = userAccess.Workshops.GetWorkshopAvailable();
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            model.JobTypes = await this.Repository.QueryWithoutTracking<JobType>()
                .OrderBy(x => x.Name)
                .ToListAsync();

            var models = await this.Repository.QueryWithoutTracking<Handpiece>()
                .Where(x => x.Job.Client.HideJobs == false)
                .OrderBy(x => x.Brand)
                .ThenBy(x => x.MakeAndModel)
                .Select(x => new { x.Brand, x.MakeAndModel })
                .Distinct()
                .ToListAsync();

            model.Models = models.Select(x => new HandpieceModelInfo(x.Brand, x.MakeAndModel)).Distinct().ToList();
        }

        private Task PreprocessReadJobsRequest(DataSourceRequest request)
        {
            request.RemapSortFields(new Dictionary<String, Action<SortDescriptor>>
            {
                ["JobStatusName"] = sort => sort.Member = "JobStatus",
            });

            return Task.CompletedTask;
        }

        private async Task<IQueryable<Job>> PrepareReadQuery()
        {
            var userAccess = await this.userEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetWorkshopAvailable();

            var query = this.Repository.Query<Job>()
                .Include(x => x.JobType)
                .Include(x => x.Workshop)
                .Include(x => x.Client)
                .Include(x => x.Creator)
                .Include(x => x.Handpieces)
                .Where(x => x.Client.HideJobs == false)
                .Where(x => accessibleWorkshops.Contains(x.WorkshopId));

            return query;
        }

        private async Task<IQueryable<Job>> PrepareReadClientJobsQuery(Client parent)
        {
            var userAccess = await this.userEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetWorkshopAvailable();

            var query = this.Repository.Query<Job>()
                .Include(x => x.JobType)
                .Include(x => x.Workshop)
                .Include(x => x.Client)
                .Include(x => x.Creator)
                .Include(x => x.Handpieces)
                .Where(x => x.ClientId == parent.Id)
                .Where(x => accessibleWorkshops.Contains(x.WorkshopId));

            return query;
        }

        private Task<IQueryable<JobReadModel>> FinalizeReadJobsQueryPreparation(IQueryable<Job> query)
        {
            var newQuery = query.Select(entity => new JobReadModel
            {
                Id = entity.Id,
                JobTypeId = entity.JobTypeId,
                JobTypeName = entity.JobType.ShortName,
                JobNumber = entity.JobNumber,
                WorkshopId = entity.WorkshopId,
                WorkshopName = entity.Workshop.Name,
                ClientId = entity.ClientId,
                ClientName = entity.Client.FullName,
                JobStatus = entity.Status,
                Received = entity.Received,
                Handpieces = entity.Handpieces.ToList(),
                HandpiecesCount = entity.Handpieces.Count,
            });

            return Task.FromResult(newQuery);
        }

        private JobReadModel ConvertJobEntityToViewModel(JobReadModel model, String[] allowedProperties)
        {
            model.JobStatusName = model.JobStatus.ToDisplayString();
            model.JobStatusConfig = Job.ComputeStatusConfig(model.JobStatus, model.Handpieces);
            return model;
        }

        private JobReadModel ConvertEntityToViewModel(Job entity, String[] allowedProperties)
        {
            return new JobReadModel
            {
                Id = entity.Id,
                JobTypeId = entity.JobTypeId,
                JobTypeName = entity.JobType.ShortName,
                JobNumber = entity.JobNumber,
                WorkshopId = entity.WorkshopId,
                WorkshopName = entity.Workshop.Name,
                ClientId = entity.ClientId,
                ClientName = entity.Client.FullName,
                JobStatus = entity.Status,
                JobStatusName = entity.Status.ToDisplayString(),
                JobStatusConfig = entity.ComputeStatusConfig(),
                Received = entity.Received,
                HandpiecesCount = entity.Handpieces.Count,
            };
        }

        private Task PreprocessReadHandpiecesRequest(DataSourceRequest request)
        {
            request.ReplaceFilterOfFiled("Serial", FilterOperator.Contains, filter => new SerialContainsFilterDescriptor(filter.Value?.ToString()));
            request.RemapSortFields(new Dictionary<String, Action<SortDescriptor>>
            {
                ["EstimatedBy"] = sort => sort.Member = "EstimatedByFullName",
                ["RepairedBy"] = sort => sort.Member = "RepairedByFullName",
            });
            return Task.CompletedTask;
        }

        private async Task<IQueryable<Handpiece>> PrepareReadHandpiecesQuery()
        {
            var userAccess = await this.userEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetWorkshopAvailable();

            var query = this.Repository.Query<Handpiece>()
                .Include(x => x.Job).ThenInclude(x => x.JobType)
                .Include(x => x.Job).ThenInclude(x => x.Workshop)
                .Include(x => x.Job).ThenInclude(x => x.Client)
                .Include(x => x.ServiceLevel)
                .Include(x => x.Creator)
                .Include(x => x.EstimatedBy)
                .Include(x => x.RepairedBy)
                .Where(x => accessibleWorkshops.Contains(x.Job.WorkshopId))
                .Where(x => x.Job.Client.HideJobs == false);

            return query;
        }

        private Task<IQueryable<JobHandpieceFullReadModel>> FinalizeReadHandpiecesQueryPreparation(IQueryable<Handpiece> query)
        {
            var intermediate = query.Select(entity => new JobHandpieceFullReadModel
            {
                Id = entity.Id,
                JobId = entity.Job.Id,
                JobTypeId = entity.Job.JobTypeId,
                JobTypeName = entity.Job.JobType.ShortName,
                JobNumber = entity.Job.JobNumber,
                JobNumberString = entity.Job.JobNumber.ToString(),
                WorkshopId = entity.Job.WorkshopId,
                WorkshopName = entity.Job.Workshop.Name,
                ClientId = entity.Job.Client.Id,
                ClientName = entity.Job.Client.FullName,
                Brand = entity.Brand,
                MakeAndModel = entity.MakeAndModel,
                Serial = entity.Serial,
                Components = entity.Components,
                DiagnosticReport = entity.DiagnosticReport,
                ServiceLevelId = entity.ServiceLevelId,
                ServiceLevelName = entity.ServiceLevelId != null ? entity.ServiceLevel.Name : "",
                JobStatus = entity.Job.Status,
                HandpieceStatus = entity.HandpieceStatus,
                EstimatedBy = entity.EstimatedById != null ? entity.EstimatedBy.FirstName + " " + entity.EstimatedBy.LastName[0] + "." : null,
                EstimatedByFullName = entity.EstimatedById != null ? entity.EstimatedBy.FirstName + " " + entity.EstimatedBy.LastName : null,
                RepairedBy = entity.RepairedById != null ? entity.RepairedBy.FirstName + " " + entity.RepairedBy.LastName[0] + "." : null,
                RepairedByFullName = entity.RepairedById != null ? entity.RepairedBy.FirstName + " " + entity.RepairedBy.LastName + "." : null,
                Rating = entity.Rating,
                Received = entity.Job.Received,
                SpeedType = entity.SpeedType,
                Parts = entity.Parts,
                PartsOutOfStock = entity.PartsOutOfStock,
                CostOfRepair = entity.CostOfRepair ?? 0m,
                InternalComment = entity.InternalComment,
            });

            return Task.FromResult(intermediate);
        }

        private JobHandpieceFullReadModel ConvertHandpieceEntityToViewModel(JobHandpieceFullReadModel entity, String[] allowedProperties)
        {
            entity.JobStatusName = entity.JobStatus.ToDisplayString();
            entity.HandpieceStatusName = entity.HandpieceStatus.ToDisplayString();
            entity.SpeedTypeName = entity.SpeedType.ToDisplayString();
            return entity;
        }

        private Task<Job> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<Job>()
                .Include(x => x.JobType)
                .Include(x => x.Workshop)
                .Include(x => x.Client)
                .Include(x => x.Creator)
                .Include(x => x.Handpieces)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<JobDetailsModel> ConvertToDetailsModelAndOpen(Job entity)
        {
            if (this.Request.Method != "POST" && !(this.Request.Query.TryGetValue("KeepNotifications", out var keepNotifications) && keepNotifications.AsBooleanOrDefault()))
            {
                await this.repairWorkflowService.HandleJobOpenAsync(entity);
            }

            return new JobDetailsModel
            {
                Id = entity.Id,
                Entity = entity,
                ActionsList = await this.repairWorkflowService.GetJobActionsListAsync(entity),
                CanAddHandpieces = await this.repairWorkflowService.CanAddHandpiecesAsync(entity),
                IsWorkshopTechnician = this.User.IsInRole(ApplicationRoles.WorkshopTechnician),
                IsOfficeAdministrator = this.User.IsInRole(ApplicationRoles.OfficeAdministrator),
                IsCompanyAdministrator = this.User.IsInRole(ApplicationRoles.CompanyAdministrator),
            };
        }

        private async Task<JobDetailsModel> ConvertToDetailsModel(Job entity)
        {
            return new JobDetailsModel
            {
                Id = entity.Id,
                Entity = entity,
                ActionsList = await this.repairWorkflowService.GetJobActionsListAsync(entity),
                CanAddHandpieces = await this.repairWorkflowService.CanAddHandpiecesAsync(entity),
                IsWorkshopTechnician = this.User.IsInRole(ApplicationRoles.WorkshopTechnician),
                IsOfficeAdministrator = this.User.IsInRole(ApplicationRoles.OfficeAdministrator),
                IsCompanyAdministrator = this.User.IsInRole(ApplicationRoles.CompanyAdministrator),
            };
        }

        private async Task InitializeCreateModel(JobType parent, JobCreateModel model, Boolean initial)
        {
            var userAccess = await this.userEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetWorkshopAvailable();

            if (initial)
            {
                if (this.Request.Query.TryGetValue("ClientId", out var stringValues) &&
                    stringValues.Count == 1 &&
                    Guid.TryParse(stringValues[0], out var clientId))
                {
                    model.ClientId = clientId;
                }

                model.Received = DateTime.Now;
            }

            model.ExpectedId = await this.sequenceService.PeekNextNumberAsync($"Jobs:{parent.Id}:JobNumber");
            model.Clients = await this.Repository.QueryWithoutTracking<Client>().OrderBy(x => x.Name).ToListAsync();
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>()
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
            model.Type = parent;
        }

        private Task<Boolean> ValidateCreateModel(JobType jobType, JobCreateModel model)
        {
            model.HandpieceList = JobCreateModelHandpiece.ParseJson(model.Handpieces);
            return Task.FromResult(this.ModelState.IsValid);
        }

        private async Task InitializeNewEntity(JobType parent, Job entity, JobCreateModel model)
        {
            var creator = await this.userEntityResolver.ResolveCurrentUserEntity();
            if (!(creator is Employee employee))
            {
                throw new InvalidOperationException("User is not an Employee");
            }

            var workshop = await this.Repository.QueryWithoutTracking<Workshop>().SingleOrDefaultAsync(x => x.Id == model.WorkshopId && x.DeletionStatus == DeletionStatus.Normal);
            if (workshop == null)
            {
                throw new InvalidOperationException("Workshop not found");
            }

            entity.WorkshopId = workshop.Id;
            entity.JobTypeId = parent.Id;
            entity.JobNumber = await this.sequenceService.TakeNextNumberAsync($"Jobs:{parent.Id}:JobNumber");

            entity.Received = model.Received;
            entity.ClientId = model.ClientId ?? throw new InvalidOperationException();
            entity.Comment = model.Comment;
            entity.Status = await this.repairWorkflowService.GetNewJobStatusAsync(parent.Id);
            entity.HasWarning = model.HasWarning;
            entity.CreatorId = employee.Id;

            var clientHandpieces = await this.Repository.Query<ClientHandpiece>()
                .Include(x => x.Components)
                .Where(x => x.ClientId == entity.ClientId)
                .ToListAsync();

            var newHandpieceStatus = await this.repairWorkflowService.GetNewHandpieceStatusAsync(parent.Id);

            var position = 1;
            var handpieces = new List<Handpiece>();
            foreach (var handpieceModel in model.HandpieceList)
            {
                var handpiece = new Handpiece
                {
                    JobPosition = position++,
                    HandpieceStatus = newHandpieceStatus,
                    Brand = handpieceModel.Brand,
                    MakeAndModel = handpieceModel.Model,
                    Serial = handpieceModel.Serial,
                    ProblemDescription = handpieceModel.ProblemDescription,
                    CreatorId = employee.Id,
                    PartsVersion = HandpiecePartsVersion.InventorySKUv1,
                    Components = new List<HandpieceComponent>(),
                };

                if (handpieceModel.Components != null && handpieceModel.Components.Count > 0)
                {
                    var componentOrder = 1;
                    foreach (var componentModel in handpieceModel.Components)
                    {
                        handpiece.Components.Add(new HandpieceComponent
                        {
                            Brand = componentModel.Brand,
                            Model = componentModel.Model,
                            Serial = componentModel.Serial,
                            OrderNo = componentOrder++,
                        });
                    }

                    handpiece.ComponentsText = String.Join("; ", handpiece.Components.OrderBy(x => x.OrderNo).Select(x => $"{x.Brand} {x.Model} S/N {x.Serial}"));
                }

                if (handpieceModel.ClientHandpieceId == null || handpieceModel.ClientHandpieceId.Value == Guid.Empty)
                {
                    handpiece.ClientHandpiece = new ClientHandpiece
                    {
                        ClientId = model.ClientId.Value,
                        Components = new List<ClientHandpieceComponent>(),
                    };

                    handpiece.ClientHandpiece.UpdateFromHandpiece(handpiece);
                }
                else
                {
                    var clientHandpiece = clientHandpieces.Single(x => x.Id == handpieceModel.ClientHandpieceId);
                    handpiece.ClientHandpiece = clientHandpiece;
                    handpiece.ClientHandpiece.UpdateFromHandpiece(handpiece);
                }

                handpieces.Add(handpiece);
            }

            entity.Handpieces = handpieces;
        }

        private async Task AfterEntityCreated(JobType parent, Job entity, JobCreateModel model, Dictionary<String, Object> additionalData)
        {
            await this.repairWorkflowService.HandleJobCreate(entity.Id);
            await this.jobChangeTrackingService.TrackCreatedEntityAsync(entity);
            foreach (var handpiece in entity.Handpieces)
            {
                await this.handpieceChangeTrackingService.TrackCreatedEntityAsync(handpiece);
            }

            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> CreateSuccessResult(JobType parent, Job entity, JobCreateModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("JobsCreate", this.RedirectToAction("Index"));
        }

        private async Task InitializeEditModel(Job entity, JobEditModel model, Boolean initial)
        {
            var userAccess = await this.userEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetWorkshopAvailable();

            model.Original = entity;
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>()
                .Where(x => accessibleWorkshops.Contains(x.Id) || x.Id == entity.WorkshopId)
                .Where(x => x.DeletionStatus == DeletionStatus.Normal || x.Id == entity.WorkshopId)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
            model.Clients = await this.Repository.QueryWithoutTracking<Client>().OrderBy(x => x.Name).ToListAsync();
            model.SelectedClient = model.ClientId == null ? null : await this.Repository.QueryWithoutTracking<Client>().SingleOrDefaultAsync(x => x.Id == model.ClientId);
            model.ActionsList = await this.repairWorkflowService.GetJobActionsListAsync(entity);
            model.Access = userAccess;
            model.AccessControl = await this.repairWorkflowService.GetPropertiesAccessControlAsync(entity);
            model.JobType = entity.JobType;
            model.JobNumber = entity.JobNumber;

            if (this.Request.Method != "POST" && !(this.Request.Query.TryGetValue("KeepNotifications", out var keepNotifications) && keepNotifications.AsBooleanOrDefault()))
            {
                await this.repairWorkflowService.HandleJobOpenAsync(entity);
            }
        }

        private Task InitializeEditModelWithEntity(Job entity, JobEditModel model)
        {
            model.WorkshopId = entity.WorkshopId;
            model.ClientId = entity.ClientId;
            model.Received = entity.Received;
            model.Comment = entity.Comment;
            model.HasWarning = entity.HasWarning;
            model.ApprovedBy = entity.ApprovedBy;
            model.ApprovedOn = entity.ApprovedOn;
            model.RatePlan = entity.RatePlan;
            return Task.CompletedTask;
        }

        private async Task UpdateExistingEntity(Job entity, JobEditModel model)
        {
            var acl = await this.repairWorkflowService.GetPropertiesAccessControlAsync(entity);

            ////if (acl.For(x => x.ClientId).CanUpdate && model.ClientId.HasValue)
            ////{
            ////    entity.ClientId = model.ClientId.Value;
            ////}

            acl.For(x => x.Received).TryUpdate(entity, model.Received);
            acl.For(x => x.Comment).TryUpdate(entity, model.Comment);
            acl.For(x => x.HasWarning).TryUpdate(entity, model.HasWarning);
            acl.For(x => x.ApprovedBy).TryUpdate(entity, model.ApprovedBy);
            acl.For(x => x.ApprovedOn).TryUpdate(entity, model.ApprovedOn);
            acl.For(x => x.RatePlan).TryUpdate(entity, model.RatePlan);

            if (!String.IsNullOrEmpty(entity.ApprovedBy) && !entity.ApprovedOn.HasValue)
            {
                acl.For(x => x.ApprovedOn).TryUpdate(entity, DateTime.Now.Date);
            }
        }

        private async Task BeforeEntityUpdated(Job entity, JobEditModel model, Dictionary<String, Object> additionalData)
        {
            var change = await this.jobChangeTrackingService.CaptureEntityForUpdate(entity);
            additionalData["ChangeTrackingService.Change"] = change;
        }

        private async Task AfterEntityUpdated(Job entity, JobEditModel model, Dictionary<String, Object> additionalData)
        {
            var change = (JobChange)additionalData["ChangeTrackingService.Change"];
            await this.jobChangeTrackingService.TrackModifyEntityAsync(entity, change);
            await this.Repository.SaveChangesAsync();
        }

        private async Task AfterEntityDeleted(Job entity, Dictionary<String, Object> additionalData)
        {
            await this.ControllerServices.ServiceProvider.GetService<NotificationsService>().RemoveJobNotifications(entity.Id);
        }

        private Task<IActionResult> GetEditSuccessResult(Job entity, JobEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("JobsEdit", this.RedirectToAction("Edit", new { id = entity.Id, KeepNotifications = true }));
        }

        private Task<IActionResult> GetDeleteSuccessResult(Job entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("JobsDelete", this.RedirectToAction("Index"));
        }

        private async Task ExecuteUpdateHandpiecesOperation(Guid id, Job entity, JobStatusChangeModel model)
        {
            var handpieces = await this.Repository.Query<Handpiece>("Job").Where(x => x.JobId == entity.Id).ToListAsync();
            foreach (var item in model.Items)
            {
                var match = handpieces.SingleOrDefault(x => x.Id == item.Id);
                if (match != null)
                {
                    if (await this.repairWorkflowService.CanChangeHandpieceStatusAsync(match, item.Status))
                    {
                        var change = await this.handpieceChangeTrackingService.CaptureEntityForUpdate(match);
                        await this.repairWorkflowService.ChangeHandpieceStatusAsync(match, item.Status);
                        await this.handpieceChangeTrackingService.TrackModifyEntityAsync(match, change);
                        await this.Repository.SaveChangesAsync();
                    }
                }
            }
        }

        private Task<IActionResult> GetUpdateHandpiecesOperationSuccessResult(Guid id, Job entity, JobStatusChangeModel model)
        {
            return Task.FromResult<IActionResult>(this.RedirectToAction("Edit", new { id = entity.Id, KeepNotifications = true }));
        }

        private async Task ExecuteSetStatusOperation(Guid id, Job entity, JobSetStatusModel model)
        {
            var change = await this.jobChangeTrackingService.CaptureEntityForUpdate(entity);
            await this.repairWorkflowService.ChangeJobStatusAsync(entity, model.Status);
            await this.jobChangeTrackingService.TrackModifyEntityAsync(entity, change);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetSetStatusOperationSuccessResult(Guid id, Job entity, JobSetStatusModel model)
        {
            return Task.FromResult<IActionResult>(this.RedirectToAction("Edit", new { id = entity.Id, KeepNotifications = true }));
        }

        private Task InitializeChangeClientModel(Guid id, Job entity, JobChangeClientModel model, Boolean initial)
        {
            model.Entity = entity;
            model.CurrentClientId = entity.ClientId;
            return Task.CompletedTask;
        }

        private async Task ExecuteChangeClient(Guid id, Job entity, JobChangeClientModel model)
        {
            await using var transaction = await this.transactionService.BeginTransactionAsync();

            var jobManager = this.ControllerServices.ServiceProvider.GetRequiredService<IJobManager>();
            var clientManager = this.ControllerServices.ServiceProvider.GetRequiredService<IClientManager>();

            var job = await jobManager.GetByIdAsync(id);
            var client = await clientManager.GetByIdAsync(model.NewClientId!.Value);

            await job.ChangeClientAsync(client);
            await transaction.CommitAsync();
        }

        private Task<IActionResult> GetChangeClientSuccessResult(Guid id, Job entity, JobChangeClientModel model)
        {
            return this.HybridFormResultAsync("JobsChangeClient", this.RedirectToAction("Edit", new { id = entity.Id, KeepNotifications = true }));
        }

        private async Task InitializeChangeWorkshopModel(Guid id, Job entity, JobChangeWorkshopModel model, Boolean initial)
        {
            var userAccess = await this.userEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetWorkshopAvailable();

            model.Entity = entity;
            model.Workshops = await this.Repository
                .QueryWithoutTracking<Workshop>()
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
            model.CurrentWorkshopId = entity.WorkshopId;
        }

        private async Task ExecuteChangeWorkshop(Guid id, Job entity, JobChangeWorkshopModel model)
        {
            await using var transaction = await this.transactionService.BeginTransactionAsync();

            var jobManager = this.ControllerServices.ServiceProvider.GetRequiredService<IJobManager>();
            var workshopManager = this.ControllerServices.ServiceProvider.GetRequiredService<IWorkshopManager>();

            var job = await jobManager.GetByIdAsync(id);
            var workshop = await workshopManager.GetByIdAsync(model.NewWorkshopId!.Value);

            await job.ChangeWorkshopAsync(workshop);
            await transaction.CommitAsync();
        }

        private Task<IActionResult> GetChangeWorkshopSuccessResult(Guid id, Job entity, JobChangeWorkshopModel model)
        {
            return this.HybridFormResultAsync("JobsChangeWorkshop", this.RedirectToAction("Edit", new { id = entity.Id, KeepNotifications = true }));
        }

        private class SerialContainsFilterDescriptor : IFilterDescriptor
        {
            private readonly String substring;

            public SerialContainsFilterDescriptor(String substring)
            {
                this.substring = substring;
            }

            public Expression CreateFilterExpression(Expression instance)
            {
                var stringContains = typeof(String).GetMethod("Contains", new Type[] { typeof(String) });
                var serialProperty = typeof(JobHandpieceFullReadModel).GetProperty("Serial");
                var serialExpression = Expression.Property(instance, serialProperty);

                var containsExpression = Expression.Call(serialExpression, stringContains, Expression.Constant(this.substring));

                var componentsProperty = typeof(JobHandpieceFullReadModel).GetProperty("Components");
                var componentsExpression = Expression.Property(instance, componentsProperty);

                var anyPredicateParameter = Expression.Parameter(typeof(HandpieceComponent), "z");
                var anyPredicateExpression = Expression.Lambda<Func<HandpieceComponent, Boolean>>(
                    Expression.Call(
                        Expression.Property(anyPredicateParameter, typeof(HandpieceComponent).GetProperty("Serial")),
                        stringContains,
                        Expression.Constant(this.substring)),
                    anyPredicateParameter);

                var methodAny = typeof(System.Linq.Enumerable)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static).Single(x => x.Name == "Any" && x.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(HandpieceComponent));

                var anyExpression = Expression.Call(null, methodAny, componentsExpression, anyPredicateExpression);

                var orElseExpression = Expression.OrElse(containsExpression, anyExpression);
                return orElseExpression;
            }
        }
    }
}
