using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendMaintenanceInstructionsTemplateModel
    {
        public Guid? PriceGuideImage { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
