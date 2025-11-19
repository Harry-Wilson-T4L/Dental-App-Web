using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum ClientEntityComponent : Int64
    {
        None = 0,

        Client = (1 << 0),
        Note = (1 << 1),
        Callback = (1 << 2),
        Repair = (1 << 3),
        Email = (1 << 4),
        Invoice = (1 << 5),
        Feedback = (1 << 6),
        Attachment = (1 << 7),
        Appearance = (1 << 8),
        Access = (1 << 9),

        All = (1 << 10) - 1,
    }
}
