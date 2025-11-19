using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendNewInterstateClientModel
    {
        public Client Client { get; set; }

        [Required]
        public String To { get; set; }

        public String Greeting { get; set; }

        public String DiscountComment { get; set; }

        public Guid? MonthlySpecialImage { get; set; }

        public Guid? PriceGuideImage { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
