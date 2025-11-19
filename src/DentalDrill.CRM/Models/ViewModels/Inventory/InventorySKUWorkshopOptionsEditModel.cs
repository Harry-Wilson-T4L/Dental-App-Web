using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUWorkshopOptionsEditModel
    {
        public Guid WorkshopId { get; set; }

        [BindNever]
        public Workshop Workshop { get; set; }

        [Display(Name = "Warning Quantity")]
        public Decimal? WarningQuantity { get; set; }
    }
}
