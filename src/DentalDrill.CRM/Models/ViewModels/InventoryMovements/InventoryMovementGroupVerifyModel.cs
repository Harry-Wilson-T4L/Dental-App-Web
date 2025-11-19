using System;
using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementGroupVerifyModel
    {
        [BindNever]
        public IWorkshop Workshop { get; set; }

        [BindNever]
        public IInventorySKU SKU { get; set; }

        [BindNever]
        public IReadOnlyList<IInventoryOrderMovement> Movements { get; set; }

        public Boolean SetPrice { get; set; }

        public Decimal? Price { get; set; }
    }
}
