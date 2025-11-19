using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.Corporates
{
    public class CorporateCreateModel
    {
        [Required]
        [MaxLength(100)]
        public String Name { get; set; }
    }
}
