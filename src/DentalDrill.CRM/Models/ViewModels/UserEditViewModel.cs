using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class UserEditViewModel : IEditModelOriginalEntity<UserDetailsViewModel>
    {
        [BindNever]
        public UserDetailsViewModel Original { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Username")]
        public String UserName { get; set; }

        [MaxLength(256)]
        [Display(Name = "E-mail")]
        public String Email { get; set; }

        [Display(Name = "Password")]
        public String Password { get; set; }

        [Compare(nameof(UserEditViewModel.Password))]
        [Display(Name = "Confirm password")]
        public String ConfirmPassword { get; set; }

        [Display(Name = "Lockout enabled")]
        public Boolean LockoutEnabled { get; set; }

        [Display(Name = "Must change password at next login")]
        public Boolean MustChangePasswordAtNextLogin { get; set; }

        public Boolean ShouldUpdatePassword()
        {
            return !String.IsNullOrEmpty(this.Password);
        }
    }
}
