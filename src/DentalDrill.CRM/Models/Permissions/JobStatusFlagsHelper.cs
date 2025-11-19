using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class JobStatusFlagsHelper
    {
        public static IReadOnlyList<JobStatusFlags> GetAllFlags()
        {
            return new JobStatusFlags[]
            {
                JobStatusFlags.Received,
                JobStatusFlags.BeingEstimated,
                JobStatusFlags.WaitingForApproval,
                JobStatusFlags.EstimateSent,
                JobStatusFlags.BeingRepaired,
                JobStatusFlags.ReadyToReturn,
                JobStatusFlags.SentComplete,
                JobStatusFlags.Cancelled,
            };
        }

        public static IReadOnlyList<JobStatusFlags> GetSupportedFlags(String jobTypeId)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => new JobStatusFlags[]
                {
                    JobStatusFlags.Received,
                    JobStatusFlags.BeingEstimated,
                    JobStatusFlags.WaitingForApproval,
                    JobStatusFlags.EstimateSent,
                    JobStatusFlags.BeingRepaired,
                    JobStatusFlags.ReadyToReturn,
                    JobStatusFlags.SentComplete,
                    JobStatusFlags.Cancelled,
                },
                JobTypes.Sale => new JobStatusFlags[]
                {
                    JobStatusFlags.SentComplete,
                    JobStatusFlags.Cancelled,
                },
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static JobStatusFlags GetSupportedJobTransitions(String jobTypeId, JobStatus jobStatus)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => jobStatus switch
                {
                    JobStatus.Received => JobStatusFlags.BeingEstimated,
                    JobStatus.BeingEstimated => JobStatusFlags.WaitingForApproval,
                    JobStatus.WaitingForApproval => JobStatusFlags.EstimateSent | JobStatusFlags.BeingRepaired | JobStatusFlags.ReadyToReturn | JobStatusFlags.Cancelled,
                    JobStatus.EstimateSent => JobStatusFlags.BeingRepaired | JobStatusFlags.ReadyToReturn | JobStatusFlags.Cancelled,
                    JobStatus.BeingRepaired => JobStatusFlags.ReadyToReturn | JobStatusFlags.Cancelled,
                    JobStatus.ReadyToReturn => JobStatusFlags.SentComplete | JobStatusFlags.Cancelled,
                    JobStatus.SentComplete => JobStatusFlags.None,
                    JobStatus.Cancelled => JobStatusFlags.WaitingForApproval,
                    _ => throw new NotSupportedException($"Job status {jobStatus} is not supported for job type {jobTypeId}"),
                },
                JobTypes.Sale => jobStatus switch
                {
                    JobStatus.SentComplete => JobStatusFlags.Cancelled,
                    JobStatus.Cancelled => JobStatusFlags.SentComplete,
                    _ => throw new NotSupportedException($"Job status {jobStatus} is not supported for job type {jobTypeId}"),
                },
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(JobStatusFlagsHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this JobStatusFlags value)
        {
            return value switch
            {
                JobStatusFlags.All => "All",
                JobStatusFlags.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<JobStatusFlags> SplitValue(this JobStatusFlags value)
        {
            return JobStatusFlagsHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static JobStatusFlags CombineValue(this IReadOnlyList<JobStatusFlags> values)
        {
            return values.Aggregate(JobStatusFlags.None, (acc, x) => acc | x);
        }

        public static JobStatusFlags ToFlag(this JobStatus value)
        {
            return value switch
            {
                JobStatus.Received => JobStatusFlags.Received,
                JobStatus.BeingEstimated => JobStatusFlags.BeingEstimated,
                JobStatus.WaitingForApproval => JobStatusFlags.WaitingForApproval,
                JobStatus.EstimateSent => JobStatusFlags.EstimateSent,
                JobStatus.BeingRepaired => JobStatusFlags.BeingRepaired,
                JobStatus.ReadyToReturn => JobStatusFlags.ReadyToReturn,
                JobStatus.SentComplete => JobStatusFlags.SentComplete,
                JobStatus.Cancelled => JobStatusFlags.Cancelled,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }

        private static String ToFlagDisplayString(this JobStatusFlags value)
        {
            return value switch
            {
                JobStatusFlags.Received => JobStatus.Received.ToDisplayString(),
                JobStatusFlags.BeingEstimated => JobStatus.BeingEstimated.ToDisplayString(),
                JobStatusFlags.WaitingForApproval => JobStatus.WaitingForApproval.ToDisplayString(),
                JobStatusFlags.EstimateSent => JobStatus.EstimateSent.ToDisplayString(),
                JobStatusFlags.BeingRepaired => JobStatus.BeingRepaired.ToDisplayString(),
                JobStatusFlags.ReadyToReturn => JobStatus.ReadyToReturn.ToDisplayString(),
                JobStatusFlags.SentComplete => JobStatus.SentComplete.ToDisplayString(),
                JobStatusFlags.Cancelled => JobStatus.Cancelled.ToDisplayString(),
                _ => value.ToString(),
            };
        }
    }
}
