using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartCreateModel
    {
        [BindNever]
        public Handpiece Handpiece { get; set; }

        [BindNever]
        public List<InventorySKU> AvailableSKUs { get; set; }

        [BindNever]
        public String InlineAjaxResponse { get; set; }

        [Display(Name = "SKU")]
        [Required]
        public Guid? SKU { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        public Decimal Quantity { get; set; }
    }
}
