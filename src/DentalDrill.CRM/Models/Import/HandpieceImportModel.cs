using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Services.Csv;

namespace DentalDrill.CRM.Models.Import
{
    public class HandpieceImportModel
    {
        public String ID { get; set; }

        public String Client { get; set; }

        public String Type { get; set; }

        public String RPM { get; set; }

        public String Model { get; set; }

        public String Serial { get; set; }

        public String Diagnostic { get; set; }

        public String Service { get; set; }

        public String Parts { get; set; }

        public String Rating { get; set; }

        public String Tech { get; set; }

        public String Invoiced { get; set; }

        public String Back { get; set; }

        [CsvColumnIgnore]
        public String ClientId { get; set; }

        [CsvColumnIgnore]
        public Client ClientEntity { get; set; }

        [CsvColumnIgnore]
        public DateTime ReceivedDate { get; set; }

        [CsvColumnIgnore]
        public Employee TechEntity { get; set; }

        [CsvColumnIgnore]
        public Int32 RatingParsed { get; set; }

        [CsvColumnIgnore]
        public DateTime? InvoicedDate { get; set; }

        [CsvColumnIgnore]
        public DateTime? BackDate { get; set; }

        [CsvColumnIgnore]
        public DateTime? JobCreatedDate { get; set; }
    }
}
