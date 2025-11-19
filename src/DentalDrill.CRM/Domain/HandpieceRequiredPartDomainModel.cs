using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class HandpieceRequiredPartDomainModel : IHandpieceRequiredPart
    {
        private readonly IRepository repository;

        private readonly IHandpiece handpiece;
        private readonly IInventorySKU sku;

        private HandpieceRequiredPart entity;
        private Boolean deleted;

        public HandpieceRequiredPartDomainModel(HandpieceRequiredPart entity, IHandpiece handpiece, IInventorySKU sku, IRepository repository)
        {
            this.repository = repository;

            this.entity = entity;
            this.handpiece = handpiece;
            this.sku = sku;
        }

        public Guid Id => this.entity.Id;

        public IHandpiece Handpiece => this.handpiece;

        public IInventorySKU SKU => this.sku;

        public Decimal Quantity => this.entity.Quantity;

        public Boolean Deleted => this.deleted;

        public async Task RefreshAsync()
        {
            if (this.deleted)
            {
                throw new InvalidOperationException("Entity was deleted and cannot be refreshed");
            }

            var newEntity = await this.repository.QueryWithoutTracking<HandpieceRequiredPart>()
                .SingleOrDefaultAsync(x => x.Id == this.entity.Id);
            if (newEntity == null)
            {
                throw new InvalidOperationException("Entity is missing after refresh");
            }

            if (newEntity.HandpieceId != this.handpiece.Id)
            {
                throw new InvalidOperationException("Handpiece of the required part has changed unexpectedly");
            }

            if (newEntity.SKUId != this.sku.Id)
            {
                throw new InvalidOperationException("SKU of the required part has changed unexpectedly");
            }

            this.entity = newEntity;
        }

        public async Task<Decimal> GetAllocatedQuantity()
        {
            var quantity = 0m;
            var repairMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(this.Id);
            if (repairMovement.Status == InventoryMovementStatus.Allocated || repairMovement.Status == InventoryMovementStatus.Completed)
            {
                quantity += repairMovement.QuantityAbsolute;
            }

            var fragmentMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairFragmentMovement>(this.Id);
            if (fragmentMovement != null)
            {
                quantity += fragmentMovement.QuantityAbsolute;
            }

            return quantity;
        }

        public async Task<Decimal> GetMissingQuantity()
        {
            var required = this.Quantity;
            var allocated = await this.GetAllocatedQuantity();

            if (required.GreaterThan(allocated, InventorySKU.QuantityPrecision))
            {
                return required - allocated;
            }

            return 0m;
        }

        public async Task<Decimal> IncreaseAllocationAsync(Decimal quantityToAllocate)
        {
            if (quantityToAllocate.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                // Nothing to allocate
                return 0m;
            }

            var repairMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(this.Id);
            if (repairMovement.Status.IsNotOneOf(InventoryMovementStatus.Allocated, InventoryMovementStatus.Waiting))
            {
                throw new InvalidOperationException("Invalid repair movement status");
            }

            var required = this.Quantity;
            var allocated = await this.GetAllocatedQuantity();

            if (allocated.GreaterThanOrEqual(required, InventorySKU.QuantityPrecision))
            {
                // Already fully allocated
                return 0m;
            }

            var missing = required - allocated;
            if (quantityToAllocate.GreaterThanOrEqual(missing, InventorySKU.QuantityPrecision))
            {
                // Allocate fully
                await repairMovement.AllocateAsync();

                var fragmentMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairFragmentMovement>(this.Id);
                if (fragmentMovement != null)
                {
                    await fragmentMovement.DeleteAsync();
                }

                return missing;
            }
            else
            {
                // Increase partial allocation
                var fragmentMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairFragmentMovement>(this.Id);
                if (fragmentMovement != null)
                {
                    await fragmentMovement.UpdateQuantityAsync(fragmentMovement.QuantityAbsolute + quantityToAllocate);
                }
                else
                {
                    fragmentMovement = await this.sku.Movements.CreateAsync<IInventoryRepairFragmentMovement, InventoryMovementBuilder.RepairFragment>(
                        this.Handpiece.Job.Workshop,
                        quantityToAllocate,
                        b => { });
                    await this.LinkMovementsAsync(new IInventoryMovement[] { fragmentMovement });
                }

                return quantityToAllocate;
            }
        }

        public async Task<Decimal> DecreaseAllocationAsync(Decimal quantityToDeallocate)
        {
            if (quantityToDeallocate.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                // Nothing to allocate
                return 0m;
            }

            var repairMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(this.Id);
            if (repairMovement.Status.IsNotOneOf(InventoryMovementStatus.Allocated, InventoryMovementStatus.Waiting))
            {
                throw new InvalidOperationException("Invalid repair movement status");
            }

            var allocated = await this.GetAllocatedQuantity();
            if (quantityToDeallocate.GreaterThanOrEqual(allocated, InventorySKU.QuantityPrecision))
            {
                // Deallocate everything that is possible to deallocate
                await repairMovement.DeallocateAsync();

                var fragmentMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairFragmentMovement>(this.Id);
                if (fragmentMovement != null)
                {
                    await fragmentMovement.DeleteAsync();
                }

                return allocated;
            }
            else
            {
                var remaining = allocated - quantityToDeallocate;
                if (repairMovement.Status == InventoryMovementStatus.Allocated)
                {
                    await repairMovement.DeallocateAsync();
                }

                var fragmentMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairFragmentMovement>(this.Id);
                if (fragmentMovement != null)
                {
                    await fragmentMovement.UpdateQuantityAsync(remaining);
                }
                else
                {
                    fragmentMovement = await this.sku.Movements.CreateAsync<IInventoryRepairFragmentMovement, InventoryMovementBuilder.RepairFragment>(
                        this.Handpiece.Job.Workshop,
                        remaining,
                        b => { });
                    await this.LinkMovementsAsync(new IInventoryMovement[] { fragmentMovement });
                }

                return quantityToDeallocate;
            }
        }

        public async Task LinkMovementsAsync(IReadOnlyList<IInventoryMovement> movements)
        {
            foreach (var movement in movements)
            {
                var requiredPartMovement = new HandpieceRequiredPartMovement
                {
                    RequiredPartId = this.Id,
                    MovementId = movement.Id,
                };

                await this.repository.InsertAsync(requiredPartMovement);
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(Decimal newQuantity)
        {
            if (newQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                throw new InvalidOperationException("New quantity must be a positive number");
            }

            if (newQuantity.Equals(this.Quantity, InventorySKU.QuantityPrecision))
            {
                return;
            }

            var availableQuantity = await this.sku.GetAvailableQuantity(this.Handpiece.Job.Workshop);

            var currentQuantity = this.Quantity;
            if (newQuantity.GreaterThan(currentQuantity, InventorySKU.QuantityPrecision))
            {
                // Updating Quantity
                await this.UpdateQuantityInternalAsync(newQuantity);

                // Updating Repair Movement
                var repairAllocated = false;
                var repairMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(this.Id);
                if (repairMovement == null)
                {
                    var status = availableQuantity.GreaterThanOrEqual(newQuantity, InventorySKU.QuantityPrecision)
                        ? InventoryMovementStatus.Allocated
                        : InventoryMovementStatus.Waiting;

                    repairMovement = await this.sku.Movements.CreateAsync<IInventoryRepairMovement, InventoryMovementBuilder.Repair>(
                        this.Handpiece.Job.Workshop,
                        newQuantity,
                        builder => builder.WithStatus(status));
                    await this.LinkMovementsAsync(new IInventoryMovement[] { repairMovement });
                }
                else if (repairMovement.Status == InventoryMovementStatus.Waiting)
                {
                    await repairMovement.UpdateQuantityAsync(newQuantity);
                }
                else if (repairMovement.Status == InventoryMovementStatus.Allocated)
                {
                    if ((newQuantity - currentQuantity).GreaterThan(availableQuantity, InventorySKU.QuantityPrecision))
                    {
                        await repairMovement.DeallocateAsync();
                        var fragmentMovement = await this.sku.Movements.CreateAsync<IInventoryRepairFragmentMovement, InventoryMovementBuilder.RepairFragment>(
                            this.Handpiece.Job.Workshop,
                            currentQuantity,
                            b => { });
                        await this.LinkMovementsAsync(new IInventoryMovement[] { fragmentMovement });
                    }
                    else
                    {
                        repairAllocated = true;
                    }

                    await repairMovement.UpdateQuantityAsync(newQuantity);
                }
                else if (repairMovement.Status == InventoryMovementStatus.Completed)
                {
                    if (this.handpiece.Status != HandpieceStatus.SentComplete)
                    {
                        throw new NotSupportedException("RepairMovement is in not supported state");
                    }

                    if ((newQuantity - currentQuantity).GreaterThan(availableQuantity, InventorySKU.QuantityPrecision))
                    {
                        throw new NotSupportedException("Unable to allocate not available SKU in SentComplete state");
                    }

                    await repairMovement.UpdateQuantityAsync(newQuantity);
                }
                else
                {
                    throw new NotSupportedException("RepairMovement is in not supported state");
                }

                ////// Updating Order Movement(s)
                ////var orderMovements = await this.sku.Movements.GetMovementsLinkedToPartAsync<IInventoryOrderMovement>(this.Id);
                ////var orderedQuantity = orderMovements.Select(x => x.QuantityAbsolute).DefaultIfEmpty(0m).Sum();

                ////// Only create order / increase order count if repair is not allocated and only by change delta
                ////if (newQuantity.GreaterThan(orderedQuantity, InventorySKU.QuantityPrecision) && !repairAllocated)
                ////{
                ////    var delta = newQuantity - currentQuantity;

                ////    if ((Int32)this.Handpiece.Status >= (Int32)HandpieceStatus.BeingRepaired)
                ////    {
                ////        var firstApproved = orderMovements.FirstOrDefault(x => x.Status == InventoryMovementStatus.Approved);
                ////        if (firstApproved != null)
                ////        {
                ////            await firstApproved.UpdateRequestedQuantityAsync(firstApproved.QuantityAbsolute + delta);
                ////        }
                ////        else
                ////        {
                ////            var newOrderMovement = await this.sku.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                ////                delta,
                ////                builder => builder.WithStatus(InventoryMovementStatus.Approved));
                ////            await this.LinkMovementsAsync(new IInventoryMovement[] { newOrderMovement });
                ////        }
                ////    }
                ////    else
                ////    {
                ////        var firstRequested = orderMovements.FirstOrDefault(x => x.Status == InventoryMovementStatus.Requested);
                ////        if (firstRequested != null)
                ////        {
                ////            await firstRequested.UpdateRequestedQuantityAsync(firstRequested.QuantityAbsolute + delta);
                ////        }
                ////        else
                ////        {
                ////            var newOrderMovement = await this.sku.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                ////                delta,
                ////                builder => builder.WithStatus(InventoryMovementStatus.Requested));
                ////            await this.LinkMovementsAsync(new IInventoryMovement[] { newOrderMovement });
                ////        }
                ////    }
                ////}
            }
            else
            {
                // Updating Quantity
                await this.UpdateQuantityInternalAsync(newQuantity);

                // Updating Repair Movement
                var repairMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(this.Id);
                if (repairMovement == null)
                {
                    var status = availableQuantity.GreaterThanOrEqual(newQuantity, InventorySKU.QuantityPrecision)
                        ? InventoryMovementStatus.Allocated
                        : InventoryMovementStatus.Waiting;

                    repairMovement = await this.sku.Movements.CreateAsync<IInventoryRepairMovement, InventoryMovementBuilder.Repair>(
                        this.Handpiece.Job.Workshop,
                        newQuantity,
                        builder => builder.WithStatus(status));
                    await this.LinkMovementsAsync(new IInventoryMovement[] { repairMovement });
                }
                else if (repairMovement.Status == InventoryMovementStatus.Waiting)
                {
                    await repairMovement.UpdateQuantityAsync(newQuantity);
                    var fragmentMovement = await this.sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairFragmentMovement>(this.Id);
                    if ((availableQuantity + (fragmentMovement?.QuantityAbsolute ?? 0m)).GreaterThanOrEqual(newQuantity, InventorySKU.QuantityPrecision))
                    {
                        await repairMovement.AllocateAsync();
                        if (fragmentMovement != null)
                        {
                            await fragmentMovement.DeleteAsync();
                        }
                    }
                    else
                    {
                        if (fragmentMovement != null && fragmentMovement.QuantityAbsolute.GreaterThan(newQuantity, InventorySKU.QuantityPrecision))
                        {
                            await fragmentMovement.UpdateQuantityAsync(newQuantity);
                        }
                    }
                }
                else if (repairMovement.Status == InventoryMovementStatus.Allocated)
                {
                    await repairMovement.UpdateQuantityAsync(newQuantity);
                }
                else if (repairMovement.Status == InventoryMovementStatus.Completed && this.handpiece.Status == HandpieceStatus.SentComplete)
                {
                    await repairMovement.UpdateQuantityAsync(newQuantity);
                }

                ////// Updating Order Movement(s)
                ////var orderMovements = await this.sku.Movements.GetMovementsLinkedToPartAsync<IInventoryOrderMovement>(this.Id);
                ////var orderedQuantity = orderMovements.Select(x => x.QuantityAbsolute).DefaultIfEmpty(0m).Sum();
                ////var fixedQuantity = orderMovements.Where(x => x.Status == InventoryMovementStatus.Ordered || x.Status == InventoryMovementStatus.Completed).Select(x => x.QuantityAbsolute).DefaultIfEmpty(0m).Sum();

                ////var deltaToRemove = currentQuantity - newQuantity;
                ////if (newQuantity.GreaterThan(fixedQuantity, InventorySKU.QuantityPrecision))
                ////{
                ////    var requested = orderMovements.Where(x => x.Status == InventoryMovementStatus.Requested || x.Status == InventoryMovementStatus.Approved).ToList();
                ////    foreach (var requestedMovement in requested)
                ////    {
                ////        if (deltaToRemove.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
                ////        {
                ////            break;
                ////        }
                ////        else if (deltaToRemove.GreaterThanOrEqual(requestedMovement.QuantityAbsolute, InventorySKU.QuantityPrecision))
                ////        {
                ////            deltaToRemove -= requestedMovement.QuantityAbsolute;
                ////            await requestedMovement.RemoveRequestedAsync();
                ////        }
                ////        else
                ////        {
                ////            await requestedMovement.UpdateRequestedQuantityAsync(requestedMovement.QuantityAbsolute - deltaToRemove);
                ////            deltaToRemove = 0;
                ////        }
                ////    }
                ////}
            }
        }

        public async Task<Boolean> CanCompleteAsync()
        {
            var linkedMovements = await this.repository.Query<HandpieceRequiredPartMovement>()
                .Include(x => x.Movement)
                .Where(x => x.RequiredPartId == this.Id)
                .ToListAsync();

            foreach (var linkedMovement in linkedMovements)
            {
                switch (linkedMovement.Movement.Type)
                {
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Allocated:
                        break;
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Waiting:
                        return false;
                    case InventoryMovementType.RepairFragment:
                        return false;
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Cancelled:
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Completed:
                        break;
                    case InventoryMovementType.Order:
                        break;
                    default:
                        throw new InvalidOperationException($"Found linked movement of unexpected type {linkedMovement.Movement.Type}");
                }
            }

            return true;
        }

        public async Task CompleteAsync()
        {
            var linkedMovements = await this.repository.Query<HandpieceRequiredPartMovement>()
                .Include(x => x.Movement)
                .Where(x => x.RequiredPartId == this.Id)
                .ToListAsync();

            foreach (var linkedMovement in linkedMovements)
            {
                switch (linkedMovement.Movement.Type)
                {
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Allocated:
                        linkedMovement.Movement.Status = InventoryMovementStatus.Completed;
                        await this.repository.UpdateAsync(linkedMovement.Movement);
                        break;
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Waiting:
                        throw new InvalidOperationException($"Repair movement has not been allocated");
                    case InventoryMovementType.RepairFragment:
                        throw new InvalidOperationException($"Repair movement has been only partially allocated");
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Cancelled:
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Completed:
                        break;
                    case InventoryMovementType.Order:
                        break;
                    default:
                        throw new InvalidOperationException($"Found linked movement of unexpected type {linkedMovement.Movement.Type} with status {linkedMovement.Movement.Status}");
                }
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task CancelAsync()
        {
            var linkedMovements = await this.repository.Query<HandpieceRequiredPartMovement>()
                .Include(x => x.Movement)
                .Where(x => x.RequiredPartId == this.Id)
                .ToListAsync();

            foreach (var linkedMovement in linkedMovements)
            {
                switch (linkedMovement.Movement.Type)
                {
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Allocated:
                        linkedMovement.Movement.Status = InventoryMovementStatus.Cancelled;
                        await this.repository.UpdateAsync(linkedMovement.Movement);
                        break;
                    case InventoryMovementType.Repair when linkedMovement.Movement.Status == InventoryMovementStatus.Waiting:
                        linkedMovement.Movement.Status = InventoryMovementStatus.Cancelled;
                        await this.repository.UpdateAsync(linkedMovement.Movement);
                        break;
                    case InventoryMovementType.Order when linkedMovement.Movement.Status == InventoryMovementStatus.Requested:
                    case InventoryMovementType.Order when linkedMovement.Movement.Status == InventoryMovementStatus.Approved:
                        linkedMovement.Movement.Status = InventoryMovementStatus.Cancelled;
                        await this.repository.UpdateAsync(linkedMovement.Movement);
                        break;
                    case InventoryMovementType.Repair:
                    case InventoryMovementType.RepairFragment:
                    case InventoryMovementType.Order:
                        break;
                    default:
                        throw new InvalidOperationException($"Found linked movement of unexpected type {linkedMovement.Movement.Type} with status {linkedMovement.Movement.Status}");
                }
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteAsync()
        {
            var updateableEntity = await this.repository.Query<HandpieceRequiredPart>()
                .SingleAsync(x => x.Id == this.Id);
            var linkedMovements = await this.repository.Query<HandpieceRequiredPartMovement>()
                .Include(x => x.Movement)
                .Where(x => x.RequiredPartId == this.Id)
                .ToListAsync();

            foreach (var linkedMovement in linkedMovements)
            {
                switch (linkedMovement.Movement.Type)
                {
                    case InventoryMovementType.Repair:
                        await this.repository.DeleteAsync(linkedMovement);
                        {
                            var movementDomain = await this.sku.Movements.GetMovementByIdAsync<IInventoryRepairMovement>(linkedMovement.MovementId);
                            await movementDomain.DeleteAsync();
                        }

                        break;
                    case InventoryMovementType.Order:
                        await this.repository.DeleteAsync(linkedMovement);
                        if (linkedMovement.Movement.Status == InventoryMovementStatus.Requested || linkedMovement.Movement.Status == InventoryMovementStatus.Approved)
                        {
                            var movementDomain = await this.sku.Movements.GetMovementByIdAsync<IInventoryOrderMovement>(linkedMovement.MovementId);
                            await movementDomain.RemoveRequestedAsync();
                        }

                        break;
                    case InventoryMovementType.RepairFragment:
                        await this.repository.DeleteAsync(linkedMovement);
                        {
                            var movementDomain = await this.sku.Movements.GetMovementByIdAsync<IInventoryRepairFragmentMovement>(linkedMovement.MovementId);
                            await movementDomain.DeleteAsync();
                        }

                        break;
                    default:
                        throw new InvalidOperationException($"Found linked movement of unexpected type {linkedMovement.Movement.Type}");
                }
            }

            await this.repository.DeleteAsync(updateableEntity);
            await this.repository.SaveChangesAsync();

            this.deleted = true;
        }

        private async Task UpdateQuantityInternalAsync(Decimal newQuantity)
        {
            var updateableEntity = await this.repository.Query<HandpieceRequiredPart>().SingleOrDefaultAsync(x => x.Id == this.Id);
            updateableEntity.Quantity = newQuantity;
            await this.repository.UpdateAsync(updateableEntity);
            await this.repository.SaveChangesAsync();
        }
    }
}
