using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementHistoryViewModel
    {
        [BindNever]
        public IInventorySKU SKU { get; set; }

        [BindNever]
        public IInventoryMovement Movement { get; set; }
    }
}
