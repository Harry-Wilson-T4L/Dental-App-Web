using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Notifications;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.EntitySequences;
using DevGuild.AspNetCore.Services.Identity;
using DevGuild.AspNetCore.Services.Mail;
using DevGuild.AspNetCore.Services.Uploads.Images;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DentalDrill.CRM.Controllers
{
    public class HandpieceStoreController : BaseTelerikIndexBasicDetailsController<
        Guid, // TIdentifier
        HandpieceStoreListing, // TEntity
        HandpieceStoreIndexModel, // TIndexModel
        HandpieceStoreReadModel, // TReadModel
        HandpieceStoreDetailsModel> // TDetailsModel
    {
        public HandpieceStoreController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.ImageUploadService = controllerServices.ServiceProvider.GetService<IImageUploadService>();
            this.UserAccessorService = controllerServices.ServiceProvider.GetService<IAuthenticatedUserAccessorService<ApplicationUser>>();
            this.EntitySequenceService = controllerServices.ServiceProvider.GetService<IEntitySequenceService>();
            this.NotificationsService = controllerServices.ServiceProvider.GetService<NotificationsService>();
            this.UserEntityResolver = controllerServices.ServiceProvider.GetService<UserEntityResolver>();

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.BuyHandler = new BasicCrudCustomOperationActionHandler<Guid, HandpieceStoreListing, HandpieceStoreDetailsModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.BuyHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.BuyHandler.Overrides.InitializeOperationModel = this.InitializeBuyOperationModel;
            this.BuyHandler.Overrides.ExecuteOperation = this.ExecuteBuyOperation;
            this.BuyHandler.Overrides.GetOperationSuccessResult = this.GetOperationSuccessResult;

            this.ListingImagesHandler = new BasicCrudDetailsActionHandler<Guid, HandpieceStoreListing, HandpieceStoreListing>(this, this.ControllerServices, this.PermissionsValidator);
            this.ListingImagesHandler.Overrides.QuerySingleEntity = this.QuerySingleListingImagesEntity;
        }

        #region Services

        protected IRepository Repository => this.ControllerServices.Repository;

        protected IImageUploadService ImageUploadService { get; }

        protected IAuthenticatedUserAccessorService<ApplicationUser> UserAccessorService { get; }

        protected IEntitySequenceService EntitySequenceService { get; }

        protected NotificationsService NotificationsService { get; }

        protected UserEntityResolver UserEntityResolver { get; }

        #endregion // Services

        #region Index

        private async Task InitializeIndexViewModel(HandpieceStoreIndexModel model)
        {
            var mostExpensiveListing = await this.Repository.QueryWithoutTracking<HandpieceStoreListing>()
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed)
                .OrderByDescending(x => x.Price)
                .FirstOrDefaultAsync();

            if (mostExpensiveListing != null)
            {
                model.HasListings = true;
                model.MaxPrice = mostExpensiveListing.Price;
            }
            else
            {
                model.HasListings = false;
                model.MaxPrice = 0m;
            }
        }

        #endregion // Index

        #region Read

        private HandpieceStoreFilterModel readFilterModel;

        [NonAction]
        public override Task<IActionResult> Read(DataSourceRequest request)
        {
            throw new InvalidOperationException();
        }

        public Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, HandpieceStoreFilterModel filterModel)
        {
            this.readFilterModel = filterModel;
            return this.AjaxReadHandler.Read(request);
        }

        private Task<IQueryable<HandpieceStoreListing>> PrepareReadQuery()
        {
            IQueryable<HandpieceStoreListing> query = this.Repository.QueryWithoutTracking<HandpieceStoreListing>(
                    "MainImage",
                    "Model",
                    "Model.Brand",
                    "Model.Image")
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed);

            if (this.readFilterModel != null)
            {
                if (!String.IsNullOrEmpty(this.readFilterModel.Search))
                {
                    query = query.Where(x => x.Model.Brand.Name.Contains(this.readFilterModel.Search) || x.Model.Name.Contains(this.readFilterModel.Search));
                }

                if (this.readFilterModel.Price != null && this.readFilterModel.Price.Length >= 2)
                {
                    var minPrice = this.readFilterModel.Price[0];
                    var maxPrice = this.readFilterModel.Price[1];
                    query = query.Where(x => x.Price >= minPrice && x.Price <= maxPrice);
                }

                if (this.readFilterModel.Brand != null && this.readFilterModel.Brand.Length > 0)
                {
                    query = query.Where(x => this.readFilterModel.Brand.Contains(x.Model.BrandId));
                }

                if (this.readFilterModel.Model != null && this.readFilterModel.Model.Length > 0)
                {
                    query = query.Where(x => this.readFilterModel.Model.Contains(x.ModelId));
                }

                if (this.readFilterModel.Coupling != null && this.readFilterModel.Coupling.Length > 0)
                {
                    query = query.Where(x => this.readFilterModel.Coupling.Contains(x.Coupling));
                }

                if (this.readFilterModel.Type != null && this.readFilterModel.Type.Length > 0)
                {
                    query = query.Where(x => this.readFilterModel.Type.Contains(x.Model.Type));
                }
            }

            query = query.OrderByDescending(x => x.CreatedOn);
            return Task.FromResult(query);
        }

        private HandpieceStoreReadModel ConvertEntityToViewModel(HandpieceStoreListing entity, String[] allowedProperties)
        {
            return new HandpieceStoreReadModel
            {
                Id = entity.Id,
                Brand = entity.Model.Brand.Name,
                Model = entity.Model.Name,
                Price = entity.Price,
                ThumbnailUrl = entity.MainImageId.HasValue
                    ? this.ImageUploadService.GetImageUrl(entity.MainImage, "300")
                    : (entity.Model.ImageId.HasValue
                        ? this.ImageUploadService.GetImageUrl(entity.Model.Image, "300")
                        : "/img/hub-logo.png"),
            };
        }

        #endregion // Read

        #region Details

        [NonAction]
        public override Task<IActionResult> Details(Guid id)
        {
            throw new InvalidOperationException();
        }

        #endregion // Details

        #region Buy

        protected BasicCrudCustomOperationActionHandler<Guid, HandpieceStoreListing, HandpieceStoreDetailsModel> BuyHandler { get; }

        [AjaxGet]
        public Task<IActionResult> Buy(Guid id) => this.BuyHandler.Execute(id);

        [AjaxPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Buy(Guid id, HandpieceStoreDetailsModel model) => this.BuyHandler.Execute(id, model);

        private Task<HandpieceStoreListing> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<HandpieceStoreListing>(
                    "Images",
                    "Images.Image",
                    "Model",
                    "Model.Brand",
                    "Model.Image")
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task InitializeBuyOperationModel(Guid id, HandpieceStoreListing storeListing, HandpieceStoreDetailsModel model, Boolean initial)
        {
            model.Listing = storeListing;
            if (await this.UserEntityResolver.ResolveCurrentUserEntity() is Client currentSurgery)
            {
                model.Surgery = currentSurgery;
                model.SurgeryName = currentSurgery.FullName;
            }
        }

        private async Task ExecuteBuyOperation(Guid id, HandpieceStoreListing storeListing, HandpieceStoreDetailsModel model)
        {
            var order = await this.CreateBlankOrderAsync();
            order.SurgeryName = model.SurgeryName;
            order.ContactName = model.ContactName;
            order.ContactEmail = model.ContactEmail;
            order.Items = new Collection<HandpieceStoreOrderItem>();
            order.Items.Add(new HandpieceStoreOrderItem
            {
                ListingId = storeListing.Id,
            });

            storeListing.Status = HandpieceStoreListingStatus.Requested;
            storeListing.RequestedOn = order.CreatedOn;

            await this.Repository.UpdateAsync(storeListing);
            await this.Repository.InsertAsync(order);
            await this.Repository.SaveChangesAsync();

            try
            {
                await this.NotificationsService.CreateCustomNotification(
                    new HandpieceStoreOrderCreatedPayload
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber,
                        CreatedOn = order.CreatedOn,
                        SurgeryName = order.SurgeryName,
                        ContactName = order.ContactName,
                        ContactEmail = order.ContactEmail,
                        Brand = storeListing.Model.Brand.Name,
                        Model = storeListing.Model.Name,
                    },
                    NotificationScope.HandpieceStoreOrder,
                    null,
                    null,
                    order);
            }
            catch (Exception)
            {
            }

            try
            {
                var emailOptions = this.ControllerServices.ServiceProvider.GetService<IOptions<HandpieceStoreOrderEmailOptions>>().Value;
                if (emailOptions.Enabled)
                {
                    var reloadedOrder = await this.Repository.Query<HandpieceStoreOrder>(
                            "Items",
                            "Items.Listing",
                            "Items.Listing.Model",
                            "Items.Listing.Model.Brand",
                            "CreatedBy",
                            "Employee",
                            "Client",
                            "Corporate")
                        .SingleOrDefaultAsync(x => x.Id == order.Id);

                    var email = new HandpieceStoreOrderCreatedEmail
                    {
                        Recipient = emailOptions.Recipient,
                        Link = $"{emailOptions.BaseUrl}HandpieceStoreOrders/Details/{reloadedOrder.Id:D}",
                        Order = reloadedOrder,
                        SubjectPrefix = emailOptions.SubjectPrefix,
                    };

                    await this.ControllerServices.ServiceProvider.GetService<IEmailService>().SendAsync(email);
                }
            }
            catch (Exception)
            {
            }
        }

        private Task<IActionResult> GetOperationSuccessResult(Guid id, HandpieceStoreListing storeListing, HandpieceStoreDetailsModel model)
        {
            return Task.FromResult<IActionResult>(this.View("BuySuccess", model));
        }

        private async Task<HandpieceStoreOrder> CreateBlankOrderAsync()
        {
            var appUser = await this.UserAccessorService.GetUserAsync();
            var order = new HandpieceStoreOrder
            {
                Id = Guid.NewGuid(),
                OrderNumber = (Int32)(await this.EntitySequenceService.TakeNextNumberAsync("HandpieceStoreOrderNumber")),
                CreatedOn = DateTime.UtcNow,
                ConfirmedOn = null,
                ShippedOn = null,
                DeliveredOn = null,
                Status = HandpieceStoreOrderStatus.Created,
            };

            if (appUser != null)
            {
                order.CreatedById = appUser.Id;
                var user = await this.UserEntityResolver.ResolveCurrentUserEntity();
                switch (user)
                {
                    case Employee employee:
                        order.EmployeeId = employee.Id;
                        break;
                    case Client client:
                        order.ClientId = client.Id;
                        break;
                    case Corporate corporate:
                        order.CorporateId = corporate.Id;
                        break;
                }
            }

            return order;
        }

        #endregion // Buy

        #region Listing Images

        protected BasicCrudDetailsActionHandler<Guid, HandpieceStoreListing, HandpieceStoreListing> ListingImagesHandler { get; }

        public Task<IActionResult> ListingImages(Guid id) => this.ListingImagesHandler.Details(id);

        private Task<HandpieceStoreListing> QuerySingleListingImagesEntity(Guid id)
        {
            return this.Repository.Query<HandpieceStoreListing>(
                    "Images",
                    "Images.Image",
                    "Model",
                    "Model.Brand",
                    "Model.Image")
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        #endregion

        #region Filters Controls Source

        [AjaxPost]
        public async Task<IActionResult> ReadBrands([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.QueryWithoutTracking<HandpieceStoreListing>()
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed)
                .Select(x => new { Id = x.Model.Brand.Id, Name = x.Model.Brand.Name, })
                .Distinct()
                .OrderBy(x => x.Name);

            var result = await query.ToDataSourceResultAsync(request, this.ModelState);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadModels([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.QueryWithoutTracking<HandpieceStoreListing>()
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed)
                .Select(x => new { Id = x.Model.Id, Name = x.Model.Brand.Name + " " + x.Model.Name, })
                .Distinct()
                .OrderBy(x => x.Name);

            var result = await query.ToDataSourceResultAsync(request, this.ModelState);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadCouplers([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.QueryWithoutTracking<HandpieceStoreListing>()
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed)
                .Where(x => x.Coupling != null && x.Coupling != "")
                .Select(x => new { Name = x.Coupling })
                .Distinct()
                .OrderBy(x => x.Name);

            var result = await query.ToDataSourceResultAsync(request, this.ModelState);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadCosmeticConditions([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.QueryWithoutTracking<HandpieceStoreListing>()
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed)
                .Where (x => x.CosmeticCondition != null && x.CosmeticCondition != "")
                .Select(x => new { Name = x.CosmeticCondition })
                .Distinct()
                .OrderBy(x => x.Name);

            var result = await query.ToDataSourceResultAsync(request, this.ModelState);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadFiberOpticBrightness([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.Repository.QueryWithoutTracking<HandpieceStoreListing>()
                .Where(x => x.Status == HandpieceStoreListingStatus.Listed)
                .Where(x => x.FiberOpticBrightness != null && x.FiberOpticBrightness != "")
                .Select(x => new { Name = x.FiberOpticBrightness })
                .Distinct()
                .OrderBy(x => x.Name);

            var result = await query.ToDataSourceResultAsync(request, this.ModelState);
            return this.Json(result);
        }

        #endregion // Filters Controls Source
    }
}
