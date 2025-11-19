using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientAccessConfigurePageUrlOperationModel
    {
        [BindNever]
        public Client Client { get; set; }

        [RegularExpression("^[-_A-Za-z0-9]*$", ErrorMessage = "Can only contains latin letters, digits, dash and underscore.")]
        public String UrlPath { get; set; }
    }
}
