using System;
using System.ComponentModel.DataAnnotations;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CorporateAppearanceEditViewModel
    {
        [BindNever]
        public Corporate Corporate { get; set; }

        [BindNever]
        public IEmployeeAccess Access { get; set; }

        [BindNever]
        public Boolean JustUpdated { get; set; }

        [Display(Name = "Logo image")]
        public Guid? LogoId { get; set; }

        [Display(Name = "Background image")]
        public Guid? BackgroundImageId { get; set; }

        [Display(Name = "Primary color")]
        [RegularExpression("#[A-Fa-f0-9]{6}", ErrorMessage = "Color must be set in HEX format: #xxxxxx")]
        public String PrimaryColor { get; set; }

        [Display(Name = "Secondary color")]
        [RegularExpression("#[A-Fa-f0-9]{6}", ErrorMessage = "Color must be set in HEX format: #xxxxxx")]
        public String SecondaryColor { get; set; }

        [Display(Name = "Header text color")]
        [RegularExpression("#[A-Fa-f0-9]{6}", ErrorMessage = "Color must be set in HEX format: #xxxxxx")]
        public String HeaderTextColor { get; set; }
    }
}
