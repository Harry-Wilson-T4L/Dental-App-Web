using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendNewClientTemplateModel
    {
        public String NewClientOfferText { get; set; }

        public Guid? MonthlySpecialImage { get; set; }

        public Guid? PriceGuideImage { get; set; }

        public List<Guid> Attachments { get; set; }
    }
}
