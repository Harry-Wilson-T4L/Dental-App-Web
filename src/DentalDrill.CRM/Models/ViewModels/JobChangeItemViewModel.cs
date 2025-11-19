using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobChangeItemViewModel
    {
        public Guid Id { get; set; }

        public DateTime ChangedOn { get; set; }

        public String ChangedByName { get; set; }

        public JobStatus? OldStatus { get; set; }

        public String OldStatusName { get; set; }

        public JobStatus? NewStatus { get; set; }

        public String NewStatusName { get; set; }
    }
}
