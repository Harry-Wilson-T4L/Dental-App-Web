using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Mail.Models;

namespace DentalDrill.CRM.Models
{
    public class ClientEmailMessage : IClientDependent
    {
        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public Int32 EmailMessageId { get; set; }

        public EmailMessage EmailMessage { get; set; }
    }
}
