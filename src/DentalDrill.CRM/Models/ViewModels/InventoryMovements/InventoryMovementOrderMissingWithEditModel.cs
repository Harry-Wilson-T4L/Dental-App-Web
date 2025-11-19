using System;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementOrderMissingWithEditModel
    {
        [BindNever]
        public IWorkshop Workshop { get; set; }

        [BindNever]
        public IInventorySKU SKU { get; set; }

        [BindNever]
        public Decimal MissingQuantity { get; set; }

        public Decimal Quantity { get; set; }

        public Decimal? Price { get; set; }

        public String Comment { get; set; }
    }
}
