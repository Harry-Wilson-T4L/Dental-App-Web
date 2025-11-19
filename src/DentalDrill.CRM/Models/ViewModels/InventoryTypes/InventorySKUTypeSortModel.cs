using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryTypes
{
    public enum InventorySKUTypeSortScope
    {
        All = 0,
        TopLevel = 1,
        Specific = 2,
        SpecificRecursive = 3,
    }

    public enum InventorySKUTypeSortMethod
    {
        AlphaAscending = 0,
        AlphaDescending = 1,
    }

    public class InventorySKUTypeSortModel
    {
        [BindNever]
        public InventorySKUType Original { get;set; }

        [BindNever]
        public List<InventorySKU> AvailableGroupSKUs { get;set; }

        [Display(Name = "Scope")]
        public InventorySKUTypeSortScope Scope { get; set; }

        [Display(Name = "Specific SKU")]
        public Guid? SpecificSKU { get; set; }

        [Display(Name = "Method")]
        public InventorySKUTypeSortMethod Method { get; set; }
    }
}
