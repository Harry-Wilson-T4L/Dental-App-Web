using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Data;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class HandpieceRequiredPartsController : BaseTelerikIndexDependentBasicCrudController<
        Guid, // TIdentifier
        HandpieceRequiredPart, // TEntity
        Guid, // TParentIdentifier
        Handpiece, // TParentEntity
        HandpieceRequiredPartIndexModel, // TIndexViewModel
        HandpieceRequiredPartReadModel, // TReadModel
        HandpieceRequiredPart, // TDetailsModel
        HandpieceRequiredPartCreateModel, // TCreateModel
        HandpieceRequiredPartEditModel, // TEditModel
        HandpieceRequiredPart> // TDeleteModel
    {
        private readonly Lazy<IDataTransactionService> dataTransactionService;
        private readonly Lazy<IHandpieceManager> handpieceManager;
        private readonly Lazy<IInventorySKUManager> inventorySKUManager;
        private readonly Lazy<IChangeTrackingService<Handpiece, HandpieceChange>> handpieceChangeTracking;

        public HandpieceRequiredPartsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.dataTransactionService = controllerServices.ServiceProvider.GetLazyService<IDataTransactionService>();
            this.handpieceManager = controllerServices.ServiceProvider.GetLazyService<IHandpieceManager>();
            this.inventorySKUManager = controllerServices.ServiceProvider.GetLazyService<IInventorySKUManager>();
            this.handpieceChangeTracking = controllerServices.ServiceProvider.GetLazyService<IChangeTrackingService<Handpiece, HandpieceChange>>();

            this.ReadAvailableHandler = new TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, InventorySKU, Guid, Handpiece, HandpieceRequiredPartAvailableSKUModel, HandpieceRequiredPartAvailableSKUModel>(
                this,
                this.ControllerServices,
                new DefaultDependentEntityPermissionsValidator<InventorySKU, Handpiece>(this.ControllerServices.PermissionsHub, null, null, null, null));

            this.IndexHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadAvailableHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.ReadAvailableHandler.Overrides.PrepareReadQuery = this.PrepareReadAvailableQuery;
            this.ReadAvailableHandler.Overrides.FinalizeQueryPreparation = this.FinalizeReadAvailableQuery;
            this.ReadAvailableHandler.Overrides.ConvertEntityToViewModel = this.ConvertAvailableEntityToViewModel;
        }

        protected IDataTransactionService DataTransactionService => this.dataTransactionService.Value;

        protected IHandpieceManager HandpieceManager => this.handpieceManager.Value;

        protected IInventorySKUManager InventorySKUManager => this.inventorySKUManager.Value;

        protected IChangeTrackingService<Handpiece, HandpieceChange> HandpieceChangeTracking => this.handpieceChangeTracking.Value;

        protected TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, InventorySKU, Guid, Handpiece, HandpieceRequiredPartAvailableSKUModel, HandpieceRequiredPartAvailableSKUModel> ReadAvailableHandler { get; }

        #region Disabled

        [NonAction]
        public override Task<IActionResult> Details(Guid id)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Create(Guid parentId)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Create(Guid parentId, HandpieceRequiredPartCreateModel model)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Edit(Guid id)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Edit(Guid id, HandpieceRequiredPartEditModel model)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Delete(Guid id)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> DeleteConfirmed(Guid id)
        {
            throw new NotSupportedException();
        }

        #endregion

        [AjaxPost]
        public override async Task<IActionResult> Read(Guid parentId, DataSourceRequest request)
        {
            var handpiece = await this.HandpieceManager.GetByIdAsync(parentId);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            var parts = await handpiece.Parts.ToReadModelAsync();
            var resultQuery = parts.AsQueryable();
            var result = await resultQuery.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [AjaxPost]
        public Task<IActionResult> ReadAvailable(Guid parentId, [DataSourceRequest] DataSourceRequest request) => this.ReadAvailableHandler.Read(parentId, request);

        [AjaxPost]
        public async Task<IActionResult> ApiAdd(Guid parentId, [FromBody] HandpieceRequiredPartApiAddModel model)
        {
            var handpiece = await this.HandpieceManager.GetByIdAsync(parentId);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            if ((handpiece.Status == HandpieceStatus.SentComplete || handpiece.Status == HandpieceStatus.Cancelled) && !this.User.IsInRole(ApplicationRoles.CompanyAdministrator))
            {
                return this.NotFound();
            }

            var sku = await this.InventorySKUManager.GetByIdAsync(model.SKU);
            if (sku == null || sku.NodeType != InventorySKUNodeType.Leaf)
            {
                return this.NotFound();
            }

            await using var transaction = await this.DataTransactionService.BeginTransactionAsync();

            var change = await this.HandpieceChangeTracking.CaptureEntityForUpdate(handpiece.Entity);
            var part = await handpiece.Parts.AddRequiredPartAsync(sku, model.Quantity);
            await handpiece.Parts.UpdateStockStatusAsync();
            await part.RefreshAsync();
            await sku.TryProcessMovementsChangesAsync(handpiece.Job.Workshop);
            await transaction.CommitAsync();
            await this.HandpieceChangeTracking.TrackModifyEntityAsync(handpiece.Entity, change);
            var result = await part.ToReadModelAsync();

            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ApiUpdate(Guid parentId, Guid id, [FromBody] HandpieceRequiredPartApiUpdateModel model)
        {
            var handpiece = await this.HandpieceManager.GetByIdAsync(parentId);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            if ((handpiece.Status == HandpieceStatus.SentComplete || handpiece.Status == HandpieceStatus.Cancelled) && !this.User.IsInRole(ApplicationRoles.CompanyAdministrator))
            {
                return this.NotFound();
            }

            var part = await handpiece.Parts.GetRequiredPartAsync(id);
            if (part == null)
            {
                return this.NotFound();
            }

            await using var transaction = await this.DataTransactionService.BeginTransactionAsync();
            var sku = part.SKU;

            var change = await this.HandpieceChangeTracking.CaptureEntityForUpdate(handpiece.Entity);
            await handpiece.Parts.UpdateQuantityAsync(part.SKU, model.OldQuantity, model.NewQuantity);
            await handpiece.Parts.UpdateStockStatusAsync();
            var updatedPart = await handpiece.Parts.GetRequiredPartAsync(id);

            await sku.TryProcessMovementsChangesAsync(handpiece.Job.Workshop);
            await transaction.CommitAsync();
            await this.HandpieceChangeTracking.TrackModifyEntityAsync(handpiece.Entity, change);
            Object result = updatedPart != null ? await updatedPart.ToReadModelAsync() : new { Id = id, RequiredQuantity = 0, };

            return this.Json(result);
        }

        public async Task<IActionResult> ApiCheckStatus(Guid parentId)
        {
            var handpiece = await this.HandpieceManager.GetByIdAsync(parentId);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            var stockStatus = await handpiece.Parts.GetStockStatusAsync();
            return this.Json(new
            {
                StockStatus = stockStatus,
            });
        }

        public async Task<IActionResult> DeallocateFrom(Guid handpiece, Guid sku)
        {
            var handpieceDomain = await this.HandpieceManager.GetByIdAsync(handpiece);
            if (handpieceDomain == null)
            {
                return this.NotFound();
            }

            var skuDomain = await this.InventorySKUManager.GetByIdAsync(sku);
            if (skuDomain == null || skuDomain.NodeType != InventorySKUNodeType.Leaf)
            {
                return this.NotFound();
            }

            var part = await handpieceDomain.Parts.FindRequiredPartAsync(skuDomain);
            if (part == null)
            {
                return this.NotFound();
            }

            var repair = await skuDomain.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(part.Id);
            if (repair == null || repair.Status != InventoryMovementStatus.Allocated)
            {
                return this.NotFound();
            }

            var model = new HandpieceRequiredPartDeallocateFromModel
            {
                Handpiece = handpieceDomain,
                SKU = skuDomain,
                Part = part,
                Repair = repair,
                AllocatedQuantity = repair.QuantityAbsolute,
                ShelfQuantity = await skuDomain.GetAvailableQuantity(handpieceDomain.Job.Workshop),
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeallocateFrom(Guid handpiece, Guid sku, HandpieceRequiredPartDeallocateFromModel model)
        {
            var handpieceDomain = await this.HandpieceManager.GetByIdAsync(handpiece);
            if (handpieceDomain == null)
            {
                return this.NotFound();
            }

            var skuDomain = await this.InventorySKUManager.GetByIdAsync(sku);
            if (skuDomain == null || skuDomain.NodeType != InventorySKUNodeType.Leaf)
            {
                return this.NotFound();
            }

            var part = await handpieceDomain.Parts.FindRequiredPartAsync(skuDomain);
            if (part == null)
            {
                return this.NotFound();
            }

            var repair = await skuDomain.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(part.Id);
            if (repair == null || repair.Status != InventoryMovementStatus.Allocated)
            {
                return this.NotFound();
            }

            try
            {
                await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                {
                    await skuDomain.Movements.ReallocateRepairsAsync(
                        handpieceDomain.Job.Workshop,
                        sourceMovements: new[] { repair.Id },
                        destinationMovements: model.ReallocateTo);
                    await skuDomain.TryProcessMovementsChangesAsync(handpieceDomain.Job.Workshop);
                    await transaction.CommitAsync();
                }

                return await this.HybridFormResultAsync("HandpieceRequiredPartsDeallocateFrom", this.RedirectToAction("Edit", "Handpieces", new { id = handpieceDomain.Id }));
            }
            catch (DomainOperationException exception)
            {
                this.ModelState.AddModelError(String.Empty, exception.Message);
            }

            model.Handpiece = handpieceDomain;
            model.SKU = skuDomain;
            model.Part = part;
            model.Repair = repair;
            model.AllocatedQuantity = repair.QuantityAbsolute;
            model.ShelfQuantity = await skuDomain.GetAvailableQuantity(handpieceDomain.Job.Workshop);

            return this.View(model);
        }

        [AjaxPost]
        public async Task<IActionResult> DeallocateFromRead(Guid handpiece, Guid sku, [DataSourceRequest] DataSourceRequest request)
        {
            var handpieceDomain = await this.HandpieceManager.GetByIdAsync(handpiece);
            if (handpieceDomain == null)
            {
                return this.NotFound();
            }

            var skuDomain = await this.InventorySKUManager.GetByIdAsync(sku);
            if (skuDomain == null || skuDomain.NodeType != InventorySKUNodeType.Leaf)
            {
                return this.NotFound();
            }

            var items = new List<HandpieceRequiredPartReallocateReadModel>();
            var availableDestinations = await skuDomain.Movements.GetMovementsByTypeAndStatusAsync<IInventoryRepairMovement>(handpieceDomain.Job.Workshop, InventoryMovementStatus.Waiting);
            foreach (var destination in availableDestinations)
            {
                var part = await destination.GetHandpieceRequiredPartAsync();
                items.Add(new HandpieceRequiredPartReallocateReadModel
                {
                    Id = destination.Id,
                    CreatedOn = destination.CreatedOn,
                    QuantityAbsolute = destination.QuantityAbsolute,
                    HandpieceId = part?.Handpiece?.Id,
                    HandpieceNumber = part?.Handpiece?.Number,
                    HandpieceStatus = part?.Handpiece?.Status,
                });
            }

            var result = await items.AsQueryable().ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        public async Task<IActionResult> AllocateTo(Guid handpiece, Guid sku)
        {
            var handpieceDomain = await this.HandpieceManager.GetByIdAsync(handpiece);
            if (handpieceDomain == null)
            {
                return this.NotFound();
            }

            var skuDomain = await this.InventorySKUManager.GetByIdAsync(sku);
            if (skuDomain == null || skuDomain.NodeType != InventorySKUNodeType.Leaf)
            {
                return this.NotFound();
            }

            var part = await handpieceDomain.Parts.FindRequiredPartAsync(skuDomain);
            if (part == null)
            {
                return this.NotFound();
            }

            var repair = await skuDomain.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(part.Id);
            if (repair == null || repair.Status != InventoryMovementStatus.Waiting)
            {
                return this.NotFound();
            }

            var model = new HandpieceRequiredPartAllocateToModel
            {
                Handpiece = handpieceDomain,
                SKU = skuDomain,
                Part = part,
                Repair = repair,
                RequiredQuantity = repair.QuantityAbsolute,
                ShelfQuantity = await skuDomain.GetAvailableQuantity(handpieceDomain.Job.Workshop),
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateTo(Guid handpiece, Guid sku, HandpieceRequiredPartAllocateToModel model)
        {
            var handpieceDomain = await this.HandpieceManager.GetByIdAsync(handpiece);
            if (handpieceDomain == null)
            {
                return this.NotFound();
            }

            var skuDomain = await this.InventorySKUManager.GetByIdAsync(sku);
            if (skuDomain == null || skuDomain.NodeType != InventorySKUNodeType.Leaf)
            {
                return this.NotFound();
            }

            var part = await handpieceDomain.Parts.FindRequiredPartAsync(skuDomain);
            if (part == null)
            {
                return this.NotFound();
            }

            var repair = await skuDomain.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(part.Id);
            if (repair == null || repair.Status != InventoryMovementStatus.Waiting)
            {
                return this.NotFound();
            }

            try
            {
                await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                {
                    await skuDomain.Movements.ReallocateRepairsAsync(
                        handpieceDomain.Job.Workshop,
                        sourceMovements: model.ReallocateFrom,
                        destinationMovements: new[] { repair.Id });
                    await skuDomain.TryProcessMovementsChangesAsync(handpieceDomain.Job.Workshop);
                    await transaction.CommitAsync();
                }

                return await this.HybridFormResultAsync("HandpieceRequiredPartsAllocateTo", this.RedirectToAction("Edit", "Handpieces", new { id = handpieceDomain.Id }));
            }
            catch (DomainOperationException exception)
            {
                this.ModelState.AddModelError(String.Empty, exception.Message);
            }

            model.Handpiece = handpieceDomain;
            model.SKU = skuDomain;
            model.Part = part;
            model.Repair = repair;
            model.RequiredQuantity = repair.QuantityAbsolute;
            model.ShelfQuantity = await skuDomain.GetAvailableQuantity(handpieceDomain.Job.Workshop);

            return this.View(model);
        }

        [AjaxPost]
        public async Task<IActionResult> AllocateToRead(Guid handpiece, Guid sku, [DataSourceRequest] DataSourceRequest request)
        {
            var handpieceDomain = await this.HandpieceManager.GetByIdAsync(handpiece);
            if (handpieceDomain == null)
            {
                return this.NotFound();
            }

            var skuDomain = await this.InventorySKUManager.GetByIdAsync(sku);
            if (skuDomain == null || skuDomain.NodeType != InventorySKUNodeType.Leaf)
            {
                return this.NotFound();
            }

            var items = new List<HandpieceRequiredPartReallocateReadModel>();
            var availableSources = await skuDomain.Movements.GetMovementsByTypeAndStatusAsync<IInventoryRepairMovement>(handpieceDomain.Job.Workshop, InventoryMovementStatus.Allocated);
            foreach (var source in availableSources)
            {
                var part = await source.GetHandpieceRequiredPartAsync();
                items.Add(new HandpieceRequiredPartReallocateReadModel
                {
                    Id = source.Id,
                    CreatedOn = source.CreatedOn,
                    QuantityAbsolute = source.QuantityAbsolute,
                    HandpieceId = part?.Handpiece?.Id,
                    HandpieceNumber = part?.Handpiece?.Number,
                    HandpieceStatus = part?.Handpiece?.Status,
                });
            }

            var result = await items.AsQueryable().ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        private Task<Handpiece> QuerySingleParentEntity(Guid id)
        {
            return this.Repository.Query<Handpiece>()
                .Include(x => x.Job).ThenInclude(x => x.Workshop)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        #region Index


        private async Task InitializeIndexViewModel(HandpieceRequiredPartIndexModel model, Handpiece handpiece)
        {
            model.Handpiece = handpiece;

            var speedCompatibility = handpiece.SpeedType switch
            {
                HandpieceSpeed.HighSpeed => HandpieceSpeedCompatibility.HighSpeed,
                HandpieceSpeed.LowSpeed => HandpieceSpeedCompatibility.LowSpeed,
                HandpieceSpeed.Other => HandpieceSpeedCompatibility.Other,
                _ => throw new InvalidOperationException("Invalid SpeedType"),
            };

            model.AvailableSKUs = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Where(x => x.NodeType == InventorySKUNodeType.Leaf)
                .Where(x => x.Type.HandpieceSpeedCompatibility == null || (x.Type.HandpieceSpeedCompatibility & speedCompatibility) == speedCompatibility)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        #endregion

        #region ReadAvailable

        private Task<IQueryable<InventorySKU>> PrepareReadAvailableQuery(Handpiece handpiece)
        {
            var query = this.Repository.QueryWithoutTracking<InventorySKU>()
                .Include(x => x.Parent)
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.NodeType == InventorySKUNodeType.Leaf);

            if (handpiece.HandpieceStatus.IsNotOneOf(HandpieceStatus.Received, HandpieceStatus.BeingEstimated))
            {
                var speedCompatibility = handpiece.SpeedType switch
                {
                    HandpieceSpeed.HighSpeed => HandpieceSpeedCompatibility.HighSpeed,
                    HandpieceSpeed.LowSpeed => HandpieceSpeedCompatibility.LowSpeed,
                    HandpieceSpeed.Other => HandpieceSpeedCompatibility.Other,
                    _ => throw new InvalidOperationException("Invalid SpeedType"),
                };

                query = query.Where(x => x.Type.HandpieceSpeedCompatibility == null || (x.Type.HandpieceSpeedCompatibility & speedCompatibility) == speedCompatibility);
            }

            return Task.FromResult(query);
        }

        private Task<IQueryable<HandpieceRequiredPartAvailableSKUModel>> FinalizeReadAvailableQuery(Handpiece handpiece, IQueryable<InventorySKU> query)
        {
            var interQuery =
                from sku in query
                join qty in this.Repository.QueryWithoutTracking<InventorySKUWorkshopQuantity>().Where(x => x.WorkshopId == handpiece.Job.WorkshopId) on sku.Id equals qty.Id into joinedQty
                from qty in joinedQty.DefaultIfEmpty()
                select new { SKU = sku, Quantity = qty, };

            IQueryable<HandpieceRequiredPartAvailableSKUModel> resultQuery = interQuery
                .Select(x => new HandpieceRequiredPartAvailableSKUModel
                {
                    Id = x.SKU.Id,
                    Name = x.SKU.Name,
                    ShelfQuantity = x.Quantity.Id != null ? x.Quantity.ShelfQuantity : 0m,
                    Price = x.SKU.AveragePrice,
                    IsDefaultChild = x.SKU.ParentId != null && x.SKU.Parent.DefaultChildId == x.SKU.Id,
                })
                .OrderBy(x => x.IsDefaultChild == false)
                .ThenBy(x => x.Name);

            return Task.FromResult(resultQuery);
        }

        private HandpieceRequiredPartAvailableSKUModel ConvertAvailableEntityToViewModel(Handpiece handpiece, HandpieceRequiredPartAvailableSKUModel skuModel, String[] allowedProperties)
        {
            return skuModel;
        }

        #endregion
    }
}
