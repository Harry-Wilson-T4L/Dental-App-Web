using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    public class PickupRequestsController : Controller
    {
        private readonly IEntityControllerServices controllerServices;

        public PickupRequestsController(IEntityControllerServices controllerServices)
        {
            this.controllerServices = controllerServices;
            var permissionsValidator = new DefaultEntityPermissionsValidator<PickupRequest>(this.controllerServices.PermissionsHub, "/Domain/PickupRequest", "/Domain/PickupRequest/Entities/{entity}", null);

            this.IndexHandler = new TelerikCrudIndexActionHandler<Guid, PickupRequest, EmptyEmployeeIndexViewModel>(this, this.controllerServices, permissionsValidator);
            this.ReadHandler = new TelerikCrudAjaxReadIntermediateActionHandler<Guid, PickupRequest, PickupRequestViewModel, PickupRequestViewModel>(this, this.controllerServices, permissionsValidator);
            this.DetailsHandler = new BasicCrudDetailsActionHandler<Guid, PickupRequest, PickupRequest>(this, this.controllerServices, permissionsValidator);
            this.CompleteHandler = new BasicCrudCustomOperationActionHandler<Guid, PickupRequest, Object>(this, this.controllerServices, permissionsValidator);
            this.CancelHandler = new BasicCrudCustomOperationActionHandler<Guid, PickupRequest, Object>(this, this.controllerServices, permissionsValidator);

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.FinalizeQueryPreparation = this.FinalizeQueryPreparation;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CompleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.CompleteHandler.Overrides.ExecuteOperation = this.ExecuteCompleteOperation;

            this.CancelHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.CancelHandler.Overrides.ExecuteOperation = this.ExecuteCancelOperation;
        }

        protected TelerikCrudIndexActionHandler<Guid, PickupRequest, EmptyEmployeeIndexViewModel> IndexHandler { get; }

        protected TelerikCrudAjaxReadIntermediateActionHandler<Guid, PickupRequest, PickupRequestViewModel, PickupRequestViewModel> ReadHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, PickupRequest, PickupRequest> DetailsHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, PickupRequest, Object> CompleteHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, PickupRequest, Object> CancelHandler { get; }

        public Task<IActionResult> Index() => this.IndexHandler.Index();

        public Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request) => this.ReadHandler.Read(request);

        public Task<IActionResult> Details(Guid id) => this.DetailsHandler.Details(id);

        [HttpPost]
        public Task<IActionResult> Complete(Guid id) => this.CompleteHandler.Execute(id, null);

        [HttpPost]
        public Task<IActionResult> Cancel(Guid id) => this.CancelHandler.Execute(id, null);

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.controllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<IQueryable<PickupRequest>> PrepareReadQuery()
        {
            var query = this.controllerServices.Repository.Query<PickupRequest>("CreatedBy", "Employee", "Client", "Corporate");
            return Task.FromResult(query);
        }

        private Task<IQueryable<PickupRequestViewModel>> FinalizeQueryPreparation(IQueryable<PickupRequest> query)
        {
            var intermediate = query.Select(x => new PickupRequestViewModel
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                CreatedBy = (x.EmployeeId != null ? "Employee" : x.ClientId != null ? "Client" : x.CorporateId != null ? "Corporate" : "Unknown")
                            + "\r\n"
                            + (x.EmployeeId != null ? (x.Employee.FirstName + " " + x.Employee.LastName) :
                                x.ClientId != null ? (x.Client.Name) :
                                x.CorporateId != null ? (x.Corporate.Name) : "Unknown")
                            + "\r\n"
                            + (x.CreatedById != null ? x.CreatedBy.UserName : ""),
                Type = x.Type.ToString(),
                Status = x.Status.ToString(),
                PracticeName = x.PracticeName,
                Contact = (x.ContactPerson != null ? x.ContactPerson : "") + "\r\n" +
                          (x.Email != null ? x.Email : "") + "\r\n" +
                          (x.Phone != null ? x.Phone : ""),
                Address = (x.AddressLine1 != null ? x.AddressLine1 : "") + "\r\n" +
                          (x.AddressLine2 != null ? x.AddressLine2 : "") + "\r\n" +
                          (x.Suburb != null ? x.Suburb : "") + "\r\n" +
                          (x.State != null ? x.State : "") + "\r\n" +
                          (x.Postcode != null ? x.Postcode : "") + "\r\n" +
                          (x.Country != null ? x.Country : ""),
                HandpiecesCount = x.HandpiecesCount,
            });

            return Task.FromResult(intermediate);
        }

        private PickupRequestViewModel ConvertEntityToViewModel(PickupRequestViewModel entity, String[] allowedProperties)
        {
            return entity;
        }

        private Task<PickupRequest> QuerySingleEntity(Guid id)
        {
            return this.controllerServices.Repository
                .Query<PickupRequest>("CreatedBy", "Employee", "Client", "Corporate")
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<PickupRequest> ConvertToDetailsModel(PickupRequest entity)
        {
            return Task.FromResult(entity);
        }

        private async Task ExecuteCompleteOperation(Guid id, PickupRequest entity, Object model)
        {
            if (entity.Status == PickupRequestStatus.Completed || entity.Status == PickupRequestStatus.Cancelled)
            {
                throw new InvalidOperationException();
            }

            entity.Status = PickupRequestStatus.Completed;
            await this.controllerServices.Repository.UpdateAsync(entity);
            await this.controllerServices.Repository.SaveChangesAsync();
        }

        private async Task ExecuteCancelOperation(Guid id, PickupRequest entity, Object model)
        {
            if (entity.Status == PickupRequestStatus.Completed || entity.Status == PickupRequestStatus.Cancelled)
            {
                throw new InvalidOperationException();
            }

            entity.Status = PickupRequestStatus.Cancelled;
            await this.controllerServices.Repository.UpdateAsync(entity);
            await this.controllerServices.Repository.SaveChangesAsync();
        }
    }
}
