using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientAccessCreateUserOperationModel
    {
        [BindNever]
        public Client Client { get; set; }

        [Required]
        public String UserName { get; set; }

        [Required]
        public String Password { get; set; }

        public Boolean SendEmail { get; set; }
    }
}
