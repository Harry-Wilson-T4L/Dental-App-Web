using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientAttachmentReadModel
    {
        public Guid Id { get; set; }

        public DateTime UploadedOn { get; set; }

        public String FileName { get; set; }

        public String Comment { get; set; }

        public Guid EmployeeId { get; set; }

        public String EmployeeName { get; set; }
    }
}
