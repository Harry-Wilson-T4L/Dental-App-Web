using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class CorporatePricingCategory
    {
        public Guid Id { get; set; }

        public Int32 OrderNo { get; set; }

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }
    }
}
