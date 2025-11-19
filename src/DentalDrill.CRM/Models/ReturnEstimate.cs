using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class ReturnEstimate
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        [Required]
        public String Name { get; set; }
    }
}
