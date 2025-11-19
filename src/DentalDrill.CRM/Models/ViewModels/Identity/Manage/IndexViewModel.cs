using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.Identity.Manage
{
    public class IndexViewModel
    {
        public String Username { get; set; }

        public Boolean IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public String PhoneNumber { get; set; }

        public String StatusMessage { get; set; }
    }
}
