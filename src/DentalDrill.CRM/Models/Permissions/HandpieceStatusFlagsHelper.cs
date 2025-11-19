using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class HandpieceStatusFlagsHelper
    {
        public static IReadOnlyList<HandpieceStatusFlags> GetAllFlags()
        {
            return new HandpieceStatusFlags[]
            {
                HandpieceStatusFlags.Received,
                HandpieceStatusFlags.BeingEstimated,
                HandpieceStatusFlags.WaitingForApproval,
                HandpieceStatusFlags.TbcHoldOn,
                HandpieceStatusFlags.NeedsReApproval,
                HandpieceStatusFlags.EstimateSent,
                HandpieceStatusFlags.BeingRepaired,
                HandpieceStatusFlags.WaitingForParts,
                HandpieceStatusFlags.TradeIn,
                HandpieceStatusFlags.ReadyToReturn,
                HandpieceStatusFlags.ReturnUnrepaired,
                HandpieceStatusFlags.SentComplete,
                HandpieceStatusFlags.Cancelled,
                HandpieceStatusFlags.Unrepairable,
            };
        }

        public static IReadOnlyList<HandpieceStatusFlags> GetSupportedFlags(String jobTypeId)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => HandpieceStatusFlagsHelper.GetAllFlags(),
                JobTypes.Sale => (HandpieceStatusFlags.SentComplete | HandpieceStatusFlags.Cancelled).SplitValue(),
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static HandpieceStatusFlags GetSupportedHandpieceTransitionsFrom(String jobTypeId, JobStatus jobStatus)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => jobStatus switch
                {
                    JobStatus.Received => HandpieceStatusFlags.None,
                    JobStatus.BeingEstimated => HandpieceStatusFlags.BeingEstimated | HandpieceStatusFlags.WaitingForApproval,
                    JobStatus.WaitingForApproval => HandpieceStatusFlags.WaitingForApproval | HandpieceStatusFlags.EstimateSent | HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.TbcHoldOn | HandpieceStatusFlags.ReturnUnrepaired | HandpieceStatusFlags.TradeIn | HandpieceStatusFlags.Cancelled,
                    JobStatus.EstimateSent => HandpieceStatusFlags.EstimateSent | HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.TbcHoldOn | HandpieceStatusFlags.ReturnUnrepaired | HandpieceStatusFlags.TradeIn | HandpieceStatusFlags.Cancelled,
                    JobStatus.BeingRepaired => HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.NeedsReApproval | HandpieceStatusFlags.ReadyToReturn | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.Unrepairable | HandpieceStatusFlags.TbcHoldOn | HandpieceStatusFlags.TradeIn | HandpieceStatusFlags.ReturnUnrepaired | HandpieceStatusFlags.Cancelled,
                    JobStatus.ReadyToReturn => HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.NeedsReApproval | HandpieceStatusFlags.ReadyToReturn | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.Unrepairable | HandpieceStatusFlags.TbcHoldOn | HandpieceStatusFlags.TradeIn | HandpieceStatusFlags.ReturnUnrepaired | HandpieceStatusFlags.Cancelled,
                    JobStatus.SentComplete => HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.ReadyToReturn | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.Unrepairable | HandpieceStatusFlags.TbcHoldOn | HandpieceStatusFlags.TradeIn | HandpieceStatusFlags.ReturnUnrepaired | HandpieceStatusFlags.Cancelled,
                    JobStatus.Cancelled => HandpieceStatusFlags.None,
                    _ => throw new NotSupportedException($"Job status {jobStatus} is not supported for job type {jobTypeId}"),
                },
                JobTypes.Sale => jobStatus switch
                {
                    JobStatus.SentComplete => HandpieceStatusFlags.SentComplete | HandpieceStatusFlags.Cancelled,
                    JobStatus.Cancelled => HandpieceStatusFlags.None,
                    _ => throw new NotSupportedException($"Job status {jobStatus} is not supported for job type {jobTypeId}"),
                },
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static HandpieceStatusFlags GetSupportedHandpieceTransitionsTo(String jobTypeId, JobStatus jobStatus)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => jobStatus switch
                {
                    JobStatus.Received => HandpieceStatusFlags.None,
                    JobStatus.BeingEstimated => HandpieceStatusFlags.BeingEstimated | HandpieceStatusFlags.WaitingForApproval,
                    JobStatus.WaitingForApproval => HandpieceStatusFlags.WaitingForApproval | HandpieceStatusFlags.EstimateSent | HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.TbcHoldOn | HandpieceStatusFlags.Cancelled | HandpieceStatusFlags.ReturnUnrepaired | HandpieceStatusFlags.TradeIn,
                    JobStatus.EstimateSent => HandpieceStatusFlags.EstimateSent | HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.TbcHoldOn | HandpieceStatusFlags.Cancelled | HandpieceStatusFlags.ReturnUnrepaired | HandpieceStatusFlags.TradeIn,
                    JobStatus.BeingRepaired => HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.ReadyToReturn | HandpieceStatusFlags.NeedsReApproval | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.Cancelled,
                    JobStatus.ReadyToReturn => HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.ReadyToReturn | HandpieceStatusFlags.SentComplete | HandpieceStatusFlags.NeedsReApproval | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.Cancelled,
                    JobStatus.SentComplete => HandpieceStatusFlags.BeingRepaired | HandpieceStatusFlags.ReadyToReturn | HandpieceStatusFlags.SentComplete | HandpieceStatusFlags.WaitingForParts | HandpieceStatusFlags.Unrepairable | HandpieceStatusFlags.Cancelled,
                    JobStatus.Cancelled => HandpieceStatusFlags.None,
                    _ => throw new NotSupportedException($"Job status {jobStatus} is not supported for job type {jobTypeId}"),
                },
                JobTypes.Sale => jobStatus switch
                {
                    JobStatus.SentComplete => HandpieceStatusFlags.SentComplete | HandpieceStatusFlags.Cancelled,
                    JobStatus.Cancelled => HandpieceStatusFlags.None,
                    _ => throw new NotSupportedException($"Job status {jobStatus} is not supported for job type {jobTypeId}"),
                },
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(HandpieceStatusFlagsHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this HandpieceStatusFlags value)
        {
            return value switch
            {
                HandpieceStatusFlags.All => "All",
                HandpieceStatusFlags.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<HandpieceStatusFlags> SplitValue(this HandpieceStatusFlags value)
        {
            return HandpieceStatusFlagsHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static HandpieceStatusFlags CombineValue(this IReadOnlyList<HandpieceStatusFlags> values)
        {
            return values.Aggregate(HandpieceStatusFlags.None, (acc, x) => acc | x);
        }

        public static HandpieceStatusFlags ToFlag(this HandpieceStatus value)
        {
            return value switch
            {
                HandpieceStatus.Received => HandpieceStatusFlags.Received,
                HandpieceStatus.BeingEstimated => HandpieceStatusFlags.BeingEstimated,
                HandpieceStatus.WaitingForApproval => HandpieceStatusFlags.WaitingForApproval,
                HandpieceStatus.TbcHoldOn => HandpieceStatusFlags.TbcHoldOn,
                HandpieceStatus.NeedsReApproval => HandpieceStatusFlags.NeedsReApproval,
                HandpieceStatus.EstimateSent => HandpieceStatusFlags.EstimateSent,
                HandpieceStatus.BeingRepaired => HandpieceStatusFlags.BeingRepaired,
                HandpieceStatus.WaitingForParts => HandpieceStatusFlags.WaitingForParts,
                HandpieceStatus.TradeIn => HandpieceStatusFlags.TradeIn,
                HandpieceStatus.ReadyToReturn => HandpieceStatusFlags.ReadyToReturn,
                HandpieceStatus.ReturnUnrepaired => HandpieceStatusFlags.ReturnUnrepaired,
                HandpieceStatus.SentComplete => HandpieceStatusFlags.SentComplete,
                HandpieceStatus.Cancelled => HandpieceStatusFlags.Cancelled,
                HandpieceStatus.Unrepairable => HandpieceStatusFlags.Unrepairable,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }

        private static String ToFlagDisplayString(this HandpieceStatusFlags value)
        {
            return value switch
            {
                HandpieceStatusFlags.Received => HandpieceStatus.Received.ToDisplayString(),
                HandpieceStatusFlags.BeingEstimated => HandpieceStatus.BeingEstimated.ToDisplayString(),
                HandpieceStatusFlags.WaitingForApproval => HandpieceStatus.WaitingForApproval.ToDisplayString(),
                HandpieceStatusFlags.TbcHoldOn => HandpieceStatus.TbcHoldOn.ToDisplayString(),
                HandpieceStatusFlags.NeedsReApproval => HandpieceStatus.NeedsReApproval.ToDisplayString(),
                HandpieceStatusFlags.EstimateSent => HandpieceStatus.EstimateSent.ToDisplayString(),
                HandpieceStatusFlags.BeingRepaired => HandpieceStatus.BeingRepaired.ToDisplayString(),
                HandpieceStatusFlags.WaitingForParts => HandpieceStatus.WaitingForParts.ToDisplayString(),
                HandpieceStatusFlags.TradeIn => HandpieceStatus.TradeIn.ToDisplayString(),
                HandpieceStatusFlags.ReadyToReturn => HandpieceStatus.ReadyToReturn.ToDisplayString(),
                HandpieceStatusFlags.ReturnUnrepaired => HandpieceStatus.ReturnUnrepaired.ToDisplayString(),
                HandpieceStatusFlags.SentComplete => HandpieceStatus.SentComplete.ToDisplayString(),
                HandpieceStatusFlags.Cancelled => HandpieceStatus.Cancelled.ToDisplayString(),
                HandpieceStatusFlags.Unrepairable => HandpieceStatus.Unrepairable.ToDisplayString(),
                _ => value.ToString(),
            };
        }
    }
}
