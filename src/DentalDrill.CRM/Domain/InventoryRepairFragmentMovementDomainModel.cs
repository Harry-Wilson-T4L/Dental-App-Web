using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class InventoryRepairFragmentMovementDomainModel : InventoryMovementDomainModelBase, IInventoryRepairFragmentMovement
    {
        private readonly IHandpieceRequiredPartManager handpieceRequiredPartManager;

        public InventoryRepairFragmentMovementDomainModel(
            InventoryMovement entity,
            IWorkshop workshop,
            IRepository repository,
            IInventorySKUManager inventorySKUManager,
            UserEntityResolver userResolver,
            IDateTimeService dateTimeService,
            IHandpieceRequiredPartManager handpieceRequiredPartManager)
            : base(
                entity,
                workshop,
                repository,
                inventorySKUManager,
                userResolver,
                dateTimeService)
        {
            this.handpieceRequiredPartManager = handpieceRequiredPartManager;
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

        public async Task UpdateQuantityAsync(Decimal newQuantity)
        {
            if (newQuantity.LessThanOrEqual(0m, InventorySKU.QuantityPrecision))
            {
                throw new ArgumentException(nameof(newQuantity));
            }

            var updateableEntity = await this.Repository.Query<InventoryMovement>().SingleAsync(x => x.Id == this.Id);
            var change = await this.PrepareTrackChangeAsync(ChangeAction.Modify, updateableEntity);
            updateableEntity.Quantity = -newQuantity;
            updateableEntity.QuantityAbsolute = newQuantity;
            await this.Repository.UpdateAsync(updateableEntity);
            await this.TrackChangeAsync(change, updateableEntity);

            await this.Repository.SaveChangesAsync();
        }

        public async Task DeleteAsync()
        {
            var updateableEntity = await this.Repository.Query<InventoryMovement>().SingleAsync(x => x.Id == this.Id);
            var change = await this.PrepareTrackChangeAsync(ChangeAction.Delete, updateableEntity);
            await this.TrackChangeAsync(change, updateableEntity);

            await this.Repository.DeleteAsync(updateableEntity);
            await this.Repository.SaveChangesAsync();
        }
    }
}
