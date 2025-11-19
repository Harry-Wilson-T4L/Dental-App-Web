using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.GenericFlags
{
    public class GenericFlagBulkGetResponseModel
    {
        public Dictionary<String, GenericFlagState> States { get; set; }
    }
}
