using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUEditModel : IEditModelOriginalEntity<InventorySKU>
    {
        [BindNever]
        public InventorySKU Original { get; set; }

        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Average Price")]
        public Decimal? AveragePrice { get; set; }

        [Display(Name = "Warning Quantity")]
        public Decimal? WarningQuantity { get; set; }

        [Display(Name = "Name")]
        public String Description { get; set; }

        [Display(Name = "Hide from statistic")]
        public Boolean HideFromStatistic { get; set; }

        [Display(Name = "Workshop Options")]
        public List<InventorySKUWorkshopOptionsEditModel> WorkshopOptions { get; set; } = new List<InventorySKUWorkshopOptionsEditModel>();
    }
}
