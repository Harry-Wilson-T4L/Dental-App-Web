using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Notifications
{
    [NotificationPayload(NotificationType.JobEstimated)]
    public class JobEstimatedPayload : NotificationPayload
    {
        public JobEstimatedPayload()
        {
        }

        public JobEstimatedPayload(Job job)
        {
            this.JobId = job.Id;
            this.JobNumber = job.JobNumber;
            this.SurgeryId = job.ClientId;
            this.SurgeryName = job.Client.Name;
        }

        public Guid JobId { get; set; }

        public Int64 JobNumber { get; set; }

        public Guid SurgeryId { get; set; }

        public String SurgeryName { get; set; }
    }
}
