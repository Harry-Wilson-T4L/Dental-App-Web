using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ComponentModels
{
    public class SurgeryHeaderViewComponentModel
    {
        [BindNever]
        public Client Client { get; set; }

        [BindNever]
        public ClientAppearance ClientAppearance { get; set; }
    }
}
