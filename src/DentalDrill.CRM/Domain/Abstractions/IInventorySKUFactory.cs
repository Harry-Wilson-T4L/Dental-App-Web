using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventorySKUFactory
    {
        IInventorySKU Create(InventorySKU entity, IInventorySKUType type);
    }
}
