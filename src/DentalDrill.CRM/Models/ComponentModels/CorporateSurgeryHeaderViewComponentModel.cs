using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ComponentModels
{
    public class CorporateSurgeryHeaderViewComponentModel
    {
        [BindNever]
        public Corporate Corporate { get; set; }

        [BindNever]
        public CorporateAppearance CorporateAppearance { get; set; }
    }
}
