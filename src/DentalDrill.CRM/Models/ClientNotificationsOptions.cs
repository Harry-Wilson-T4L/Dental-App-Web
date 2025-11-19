using System;

namespace DentalDrill.CRM.Models
{
    [Flags]
    public enum ClientNotificationsOptions
    {
        None = 0,
        Enabled = 1,
        DisabledManualEmails = 2,
        DisabledHandpieceNotifications = 4,
        DisabledFeedbackRequests = 8,
        DisabledMaintenanceReminders = 16,

        DisabledEverything = ClientNotificationsOptions.DisabledManualEmails |
                             ClientNotificationsOptions.DisabledHandpieceNotifications |
                             ClientNotificationsOptions.DisabledFeedbackRequests |
                             ClientNotificationsOptions.DisabledMaintenanceReminders,
    }
}
