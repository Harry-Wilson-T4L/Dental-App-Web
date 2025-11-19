using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class CorporatePricingServiceLevel
    {
        public Guid Id { get; set; }

        public Guid ServiceLevelId { get; set; }

        public ServiceLevel ServiceLevel { get; set; }

        public Guid CategoryId { get; set; }

        public CorporatePricingCategory Category { get; set; }

        public Decimal CostOfRepair { get; set; }
    }
}
