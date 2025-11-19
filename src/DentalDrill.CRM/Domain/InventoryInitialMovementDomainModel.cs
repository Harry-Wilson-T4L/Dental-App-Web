using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;

namespace DentalDrill.CRM.Domain
{
    public class InventoryInitialMovementDomainModel : InventoryMovementDomainModelBase, IInventoryInitialMovement
    {
        public InventoryInitialMovementDomainModel(
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
