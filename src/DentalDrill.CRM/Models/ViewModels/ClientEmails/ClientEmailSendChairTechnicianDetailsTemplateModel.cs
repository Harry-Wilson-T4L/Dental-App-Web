using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendChairTechnicianDetailsTemplateModel
    {
        public String Following { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
