using System;
using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementIndexModel
    {
        [BindNever]
        public IInventorySKU RequestedSKU { get; set; }

        [BindNever]
        public IReadOnlyList<IInventorySKU> FilteredSKU { get; set; }

        [BindNever]
        public IReadOnlyList<IInventorySKUType> StatsTypes { get; set; }

        [BindNever]
        public Boolean GroupMovements { get; set; }

        [BindNever]
        public String Tab { get; set; }

        [BindNever]
        public Workshop SelectedWorkshop { get; set; }

        [BindNever]
        public IReadOnlyList<Workshop> Workshops { get; set; }
    }
}
