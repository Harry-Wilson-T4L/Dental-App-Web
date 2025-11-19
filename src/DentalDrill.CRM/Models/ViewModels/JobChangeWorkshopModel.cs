using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobChangeWorkshopModel
    {
        [BindNever]
        public Job Entity { get; set; }

        [BindNever]
        public List<Workshop> Workshops { get; set; }

        public Guid? CurrentWorkshopId { get; set; }

        [Required]
        public Guid? NewWorkshopId { get; set; }
    }
}
