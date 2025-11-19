using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    [PermissionsManager("Type", "/Domain/HandpieceStore/Order")]
    [PermissionsManager("Entity", "/Domain/HandpieceStore/Order/{entity}")]
    public class HandpieceStoreOrdersController : BaseTelerikIndexBasicDetailsController<
        Guid,
        HandpieceStoreOrder,
        EmptyEmployeeIndexViewModel,
        HandpieceStoreOrderReadModel,
        HandpieceStoreOrderDetailsModel>
    {
        public HandpieceStoreOrdersController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.UserEntityResolver = controllerServices.ServiceProvider.GetService<UserEntityResolver>();
            this.NotificationsService = controllerServices.ServiceProvider.GetService<NotificationsService>();

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.ChangeStatusHandler = new BasicCrudCustomOperationActionHandler<Guid, HandpieceStoreOrder, HandpieceStoreOrderChangeStatusModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ChangeStatusHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ChangeStatusHandler.Overrides.ExecuteOperation = this.ExecuteChangeStatusOperation;
            this.ChangeStatusHandler.Overrides.GetOperationSuccessResult = this.GetChangeStatusOperationSuccessResult;
        }

        #region Services

        protected IRepository Repository => this.ControllerServices.Repository;

        protected NotificationsService NotificationsService { get; }

        protected UserEntityResolver UserEntityResolver { get; }

        #endregion // Services

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        #region Read

        private async Task<IQueryable<HandpieceStoreOrder>> PrepareReadQuery()
        {
            var query = this.Repository.Query<HandpieceStoreOrder>(
                    "Items",
                    "Items.Listing",
                    "Items.Listing.Model",
                    "Items.Listing.Model.Brand")
                .Where(x => x.Status != HandpieceStoreOrderStatus.Removed);

            var user = await this.UserEntityResolver.ResolveCurrentUserEntity();
            switch (user)
            {
                case Administrator _:
                    break;
                case Employee employee:
                    var access = await this.UserEntityResolver.GetEmployeeAccessAsync();
                    if (!access.Global.CanReadComponent(GlobalComponent.HandpiecesOrder))
                    {
                        query = query.Where(x => false);
                    }

                    break;
                case Client client:
                    query = query.Where(x => x.ClientId == client.Id);
                    break;
                case Corporate corporate:
                    query = query.Where(x => x.CorporateId == corporate.Id || x.Client.CorporateId == corporate.Id);
                    break;
                default:
                    query = query.Where(x => false);
                    break;
            }

            return query;
        }

        private HandpieceStoreOrderReadModel ConvertEntityToViewModel(HandpieceStoreOrder order, String[] allowedProperties1)
        {
            return new HandpieceStoreOrderReadModel
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = order.Status.ToString(),
                CreatedOn = order.CreatedOn,
                BrandModel = String.Join("\n", order.Items.Select(x => $"{x.Listing.Model.Brand.Name} {x.Listing.Model.Name}")),
                Coupling = String.Join("\n", order.Items.Select(x => x.Listing.Coupling)),
                CosmeticCondition = String.Join("\n", order.Items.Select(x => x.Listing.CosmeticCondition)),
                FiberOpticBrightness = String.Join("\n", order.Items.Select(x => x.Listing.FiberOpticBrightness)),
                Notes = String.Join("\n", order.Items.Select(x => x.Listing.Notes)),
                Warranty = String.Join("\n", order.Items.Select(x => x.Listing.Warranty)),
                Price = order.Items.Sum(x => x.Listing.Price),
            };
        }

        #endregion // Read

        #region Details

        private async Task<HandpieceStoreOrder> QuerySingleEntity(Guid id)
        {
            var query = this.Repository.Query<HandpieceStoreOrder>(
                    "Items",
                    "Items.Listing",
                    "Items.Listing.Images",
                    "Items.Listing.Images.Image",
                    "Items.Listing.Model",
                    "Items.Listing.Model.Brand",
                    "Items.Listing.Model.Image")
                .Where(x => x.Status != HandpieceStoreOrderStatus.Removed);

            var user = await this.UserEntityResolver.ResolveCurrentUserEntity();
            switch (user)
            {
                case Administrator _:
                case Employee employee:
                    var access = await this.UserEntityResolver.GetEmployeeAccessAsync();
                    if (!access.Global.CanReadComponent(GlobalComponent.HandpiecesOrder))
                    {
                        query = query.Where(x => false);
                    }

                    break;
                case Client client:
                    query = query.Where(x => x.ClientId == client.Id);
                    break;
                case Corporate corporate:
                    query = query.Where(x => x.CorporateId == corporate.Id || x.Client.CorporateId == corporate.Id);
                    break;
                default:
                    query = query.Where(x => false);
                    break;
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<HandpieceStoreOrderDetailsModel> ConvertToDetailsModel(HandpieceStoreOrder order)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            var model = new HandpieceStoreOrderDetailsModel
            {
                Order = order,
                Access = employeeAccess,
                StatusTransitions = new List<HandpieceStoreOrderStatus>(),
            };

            var user = await this.UserEntityResolver.ResolveCurrentUserEntity();
            var access = await this.UserEntityResolver.GetEmployeeAccessAsync();
            var userCanManageOrders = access.Global.CanWriteComponent(GlobalComponent.HandpiecesOrder);

            switch (order.Status)
            {
                case HandpieceStoreOrderStatus.Created:
                    model.StatusTransitions.Add(HandpieceStoreOrderStatus.Cancelled);
                    if (userCanManageOrders)
                    {
                        model.StatusTransitions.Add(HandpieceStoreOrderStatus.Confirmed);
                    }

                    break;

                case HandpieceStoreOrderStatus.Confirmed:
                    if (userCanManageOrders)
                    {
                        model.StatusTransitions.Add(HandpieceStoreOrderStatus.Cancelled);
                        model.StatusTransitions.Add(HandpieceStoreOrderStatus.Shipped);
                    }

                    break;
                case HandpieceStoreOrderStatus.Shipped:
                    if (userCanManageOrders)
                    {
                        model.StatusTransitions.Add(HandpieceStoreOrderStatus.Cancelled);
                        model.StatusTransitions.Add(HandpieceStoreOrderStatus.Delivered);
                    }

                    break;
            }

            return model;
        }

        #endregion // Details

        #region ChangeStatus

        protected BasicCrudCustomOperationActionHandler<Guid, HandpieceStoreOrder, HandpieceStoreOrderChangeStatusModel> ChangeStatusHandler { get; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ChangeStatus(Guid id, HandpieceStoreOrderChangeStatusModel model) => this.ChangeStatusHandler.Execute(id, model);

        private async Task ExecuteChangeStatusOperation(Guid id, HandpieceStoreOrder order, HandpieceStoreOrderChangeStatusModel model)
        {
            if (order.Status == model.Status)
            {
                return;
            }

            var user = await this.UserEntityResolver.ResolveCurrentUserEntity();
            var access = await this.UserEntityResolver.GetEmployeeAccessAsync();
            var userCanManageOrders = access.Global.CanWriteComponent(GlobalComponent.HandpiecesOrder);

            var changed = false;

            switch (order.Status)
            {
                case HandpieceStoreOrderStatus.Created when model.Status == HandpieceStoreOrderStatus.Cancelled:
                case HandpieceStoreOrderStatus.Confirmed when model.Status == HandpieceStoreOrderStatus.Cancelled && userCanManageOrders:
                case HandpieceStoreOrderStatus.Shipped when model.Status == HandpieceStoreOrderStatus.Cancelled && userCanManageOrders:
                    changed = true;
                    order.Status = HandpieceStoreOrderStatus.Cancelled;
                    order.ConfirmedOn = null;
                    order.ShippedOn = null;
                    order.DeliveredOn = null;
                    foreach (var item in order.Items)
                    {
                        item.Listing.Status = HandpieceStoreListingStatus.Listed;
                        item.Listing.RequestedOn = null;
                        item.Listing.SoldOn = null;
                    }

                    break;

                case HandpieceStoreOrderStatus.Created when model.Status == HandpieceStoreOrderStatus.Confirmed && userCanManageOrders:
                    changed = true;
                    order.Status = HandpieceStoreOrderStatus.Confirmed;
                    order.ConfirmedOn = DateTime.UtcNow;
                    foreach (var item in order.Items)
                    {
                        item.Listing.Status = HandpieceStoreListingStatus.Sold;
                        item.Listing.SoldOn = DateTime.UtcNow;
                    }

                    break;

                case HandpieceStoreOrderStatus.Confirmed when model.Status == HandpieceStoreOrderStatus.Shipped && userCanManageOrders:
                    changed = true;
                    order.Status = HandpieceStoreOrderStatus.Shipped;
                    order.ShippedOn = DateTime.UtcNow;

                    break;

                case HandpieceStoreOrderStatus.Shipped when model.Status == HandpieceStoreOrderStatus.Delivered && userCanManageOrders:
                    changed = true;
                    order.Status = HandpieceStoreOrderStatus.Delivered;
                    order.DeliveredOn = DateTime.UtcNow;

                    break;
            }

            if (changed)
            {
                await this.Repository.UpdateAsync(order);
                await this.Repository.SaveChangesAsync();

                await this.NotificationsService.ReadHandpieceStoreOrderNotifications(order, NotificationType.HandpieceStoreOrderCreated);
            }
        }

        private async Task<IActionResult> GetChangeStatusOperationSuccessResult(Guid id, HandpieceStoreOrder order, HandpieceStoreOrderChangeStatusModel model)
        {
            if (this.Request.IsAjaxRequest())
            {
                var detailsModel = await this.ConvertToDetailsModel(order);
                return this.View("Details", detailsModel);
            }
            else
            {
                return this.RedirectToAction("Details", "HandpieceStoreOrders", new { id = order.Id });
            }
        }

        #endregion // ChangeStatus
    }
}
