using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobInvoiceReadModel
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public String FullInvoiceNumber { get; set; }

        public String FileName { get; set; }

        public DateTime CreatedOn { get; set; }

        public String EmployeeName { get; set; }
    }
}
