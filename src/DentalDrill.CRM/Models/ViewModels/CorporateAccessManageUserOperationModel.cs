using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CorporateAccessManageUserOperationModel
    {
        [BindNever]
        public Corporate Corporate { get; set; }

        [Required]
        public String UserName { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }

        public String Password { get; set; }

        public Boolean ShouldUpdatePassword()
        {
            return !String.IsNullOrEmpty(this.Password);
        }
    }
}
