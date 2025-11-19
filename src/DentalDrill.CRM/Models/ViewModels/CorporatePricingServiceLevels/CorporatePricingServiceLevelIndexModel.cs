using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.CorporatePricingServiceLevels
{
    public class CorporatePricingServiceLevelIndexModel
    {
        [BindNever]
        public List<CorporatePricingCategory> Categories { get; set; }

        [BindNever]
        public IEmployeeAccess Access { get; set; }
    }
}
