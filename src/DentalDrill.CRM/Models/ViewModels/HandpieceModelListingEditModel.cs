using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelListingEditModel
    {
        [BindNever]
        public HandpieceModel Parent { get; set; }

        [BindNever]
        public HandpieceStoreListing Entity { get; set; }

        [Display(Name = "S/N")]
        public String SerialNumber { get; set; }

        [Display(Name = "Price")]
        public Decimal Price { get; set; }

        [Display(Name = "Warranty")]
        public String Warranty { get; set; }

        [Display(Name = "Notes")]
        public String Notes { get; set; }

        [Display(Name = "Coupling/Fitting")]
        public String Coupling { get; set; }

        [Display(Name = "Cosmetic Condition")]
        public String CosmeticCondition { get; set; }

        [Display(Name = "Fibre Optic Brightness")]
        public String FiberOpticBrightness { get; set; }

        public HandpieceStoreListingStatus? Status { get; set; }

        public List<Guid> Images { get; set; } = new List<Guid>();
    }
}
