using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models
{
    public class Notification
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime? ResolvedOn { get; set; }

        public NotificationScope Scope { get; set; }

        public Guid? RecipientId { get; set; }

        public Employee Recipient { get; set; }

        public NotificationType Type { get; set; }

        public String Payload { get; set; }

        public NotificationStatus Status { get; set; }

        public Guid? WorkshopId { get; set; }

        public Workshop Workshop { get; set; }

        public ICollection<NotificationRelatedEntity> RelatedEntities { get; set; }
    }
}
