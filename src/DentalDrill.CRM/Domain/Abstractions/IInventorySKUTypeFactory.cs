using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventorySKUTypeFactory
    {
        IInventorySKUType Create(InventorySKUType entity);
    }
}
