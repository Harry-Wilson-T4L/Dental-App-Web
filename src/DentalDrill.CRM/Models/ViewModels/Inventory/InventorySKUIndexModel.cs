using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUIndexModel
    {
        [BindNever]
        public Workshop SelectedWorkshop { get; set; }

        [BindNever]
        public IReadOnlyList<Workshop> Workshops { get; set; }
    }
}
