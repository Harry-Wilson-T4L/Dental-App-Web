using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.Identity.Account
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }
    }
}
