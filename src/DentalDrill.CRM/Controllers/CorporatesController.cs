using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.QueryModels;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Corporates;
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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/InternalCorporate")]
    [PermissionsManager("Entity", "/Domain/InternalCorporate/Entities/{entity}")]
    public class CorporatesController : BaseTelerikIndexBasicCrudController<Guid, Corporate, EmptyEmployeeIndexViewModel, CorporateReadModel, CorporateDetailsModel, CorporateCreateModel, CorporateEditModel, Corporate>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public CorporatesController(IEntityControllerServices controllerServices, UserManager<ApplicationUser> userManager)
            : base(controllerServices)
        {
            this.userManager = userManager;

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.AjaxReadHandler = new TelerikCrudAjaxReadIntermediateActionHandler<Guid, Corporate, CorporateReadModel, CorporateReadModel>(this, controllerServices, this.PermissionsValidator);
            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.FinalizeQueryPreparation = this.FinalizeReadQueryPreparation;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.DeleteEntity = this.DeleteEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;

            this.ClientsHandler = new BasicCrudDetailsActionHandler<Guid, Corporate, CorporateDetailsModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ClientsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ClientsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.ReadClientsHandler = new TelerikCrudAjaxDependentReadActionHandler<Guid, ClientIndexItem, Guid, Corporate, CorporateClientReadModel>(
                this,
                this.ControllerServices,
                new DefaultDependentEntityPermissionsValidator<ClientIndexItem, Corporate>(this.ControllerServices.PermissionsHub, null, null, null, "/Domain/InternalCorporate/Entities/{entity}"));
            this.ReadClientsHandler.Overrides.PrepareReadQuery = this.PrepareReadClientsQuery;
            this.ReadClientsHandler.Overrides.ConvertEntityToViewModel = this.ConvertClientEntityToViewModel;

            this.AppearanceHandler = new BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAppearanceEditViewModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.AppearanceHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.AppearanceHandler.Overrides.DemandPermissions = this.DemandAppearancePermissions;
            this.AppearanceHandler.Overrides.InitializeOperationModel = this.InitializeAppearanceOperationModel;
            this.AppearanceHandler.Overrides.ExecuteOperation = this.ExecuteAppearanceOperation;
            this.AppearanceHandler.Overrides.GetOperationSuccessResult = this.GetAppearanceSuccessResult;

            this.AccessHandler = new BasicCrudDetailsActionHandler<Guid, Corporate, CorporateDetailsModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.AccessHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.AccessHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;
        }

        protected BasicCrudDetailsActionHandler<Guid, Corporate, CorporateDetailsModel> ClientsHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAppearanceEditViewModel> AppearanceHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Corporate, CorporateDetailsModel> AccessHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<Guid, ClientIndexItem, Guid, Corporate, CorporateClientReadModel> ReadClientsHandler { get; }

        protected new TelerikCrudAjaxReadIntermediateActionHandler<Guid, Corporate, CorporateReadModel, CorporateReadModel> AjaxReadHandler { get; }

        public Task<IActionResult> Clients(Guid id) => this.ClientsHandler.Details(id);

        [AjaxPost]
        public override Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request) => this.AjaxReadHandler.Read(request);

        [AjaxPost]
        public Task<IActionResult> ReadClients(Guid parentId, [DataSourceRequest] DataSourceRequest request) => this.ReadClientsHandler.Read(parentId, request);

        public Task<IActionResult> Appearance(Guid id) => this.AppearanceHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Appearance(Guid id, CorporateAppearanceEditViewModel model) => this.AppearanceHandler.Execute(id, model);

        public Task<IActionResult> Access(Guid id) => this.AccessHandler.Details(id);

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<IQueryable<Corporate>> PrepareReadQuery()
        {
            var query = this.Repository.Query<Corporate>("User", "Clients");
            return Task.FromResult(query);
        }

        private Task<IQueryable<CorporateReadModel>> FinalizeReadQueryPreparation(IQueryable<Corporate> query)
        {
            var finalized = query.Select(x => new CorporateReadModel
            {
                Id = x.Id,
                Name = x.Name,
                UserName = x.User != null ? x.User.UserName : null,
                UserEmail = x.User != null ? x.User.Email : null,
                ClientsCount = x.Clients.Count(),
            });

            return Task.FromResult<IQueryable<CorporateReadModel>>(finalized);
        }

        private CorporateReadModel ConvertEntityToViewModel(CorporateReadModel entity, String[] allowedProperties)
        {
            return entity;
        }

        private Task<Corporate> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<Corporate>("User").SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task InitializeNewEntity(Corporate entity, CorporateCreateModel model)
        {
            var last = await this.Repository.QueryWithoutTracking<Corporate>().OrderByDescending(x => x.CorporateNo).FirstOrDefaultAsync();
            entity.CorporateNo = last?.CorporateNo + 1 ?? 1;
            entity.UrlPath = $"Corporate{entity.CorporateNo}";
            entity.Name = model.Name;
        }

        private Task<IActionResult> GetCreateSuccessResult(Corporate entity, CorporateCreateModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("CorporatesCreate", this.RedirectToAction("Index"));
        }

        private Task<IActionResult> GetEditSuccessResult(Corporate entity, CorporateEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("CorporatesEdit", this.RedirectToAction("Index"));
        }

        private async Task DeleteEntity(Corporate entity, Dictionary<String, Object> additionalData)
        {
            var userId = entity.UserId;

            await this.Repository.DeleteAsync(entity);
            await this.Repository.SaveChangesAsync();

            if (userId.HasValue)
            {
                var user = await this.Repository.Query<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    await this.userManager.DeleteAsync(user).ThrowOnErrorsAsync();
                }
            }
        }

        private Task<IActionResult> GetDeleteSuccessResult(Corporate entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("CorporatesDelete", this.RedirectToAction("Index"));
        }

        private Task<IQueryable<ClientIndexItem>> PrepareReadClientsQuery(Corporate parent)
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
left join (select ff.[ClientId], cast(cast(sum(ff.[TotalRating]) as decimal(18,2)) / count(*) / 5 as decimal(18,2)) as [AverageRating] from [FeedbackForms] ff where ff.[Status] = 1 group by ff.[ClientId]) as ff on c.[Id] = ff.[ClientId]
where c.[CorporateId] = @corporateId";
            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("@corporateId", SqlDbType.UniqueIdentifier) { Value = parent.Id });

            var query = dbContext.Set<ClientIndexItem>().FromSqlRaw(queryText, queryParameters.ToArray());
            return Task.FromResult(query);
        }

        private CorporateClientReadModel ConvertClientEntityToViewModel(Corporate parent, ClientIndexItem entity, String[] allowedProperties)
        {
            return new CorporateClientReadModel
            {
                Id = entity.Id,
                CorporateId = parent.Id,
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

        private async Task DemandAppearancePermissions(Guid id, Corporate entity)
        {
            if (String.Equals(this.HttpContext.Request.Method, "POST", StringComparison.InvariantCultureIgnoreCase))
            {
                await this.PermissionsValidator.DemandCanEditAsync(entity);
            }
        }

        private async Task InitializeAppearanceOperationModel(Guid id, Corporate entity, CorporateAppearanceEditViewModel model, Boolean initial)
        {
            if (initial)
            {
                var existing = await this.Repository.QueryWithoutTracking<CorporateAppearance>().SingleOrDefaultAsync(x => x.CorporateId == entity.Id);
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
                    var defaultAppearance = CorporateAppearance.Default();
                    model.PrimaryColor = defaultAppearance.PrimaryColor;
                    model.SecondaryColor = defaultAppearance.SecondaryColor;
                    model.HeaderTextColor = defaultAppearance.HeaderTextColor;
                }
            }

            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            model.Corporate = entity;
            model.Access = employeeAccess;
        }

        private async Task ExecuteAppearanceOperation(Guid id, Corporate entity, CorporateAppearanceEditViewModel model)
        {
            var existing = await this.Repository.QueryWithoutTracking<CorporateAppearance>().SingleOrDefaultAsync(x => x.CorporateId == entity.Id);
            if (existing == null)
            {
                existing = new CorporateAppearance
                {
                    CorporateId = entity.Id,
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

        private Task<IActionResult> GetAppearanceSuccessResult(Guid id, Corporate entity, CorporateAppearanceEditViewModel model)
        {
            if (this.Request.IsAjaxRequest())
            {
                model.Corporate = entity;
                model.JustUpdated = true;
                return Task.FromResult<IActionResult>(this.View(model));
            }
            else
            {
                return Task.FromResult<IActionResult>(this.RedirectToAction("Appearance", new { id = entity.Id }));
            }
        }

        private async Task<CorporateDetailsModel> ConvertToDetailsModel(Corporate entity)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            return new CorporateDetailsModel
            {
                Entity = entity,
                Access = employeeAccess,
            };
        }
    }
}
