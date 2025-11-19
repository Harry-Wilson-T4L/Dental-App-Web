using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendScalerInformationTemplateModel
    {
        public Guid? ScalersPricesImage { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
