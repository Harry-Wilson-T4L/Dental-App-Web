using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobChangeClientModel
    {
        [BindNever]
        public Job Entity { get; set; }

        [BindNever]
        public Guid? CurrentClientId { get; set; }

        [Required]
        public Guid? NewClientId { get; set; }
    }
}
