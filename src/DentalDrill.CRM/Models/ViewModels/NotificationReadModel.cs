using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models.Notifications;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class NotificationReadModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime? ResolvedOn { get; set; }

        public NotificationType Type { get; set; }

        public NotificationPayload Payload { get; set; }

        public NotificationStatus Status { get; set; }
    }
}
