using System;
using DevGuild.AspNetCore.Services.Mail.Models;

namespace DentalDrill.CRM.Models.ViewModels.ClientEmails
{
    public class ClientEmailReadModel
    {
        public Int32 Id { get; set; }

        public DateTime Created { get; set; }

        public String To { get; set; }

        public String Subject { get; set; }

        public EmailMessageStatus Status { get; set; }
    }
}
