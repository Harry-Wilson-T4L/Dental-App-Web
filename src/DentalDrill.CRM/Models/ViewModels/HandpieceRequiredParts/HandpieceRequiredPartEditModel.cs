using System;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartEditModel : IEditModelOriginalEntity<HandpieceRequiredPart>
    {
        [BindNever]
        public HandpieceRequiredPart Original { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        public Decimal Quantity { get; set; }
    }
}
