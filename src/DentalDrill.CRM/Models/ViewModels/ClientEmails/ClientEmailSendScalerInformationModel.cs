using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendScalerInformationModel
    {
        public Client Client { get; set; }

        [Required]
        public String To { get; set; }

        public String Greeting { get; set; }

        public Guid? ScalersPricesImage { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
