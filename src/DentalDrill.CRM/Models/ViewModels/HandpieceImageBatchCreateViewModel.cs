using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceImageBatchCreateViewModel
    {
        [BindNever]
        public Handpiece Parent { get; set; }

        [Display(Name = "Images")]
        public List<Guid> Images { get; set; } = new List<Guid>();

        [Display(Name = "Display")]
        public Boolean Display { get; set; }
    }
}
