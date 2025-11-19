using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Notifications
{
    [NotificationPayload(NotificationType.HandpieceStoreOrderCreated)]
    public class HandpieceStoreOrderCreatedPayload : NotificationPayload
    {
        public Guid OrderId { get; set; }

        public Int32 OrderNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String SurgeryName { get; set; }

        public String ContactName { get; set; }

        public String ContactEmail { get; set; }
    }
}
