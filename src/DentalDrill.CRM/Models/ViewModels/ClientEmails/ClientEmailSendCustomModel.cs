using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendCustomModel
    {
        public Client Client { get; set; }

        [Required]
        public String To { get; set; }

        [Required]
        public String Subject { get; set; }

        [Required]
        public String Text { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
