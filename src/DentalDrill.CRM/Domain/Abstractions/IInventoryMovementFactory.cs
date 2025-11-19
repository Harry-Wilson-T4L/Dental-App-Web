using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventoryMovementFactory
    {
        IInventoryMovement Create(InventoryMovement entity, IWorkshop workshop);

        TMovement Create<TMovement>(InventoryMovement entity, IWorkshop workshop)
            where TMovement : class, IInventoryMovement;
    }
}
