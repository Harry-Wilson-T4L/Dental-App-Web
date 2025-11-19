using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class HandpieceStatusHelper
    {
        public static Int32 ToInternalVisualisationNumber(this HandpieceStatus handpieceStatus)
        {
            switch (handpieceStatus)
            {
                case HandpieceStatus.Received:
                    return 1;
                case HandpieceStatus.BeingEstimated:
                    return 2;
                case HandpieceStatus.WaitingForApproval:
                case HandpieceStatus.TbcHoldOn:
                    return 3;
                case HandpieceStatus.EstimateSent:
                    return 4;
                case HandpieceStatus.BeingRepaired:
                case HandpieceStatus.NeedsReApproval:
                case HandpieceStatus.WaitingForParts:
                    return 5;
                case HandpieceStatus.ReadyToReturn:
                    return 6;
                case HandpieceStatus.SentComplete:
                    return 7;
                case HandpieceStatus.Cancelled:
                case HandpieceStatus.Unrepairable:
                case HandpieceStatus.ReturnUnrepaired:
                case HandpieceStatus.TradeIn:
                default:
                    return -7;
            }
        }

        public static String ToDisplayString(this HandpieceStatus handpieceStatus)
        {
            switch (handpieceStatus)
            {
                case HandpieceStatus.Received:
                    return "Received";
                case HandpieceStatus.BeingEstimated:
                    return "Being estimated";
                case HandpieceStatus.WaitingForApproval:
                    return "Estimate complete";
                case HandpieceStatus.TbcHoldOn:
                    return "Tbc hold on";
                case HandpieceStatus.EstimateSent:
                    return "Estimate sent";
                case HandpieceStatus.BeingRepaired:
                    return "Being repaired";
                case HandpieceStatus.NeedsReApproval:
                    return "Needs approval";
                case HandpieceStatus.WaitingForParts:
                    return "Waiting for parts";
                case HandpieceStatus.TradeIn:
                    return "Trade-in";
                case HandpieceStatus.ReadyToReturn:
                    return "Ready to return";
                case HandpieceStatus.ReturnUnrepaired:
                    return "Return unrepaired";
                case HandpieceStatus.SentComplete:
                    return "Sent complete";
                case HandpieceStatus.Cancelled:
                    return "Cancelled";
                case HandpieceStatus.Unrepairable:
                    return "Unrepairable";
                default:
                    return "Unknown";
            }
        }

        public static String ToInternalStatusDescription(this HandpieceStatus handpieceStatus)
        {
            switch (handpieceStatus)
            {
                case HandpieceStatus.Received:
                    return "Received";
                case HandpieceStatus.BeingEstimated:
                    return "Being estimated";
                case HandpieceStatus.WaitingForApproval:
                    return "Estimate complete";
                case HandpieceStatus.TbcHoldOn:
                    return "Estimate complete (Tbc hold on)";
                case HandpieceStatus.EstimateSent:
                    return "Estimate sent";
                case HandpieceStatus.BeingRepaired:
                    return "Being repaired";
                case HandpieceStatus.NeedsReApproval:
                    return "Being repaired (Needs approval)";
                case HandpieceStatus.WaitingForParts:
                    return "Being repaired (Waiting for parts)";
                case HandpieceStatus.TradeIn:
                    return "Being repaired (Trade-in)";
                case HandpieceStatus.ReadyToReturn:
                    return "Ready to return";
                case HandpieceStatus.ReturnUnrepaired:
                    return "Ready to return (Return unrepaired)";
                case HandpieceStatus.SentComplete:
                    return "Sent complete";
                case HandpieceStatus.Cancelled:
                    return "Cancelled";
                case HandpieceStatus.Unrepairable:
                    return "Cancelled (unrepairable)";
                default:
                    return "Some Error";
            }
        }

        public static IEnumerable<HandpieceStatus> List()
        {
            return new HandpieceStatus[]
            {
                HandpieceStatus.Received,
                HandpieceStatus.BeingEstimated,
                HandpieceStatus.WaitingForApproval,
                HandpieceStatus.TbcHoldOn,
                HandpieceStatus.EstimateSent,
                HandpieceStatus.BeingRepaired,
                HandpieceStatus.NeedsReApproval,
                HandpieceStatus.WaitingForParts,
                HandpieceStatus.TradeIn,
                HandpieceStatus.ReadyToReturn,
                HandpieceStatus.ReturnUnrepaired,
                HandpieceStatus.SentComplete,
                HandpieceStatus.Cancelled,
                HandpieceStatus.Unrepairable,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList(IEnumerable<HandpieceStatus> availableOptions)
        {
            var optionsHashSet = new HashSet<HandpieceStatus>(availableOptions);
            return new SelectList(
                new[]
                {
                    Tuple.Create(HandpieceStatus.Received, "Received"),
                    Tuple.Create(HandpieceStatus.BeingEstimated, "Being estimated"),
                    Tuple.Create(HandpieceStatus.WaitingForApproval, "Estimate complete"),
                    Tuple.Create(HandpieceStatus.TbcHoldOn, "Tbc hold on"),
                    Tuple.Create(HandpieceStatus.EstimateSent, "Estimate sent"),
                    Tuple.Create(HandpieceStatus.BeingRepaired, "Being repaired"),
                    Tuple.Create(HandpieceStatus.NeedsReApproval, "Needs approval"),
                    Tuple.Create(HandpieceStatus.WaitingForParts, "Waiting for parts"),
                    Tuple.Create(HandpieceStatus.TradeIn, "Trade-in"),
                    Tuple.Create(HandpieceStatus.ReadyToReturn, "Ready to return"),
                    Tuple.Create(HandpieceStatus.ReturnUnrepaired, "Return unrepaired"),
                    Tuple.Create(HandpieceStatus.SentComplete, "Sent complete"),
                    Tuple.Create(HandpieceStatus.Cancelled, "Cancelled"),
                    Tuple.Create(HandpieceStatus.Unrepairable, "Unrepairable"),
                }.Where(x => optionsHashSet.Contains(x.Item1)), "Item1", "Item2");
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(HandpieceStatus.Received, "Received"),
                    Tuple.Create(HandpieceStatus.BeingEstimated, "Being estimated"),
                    Tuple.Create(HandpieceStatus.WaitingForApproval, "Estimate complete"),
                    Tuple.Create(HandpieceStatus.TbcHoldOn, "Tbc hold on"),
                    Tuple.Create(HandpieceStatus.EstimateSent, "Estimate sent"),
                    Tuple.Create(HandpieceStatus.BeingRepaired, "Being repaired"),
                    Tuple.Create(HandpieceStatus.NeedsReApproval, "Needs approval"),
                    Tuple.Create(HandpieceStatus.WaitingForParts, "Waiting for parts"),
                    Tuple.Create(HandpieceStatus.TradeIn, "Trade-in"),
                    Tuple.Create(HandpieceStatus.ReadyToReturn, "Ready to return"),
                    Tuple.Create(HandpieceStatus.ReturnUnrepaired, "Return unrepaired"),
                    Tuple.Create(HandpieceStatus.SentComplete, "Sent complete"),
                    Tuple.Create(HandpieceStatus.Cancelled, "Cancelled"),
                    Tuple.Create(HandpieceStatus.Unrepairable, "Unrepairable"),
                }, "Item1", "Item2");
        }

        public static ExternalHandpieceStatus ToExternal(this HandpieceStatus status)
        {
            switch (status)
            {
                case HandpieceStatus.Received:
                    return ExternalHandpieceStatus.Received;

                case HandpieceStatus.BeingEstimated:
                    return ExternalHandpieceStatus.BeingEstimated;

                case HandpieceStatus.WaitingForApproval:
                case HandpieceStatus.TbcHoldOn:
                    return ExternalHandpieceStatus.WaitingForApproval;

                case HandpieceStatus.EstimateSent:
                    return ExternalHandpieceStatus.EstimateSent;

                case HandpieceStatus.BeingRepaired:
                case HandpieceStatus.NeedsReApproval:
                case HandpieceStatus.WaitingForParts: 
                    return ExternalHandpieceStatus.BeingRepaired;

                case HandpieceStatus.ReadyToReturn:
                    return ExternalHandpieceStatus.ReadyForReturn;

                case HandpieceStatus.SentComplete:
                    return ExternalHandpieceStatus.SentComplete;

                case HandpieceStatus.Cancelled:
                case HandpieceStatus.Unrepairable:
                case HandpieceStatus.ReturnUnrepaired:
                case HandpieceStatus.TradeIn:
                    return ExternalHandpieceStatus.Cancel;

                case HandpieceStatus.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public static HandpieceStatus[] FromExternal(ExternalHandpieceStatus external)
        {
            switch (external)
            {
                case ExternalHandpieceStatus.Received:
                    return new[] { HandpieceStatus.Received };
                case ExternalHandpieceStatus.BeingEstimated:
                    return new[] { HandpieceStatus.BeingEstimated };
                case ExternalHandpieceStatus.WaitingForApproval:
                    return new[] { HandpieceStatus.WaitingForApproval, HandpieceStatus.TbcHoldOn };
                case ExternalHandpieceStatus.EstimateSent:
                    return new[] { HandpieceStatus.EstimateSent };
                case ExternalHandpieceStatus.BeingRepaired:
                    return new[] { HandpieceStatus.BeingRepaired, HandpieceStatus.NeedsReApproval, HandpieceStatus.WaitingForParts };
                case ExternalHandpieceStatus.ReadyForReturn:
                    return new[] { HandpieceStatus.ReadyToReturn };
                case ExternalHandpieceStatus.SentComplete:
                    return new[] { HandpieceStatus.SentComplete };
                case ExternalHandpieceStatus.Cancel:
                    return new[] { HandpieceStatus.Cancelled, HandpieceStatus.Unrepairable, HandpieceStatus.ReturnUnrepaired, HandpieceStatus.TradeIn };
                default:
                    throw new ArgumentOutOfRangeException(nameof(external), external, null);
            }
        }

        public static Boolean IsActive(this HandpieceStatus status)
        {
            return status.IsNotOneOf(HandpieceStatus.SentComplete, HandpieceStatus.Cancelled, HandpieceStatus.Unrepairable, HandpieceStatus.ReturnUnrepaired, HandpieceStatus.TradeIn);
        }
    }
}
