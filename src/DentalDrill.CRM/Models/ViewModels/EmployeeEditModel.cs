using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Services.Theming;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class EmployeeEditModel : IEditModelOriginalEntity<Employee>
    {
        [BindNever]
        public IReadOnlyList<ThemeDescriptor> AvailableThemes { get; set; }

        [BindNever]
        public IReadOnlyList<EmployeeRole> AvailableRoles { get; set; }

        [BindNever]
        public Employee Original { get; set; }

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
    }
}
