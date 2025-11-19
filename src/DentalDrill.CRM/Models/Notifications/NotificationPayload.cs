using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DentalDrill.CRM.Models.Notifications
{
    public abstract class NotificationPayload
    {
        public NotificationType Type => this.GetType().GetCustomAttribute<NotificationPayloadAttribute>().Type;

        public static NotificationPayload LoadFrom(NotificationType type, String content)
        {
            var clrType = typeof(NotificationPayload).Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(NotificationPayload)))
                .Select(x => new
                {
                    ClrType = x,
                    NotificationType = x.GetCustomAttribute<NotificationPayloadAttribute>()?.Type,
                })
                .Where(x => x.NotificationType == type)
                .Select(x => x.ClrType)
                .SingleOrDefault();

            if (clrType == null)
            {
                throw new ArgumentException($"Not supported type {type}", nameof(type));
            }

            return JsonConvert.DeserializeObject(content, clrType) as NotificationPayload;
        }

        public String Save()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
