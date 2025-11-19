using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class ServiceLevel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength]
        public String Name { get; set; }

        public Boolean DisableReminders { get; set; }

        public ICollection<CorporatePricingServiceLevel> CorporatePricing { get; set; }
    }
}
