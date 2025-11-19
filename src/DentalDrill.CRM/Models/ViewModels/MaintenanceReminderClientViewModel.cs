using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class MaintenanceReminderClientViewModel
    {
        [BindNever]
        public ClientViewModel Client { get; set; }

        [BindNever]
        public List<ClientRepairedItemViewModel> ItemsUpForService { get; set; }
    }
}
