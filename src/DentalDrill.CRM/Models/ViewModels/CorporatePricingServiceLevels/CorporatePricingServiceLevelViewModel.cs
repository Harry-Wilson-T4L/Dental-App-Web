using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.CorporatePricingServiceLevels
{
    public class CorporatePricingServiceLevelViewModel
    {
        public Guid Id { get; set; }

        public String ServiceLevelName { get; set; }

        public Dictionary<Guid, Decimal?> CategoriesPricing { get; set; }
    }
}
