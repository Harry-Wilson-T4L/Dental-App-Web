using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Notifications
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NotificationPayloadAttribute : Attribute
    {
        public NotificationPayloadAttribute(NotificationType type)
        {
            this.Type = type;
        }

        public NotificationType Type { get; }
    }
}
