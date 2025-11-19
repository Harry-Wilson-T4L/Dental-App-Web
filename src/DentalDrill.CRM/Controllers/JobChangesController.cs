using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class JobChangesController : Controller
    {
        private readonly IEntityControllerServices controllerServices;

        public JobChangesController(IEntityControllerServices controllerServices)
        {
            this.controllerServices = controllerServices;

            this.IndexHandler = new TelerikCrudDependentIndexActionHandler<Guid, JobChange, Guid, Job, JobChangeIndexViewModel>(
                this,
                this.controllerServices,
                new DefaultDependentEntityPermissionsValidator<JobChange, Job>(controllerServices.PermissionsHub, null, null, null, null));

            this.ReadHandler = new TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, JobChange, Guid, Job, JobChangeItemViewModel, JobChangeItemViewModel>(
                this,
                this.controllerServices,
                new DefaultDependentEntityPermissionsValidator<JobChange, Job>(controllerServices.PermissionsHub, null, null, null, null));

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.FinalizeQueryPreparation = this.FinalizeQueryPreparation;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;
        }

        protected IRepository Repository => this.controllerServices.Repository;

        protected TelerikCrudDependentIndexActionHandler<Guid, JobChange, Guid, Job, JobChangeIndexViewModel> IndexHandler { get; }

        protected TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, JobChange, Guid, Job, JobChangeItemViewModel, JobChangeItemViewModel> ReadHandler { get; }

        [AjaxGet]
        public Task<IActionResult> Index(Guid parentId) => this.IndexHandler.Index(parentId);

        [AjaxPost]
        public Task<IActionResult> Read(Guid parentId, [DataSourceRequest] DataSourceRequest request) => this.ReadHandler.Read(parentId, request);

        private Task InitializeIndexViewModel(JobChangeIndexViewModel model, Job entity)
        {
            model.Parent = entity;
            return Task.CompletedTask;
        }

        private Task<Job> QuerySingleParentEntity(Guid id)
        {
            return this.Repository.Query<Job>().SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<IQueryable<JobChange>> PrepareReadQuery(Job parent)
        {
            IQueryable<JobChange> query = this.Repository.Query<JobChange>()
                .Where(x => x.JobId == parent.Id && ((x.OldStatus == null && x.NewStatus != null) || (x.NewStatus == null && x.OldStatus != null) || x.OldStatus != x.NewStatus))
                .OrderByDescending(x => x.ChangedOn);

            return Task.FromResult(query);
        }

        private Task<IQueryable<JobChangeItemViewModel>> FinalizeQueryPreparation(Job parent, IQueryable<JobChange> query)
        {
            var newQuery = query.Select(x => new JobChangeItemViewModel
            {
                Id = x.Id,
                ChangedOn = x.ChangedOn,
                ChangedByName = x.ChangedBy.FirstName + " " + x.ChangedBy.LastName,
                OldStatus = x.OldStatus,
                NewStatus = x.NewStatus,
            });

            return Task.FromResult(newQuery);
        }

        private JobChangeItemViewModel ConvertEntityToViewModel(Job parent, JobChangeItemViewModel entity, String[] allowedProperties)
        {
            entity.ChangedOn = DateTime.SpecifyKind(entity.ChangedOn, DateTimeKind.Utc);
            entity.OldStatusName = entity.OldStatus.HasValue ? entity.OldStatus.Value.ToDisplayString() : String.Empty;
            entity.NewStatusName = entity.NewStatus.HasValue ? entity.NewStatus.Value.ToDisplayString() : String.Empty;
            return entity;
        }
    }
}
