using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelEditModel
    {
        [BindNever]
        public HandpieceModel Original { get; set; }

        [BindNever]
        public IEmployeeAccess Access { get; set; }

        [BindNever]
        public HandpieceBrand Parent { get; set; }

        [BindNever]
        public List<HandpieceBrand> Brands { get; set; }

        [Display(Name = "Brand")]
        public Guid BrandId { get; set; }

        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Type")]
        public HandpieceSpeed Type { get; set; }

        [Display(Name = "Image")]
        public Guid? ImageId { get; set; }

        [Display(Name = "Price New")]
        public Decimal? PriceNew { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Workshop Notes")]
        public String WorkshopNotes { get; set; }
    }
}
