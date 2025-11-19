using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class JobEntityFieldHelper
    {
        public static IReadOnlyList<JobEntityField> GetAllFlags()
        {
            return new JobEntityField[]
            {
                JobEntityField.Client,
                JobEntityField.Workshop,
                JobEntityField.Received,
                JobEntityField.Comment,
                JobEntityField.HasWarning,
                JobEntityField.Creator,
                JobEntityField.ApprovedBy,
                JobEntityField.ApprovedOn,
                JobEntityField.RatePlan,
            };
        }

        public static IReadOnlyList<JobEntityField> GetSupportedFlags(String jobTypeId)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => JobEntityFieldHelper.GetAllFlags(),
                JobTypes.Sale => JobEntityFieldHelper.GetAllFlags(),
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(JobEntityFieldHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this JobEntityField value)
        {
            return value switch
            {
                JobEntityField.All => "All",
                JobEntityField.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<JobEntityField> SplitValue(this JobEntityField value)
        {
            return JobEntityFieldHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static JobEntityField CombineValue(this IReadOnlyList<JobEntityField> values)
        {
            return values.Aggregate(JobEntityField.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this JobEntityField value)
        {
            return value switch
            {
                JobEntityField.Client => "Client",
                JobEntityField.Workshop => "Workshop",
                JobEntityField.Received => "Received",
                JobEntityField.Comment => "Comment",
                JobEntityField.HasWarning => "Has warning",
                JobEntityField.ApprovedBy => "Approved by",
                JobEntityField.ApprovedOn => "Approved on",
                JobEntityField.RatePlan => "Rate plan",
                JobEntityField.Creator => "Creator",
                _ => value.ToString(),
            };
        }
    }
}
