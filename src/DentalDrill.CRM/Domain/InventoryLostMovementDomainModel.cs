using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;

namespace DentalDrill.CRM.Domain
{
    public class InventoryLostMovementDomainModel : InventoryMovementDomainModelBase, IInventoryLostMovement
    {
        public InventoryLostMovementDomainModel(
            InventoryMovement entity,
            IWorkshop workshop,
            IRepository repository,
            IInventorySKUManager inventorySKUManager,
            UserEntityResolver userResolver,
            IDateTimeService dateTimeService)
            : base(
                entity,
                workshop,
                repository,
                inventorySKUManager,
                userResolver,
                dateTimeService)
        {
        }
    }
}
