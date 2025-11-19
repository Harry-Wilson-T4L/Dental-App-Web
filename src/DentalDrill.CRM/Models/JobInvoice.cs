using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;

namespace DentalDrill.CRM.Models
{
    public class JobInvoice
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }

        public Int32 InvoiceNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid FileId { get; set; }

        public UploadedFile File { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
