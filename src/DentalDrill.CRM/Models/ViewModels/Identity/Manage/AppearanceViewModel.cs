using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Services.Theming;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Identity.Manage
{
    public class AppearanceViewModel
    {
        [BindNever]
        public IReadOnlyList<ThemeDescriptor> AvailableThemes { get; set; }

        [Display(Name = "Theme")]
        public String Theme { get; set; }

        [Display(Name = "Background")]
        public Guid? Background { get; set; }

        [Display(Name = "Opacity")]
        [Range(0, 1)]
        public Decimal Opacity { get; set; }

        public String StatusMessage { get; set; }
    }
}
