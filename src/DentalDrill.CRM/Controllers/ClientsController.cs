using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.QueryModels;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.ClientFeedbackForms;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Identity.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Amazon.S3.Util.S3EventNotification;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    [PermissionsManager("Type", "/Domain/Internal/Client")]
    [PermissionsManager("Entity", "/Domain/Internal/Client/{entity}")]
    public class ClientsController : BaseTelerikIndexBasicCrudController<Guid, Client, EmptyIndexViewModel, ClientViewModel, ClientDetailsViewModel, ClientEditViewModel, ClientEditViewModel, ClientDetailsViewModel>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ClientsController(IEntityControllerServices controllerServices, UserManager<ApplicationUser> userManager)
            : base(controllerServices)
        {
            this.userManager = userManager;

            this.ReadClientsHandler = new TelerikCrudAjaxReadActionHandler<Guid, ClientIndexItem, ClientViewModel>(
                this,
                this.ControllerServices,
                new DefaultEntityPermissionsValidator<ClientIndexItem>(this.ControllerServices.PermissionsHub, null, null, null));

            this.DetailsTabRepairsActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientHandpiecesController>());
            this.DetailsTabNotesActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientNotesController>());
            this.DetailsTabEmailsActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientEmailsController>());
            this.DetailsTabAttachmentsActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientAttachmentsController>());
            this.DetailsTabInvoicesActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientInvoicesController>());
            this.DetailsTabFeedbackActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientFeedbackFormIndexModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientFeedbackFormsController>());
            this.DetailsTabAccessActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientAccessController>());
            this.DetailsTabCallbackNotificationsActionHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientCallbackNotificationsController>());
            this.DetailsTabRepairHistoryHandler = new BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel>(this, controllerServices, this.GetExternalPermissionsValidator<ClientRepairHistoryController>());

            this.AppearanceHandler = new BasicCrudCustomOperationActionHandler<Guid, Client, ClientAppearanceEditViewModel>(this, controllerServices, this.PermissionsValidator);
            this.AppearanceHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.AppearanceHandler.Overrides.DemandPermissions = this.DemandAppearancePermissions;
            this.AppearanceHandler.Overrides.InitializeOperationModel = this.InitializeAppearanceOperationModel;
            this.AppearanceHandler.Overrides.ExecuteOperation = this.ExecuteAppearanceChangeOperation;
            this.AppearanceHandler.Overrides.GetOperationSuccessResult = this.GetAppearanceOperationSuccessResult;

            this.EnableEmailHandler = new BasicCrudCustomOperationActionHandler<Guid, Client, ClientEnableEmailOperationModel>(this, controllerServices, this.PermissionsValidator);
            this.EnableEmailHandler.Overrides.ExecuteOperation = this.ExecuteEnableEmailOperation;

            this.ReadClientsHandler.Overrides.PrepareReadQuery = this.PrepareReadClientsQuery;
            this.ReadClientsHandler.Overrides.ConvertEntityToViewModel = this.ConvertClientEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;
            this.DeleteHandler.Overrides.DeleteEntity = this.DeleteEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;
            this.CreateHandler.Overrides.ValidateCreateModel = this.ValidateCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;
            this.EditHandler.Overrides.ValidateEditModel = this.ValidateEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DetailsTabRepairsActionHandler.Overrides.QuerySingleEntity = this.QuerySingleTabEntity;
            this.DetailsTabRepairsActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabNotesActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabEmailsActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabAttachmentsActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabInvoicesActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabFeedbackActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToFeedbackTabModel;

            this.DetailsTabAccessActionHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsTabAccessActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabCallbackNotificationsActionHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DetailsTabRepairHistoryHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;
        }

        protected TelerikCrudAjaxReadActionHandler<Guid, ClientIndexItem, ClientViewModel> ReadClientsHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabNotesActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabRepairsActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabEmailsActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabAttachmentsActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabInvoicesActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientFeedbackFormIndexModel> DetailsTabFeedbackActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabAccessActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabCallbackNotificationsActionHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Client, ClientDetailsViewModel> DetailsTabRepairHistoryHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Client, ClientAppearanceEditViewModel> AppearanceHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Client, ClientEnableEmailOperationModel> EnableEmailHandler { get; }

        public Task<IActionResult> Appearance(Guid id) => this.AppearanceHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Appearance(Guid id, ClientAppearanceEditViewModel model) => this.AppearanceHandler.Execute(id, model);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EnableEmail(Guid id, ClientEnableEmailOperationModel model) => this.EnableEmailHandler.Execute(id, model);

        [AjaxPost]
        public override Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request) => this.ReadClientsHandler.Read(request);

        public Task<IActionResult> DetailsTabRepairs(Guid id) => this.DetailsTabRepairsActionHandler.Details(id);

        public Task<IActionResult> DetailsTabNotes(Guid id) => this.DetailsTabNotesActionHandler.Details(id);

        public Task<IActionResult> DetailsTabEmails(Guid id) => this.DetailsTabEmailsActionHandler.Details(id);

        public Task<IActionResult> DetailsTabAttachments(Guid id) => this.DetailsTabAttachmentsActionHandler.Details(id);

        public Task<IActionResult> DetailsTabInvoices(Guid id) => this.DetailsTabInvoicesActionHandler.Details(id);

        public Task<IActionResult> DetailsTabFeedback(Guid id) => this.DetailsTabFeedbackActionHandler.Details(id);

        public Task<IActionResult> DetailsTabAccess(Guid id) => this.DetailsTabAccessActionHandler.Details(id);

        public Task<IActionResult> DetailsTabCallbackNotifications(Guid id) => this.DetailsTabCallbackNotificationsActionHandler.Details(id);

        public Task<IActionResult> DetailsTabRepairHistory(Guid id) => this.DetailsTabRepairHistoryHandler.Details(id);

        private Task<IQueryable<ClientIndexItem>> PrepareReadClientsQuery()
        {
            var dbContext = this.ControllerServices.ServiceProvider.GetService<ApplicationDbContext>();
            var queryText = @"select
  c.[Id],
  z.[Id] as [ZoneId],
  z.[Name] as [ZoneName],
  c.[Name],
  c.[PrincipalDentist],
  c.[Phone],
  c.[Address],
  c.[Suburb],
  s.[Id] as [StateId],
  s.[Name] as [StateName],
  c.[PostCode],
  c.[Email],
  iif(hc.[ClientId] is not null, hc.[HandpiecesCount], 0) as [HandpiecesCount],
  ff.[AverageRating]
from [Clients] c
left join [Zones] z on c.[ZoneId] = z.[Id]
left join [States] s on c.[StateId] = s.[Id]
left join (select j.[ClientId], count(*) as [HandpiecesCount] from [Handpieces] h inner join [Jobs] j on h.[JobId] = j.[Id] group by j.[ClientId]) hc on c.[Id] = hc.[ClientId]
left join (select ff.[ClientId], cast(cast(sum(ff.[TotalRating]) as decimal(18,2)) / count(*) / 5 as decimal(18,2)) as [AverageRating] from [FeedbackForms] ff where ff.[Status] = 1 group by ff.[ClientId]) as ff on c.[Id] = ff.[ClientId]";
            var query = dbContext.Set<ClientIndexItem>().FromSqlRaw(queryText);
            return Task.FromResult(query);
        }

        private ClientViewModel ConvertClientEntityToViewModel(ClientIndexItem entity, String[] allowedProperties)
        {
            return new ClientViewModel
            {
                Id = entity.Id,
                ZoneId = entity.ZoneId,
                ZoneName = entity.ZoneName,
                Name = entity.Name,
                PrincipalDentist = entity.PrincipalDentist,
                Phone = entity.Phone,
                Address = entity.Address,
                Suburb = entity.Suburb,
                StateId = entity.StateId,
                StateName = entity.StateName,
                Postcode = entity.Postcode,
                Email = entity.Email,
                HandpiecesCount = entity.HandpiecesCount,
                AverageRating = entity.AverageRating,
            };
        }

        private Task<IActionResult> GetDeleteSuccessResult(Client entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("ClientDelete", this.RedirectToAction("Index"));
        }

        private Task<IActionResult> GetEditSuccessResult(Client entity, ClientEditViewModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("ClientEdit", this.RedirectToAction("Details", null, new { Id = entity.Id }));
        }

        private async Task InitializeCreateModel(ClientEditViewModel model, Boolean initial)
        {
            if (initial)
            {
                var lastClient = await this.Repository.QueryWithoutTracking<Client>()
                    .OrderByDescending(x => x.ClientNo)
                    .FirstOrDefaultAsync();
                model.ClientNo = lastClient != null ? lastClient.ClientNo + 1 : 1;
            }

            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            model.Access = employeeAccess.Clients;

            model.States = this.Repository.Query<State>()
                .Select(x => new { x.Id, x.Name }).ToList()
                .Select(x => new SelectItemViewModel() { Id = x.Id, Name = x.Name }).ToList();

            model.Zones = this.Repository.Query<Zone>().ToList()
                .Select(x => new { x.Id, x.Name }).ToList()
                .Select(x => new SelectItemViewModel() { Id = x.Id, Name = x.Name }).ToList();

            model.Corporates = await this.Repository.QueryWithoutTracking<Corporate>()
                .OrderBy(x => x.Name)
                .ToListAsync();

            model.PricingCategories = await this.Repository.QueryWithoutTracking<CorporatePricingCategory>()
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            var accessibleWorkshops = employeeAccess.Workshops.GetWorkshopDeclared();
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
        }

        private Task<Client> QuerySingleEntity(Guid id)
        {
            return Task.FromResult(this.Repository.Query<Client>("User", "Corporate", "State", "Zone", "PricingCategory", "PrimaryWorkshop").SingleOrDefault(x => x.Id == id));
        }

        private Task<Client> QuerySingleTabEntity(Guid id)
        {
            return Task.FromResult(this.Repository.Query<Client>().SingleOrDefault(x => x.Id == id));
        }

        private Task<IActionResult> GetCreateSuccessResult(Client entity, ClientEditViewModel model, Dictionary<String, Object> data)
        {
            return this.HybridFormResultAsync("CreateClients", this.RedirectToAction("Index"));
        }

        private async Task<ClientDetailsViewModel> ConvertToDetailsModel(Client entity)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            var workshops = await this.Repository.Query<Workshop>().ToListAsync();
            var workshopsMap = workshops.ToDictionary(x => x.Id);
            var workshopsJobs = await this.Repository.Query<Job>().Where(x => x.ClientId == entity.Id).GroupBy(x => x.WorkshopId).Select(x => new { WorkshopId = x.Key, Count = x.Count() }).ToListAsync();
            return new ClientDetailsViewModel
            {
                Id = entity.Id,
                Entity = entity,
                AllAccess = employeeAccess,
                Access = employeeAccess.Clients,
                Workshops = workshopsJobs.Select(x => new ClientDetailsWorkshopStatistics { Workshop = workshopsMap[x.WorkshopId], Jobs = x.Count, }).ToList(),
            };
        }

        private async Task<Boolean> ValidateCreateModel(ClientEditViewModel model)
        {
            if (await this.Repository.QueryWithoutTracking<Client>().AnyAsync(x => x.ClientNo == model.ClientNo))
            {
                this.ModelState.AddModelError(nameof(model.ClientNo), "Client # must be unique");
            }

            if (await this.Repository.QueryWithoutTracking<Client>().AnyAsync(x => x.Name == model.Name))
            {
                this.ModelState.AddModelError(nameof(model.Name), "Name must be unique");
            }

            var urlPath = ClientUrlPathHelper.ConvertToUrlPath(model.Name);
            if (await this.Repository.QueryWithoutTracking<Client>().AnyAsync(x => x.UrlPath == urlPath))
            {
                this.ModelState.AddModelError(nameof(model.Name), "Name must be unique");
            }

            return this.ModelState.IsValid;
        }

        private async Task InitializeNewEntity(Client entity, ClientEditViewModel model)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();

            if (employeeAccess.Clients.CanInitField(ClientEntityField.ClientNo))
            {
                entity.ClientNo = model.ClientNo ?? throw new InvalidOperationException("ClientNo is missing");
            }
            else
            {
                throw new InvalidOperationException("ClientNo is missing");
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.Name))
            {
                entity.Name = model.Name;
            }
            else
            {
                throw new InvalidOperationException("Name is missing");
            }

            entity.UrlPath = $"Client{entity.ClientNo}";

            if (employeeAccess.Clients.CanInitField(ClientEntityField.OtherContact))
            {
                entity.OtherContact = model.OtherContact;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.MainEmail))
            {
                entity.Email = model.Email;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.OtherEmails))
            {
                entity.SecondaryEmails = model.SecondaryEmails;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.MainPhone))
            {
                entity.Phone = model.Phone;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.OtherPhones))
            {
                entity.SecondaryPhones = model.SecondaryPhones;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.Address))
            {
                entity.Address = model.Address;
                entity.Suburb = model.Suburb;
                entity.PostCode = model.PostCode;
                entity.StateId = model.StateId;
                entity.ZoneId = model.ZoneId;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.PrincipalDentist))
            {
                entity.PrincipalDentist = model.PrincipalDentist;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.OpeningHours))
            {
                entity.OpeningHours = model.OpeningHours;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.Brands))
            {
                entity.Brands = model.Brands;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.Comment))
            {
                entity.Comment = model.Comment;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.Corporate))
            {
                entity.CorporateId = model.CorporateId;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.PricingCategory))
            {
                entity.PricingCategoryId = model.PricingCategoryId;
            }

            if (employeeAccess.Clients.CanInitField(ClientEntityField.PrimaryWorkshop))
            {
                entity.PrimaryWorkshopId = model.PrimaryWorkshopId ?? throw new InvalidOperationException("PrimaryWorkshopId is missing");
            }
            else
            {
                var workshopIds = employeeAccess.Workshops.GetWorkshopDeclared();
                entity.PrimaryWorkshopId = workshopIds.Count > 0 ? workshopIds[0] : throw new InvalidOperationException("PrimaryWorkshopId is missing");
            }

            entity.FullName = entity.ComputeFullName();
            entity.NotificationsOptions |= ClientNotificationsOptions.Enabled;
        }

        private Task InitializeEditModelWithEntity(Client entity, ClientEditViewModel model)
        {
            model.ClientNo = entity.ClientNo;
            model.Name = entity.Name;
            model.OtherContact = entity.OtherContact;
            model.Email = entity.Email;
            model.SecondaryEmails = entity.SecondaryEmails;
            model.Phone = entity.Phone;
            model.SecondaryPhones = entity.SecondaryPhones;
            model.Address = entity.Address;
            model.Suburb = entity.Suburb;
            model.PostCode = entity.PostCode;
            model.PrincipalDentist = entity.PrincipalDentist;
            model.OpeningHours = entity.OpeningHours;
            model.Brands = entity.Brands;
            model.Comment = entity.Comment;
            model.StateId = entity.StateId;
            model.ZoneId = entity.ZoneId;
            model.CorporateId = entity.CorporateId;
            model.PricingCategoryId = entity.PricingCategoryId;
            model.PrimaryWorkshopId = entity.PrimaryWorkshopId;

            return Task.CompletedTask;
        }

        private async Task InitializeEditModel(Client entity, ClientEditViewModel model, Boolean initial)
        {
            model.Original = entity;

            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            model.Access = employeeAccess.Clients;

            model.States = this.Repository.Query<State>()
                .Select(x => new { x.Id, x.Name }).ToList()
                .Select(x => new SelectItemViewModel() { Id = x.Id, Name = x.Name }).ToList();

            model.Zones = this.Repository.Query<Zone>()
                .Select(x => new { x.Id, x.Name }).ToList()
                .Select(x => new SelectItemViewModel() { Id = x.Id, Name = x.Name }).ToList();

            model.Corporates = await this.Repository.QueryWithoutTracking<Corporate>()
                .OrderBy(x => x.Name)
                .ToListAsync();

            model.PricingCategories = await this.Repository.QueryWithoutTracking<CorporatePricingCategory>()
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            var accessibleWorkshops = employeeAccess.Workshops.GetWorkshopDeclared();
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>()
                .Where(x => x.Id == entity.PrimaryWorkshopId || x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.Id == entity.PrimaryWorkshopId || accessibleWorkshops.Contains(x.Id))
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
        }

        private async Task<Boolean> ValidateEditModel(Client entity, ClientEditViewModel model)
        {
            if (await this.Repository.QueryWithoutTracking<Client>().AnyAsync(x => x.ClientNo == model.ClientNo && x.Id != entity.Id))
            {
                this.ModelState.AddModelError(nameof(model.ClientNo), "Client # must be unique");
            }

            if (await this.Repository.QueryWithoutTracking<Client>().AnyAsync(x => x.Name == model.Name && x.Id != entity.Id))
            {
                this.ModelState.AddModelError(nameof(model.Name), "Name must be unique");
            }

            var urlPath = ClientUrlPathHelper.ConvertToUrlPath(model.Name);
            if (await this.Repository.QueryWithoutTracking<Client>().AnyAsync(x => x.UrlPath == urlPath && x.Id != entity.Id))
            {
                this.ModelState.AddModelError(nameof(model.Name), "Name must be unique");
            }

            return this.ModelState.IsValid;
        }

        private async Task UpdateExistingEntity(Client entity, ClientEditViewModel model)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.ClientNo))
            {
                entity.ClientNo = model.ClientNo ?? throw new InvalidOperationException("ClientNo is missing");
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.Name))
            {
                entity.Name = model.Name;
                entity.UrlPath = ClientUrlPathHelper.ConvertToUrlPath(model.Name);
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.OtherContact))
            {
                entity.OtherContact = model.OtherContact;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.MainEmail))
            {
                entity.Email = model.Email;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.OtherEmails))
            {
                entity.SecondaryEmails = model.SecondaryEmails;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.MainPhone))
            {
                entity.Phone = model.Phone;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.OtherPhones))
            {
                entity.SecondaryPhones = model.SecondaryPhones;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.Address))
            {
                entity.Address = model.Address;
                entity.Suburb = model.Suburb;
                entity.PostCode = model.PostCode;
                entity.StateId = model.StateId;
                entity.ZoneId = model.ZoneId;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.PrincipalDentist))
            {
                entity.PrincipalDentist = model.PrincipalDentist;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.OpeningHours))
            {
                entity.OpeningHours = model.OpeningHours;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.Brands))
            {
                entity.Brands = model.Brands;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.Comment))
            {
                entity.Comment = model.Comment;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.Corporate))
            {
                entity.CorporateId = model.CorporateId;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.PricingCategory))
            {
                entity.PricingCategoryId = model.PricingCategoryId;
            }

            if (employeeAccess.Clients.CanWriteField(ClientEntityField.PrimaryWorkshop))
            {
                entity.PrimaryWorkshopId = model.PrimaryWorkshopId ?? throw new InvalidOperationException("PrimaryWorkshopId is missing");
            }

            entity.FullName = entity.ComputeFullName();
            if (entity.User != null && entity.User.Email != entity.Email)
            {
                entity.User.Email = !String.IsNullOrEmpty(entity.Email) ? entity.Email : null;
                await this.ControllerServices.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>().UpdateAsync(entity.User);
            }
        }

        private async Task DeleteEntity(Client entity, Dictionary<String, Object> additionalData)
        {
            var userId = entity.UserId;

            await this.Repository.DeleteAsync(entity);
            await this.Repository.SaveChangesAsync();

            if (userId.HasValue)
            {
                var user = await this.Repository.Query<ApplicationUser>().SingleAsync(x => x.Id == userId);
                await this.userManager.DeleteAsync(user).ThrowOnErrorsAsync();
            }
        }

        private async Task DemandAppearancePermissions(Guid id, Client client)
        {
            var permissionValidator = new DefaultEntityPermissionsValidator<Client>(
                this.ControllerServices.PermissionsHub,
                "/Domain/Internal/ClientAppearance",
                "/Domain/Internal/ClientAppearance/{entity}",
                null);

            if (String.Equals(this.HttpContext.Request.Method, "POST", StringComparison.InvariantCultureIgnoreCase))
            {
                await permissionValidator.DemandCanEditAsync(client);
            }
            else
            {
                await permissionValidator.DemandCanDetailsAsync(client);
            }
        }

        private async Task InitializeAppearanceOperationModel(Guid id, Client client, ClientAppearanceEditViewModel model, Boolean initial)
        {
            if (initial)
            {
                var existing = await this.Repository.QueryWithoutTracking<ClientAppearance>().SingleOrDefaultAsync(x => x.ClientId == client.Id);
                if (existing != null)
                {
                    model.PrimaryColor = existing.PrimaryColor;
                    model.SecondaryColor = existing.SecondaryColor;
                    model.HeaderTextColor = existing.HeaderTextColor;
                    model.LogoId = existing.LogoId;
                    model.BackgroundImageId = existing.BackgroundImageId;
                }
                else
                {
                    var defaultAppearance = ClientAppearance.Default();
                    model.PrimaryColor = defaultAppearance.PrimaryColor;
                    model.SecondaryColor = defaultAppearance.SecondaryColor;
                    model.HeaderTextColor = defaultAppearance.HeaderTextColor;
                }
            }

            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            model.Client = client;
            model.Access = employeeAccess.Clients;
        }

        private async Task ExecuteAppearanceChangeOperation(Guid id, Client client, ClientAppearanceEditViewModel model)
        {
            var existing = await this.Repository.QueryWithoutTracking<ClientAppearance>().SingleOrDefaultAsync(x => x.ClientId == client.Id);
            if (existing == null)
            {
                existing = new ClientAppearance
                {
                    ClientId = client.Id,
                    PrimaryColor = model.PrimaryColor,
                    SecondaryColor = model.SecondaryColor,
                    HeaderTextColor = model.HeaderTextColor,
                    LogoId = model.LogoId,
                    BackgroundImageId = model.BackgroundImageId,
                };

                await this.Repository.InsertAsync(existing);
                await this.Repository.SaveChangesAsync();
            }
            else
            {
                existing.PrimaryColor = model.PrimaryColor;
                existing.SecondaryColor = model.SecondaryColor;
                existing.HeaderTextColor = model.HeaderTextColor;
                existing.LogoId = model.LogoId;
                existing.BackgroundImageId = model.BackgroundImageId;

                await this.Repository.UpdateAsync(existing);
                await this.Repository.SaveChangesAsync();
            }
        }

        private async Task<IActionResult> GetAppearanceOperationSuccessResult(Guid id, Client client, ClientAppearanceEditViewModel model)
        {
            if (this.Request.IsAjaxRequest())
            {
                var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
                model.Client = client;
                model.Access = employeeAccess.Clients;
                model.JustUpdated = true;
                return this.View(model);
            }
            else
            {
                return this.RedirectToAction("Appearance", new { id = client.Id });
            }
        }

        private async Task ExecuteEnableEmailOperation(Guid id, Client client, ClientEnableEmailOperationModel model)
        {
            client.NotificationsOptions |= ClientNotificationsOptions.Enabled;
            if (!model.Enable)
            {
                client.NotificationsOptions ^= ClientNotificationsOptions.Enabled;
            }

            await this.Repository.UpdateAsync(client);
            await this.Repository.SaveChangesAsync();
        }

        private async Task<ClientFeedbackFormIndexModel> ConvertToFeedbackTabModel(Client client)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            return new ClientFeedbackFormIndexModel
            {
                Client = client,
                Questions = await this.Repository.QueryWithoutTracking<FeedbackFormQuestion>()
                    .Where(x => x.IsDisplayed)
                    .OrderBy(x => x.OrderNo)
                    .ToListAsync(),
                Access = employeeAccess.Clients,
            };
        }

        private IEntityPermissionsValidator<Client> GetExternalPermissionsValidator<TController>()
            where TController : ControllerBase
        {
            return new DefaultEntityPermissionsValidator<Client>(
                this.ControllerServices.PermissionsHub,
                typeManagerPath: PermissionsManagerAttribute.GetAnnotatedPermissionsManager(typeof(TController), "Type"),
                null,
                null);
        }
    }
}