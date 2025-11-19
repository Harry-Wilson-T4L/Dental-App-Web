using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum ClientEntityField : Int64
    {
        None = 0,

        ClientNo = (1 << 0),
        Name = (1 << 1),
        PrincipalDentist = (1 << 2),
        Corporate = (1 << 3),

        MainEmail = (1 << 4),
        OtherEmails = (1 << 5),
        MainPhone = (1 << 6),
        OtherPhones = (1 << 7),

        Address = (1 << 8),

        OtherContact = (1 << 9),
        OpeningHours = (1 << 10),
        Brands = (1 << 11),
        PricingCategory = (1 << 12),
        Comment = (1 << 13),

        PrimaryWorkshop = (1 << 14),

        All = (1 << 15) - 1,
    }
}
