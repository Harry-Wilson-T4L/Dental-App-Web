using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendHandpieceTradersModel
    {
        public Client Client { get; set; }

        [Required]
        public String To { get; set; }

        public String Greeting { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
