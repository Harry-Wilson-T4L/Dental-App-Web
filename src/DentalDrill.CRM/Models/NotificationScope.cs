using System;

namespace DentalDrill.CRM.Models
{
    [Flags]
    public enum NotificationScope
    {
        None = 0,

        Office = 1,
        Workshop = 2,
        Administrator = 4,

        HandpieceStoreOrder = 8,
        ClientCallback = 16,

        WorkshopSpecific = NotificationScope.Office | NotificationScope.Workshop | NotificationScope.Administrator,
        WorkshopIndependent = NotificationScope.HandpieceStoreOrder | NotificationScope.ClientCallback,

        Global = NotificationScope.Office | NotificationScope.Workshop | NotificationScope.Administrator | NotificationScope.HandpieceStoreOrder,
    }
}
