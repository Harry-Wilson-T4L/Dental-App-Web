using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUConvertToGroupModel
    {
        [BindNever]
        public InventorySKU Entity { get; set; }

        [BindNever]
        public Boolean HasMovements { get; set; }

        [Display(Name = "Create New Group")]
        public Boolean CreateNewGroup { get; set; }

        [Display(Name = "Group Name")]
        public String GroupName { get; set; }

        [Display(Name = "Leaf Name")]
        public String LeafName { get; set; }
    }
}
