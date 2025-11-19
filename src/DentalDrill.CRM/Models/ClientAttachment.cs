using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;

namespace DentalDrill.CRM.Models
{
    public class ClientAttachment : IClientDependent
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public DateTime UploadedOn { get; set; }

        public Guid FileId { get; set; }

        public UploadedFile File { get; set; }

        public String Comment { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
