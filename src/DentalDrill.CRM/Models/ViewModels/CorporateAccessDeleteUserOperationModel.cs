using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CorporateAccessDeleteUserOperationModel
    {
        [BindNever]
        public Corporate Corporate { get; set; }
    }
}
