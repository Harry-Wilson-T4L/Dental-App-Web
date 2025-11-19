using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobReadModel
    {
        public Guid Id { get; set; }

        public String JobTypeId { get; set; }

        public String JobTypeName { get; set; }

        public Int64 JobNumber { get; set; }

        public Guid WorkshopId { get; set; }

        public String WorkshopName { get; set; }

        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        public JobStatus JobStatus { get; set; }

        public String JobStatusName { get; set; }

        public String JobStatusConfig { get; set; }

        public DateTime Received { get; set; }

        public List<Handpiece> Handpieces { get; set; }

        public Int32 HandpiecesCount { get; set; }
    }
}
