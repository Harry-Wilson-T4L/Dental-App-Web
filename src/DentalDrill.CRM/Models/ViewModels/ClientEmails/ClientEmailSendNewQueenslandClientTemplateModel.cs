using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendNewQueenslandClientTemplateModel
    {
        public Guid? LocationImage { get; set; }
        
        public String LocationText { get; set; }

        public String NewClientOfferText { get; set; }

        public Guid? MonthlySpecialImage { get; set; }

        public Guid? PriceGuideImage { get; set; }

        public List<Guid> Attachments { get; set; }
    }
}
