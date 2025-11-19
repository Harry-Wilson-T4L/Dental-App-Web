using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class JobStatusHelper
    {
        public static List<JobStatus> List()
        {
            return new List<JobStatus>
            {
                JobStatus.Received,
                JobStatus.BeingEstimated,
                JobStatus.WaitingForApproval,
                JobStatus.EstimateSent,
                JobStatus.BeingRepaired,
                JobStatus.ReadyToReturn,
                JobStatus.SentComplete,
                JobStatus.Cancelled,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(JobStatus.Received, "Received"),
                    Tuple.Create(JobStatus.BeingEstimated, "Being estimated"),
                    Tuple.Create(JobStatus.WaitingForApproval, "Estimate complete"),
                    Tuple.Create(JobStatus.EstimateSent, "Estimate sent"),
                    Tuple.Create(JobStatus.BeingRepaired, "Being repaired"),
                    Tuple.Create(JobStatus.ReadyToReturn, "Ready to return"),
                    Tuple.Create(JobStatus.SentComplete, "Sent complete"),
                    Tuple.Create(JobStatus.Cancelled, "Cancelled"),
                }, "Item1", "Item2");
        }

        public static String ToDisplayString(this JobStatus status)
        {
            switch (status)
            {
                case JobStatus.Unknown:
                    return "Unknown";
                case JobStatus.Received:
                    return "Received";
                case JobStatus.BeingEstimated:
                    return "Being estimated";
                case JobStatus.WaitingForApproval:
                    return "Estimate complete";
                case JobStatus.EstimateSent:
                    return "Estimate sent";
                case JobStatus.BeingRepaired:
                    return "Being repaired";
                case JobStatus.ReadyToReturn:
                    return "Ready to return";
                case JobStatus.SentComplete:
                    return "Sent complete";
                case JobStatus.Cancelled:
                    return "Cancelled";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public static Int32 ToIndicatorValue(this JobStatus status)
        {
            switch (status)
            {
                case JobStatus.Unknown:
                    return 0;
                case JobStatus.Received:
                    return 1;
                case JobStatus.BeingEstimated:
                    return 2;
                case JobStatus.WaitingForApproval:
                    return 3;
                case JobStatus.EstimateSent:
                    return 4;
                case JobStatus.BeingRepaired:
                    return 5;
                case JobStatus.ReadyToReturn:
                    return 6;
                case JobStatus.SentComplete:
                    return 7;
                case JobStatus.Cancelled:
                    return -7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public static IReadOnlyList<JobStatus> GetSupportedStatuses(String jobTypeId)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => new JobStatus[]
                {
                    JobStatus.Received,
                    JobStatus.BeingEstimated,
                    JobStatus.WaitingForApproval,
                    JobStatus.EstimateSent,
                    JobStatus.BeingRepaired,
                    JobStatus.ReadyToReturn,
                    JobStatus.SentComplete,
                    JobStatus.Cancelled,
                },
                JobTypes.Sale => new JobStatus[]
                {
                    JobStatus.SentComplete,
                    JobStatus.Cancelled,
                },
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }
    }
}
