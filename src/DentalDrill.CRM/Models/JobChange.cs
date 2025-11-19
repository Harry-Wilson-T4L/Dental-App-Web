using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class JobChange : IEntityChange<Job, Guid>
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }

        public DateTime ChangedOn { get; set; }

        public Guid ChangedById { get; set; }

        public Employee ChangedBy { get; set; }

        public ChangeAction Action { get; set; }

        public JobStatus? OldStatus { get; set; }

        public JobStatus? NewStatus { get; set; }

        public String OldContent { get; set; }

        public String NewContent { get; set; }

        Guid IEntityChange<Job, Guid>.EntityId
        {
            get => this.JobId;
            set => this.JobId = value;
        }

        Job IEntityChange<Job, Guid>.Entity
        {
            get => this.Job;
            set => this.Job = value;
        }
    }
}
