using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendMaintenanceRequiredModel
    {
        [BindNever]
        public Client Client { get; set; }

        [BindNever]
        public List<ClientRepairedItemViewModel> Items { get; set; }
    }
}
