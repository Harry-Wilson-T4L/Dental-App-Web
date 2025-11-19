using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
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
    public class HandpieceHistoryController : Controller
    {
        private readonly IEntityControllerServices controllerServices;

        public HandpieceHistoryController(IEntityControllerServices controllerServices)
        {
            this.controllerServices = controllerServices;

            this.IndexHandler = new TelerikCrudDependentIndexActionHandler<Guid, Handpiece, Guid, Handpiece, HandpieceHistoryIndexViewModel>(
                this,
                controllerServices,
                new DefaultDependentEntityPermissionsValidator<Handpiece, Handpiece>(controllerServices.PermissionsHub, null, null, null, null));
            this.ReadHandler = new TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, Guid, Handpiece, HandpieceHistoryItemViewModel>(
                this,
                controllerServices,
                new DefaultDependentEntityPermissionsValidator<Handpiece, Handpiece>(controllerServices.PermissionsHub, null, null, null, null));

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.ReadHandler.Overrides.PreprocessRequest = this.PreprocessRequest;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;
        }

        protected TelerikCrudDependentIndexActionHandler<Guid, Handpiece, Guid, Handpiece, HandpieceHistoryIndexViewModel> IndexHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, Guid, Handpiece, HandpieceHistoryItemViewModel> ReadHandler { get; }

        protected IRepository Repository => this.controllerServices.Repository;

        [AjaxGet]
        public Task<IActionResult> Index(Guid parentId) => this.IndexHandler.Index(parentId);

        [AjaxPost]
        public Task<IActionResult> Read(Guid parentId, [DataSourceRequest] DataSourceRequest request) => this.ReadHandler.Read(parentId, request);

        private Task InitializeIndexViewModel(HandpieceHistoryIndexViewModel model, Handpiece parent)
        {
            model.Parent = parent;
            return Task.CompletedTask;
        }

        private Task<Handpiece> QuerySingleParentEntity(Guid id)
        {
            return this.Repository.Query<Handpiece>("Job").SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task PreprocessRequest(DataSourceRequest request)
        {
            request.RenameFields(new Dictionary<String, String>
            {
                ["JobNumber"] = "Job.Number",
                ["JobReceived"] = "Job.Received",
            });

            return Task.CompletedTask;
        }

        private Task<IQueryable<Handpiece>> PrepareReadQuery(Handpiece parent)
        {
            var query = this.Repository.Query<Handpiece>("Job", "Creator", "EstimatedBy", "RepairedBy")
                .Where(x => x.Job.ClientId == parent.Job.ClientId && x.ClientHandpieceId == parent.ClientHandpieceId);

            return Task.FromResult(query);
        }

        private HandpieceHistoryItemViewModel ConvertEntityToViewModel(Handpiece parent, Handpiece entity, String[] allowedProperties)
        {
            return new HandpieceHistoryItemViewModel
            {
                Id = entity.Id,
                JobNumber = entity.Job.JobNumber,
                JobReceived = entity.Job.Received,
                CompletedOn = entity.CompletedOn,
                DiagnosticReport = entity.DiagnosticReport,
                Rating = entity.Rating,
                TechnicianName = entity.EstimatedById != null ? entity.EstimatedBy.FirstName + " " + entity.EstimatedBy.LastName : String.Empty,
                CostOfRepair = entity.CostOfRepair ?? 0m,
            };
        }
    }
}
