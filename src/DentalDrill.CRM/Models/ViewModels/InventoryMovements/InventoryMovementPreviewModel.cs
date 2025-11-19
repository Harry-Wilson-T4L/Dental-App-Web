using System;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementPreviewModel
    {
        [BindNever]
        public IInventorySKU RequestedSKU { get; set; }

        [BindNever]
        public Workshop Workshop { get; set; }

        [BindNever]
        public String Tab { get; set; }
    }
}
