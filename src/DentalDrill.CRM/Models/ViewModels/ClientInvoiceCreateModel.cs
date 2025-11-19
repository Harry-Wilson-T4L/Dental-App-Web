using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientInvoiceCreateModel
    {
        [BindNever]
        public Client Parent { get; set; }

        [BindNever]
        public List<Job> Jobs { get; set; }

        [Display(Name = "Estimate")]
        [Required]
        public Guid? JobId { get; set; }

        [Display(Name = "File")]
        [Required]
        public Guid? FileId { get; set; }
    }
}
