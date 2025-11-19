using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum HandpieceEntityField : Int64
    {
        None = 0,

        Brand = (1 << 0),
        Model = (1 << 1),
        Serial = (1 << 2),
        Components = (1 << 3),
        Rating = (1 << 4),
        SpeedType = (1 << 5),
        Speed = (1 << 6),
        Parts = (1 << 7),
        PartsOutOfStock = (1 << 8),
        PartsComment = (1 << 9),
        PartsOrdered = (1 << 10),
        PartsRestocked = (1 << 11),
        ProblemDescription = (1 << 12),
        DiagnosticReport = (1 << 13),
        ReturnBy = (1 << 14),
        CostOfRepair = (1 << 15),
        ServiceLevel = (1 << 16),
        InternalComment = (1 << 17),
        PublicComment = (1 << 18),
        EstimatedBy = (1 << 19),

        All = (1 << 20) - 1,
    }
}
