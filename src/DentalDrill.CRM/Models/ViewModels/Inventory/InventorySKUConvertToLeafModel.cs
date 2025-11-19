using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUConvertToLeafModel
    {
        [BindNever]
        public InventorySKU Entity { get; set; }

        [BindNever]
        public Boolean HasChildren { get; set; }
    }
}
