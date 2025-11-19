using System;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementVerifyModel
    {
        [BindNever]
        public IInventorySKU SKU { get; set; }

        [BindNever]
        public IInventoryOrderMovement Movement { get; set; }

        public Decimal? Price { get; set; }
    }
}
