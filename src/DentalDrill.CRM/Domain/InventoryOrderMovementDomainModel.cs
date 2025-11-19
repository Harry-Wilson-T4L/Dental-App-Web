using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM.Domain
{
    public class InventoryOrderMovementDomainModel : InventoryMovementDomainModelBase, IInventoryOrderMovement
    {
        private readonly IHandpieceRequiredPartManager handpieceRequiredPartManager;
        private readonly IDateTimeService dateTimeService;
        private readonly ILogger logger;

        public InventoryOrderMovementDomainModel(
            InventoryMovement entity,
            IWorkshop workshop,
            IRepository repository,
            IInventorySKUManager inventorySkuManager,
            UserEntityResolver userResolver,
            IHandpieceRequiredPartManager handpieceRequiredPartManager,
            IDateTimeService dateTimeService,
            ILogger<InventoryOrderMovementDomainModel> logger)
            : base(
                entity,
                workshop,
                repository,
                inventorySkuManager,
                userResolver,
                dateTimeService)
        {
            this.handpieceRequiredPartManager = handpieceRequiredPartManager;
            this.dateTimeService = dateTimeService;
            this.logger = logger;
        }

        public async Task<IHandpieceRequiredPart> GetHandpieceRequiredPartAsync()
        {
            var part = await this.Repository.QueryWithoutTracking<HandpieceRequiredPartMovement>()
                .Include(x => x.RequiredPart)
                .SingleOrDefaultAsync(x => x.MovementId == this.Id);

            if (part == null)
            {
                return null;
            }

            return await this.handpieceRequiredPartManager.GetFromEntityAsync(part.RequiredPart, null, null);
        }

        public async Task UpdateRequestedQuantityAsync(Decimal newQuantity)
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).UpdateRequestedQuantityAsync({newQuantity})");

            if (newQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                throw new ArgumentException(nameof(newQuantity));
            }

            if (this.Status != InventoryMovementStatus.Requested && this.Status != InventoryMovementStatus.Approved)
            {
                throw new InvalidOperationException("Invalid movement status");
            }

            var updateableEntity = await this.Repository.Query<InventoryMovement>().SingleOrDefaultAsync(x => x.Id == this.Id);
            var change = await this.PrepareTrackChangeAsync(ChangeAction.Modify, updateableEntity);
            updateableEntity.Quantity = newQuantity;
            updateableEntity.QuantityAbsolute = newQuantity;
            await this.Repository.UpdateAsync(updateableEntity);

            await this.TrackChangeAsync(change, updateableEntity);
            await this.Repository.SaveChangesAsync();
        }

        public async Task UpdateOrderedQuantityAsync(Decimal newQuantity)
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).UpdateOrderedQuantityAsync({newQuantity})");

            if (newQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                throw new ArgumentException(nameof(newQuantity));
            }

            if (this.Status != InventoryMovementStatus.Ordered)
            {
                throw new InvalidOperationException("Invalid movement status");
            }

            var updateableEntity = await this.Repository.Query<InventoryMovement>().SingleOrDefaultAsync(x => x.Id == this.Id);
            var change = await this.PrepareTrackChangeAsync(ChangeAction.Modify, updateableEntity);
            updateableEntity.Quantity = newQuantity;
            updateableEntity.QuantityAbsolute = newQuantity;
            await this.Repository.UpdateAsync(updateableEntity);

            await this.TrackChangeAsync(change, updateableEntity);
            await this.Repository.SaveChangesAsync();
        }

        public async Task RemoveRequestedAsync()
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).RemoveRequestedAsync()");

            if (this.Status != InventoryMovementStatus.Requested && this.Status != InventoryMovementStatus.Approved)
            {
                throw new InvalidOperationException("Invalid movement status");
            }

            var updateableEntity = await this.Repository.Query<InventoryMovement>().SingleOrDefaultAsync(x => x.Id == this.Id);
            var change = await this.PrepareTrackChangeAsync(ChangeAction.Delete, updateableEntity);
            await this.TrackChangeAsync(change, updateableEntity);

            await this.Repository.DeleteAsync(updateableEntity);
            await this.Repository.SaveChangesAsync();
        }

        public async Task SetPriceAsync(Decimal price)
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).SetPriceAsync({price})");

            await this.ChangeAsync(
                new[]
                {
                    InventoryMovementStatus.Requested,
                    InventoryMovementStatus.Approved,
                    InventoryMovementStatus.Ordered,
                },
                entity =>
                {
                    entity.Price = price;
                });

            var sku = await this.GetSKUAsync();
            var requiredPart = await this.GetHandpieceRequiredPartAsync();
            if (requiredPart != null)
            {
                var repair = await sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(requiredPart.Id);
                if (repair != null)
                {
                    await repair.SetPriceInternalAsync(price);
                }
            }
        }

        public async Task ClearPriceAsync()
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).ClearPriceAsync()");

            await this.ChangeAsync(
                new[]
                {
                    InventoryMovementStatus.Requested,
                    InventoryMovementStatus.Approved,
                    InventoryMovementStatus.Ordered,
                },
                entity =>
                {
                    entity.Price = null;
                });

            var sku = await this.GetSKUAsync();
            var requiredPart = await this.GetHandpieceRequiredPartAsync();
            if (requiredPart != null)
            {
                var repair = await sku.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(requiredPart.Id);
                if (repair != null)
                {
                    await repair.ClearPriceInternalAsync();
                }
            }
        }

        public async Task<Boolean> TryChangePriceAsync(Decimal? newPrice)
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).TryChangePriceAsync({newPrice})");

            if (this.Price != newPrice)
            {
                if (newPrice.HasValue)
                {
                    await this.SetPriceAsync(newPrice.Value);
                }
                else
                {
                    await this.ClearPriceAsync();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public Task ApproveAsync()
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).ApproveAsync()");

            return this.ChangeAsync(
                new[] { InventoryMovementStatus.Requested, },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Approved;
                });
        }

        public Task ApproveWithEditAsync(Decimal quantity, String comment)
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).ApproveWithEditAsync({quantity}, '{comment}')");

            if (quantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                throw new ArgumentException(nameof(quantity));
            }

            return this.ChangeAsync(
                new[] { InventoryMovementStatus.Requested, },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Approved;
                    entity.QuantityAbsolute = quantity;
                    entity.Quantity = quantity;
                    entity.Comment = comment;
                });
        }

        public Task OrderAsync()
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).OrderAsync()");

            return this.ChangeAsync(
                new[] { InventoryMovementStatus.Approved, },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Ordered;
                });
        }

        public Task OrderWithEditAsync(Decimal quantity, String comment)
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).OrderWithEditAsync({quantity}, '{comment}')");

            if (quantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                throw new ArgumentException(nameof(quantity));
            }

            return this.ChangeAsync(
                new[] { InventoryMovementStatus.Approved, },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Ordered;
                    entity.QuantityAbsolute = quantity;
                    entity.Quantity = quantity;
                    entity.Comment = comment;
                });
        }

        public Task VerifyAsync()
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).VerifyAsync()");

            return this.ChangeAsync(
                new[] { InventoryMovementStatus.Ordered, },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Completed;
                    entity.CompletedOn = this.dateTimeService.CurrentUtcTime;
                });
        }

        public Task VerifyWithEditAsync(Decimal quantity, String comment)
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).VerifyWithEditAsync({quantity}, '{comment}')");

            if (quantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                throw new ArgumentException(nameof(quantity));
            }

            return this.ChangeAsync(
                new[] { InventoryMovementStatus.Ordered, },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Completed;
                    entity.QuantityAbsolute = quantity;
                    entity.Quantity = quantity;
                    entity.Comment = comment;
                });
        }

        public Task CancelAsync()
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).CancelAsync()");

            return this.ChangeAsync(
                new[]
                {
                    InventoryMovementStatus.Requested,
                    InventoryMovementStatus.Approved,
                    InventoryMovementStatus.Ordered,
                },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Cancelled;
                });
        }

        public Task RestoreAsync()
        {
            this.logger.LogInformation($"InventoryMovement({this.Id}).RestoreAsync()");

            return this.ChangeAsync(
                new[] { InventoryMovementStatus.Cancelled, },
                entity =>
                {
                    entity.Status = InventoryMovementStatus.Requested;
                });
        }

        public async Task UnlinkFromPartAsync()
        {
            var part = await this.Repository.Query<HandpieceRequiredPartMovement>()
                .Include(x => x.RequiredPart)
                .SingleOrDefaultAsync(x => x.MovementId == this.Id);

            if (part == null)
            {
                return;
            }

            await this.Repository.DeleteAsync(part);
            await this.Repository.SaveChangesAsync();
        }

        private async Task ChangeAsync(InventoryMovementStatus[] allowedStatus, Action<InventoryMovement> change)
        {
            if (allowedStatus != null && !allowedStatus.Contains(this.Status))
            {
                throw new InvalidOperationException("Invalid movement status");
            }

            var updateableEntity = await this.Repository.Query<InventoryMovement>().SingleOrDefaultAsync(x => x.Id == this.Id);
            var trackedChange = await this.PrepareTrackChangeAsync(ChangeAction.Modify, updateableEntity);

            change(updateableEntity);

            await this.Repository.UpdateAsync(updateableEntity);
            await this.TrackChangeAsync(trackedChange, updateableEntity);

            await this.Repository.SaveChangesAsync();
        }
    }
}
