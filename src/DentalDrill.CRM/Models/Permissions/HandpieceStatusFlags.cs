using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum HandpieceStatusFlags : Int64
    {
        None = 0,

        Received = (1 << 0),
        BeingEstimated = (1 << 1),
        WaitingForApproval = (1 << 2),
        TbcHoldOn = (1 << 3),
        NeedsReApproval = (1 << 4),
        EstimateSent = (1 << 5),
        BeingRepaired = (1 << 6),
        WaitingForParts = (1 << 7),
        TradeIn = (1 << 8),
        ReadyToReturn = (1 << 9),
        ReturnUnrepaired = (1 << 10),
        SentComplete = (1 << 11),
        Cancelled = (1 << 12),
        Unrepairable = (1 << 13),

        All = (1 << 14) - 1,
    }
}
