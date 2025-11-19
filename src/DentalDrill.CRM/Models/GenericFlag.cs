using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class GenericFlag
    {
        [Required]
        [MaxLength(200)]
        public String Id { get; set; }

        public GenericFlagState State { get; set; }
    }
}
