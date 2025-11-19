using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceStoreDetailsModel
    {
        [BindNever]
        public HandpieceStoreListing Listing { get; set; }

        [BindNever]
        public Client Surgery { get; set; }

        [Required]
        [MaxLength(200)]
        public String SurgeryName { get; set; }

        [Required]
        [MaxLength(200)]
        public String ContactName { get; set; }

        [Required]
        [MaxLength(256)]
        public String ContactEmail { get; set; }
    }
}
