using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class HandpieceDomainModel : IHandpiece
    {
        private readonly Handpiece entity;
        private readonly IJob job;
        private readonly IClientHandpiece clientHandpiece;

        private readonly HandpiecePartsDomainModel parts;

        private readonly IRepository repository;
        private readonly IHandpieceRequiredPartManager requiredPartManager;
        private readonly IChangeTrackingService<Handpiece, HandpieceChange> changeTracker;

        public HandpieceDomainModel(
            Handpiece entity,
            IJob job,
            IClientHandpiece clientHandpiece,
            IRepository repository,
            IHandpieceRequiredPartManager requiredPartManager,
            IChangeTrackingService<Handpiece, HandpieceChange> changeTracker)
        {
            this.entity = entity;
            this.job = job;
            this.clientHandpiece = clientHandpiece;

            this.parts = new HandpiecePartsDomainModel(this);

            this.repository = repository;
            this.requiredPartManager = requiredPartManager;
            this.changeTracker = changeTracker;
        }

        public Guid Id => this.entity.Id;

        public IJob Job => this.job;

        public IClientHandpiece ClientHandpiece => this.clientHandpiece;

        public Handpiece Entity => this.entity;

        public String Number => $"{(this.entity.Job.JobTypeId == "Sale" ? "Sale " : String.Empty)}{this.entity.Job.JobNumber}-{this.entity.JobPosition}";

        public HandpieceStatus Status => this.entity.HandpieceStatus;

        public String Brand => this.entity.Brand;

        public String Model => this.entity.MakeAndModel;

        public String Serial => this.entity.Serial;

        public Int32 Rating => this.entity.Rating;

        public HandpieceSpeed SpeedType => this.entity.SpeedType;

        public Int32? Speed => this.entity.Speed;

        public IHandpiece.IParts Parts => this.parts;

        public String PartsComment => this.entity.PartsComment;

        public DateTime ReceivedOn => this.entity.Job.Received;

        public DateTime? EstimatedOn => this.entity.EstimatedOn;

        public DateTime? ApprovedOn => this.entity.ApprovedOn;

        public DateTime? RepairedOn => this.entity.RepairedOn;

        public DateTime? CompletedOn => this.entity.CompletedOn;

        private class HandpiecePartsDomainModel : IHandpiece.IParts
        {
            private readonly HandpieceDomainModel handpiece;

            public HandpiecePartsDomainModel(HandpieceDomainModel handpiece)
            {
                this.handpiece = handpiece;
            }

            public async Task<HandpiecePartsStockStatus> GetStockStatusAsync()
            {
                var parts = await this.GetRequiredPartsAsync();
                var inStock = 0;
                foreach (var part in parts)
                {
                    if (await part.CanCompleteAsync())
                    {
                        inStock++;
                    }
                }

                if (inStock >= parts.Count)
                {
                    return HandpiecePartsStockStatus.InStock;
                }
                else if (inStock > 0)
                {
                    return HandpiecePartsStockStatus.PartialStock;
                }
                else
                {
                    return HandpiecePartsStockStatus.OutOfStock;
                }
            }

            public async Task UpdateStockStatusAsync(Boolean trackChange = false)
            {
                var updateable = await this.handpiece.repository.Query<Handpiece>().SingleOrDefaultAsync(x => x.Id == this.handpiece.Id);
                if (updateable.HandpieceStatus.IsNotOneOf(HandpieceStatus.SentComplete, HandpieceStatus.Cancelled) &&
                    updateable.PartsVersion == HandpiecePartsVersion.InventorySKUv1)
                {
                    if (trackChange)
                    {
                        var change = await this.handpiece.changeTracker.CaptureEntityForUpdate(updateable);
                        await UpdateAsync();
                        await this.handpiece.repository.SaveChangesAsync();
                        await this.handpiece.changeTracker.TrackModifyEntityAsync(updateable, change, useCurrentRepository: true);
                        await this.handpiece.repository.SaveChangesAsync();
                    }
                    else
                    {
                        await UpdateAsync();
                        await this.handpiece.repository.SaveChangesAsync();
                    }
                }

                async Task UpdateAsync()
                {
                    var oldOutOfStock = updateable.PartsOutOfStock;
                    var newOutOfStock = await this.GetStockStatusAsync();
                    updateable.PartsOutOfStock = newOutOfStock;
                    if (updateable.HandpieceStatus == HandpieceStatus.WaitingForParts &&
                        oldOutOfStock is HandpiecePartsStockStatus.OutOfStock or HandpiecePartsStockStatus.PartialStock &&
                        newOutOfStock is HandpiecePartsStockStatus.InStock)
                    {
                        updateable.HandpieceStatus = HandpieceStatus.BeingRepaired;
                    }
                    else if (updateable.HandpieceStatus == HandpieceStatus.BeingRepaired &&
                             oldOutOfStock is HandpiecePartsStockStatus.InStock &&
                             newOutOfStock is HandpiecePartsStockStatus.OutOfStock or HandpiecePartsStockStatus.PartialStock)
                    {
                        updateable.HandpieceStatus = HandpieceStatus.WaitingForParts;
                    }
                }
            }

            public async Task<Boolean> CanCompleteAsync()
            {
                var parts = await this.GetRequiredPartsAsync();
                foreach (var part in parts)
                {
                    if (!await part.CanCompleteAsync())
                    {
                        return false;
                    }
                }

                return true;
            }

            public async Task CompleteAsync()
            {
                var parts = await this.GetRequiredPartsAsync();
                foreach (var part in parts)
                {
                    await part.CompleteAsync();
                }
            }

            public async Task CancelAsync()
            {
                var parts = await this.GetRequiredPartsAsync();
                foreach (var part in parts)
                {
                    await part.CancelAsync();
                }
            }

            public Task<IReadOnlyList<IHandpieceRequiredPart>> GetRequiredPartsAsync()
            {
                return this.handpiece.requiredPartManager.ListAsync(
                    predicate: null,
                    handpiece: this.handpiece,
                    sku: null);
            }

            public Task<IHandpieceRequiredPart> FindRequiredPartAsync(Guid skuId)
            {
                return this.handpiece.requiredPartManager.FindByHandpieceAndSKUAsync(this.handpiece.Id, this.handpiece, skuId, null);
            }

            public Task<IHandpieceRequiredPart> FindRequiredPartAsync(IInventorySKU sku)
            {
                return this.handpiece.requiredPartManager.FindByHandpieceAndSKUAsync(this.handpiece.Id, this.handpiece, sku.Id, sku);
            }

            public Task<IHandpieceRequiredPart> GetRequiredPartAsync(Guid id)
            {
                return this.handpiece.requiredPartManager.FindAsync(x => x.Id == id, this.handpiece, null);
            }

            public async Task<IHandpieceRequiredPart> AddRequiredPartAsync(IInventorySKU sku, Decimal quantity)
            {
                var part = new HandpieceRequiredPart
                {
                    Id = Guid.NewGuid(),
                    HandpieceId = this.handpiece.Id,
                    SKUId = sku.Id,
                    Quantity = quantity,
                };

                await this.handpiece.repository.InsertAsync(part);
                await this.handpiece.repository.SaveChangesAsync();

                var partDomain = await this.handpiece.requiredPartManager.GetFromEntityAsync(part, this.handpiece, sku);

                var availableQuantity = await sku.GetAvailableQuantity(this.handpiece.Job.Workshop);
                if (this.handpiece.Entity.HandpieceStatus.IsOneOf(HandpieceStatus.SentComplete))
                {
                    if (availableQuantity.GreaterThanOrEqual(quantity, InventorySKU.QuantityPrecision))
                    {
                        var repairMovement = await sku.Movements.CreateAsync<IInventoryRepairMovement, InventoryMovementBuilder.Repair>(
                            this.handpiece.Job.Workshop,
                            quantity,
                            b => b.WithStatus(InventoryMovementStatus.Completed));
                        await partDomain.LinkMovementsAsync(new IInventoryMovement[] { repairMovement });
                    }
                    else
                    {
                        throw new InvalidOperationException("Unable to add required parts to completed handpieces unless they are in stock");
                    }
                }
                else
                {
                    if (availableQuantity.GreaterThanOrEqual(quantity, InventorySKU.QuantityPrecision))
                    {
                        var repairMovement = await sku.Movements.CreateAsync<IInventoryRepairMovement, InventoryMovementBuilder.Repair>(
                            this.handpiece.Job.Workshop,
                            quantity,
                            b => b.WithStatus(InventoryMovementStatus.Allocated));
                        await partDomain.LinkMovementsAsync(new IInventoryMovement[] { repairMovement });
                    }
                    else if (availableQuantity.GreaterThan(0m, InventorySKU.QuantityPrecision))
                    {
                        var repairMovement = await sku.Movements.CreateAsync<IInventoryRepairMovement, InventoryMovementBuilder.Repair>(
                            this.handpiece.Job.Workshop,
                            quantity,
                            b => b.WithStatus(InventoryMovementStatus.Waiting));
                        var fragmentMovement = await sku.Movements.CreateAsync<IInventoryRepairFragmentMovement, InventoryMovementBuilder.RepairFragment>(
                            this.handpiece.Job.Workshop,
                            availableQuantity,
                            b => { });
                        await partDomain.LinkMovementsAsync(new IInventoryMovement[] { repairMovement, fragmentMovement });
                    }
                    else
                    {
                        var repairMovement = await sku.Movements.CreateAsync<IInventoryRepairMovement, InventoryMovementBuilder.Repair>(
                            this.handpiece.Job.Workshop,
                            quantity,
                            b => b.WithStatus(InventoryMovementStatus.Waiting));
                        await partDomain.LinkMovementsAsync(new IInventoryMovement[] { repairMovement });
                    }
                }

                return partDomain;
            }

            public async Task<IHandpieceRequiredPart> UpdateQuantityAsync(IInventorySKU sku, Decimal currentQuantity, Decimal newQuantity)
            {
                if (sku == null)
                {
                    throw new ArgumentNullException(nameof(sku));
                }

                if (newQuantity.LessThan(0m, InventorySKU.QuantityPrecision))
                {
                    throw new ArgumentException("New quantity cannot be lower than 0", nameof(newQuantity));
                }

                var part = await this.FindRequiredPartAsync(sku);

                var actualQuantity = part?.Quantity ?? 0m;
                if (currentQuantity.NotEquals(actualQuantity, InventorySKU.QuantityPrecision))
                {
                    throw new InvalidOperationException("Expected current quantity is different from the actual one. Possible race condition.");
                }

                if (newQuantity.Equals(0m, InventorySKU.QuantityPrecision))
                {
                    if (part != null)
                    {
                        await part.DeleteAsync();
                    }

                    return null;
                }

                if (currentQuantity.Equals(newQuantity, InventorySKU.QuantityPrecision))
                {
                    // No change in quantity
                    return part;
                }

                if (part == null)
                {
                    return await this.AddRequiredPartAsync(sku, newQuantity);
                }

                await part.UpdateQuantityAsync(newQuantity);
                await part.RefreshAsync();
                return part;
            }
        }
    }
}
