using System;
using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementGroupOrderWithEditModel
    {
        [BindNever]
        public IWorkshop Workshop { get; set; }

        [BindNever]
        public IInventorySKU SKU { get; set; }

        [BindNever]
        public IReadOnlyList<IInventoryOrderMovement> Movements { get; set; }

        public String Comment { get; set; }

        public List<InventoryMovementGroupOrderWithEditItemModel> Result { get; set; } = new();
    }
}
