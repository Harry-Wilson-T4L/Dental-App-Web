using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientAttachmentCreateModel
    {
        [BindNever]
        public Client Parent { get; set; }

        [Required]
        public Guid? FileId { get; set; }

        public String Comment { get; set; }
    }
}
