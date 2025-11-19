using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.InventoryMovements;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controls.HybridForms;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    public partial class InventoryMovementsController
    {
        [AjaxPost]
        public Task<IActionResult> ReadAll(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(sku, workshop, request, filter: null);
        }

        [AjaxPost]
        public Task<IActionResult> ReadTray(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(sku, workshop, request, filter: x => (x.Type == InventoryMovementType.Repair || x.Type == InventoryMovementType.RepairFragment) && x.Status == InventoryMovementStatus.Allocated);
        }

        [AjaxPost]
        public Task<IActionResult> ReadRequested(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && x.Status == InventoryMovementStatus.Requested);
        }

        [AjaxPost]
        public Task<IActionResult> ReadApproved(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && x.Status == InventoryMovementStatus.Approved);
        }

        [AjaxPost]
        public Task<IActionResult> ReadApprovedAndMissing(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(
                sku,
                workshop,
                request,
                filter: x => (x.Type == InventoryMovementType.Order || x.Type == InventoryMovementType.EphemeralMissingRequiredQuantity) && x.Status == InventoryMovementStatus.Approved,
                includeMissingQuantity: true);
        }

        [AjaxPost]
        public Task<IActionResult> ReadApprovedAndRequested(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && (x.Status == InventoryMovementStatus.Approved || x.Status == InventoryMovementStatus.Requested));
        }

        [AjaxPost]
        public Task<IActionResult> ReadOrdered(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && x.Status == InventoryMovementStatus.Ordered);
        }

        [AjaxPost]
        public Task<IActionResult> ReadComplete(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadCustom(sku, workshop, request, filter: x => x.Type != InventoryMovementType.Repair && x.Status == InventoryMovementStatus.Completed);
        }

        public async Task<IActionResult> Approve(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementApproveModel
            {
                SKU = await movement.GetSKUAsync(),
                Movement = movement,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id, InventoryMovementApproveModel model)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await movement.ApproveAsync();
                await sku.TryProcessMovementsChangesAsync(movement.Workshop);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsApprove", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> ApproveWithEdit(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementApproveWithEditModel
            {
                SKU = await movement.GetSKUAsync(),
                Movement = movement,
                Quantity = movement.Quantity,
                Comment = movement.Comment,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveWithEdit(Guid id, InventoryMovementApproveWithEditModel model)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await movement.ApproveWithEditAsync(model.Quantity, model.Comment);
                await sku.TryProcessMovementsChangesAsync(movement.Workshop);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsApproveWithEdit", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> Order(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementOrderModel
            {
                SKU = await movement.GetSKUAsync(),
                Movement = movement,
                Price = movement.Price,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(Guid id, InventoryMovementOrderModel model)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await movement.TryChangePriceAsync(model.Price);
                await movement.OrderAsync();

                await sku.TryProcessMovementsChangesAsync(movement.Workshop);
                await this.GenericFlagsService.ClearFlagsByPrefixAsync($"/Inventory/{sku.Id}/Movements/SelectedForOrder/");
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsOrder", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> OrderWithEdit(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementOrderWithEditModel
            {
                SKU = await movement.GetSKUAsync(),
                Movement = movement,
                Quantity = movement.Quantity,
                Price = movement.Price,
                Comment = movement.Comment,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderWithEdit(Guid id, InventoryMovementOrderWithEditModel model)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await movement.TryChangePriceAsync(model.Price);
                await movement.OrderWithEditAsync(model.Quantity, model.Comment);

                await sku.TryProcessMovementsChangesAsync(movement.Workshop);
                await this.GenericFlagsService.ClearFlagsByPrefixAsync($"/Inventory/{sku.Id}/Movements/SelectedForOrder/");
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsOrderWithEdit", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> OrderMissing(Guid sku, Guid workshop)
        {
            var selectedWorkshop = await this.WorkshopManager.GetActiveByIdAsync(workshop);
            if (selectedWorkshop == null)
            {
                return this.NotFound();
            }

            var inventorySKU = await this.InventorySKUManager.GetByIdAsync(sku);
            if (inventorySKU == null)
            {
                return this.NotFound();
            }

            var missingQuantity = await inventorySKU.GetMissingQuantity(selectedWorkshop);
            if (missingQuantity.LessThanOrEqual(0, InventorySKU.QuantityPrecision))
            {
                return this.NotFound();
            }

            var model = new InventoryMovementOrderMissingModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                MissingQuantity = missingQuantity,
                Price = inventorySKU.AveragePrice,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderMissing(Guid sku, Guid workshop, InventoryMovementOrderMissingModel model)
        {
            var selectedWorkshop = await this.WorkshopManager.GetActiveByIdAsync(workshop);
            if (selectedWorkshop == null)
            {
                return this.NotFound();
            }

            var inventorySKU = await this.InventorySKUManager.GetByIdAsync(sku);
            if (inventorySKU == null)
            {
                return this.NotFound();
            }

            var missingQuantity = await inventorySKU.GetMissingQuantity(selectedWorkshop);
            if (missingQuantity.LessThanOrEqual(0, InventorySKU.QuantityPrecision))
            {
                return this.NotFound();
            }

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await inventorySKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(selectedWorkshop, missingQuantity, builder => builder
                    .WithStatus(InventoryMovementStatus.Ordered)
                    .WithPrice(model.Price));

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop);
                await this.GenericFlagsService.ClearFlagsByPrefixAsync($"/Inventory/{inventorySKU.Id}/Movements/SelectedForOrder/");
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsOrderMissing", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> OrderMissingWithEdit(Guid sku, Guid workshop)
        {
            var selectedWorkshop = await this.WorkshopManager.GetActiveByIdAsync(workshop);
            if (selectedWorkshop == null)
            {
                return this.NotFound();
            }

            var inventorySKU = await this.InventorySKUManager.GetByIdAsync(sku);
            if (inventorySKU == null)
            {
                return this.NotFound();
            }

            var missingQuantity = await inventorySKU.GetMissingQuantity(selectedWorkshop);
            if (missingQuantity.LessThanOrEqual(0, InventorySKU.QuantityPrecision))
            {
                return this.NotFound();
            }

            var model = new InventoryMovementOrderMissingWithEditModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                MissingQuantity = missingQuantity,
                Quantity = missingQuantity,
                Price = inventorySKU.AveragePrice,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderMissingWithEdit(Guid sku, Guid workshop, InventoryMovementOrderMissingWithEditModel model)
        {
            var selectedWorkshop = await this.WorkshopManager.GetActiveByIdAsync(workshop);
            if (selectedWorkshop == null)
            {
                return this.NotFound();
            }

            var inventorySKU = await this.InventorySKUManager.GetByIdAsync(sku);
            if (inventorySKU == null)
            {
                return this.NotFound();
            }

            var missingQuantity = await inventorySKU.GetMissingQuantity(selectedWorkshop);
            if (missingQuantity.LessThanOrEqual(0, InventorySKU.QuantityPrecision))
            {
                return this.NotFound();
            }

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await inventorySKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(selectedWorkshop, model.Quantity, builder => builder
                    .WithStatus(InventoryMovementStatus.Ordered)
                    .WithPrice(model.Price)
                    .WithComment(model.Comment));

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop);
                await this.GenericFlagsService.ClearFlagsByPrefixAsync($"/Inventory/{inventorySKU.Id}/Movements/SelectedForOrder/");
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsOrderMissingWithEdit", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> Verify(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementVerifyModel
            {
                SKU = await movement.GetSKUAsync(),
                Movement = movement,
                Price = movement.Price,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify(Guid id, InventoryMovementVerifyModel model)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await movement.TryChangePriceAsync(model.Price);
                await movement.VerifyAsync();

                var requiredPart = await movement.GetHandpieceRequiredPartAsync();
                await sku.TryProcessMovementsChangesAsync(
                    movement.Workshop,
                    async repair => requiredPart != null && (await repair.GetHandpieceRequiredPartAsync())?.Id == requiredPart.Id ? 100 : 0);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsVerify", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> VerifyWithEdit(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementVerifyWithEditModel
            {
                SKU = await movement.GetSKUAsync(),
                Movement = movement,
                Quantity = movement.Quantity,
                Price = movement.Price,
                Comment = movement.Comment,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyWithEdit(Guid id, InventoryMovementVerifyWithEditModel model)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await movement.TryChangePriceAsync(model.Price);
                await movement.VerifyWithEditAsync(model.Quantity, model.Comment);

                var requiredPart = await movement.GetHandpieceRequiredPartAsync();
                await sku.TryProcessMovementsChangesAsync(
                    movement.Workshop,
                    async repair => requiredPart != null && (await repair.GetHandpieceRequiredPartAsync())?.Id == requiredPart.Id ? 100 : 0);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsVerifyWithEdit", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> Cancel(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementCancelModel
            {
                SKU = await movement.GetSKUAsync(),
                Movement = movement,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id, InventoryMovementCancelModel model)
        {
            var movement = await this.InventoryMovementManager.GetById<IInventoryOrderMovement>(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                await movement.CancelAsync();
                await sku.TryProcessMovementsChangesAsync(movement.Workshop);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsVerify", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> History(Guid id)
        {
            var movement = await this.InventoryMovementManager.GetById(id);
            if (movement == null)
            {
                return this.NotFound();
            }

            var sku = await movement.GetSKUAsync();
            var model = new InventoryMovementHistoryViewModel
            {
                SKU = sku,
                Movement = movement,
            };

            return this.View(model);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadChanges(Guid movement, [DataSourceRequest] DataSourceRequest request)
        {
            var movementObj = await this.InventoryMovementManager.GetById(movement);
            if (movementObj == null)
            {
                return this.NotFound();
            }

            var changes = await movementObj.GetChangesAsync();
            var result = await changes.Select(x => new InventoryMovementHistoryChangeReadModel
            {
                Id = x.Id,
                ChangedOn = x.ChangedOn,
                ChangedBy = x.ChangedById,
                ChangedByName = x.ChangedBy.GetFullName(),
                Action = x.Action,
                ActionName = x.Action.ToString(),
                OldStatus = x.OldStatus,
                OldStatusName = x.OldStatus?.ToDisplayName(),
                OldQuantity = x.OldQuantity,
                OldPrice = x.OldPrice,
                OldComment = x.OldComment,
                NewStatus = x.NewStatus,
                NewStatusName = x.NewStatus?.ToDisplayName(),
                NewQuantity = x.NewQuantity,
                NewPrice = x.NewPrice,
                NewComment = x.NewComment,
            }).ToDataSourceResultAsync(request);

            return this.Json(result);
        }

        private async Task<IActionResult> ReadCustom(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request, Expression<Func<InventoryMovementReadModel, Boolean>> filter = null, Boolean includeMissingQuantity = false)
        {
            var employeeAccess = await this.UserEntityResolver.GetEmployeeAccessAsync();
            var requestedWorkshop = workshop == null ? null : await this.WorkshopManager.GetByIdAsync(workshop.Value);
            if (requestedWorkshop == null && workshop.HasValue)
            {
                return this.NotFound();
            }

            if (requestedWorkshop != null && !employeeAccess.Workshops[requestedWorkshop.Id].HasInventoryPermission(InventoryMovementPermissions.MovementRead))
            {
                return this.NotFound();
            }

            var requestedSKU = sku.HasValue ? await this.InventorySKUManager.GetByIdAsync(sku.Value) : null;
            if (sku.HasValue && requestedSKU == null)
            {
                return this.NotFound();
            }

            var filteredSKU = requestedSKU != null ? await requestedSKU.GetDescendantsAndSelfAsync() : null;
            var sql = @"select
  im.[Id],
  cast(im.[CreatedOn] as date) as [Date],
  im.[CreatedOn] as [DateTime],
  w.[Id] as [WorkshopId],
  w.[Name] as [WorkshopName],
  sku.[Id] as [SKUId],
  sku.[Name] as [SKUName],
  t.[OrderNo] as [SKUTypeOrderNo],
  sh.[HierarchyOrderNo] as [SKUOrderNo],
  im.[Quantity],
  im.[QuantityAbsolute],
  coalesce(sq.[ShelfQuantity], 0) as [ShelfQuantity],
  im.[Type],
  im.[Status],
  lh.[HandpieceId],
  lh.[HandpieceNumber],
  lh.[HandpieceStatus],
  lh.[HandpiecePartsComment],
  lh.[ClientId],
  lh.[ClientFullName],
  im.[Comment] as [MovementComment],
  sku.[AveragePrice],
  im.[Price] as [MovementPrice],
  coalesce(im.[Price], sku.[AveragePrice]) as [FinalPrice],
  coalesce(im.[Price], sku.[AveragePrice]) * im.[Quantity] as [TotalPrice],
  coalesce(im.[Price], sku.[AveragePrice]) * im.[QuantityAbsolute] as [TotalPriceAbsolute]
from [InventoryMovements] im
inner join [Workshops] w on im.[WorkshopId] = w.[Id]
inner join [InventorySKUs] sku on sku.[Id] = im.[SKUId]
inner join [InventorySKUsHierarchy] sh on sku.[Id] = sh.[Id]
inner join [InventorySKUTypes] t on sku.[TypeId] = t.[Id]
left join [InventoryMovementLinkedHandpieces] lh on lh.[Id] = im.[Id]
left join [InventorySKUsQuantity] sq on sku.[Id] = sq.[Id]
where sku.[DeletionStatus] = 0";

            if (includeMissingQuantity)
            {
                sql += @"
union all
select
  sku.[Id],
  null as [Date],
  null as [DateTime],
  w.[Id] as [WorkshopId],
  w.[Name] as [WorkshopName],
  sku.[Id] as [SKUId],
  sku.[Name] as [SKUName],
  t.[OrderNo] as [SKUTypeOrderNo],
  sh.[HierarchyOrderNo] as [SKUOrderNo],
  mq.[MissingQuantity] as [Quantity],
  mq.[MissingQuantity] as [QuantityAbsolute],
  coalesce(sq.[ShelfQuantity], 0) as [ShelfQuantity],
  500 as [Type],
  20 as [Status],
  null as [HandpieceId],
  null as [HandpieceNumber],
  null as [HandpieceStatus],
  null as [HandpiecePartsComment],
  null as [ClientId],
  null as [ClientFullName],
  null as [MovementComment],
  sku.[AveragePrice],
  null as [MovementPrice],
  sku.[AveragePrice] as [FinalPrice],
  sku.[AveragePrice] * mq.[MissingQuantity] as [TotalPrice],
  sku.[AveragePrice] * mq.[MissingQuantity] as [TotalPriceAbsolute]
from [InventorySKUs] sku
cross join [Workshops] w
inner join [InventorySKUsHierarchy] sh on sku.[Id] = sh.[Id]
inner join [InventorySKUTypes] t on sku.[TypeId] = t.[Id]
inner join [InventorySKUsWorkshopMissingQuantity] mq on mq.[Id] = sku.[Id] and mq.[WorkshopId] = w.[Id]
left join [InventorySKUsWorkshopQuantity] sq on sku.[Id] = sq.[Id] and sq.[WorkshopId] = w.[Id]
where sku.[NodeType] = 0 and sku.[DeletionStatus] = 0";
            }

            var query = this.ControllerServices.ServiceProvider.GetRequiredService<DbContext>().Set<InventoryMovementReadModel>().FromSqlRaw(sql);
            if (requestedWorkshop != null)
            {
                query = query.Where(x => x.WorkshopId == requestedWorkshop.Id);
            }
            else
            {
                var accessibleWorkshops = employeeAccess.Workshops.GetAvailable(x => x.HasInventoryPermission(InventoryMovementPermissions.MovementRead));
                query = query.Where(x => accessibleWorkshops.Contains(x.WorkshopId));
            }

            if (filteredSKU != null && filteredSKU.Count > 0)
            {
                var ids = filteredSKU.Select(x => x.Id).ToArray();
                query = query.Where(x => ids.Contains(x.SKUId));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            request.RemapSortFields(new Dictionary<String, Action<SortDescriptor>> { ["Date"] = descriptor => descriptor.Member = "DateTime" });
            var result = await query.ToDataSourceResultAsync(request);
            return this.Json(result);
        }
    }
}
