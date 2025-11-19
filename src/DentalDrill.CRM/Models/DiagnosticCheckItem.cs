using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class DiagnosticCheckItem
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public String Name { get; set; }

        public ICollection<DiagnosticCheckItemType> Types { get; set; }
    }
}
