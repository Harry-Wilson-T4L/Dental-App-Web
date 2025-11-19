using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class ProblemDescriptionOption
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }
    }
}
