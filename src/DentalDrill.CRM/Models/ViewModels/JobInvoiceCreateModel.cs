using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobInvoiceCreateModel
    {
        [BindNever]
        public Job Parent { get; set; }

        [Display(Name = "File")]
        [Required]
        public Guid? FileId { get; set; }
    }
}
