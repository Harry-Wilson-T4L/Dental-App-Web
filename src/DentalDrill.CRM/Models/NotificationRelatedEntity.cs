using System;

namespace DentalDrill.CRM.Models
{
    public class NotificationRelatedEntity
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public Notification Notification { get; set; }

        public NotificationRelatedEntityType EntityType { get; set; }

        public Guid EntityId { get; set; }
    }
}
