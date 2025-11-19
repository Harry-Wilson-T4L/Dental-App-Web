using System.Collections.Generic;

namespace DentalDrill.CRM.Models.Notifications
{
    public class NotificationVisibilityOptions
    {
        public Employee Employee { get; set; }

        public NotificationScope GlobalScope { get; set; }

        public List<NotificationWorkshopVisibilityOptions> WorkshopScopes { get; set; }
    }
}
