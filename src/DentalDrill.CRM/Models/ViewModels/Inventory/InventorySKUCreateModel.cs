using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUCreateModel : IHierarchicalCreateModel<InventorySKU>
    {
        [BindNever]
        public InventorySKUType Type { get; set; }

        [BindNever]
        public InventorySKU Parent { get; set; }

        [Display(Name = "Name")]
        [Required]
        public String Name { get; set; }

        [Display(Name = "QTY Shelf")]
        public Decimal? InitialShelfQuantity { get; set; }

        [Display(Name = "QTY Ordered")]
        public Decimal? InitialOrderedQuantity { get; set; }

        [Display(Name = "QTY Requested")]
        public Decimal? InitialRequestedQuantity { get; set; }

        [Display(Name = "Average Price")]
        public Decimal? AveragePrice { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Group")]
        public Boolean IsGroupNode { get; set; }

        [Display(Name = "Workshop Options")]
        public List<InventorySKUWorkshopOptionsEditModel> WorkshopOptions { get; set; } = new List<InventorySKUWorkshopOptionsEditModel>();
    }
}
