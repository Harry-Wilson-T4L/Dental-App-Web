using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementCancelModel
    {
        [BindNever]
        public IInventorySKU SKU { get; set; }

        [BindNever]
        public IInventoryOrderMovement Movement { get; set; }
    }
}
