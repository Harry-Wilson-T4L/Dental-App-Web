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
        public Task<IActionResult> ReadGroupAll(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadGroupCustom(sku, workshop, request, filter: null);
        }

        [AjaxPost]
        public Task<IActionResult> ReadGroupRequested(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadGroupCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && x.Status == InventoryMovementStatus.Requested);
        }

        [AjaxPost]
        public Task<IActionResult> ReadGroupApproved(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadGroupCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && x.Status == InventoryMovementStatus.Approved);
        }

        [AjaxPost]
        public Task<IActionResult> ReadGroupApprovedAndMissing(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadGroupCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && x.Status == InventoryMovementStatus.Approved, includeMissing: true);
        }

        [AjaxPost]
        public Task<IActionResult> ReadGroupOrdered(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadGroupCustom(sku, workshop, request, filter: x => x.Type == InventoryMovementType.Order && x.Status == InventoryMovementStatus.Ordered);
        }

        [AjaxPost]
        public Task<IActionResult> ReadGroupComplete(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            return this.ReadGroupCustom(sku, workshop, request, filter: x => x.Type != InventoryMovementType.Repair && x.Status == InventoryMovementStatus.Completed);
        }

        public async Task<IActionResult> GroupApprove(Guid sku, Guid workshop)
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

            var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Requested);
            var model = new InventoryMovementGroupApproveModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                Movements = movements,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupApprove(Guid sku, Guid workshop, InventoryMovementGroupApproveModel model)
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

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Requested);
                foreach (var movement in movements)
                {
                    if (model.SetPrice)
                    {
                        await movement.TryChangePriceAsync(model.Price);
                    }

                    await movement.ApproveAsync();
                }

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsGroupApprove", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> GroupApproveWithEdit(Guid sku, Guid workshop)
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

            var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Requested);
            var model = new InventoryMovementGroupApproveWithEditModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                Movements = movements,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupApproveWithEdit(Guid sku, Guid workshop, InventoryMovementGroupApproveWithEditModel model)
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

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Requested);
                foreach (var result in model.Result.Where(x => x.BulkEditStatus == InventoryMovementBulkEditStatus.Normal))
                {
                    if (result.Id == Guid.Empty)
                    {
                        if (result.Quantity.HasValue && result.Quantity.Value.GreaterThan(0m, InventorySKU.QuantityPrecision))
                        {
                            await inventorySKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                                selectedWorkshop,
                                result.Quantity.Value,
                                order => order.WithStatus(InventoryMovementStatus.Approved).WithPrice(result.Price).WithComment(model.Comment));
                        }
                    }
                    else
                    {
                        var matching = movements.FirstOrDefault(x => x.Id == result.Id);
                        if (matching != null && result.Quantity.HasValue)
                        {
                            if (result.Quantity.Value.GreaterThan(0m, InventorySKU.QuantityPrecision))
                            {
                                await matching.TryChangePriceAsync(result.Price);
                                await matching.ApproveWithEditAsync(result.Quantity.Value, model.Comment ?? matching.Comment);
                            }
                            else if (result.Quantity.Value.Equals(0m, InventorySKU.QuantityPrecision))
                            {
                                await matching.CancelAsync();
                            }
                        }
                    }
                }

                foreach (var result in model.Result.Where(x => x.BulkEditStatus == InventoryMovementBulkEditStatus.Cancelled))
                {
                    var matching = movements.FirstOrDefault(x => x.Id == result.Id);
                    if (matching != null)
                    {
                        await matching.CancelAsync();
                    }
                }

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsGroupApproveWithEdit", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> GroupOrder(Guid sku, Guid workshop)
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

            var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Approved);
            var model = new InventoryMovementGroupOrderModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                Movements = movements,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupOrder(Guid sku, Guid workshop, InventoryMovementGroupOrderModel model)
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

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Approved);
                foreach (var movement in movements)
                {
                    if (model.SetPrice)
                    {
                        await movement.TryChangePriceAsync(model.Price);
                    }

                    await movement.OrderAsync();
                }

                var missingQuantity = await inventorySKU.GetMissingQuantity(selectedWorkshop);
                if (missingQuantity.GreaterThan(0, InventorySKU.QuantityPrecision))
                {
                    await inventorySKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(selectedWorkshop, missingQuantity, builder =>
                    {
                        builder.WithStatus(InventoryMovementStatus.Ordered);
                        if (model.SetPrice)
                        {
                            builder.WithPrice(model.Price);
                        }
                    });
                }

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop);
                await this.GenericFlagsService.ClearFlagsByPrefixAsync($"/Inventory/{inventorySKU.Id}/Movements/SelectedForOrder/");
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsGroupOrder", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> GroupOrderWithEdit(Guid sku, Guid workshop)
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

            var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Approved);
            var model = new InventoryMovementGroupOrderWithEditModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                Movements = movements,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupOrderWithEdit(Guid sku, Guid workshop, InventoryMovementGroupOrderWithEditModel model)
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

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Approved);
                foreach (var result in model.Result.Where(x => x.BulkEditStatus == InventoryMovementBulkEditStatus.Normal))
                {
                    if (result.Id == Guid.Empty)
                    {
                        if (result.Quantity.HasValue && result.Quantity.Value.GreaterThan(0m, InventorySKU.QuantityPrecision))
                        {
                            await inventorySKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                                selectedWorkshop,
                                result.Quantity.Value,
                                order => order.WithStatus(InventoryMovementStatus.Ordered).WithPrice(result.Price).WithComment(model.Comment));
                        }
                    }
                    else
                    {
                        var matching = movements.FirstOrDefault(x => x.Id == result.Id);
                        if (matching != null && result.Quantity.HasValue)
                        {
                            if (result.Quantity.Value.GreaterThan(0m, InventorySKU.QuantityPrecision))
                            {
                                await matching.TryChangePriceAsync(result.Price);
                                await matching.OrderWithEditAsync(result.Quantity.Value, model.Comment ?? matching.Comment);
                            }
                            else if (result.Quantity.Value.Equals(0m, InventorySKU.QuantityPrecision))
                            {
                                await matching.CancelAsync();
                            }
                        }
                    }
                }

                foreach (var result in model.Result.Where(x => x.BulkEditStatus == InventoryMovementBulkEditStatus.Cancelled))
                {
                    var matching = movements.FirstOrDefault(x => x.Id == result.Id);
                    if (matching != null)
                    {
                        await matching.CancelAsync();
                    }
                }

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop);
                await this.GenericFlagsService.ClearFlagsByPrefixAsync($"/Inventory/{inventorySKU.Id}/Movements/SelectedForOrder/");
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsGroupOrderWithEdit", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> GroupVerify(Guid sku, Guid workshop)
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

            var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Ordered);
            var model = new InventoryMovementGroupVerifyModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                Movements = movements,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupVerify(Guid sku, Guid workshop, InventoryMovementGroupVerifyModel model)
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

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Ordered);
                var affectedPartsIds = new List<Guid>();

                foreach (var movement in movements)
                {
                    if (model.SetPrice)
                    {
                        await movement.TryChangePriceAsync(model.Price);
                    }

                    await movement.VerifyAsync();
                    var part = await movement.GetHandpieceRequiredPartAsync();
                    if (part != null)
                    {
                        affectedPartsIds.Add(part.Id);
                    }
                }

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop, async repair =>
                {
                    var repairPart = await repair.GetHandpieceRequiredPartAsync();
                    return repairPart != null && affectedPartsIds.Contains(repairPart.Id) ? 100 : 0;
                });

                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsGroupVerify", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> GroupVerifyWithEdit(Guid sku, Guid workshop)
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

            var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Ordered);
            var model = new InventoryMovementGroupVerifyWithEditModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                Movements = movements,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupVerifyWithEdit(Guid sku, Guid workshop, InventoryMovementGroupVerifyWithEditModel model)
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

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, InventoryMovementStatus.Ordered);
                var affectedPartsIds = new List<Guid>();

                foreach (var result in model.Result.Where(x => x.BulkEditStatus == InventoryMovementBulkEditStatus.Normal))
                {
                    if (result.Id == Guid.Empty)
                    {
                        if (result.Quantity.HasValue && result.Quantity.Value.GreaterThan(0m, InventorySKU.QuantityPrecision))
                        {
                            await inventorySKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                                selectedWorkshop,
                                result.Quantity.Value,
                                order => order.WithStatus(InventoryMovementStatus.Completed).WithPrice(result.Price).WithComment(model.Comment));
                        }
                    }
                    else
                    {
                        var matching = movements.FirstOrDefault(x => x.Id == result.Id);
                        if (matching != null && result.Quantity.HasValue)
                        {
                            if (result.Quantity.Value.GreaterThan(0m, InventorySKU.QuantityPrecision))
                            {
                                await matching.TryChangePriceAsync(result.Price);
                                await matching.VerifyWithEditAsync(result.Quantity.Value, model.Comment ?? matching.Comment);

                                var part = await matching.GetHandpieceRequiredPartAsync();
                                if (part != null)
                                {
                                    affectedPartsIds.Add(part.Id);
                                }
                            }
                            else if (result.Quantity.Value.Equals(0m, InventorySKU.QuantityPrecision))
                            {
                                await matching.CancelAsync();
                            }
                        }
                    }
                }

                foreach (var result in model.Result.Where(x => x.BulkEditStatus == InventoryMovementBulkEditStatus.Cancelled))
                {
                    var matching = movements.FirstOrDefault(x => x.Id == result.Id);
                    if (matching != null)
                    {
                        await matching.CancelAsync();
                    }
                }

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop, async repair =>
                {
                    var repairPart = await repair.GetHandpieceRequiredPartAsync();
                    return repairPart != null && affectedPartsIds.Contains(repairPart.Id) ? 100 : 0;
                });

                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsGroupVerifyWithEdit", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> GroupCancel(Guid sku, Guid workshop, InventoryMovementStatus status)
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

            var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, status);
            var model = new InventoryMovementGroupCancelModel
            {
                Workshop = selectedWorkshop,
                SKU = inventorySKU,
                Status = status,
                Movements = movements,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupCancel(Guid sku, Guid workshop, InventoryMovementStatus status, InventoryMovementGroupCancelModel model)
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

            await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
            {
                var movements = await inventorySKU.Movements.GetMovementsByTypeAndStatusAsync<IInventoryOrderMovement>(selectedWorkshop, status);
                foreach (var movement in movements)
                {
                    await movement.CancelAsync();
                }

                await inventorySKU.TryProcessMovementsChangesAsync(selectedWorkshop);
                await transaction.CommitAsync();
            }

            return await this.HybridFormResultAsync("InventoryMovementsGroupCancel", this.RedirectToAction("Index", "InventoryMovements"));
        }

        private async Task<IActionResult> ReadGroupCustom(Guid? sku, Guid? workshop, [DataSourceRequest] DataSourceRequest request, Expression<Func<InventoryMovementGroupReadModel, Boolean>> filter = null, Boolean includeMissing = false)
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
  cast(sku.[Id] as nvarchar(36)) + ':' + cast(im.[Type] as nvarchar(10)) + ':' + cast(im.[Status] as nvarchar(10)) as [Id],
  im.[WorkshopId],
  w.[Name] as [WorkshopName],
  sku.[Id] as [SKUId],
  sku.[Name] as [SKUName],
  t.[OrderNo] as [SKUTypeOrderNo],
  sh.[HierarchyOrderNo] as [SKUOrderNo],
  im.[Type],
  im.[Status],
  min(cast(im.[CreatedOn] as date)) as [MinDate],
  min(im.[CreatedOn]) as [MinDateTime],
  max(cast(im.[CreatedOn] as date)) as [MaxDate],
  max(im.[CreatedOn]) as [MaxDateTime],
  sum(im.[Quantity]) as [Quantity],
  sum(im.[QuantityAbsolute]) as [QuantityAbsolute],
  sum(case when im.[Price] is not null then im.[QuantityAbsolute] else 0 end) as [QuantityAbsoluteWithPrice],
  max(coalesce(sq.[ShelfQuantity], 0)) as [ShelfQuantity],
  max(coalesce(sq.[OrderedQuantity], 0)) as [OrderedQuantity],
  max(coalesce(sq.[RepairQuantity], 0) - coalesce(sq.[TrayQuantity], 0)) as [MissingQuantity],
  count(distinct lh.[HandpieceId]) as [HandpiecesCount],
  case when sum(im.[QuantityAbsolute]) > 0 then sum(coalesce(im.[Price], sku.[AveragePrice]) * im.[QuantityAbsolute]) / sum(im.[QuantityAbsolute]) else 0 end as [AverageFinalPrice],
  sum(coalesce(im.[Price], sku.[AveragePrice]) * im.[Quantity]) as [TotalPrice],
  sum(coalesce(im.[Price], sku.[AveragePrice]) * im.[QuantityAbsolute]) as [TotalPriceAbsolute]";

            if (includeMissing)
            {
                sql += @"
from (
  select
    im.[Id],
    im.[WorkshopId],
	im.[SKUId],
    im.[Type],
	im.[Status],
	im.[CreatedOn],
	im.[Quantity],
	im.[QuantityAbsolute],
	im.[Price]
  from [InventoryMovements] im
  union all 
  select
    sku.[Id],
    mq.[WorkshopId],
	sku.[Id],
	1, -- So they would be grouped with other Order groups
	20,
	null,
	mq.[MissingQuantity],
	mq.[MissingQuantity],
	null
  from [InventorySKUsWorkshopMissingQuantity] mq inner join [InventorySKUs] sku on mq.[Id] = sku.[Id]
) im";
            }
            else
            {
                sql += @"
from [InventoryMovements] im";
            }

            sql += @"
inner join [Workshops] w on w.[Id] = im.[WorkshopId]
inner join [InventorySKUs] sku on sku.[Id] = im.[SKUId]
inner join [InventorySKUsHierarchy] sh on sku.[Id] = sh.[Id]
inner join [InventorySKUTypes] t on sku.[TypeId] = t.[Id]
left join [InventoryMovementLinkedHandpieces] lh on lh.[Id] = im.[Id]
left join [InventorySKUsWorkshopQuantity] sq on sku.[Id] = sq.[Id] and sq.[WorkshopId] = w.[Id]
where sku.[DeletionStatus] = 0
group by im.[WorkshopId], w.[Name], sku.[Id], sku.[Name], t.[OrderNo], sh.[HierarchyOrderNo], im.[Type], im.[Status]";

            var query = this.ControllerServices.ServiceProvider.GetRequiredService<DbContext>().Set<InventoryMovementGroupReadModel>().FromSqlRaw(sql);
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

            request.RemapSortFields(new Dictionary<String, Action<SortDescriptor>>
            {
                ["MinDate"] = descriptor => descriptor.Member = "MinDateTime",
                ["MaxDate"] = descriptor => descriptor.Member = "MaxDateTime",
            });

            var result = await query.ToDataSourceResultAsync(request);
            return this.Json(result);
        }
    }
}
