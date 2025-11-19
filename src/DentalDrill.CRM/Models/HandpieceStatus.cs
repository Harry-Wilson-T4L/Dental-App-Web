using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum HandpieceStatus
    {
        None = 0,

        Received = 10,

        BeingEstimated = 20,

        WaitingForApproval = 30,
        TbcHoldOn = 31,
        NeedsReApproval = 32,

        EstimateSent = 35,

        BeingRepaired = 40,
        WaitingForParts = 41,
        TradeIn = 42,

        ReadyToReturn = 50,
        ReturnUnrepaired = 51,

        SentComplete = 60,

        Cancelled = 70,
        Unrepairable = 71,
    }
}
