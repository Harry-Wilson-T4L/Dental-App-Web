using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Identity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class UserCreateViewModel
    {
        [BindNever]
        public List<Role> Roles { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Username")]
        public String UserName { get; set; }

        [MaxLength(256)]
        [Display(Name = "E-mail")]
        public String Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Required]
        [Compare(nameof(UserCreateViewModel.Password))]
        [Display(Name = "Confirm password")]
        public String ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public Guid? RoleId { get; set; }

        [Display(Name = "Lockout enabled")]
        public Boolean LockoutEnabled { get; set; }
    }
}
