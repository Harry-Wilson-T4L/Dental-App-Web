using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Services.Theming;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class EmployeeCreateModel
    {
        [BindNever]
        public IReadOnlyList<ThemeDescriptor> AvailableThemes { get; set; }

        [BindNever]
        public IReadOnlyList<EmployeeRole> AvailableRoles { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "First name")]
        public String FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Last name")]
        public String LastName { get; set; }

        [Display(Name = "Role")]
        public Guid RoleId { get; set; }

        [Display(Name = "Type")]
        public EmployeeType Type { get; set; }

        [Display(Name = "Appearance Theme")]
        public String AppearanceTheme { get; set; }

        [Display(Name = "Background")]
        public Guid? AppearanceBackgroundId { get; set; }

        [Display(Name = "Opacity")]
        [Range(0, 1)]
        public Decimal? AppearanceOpacity { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "User name")]
        public String UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Compare(nameof(EmployeeCreateModel.Password))]
        [Display(Name = "Confirm password")]
        public String ConfirmPassword { get; set; }
    }
}
