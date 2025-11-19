using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailSendWelcomeModel
    {
        public Client Client { get; set; }

        [Required]
        public String To { get; set; }

        public String Username { get; set; }

        public String Password { get; set; }

        public List<Guid> Attachments { get; set; } = new List<Guid>();
    }
}
