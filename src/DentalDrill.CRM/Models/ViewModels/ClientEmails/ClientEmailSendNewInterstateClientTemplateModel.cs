using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendNewInterstateClientTemplateModel
    {
        public Guid? MonthlySpecialImage { get; set; }

        public Guid? PriceGuideImage { get; set; }

        public List<Guid> Attachments { get; set; }
    }
}
