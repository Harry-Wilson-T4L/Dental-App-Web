using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class JobEntityComponentHelper
    {
        public static IReadOnlyList<JobEntityComponent> GetAllFlags()
        {
            return new JobEntityComponent[]
            {
                JobEntityComponent.Handpiece,
                JobEntityComponent.Invoice,
                JobEntityComponent.History,
            };
        }

        public static IReadOnlyList<JobEntityComponent> GetSupportedFlags(String jobTypeId)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => JobEntityComponentHelper.GetAllFlags(),
                JobTypes.Sale => JobEntityComponentHelper.GetAllFlags(),
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(JobEntityComponentHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this JobEntityComponent value)
        {
            return value switch
            {
                JobEntityComponent.All => "All",
                JobEntityComponent.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<JobEntityComponent> SplitValue(this JobEntityComponent value)
        {
            return JobEntityComponentHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static JobEntityComponent CombineValue(this IReadOnlyList<JobEntityComponent> values)
        {
            return values.Aggregate(JobEntityComponent.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this JobEntityComponent value)
        {
            return value switch
            {
                JobEntityComponent.Handpiece => "Handpiece",
                JobEntityComponent.Invoice => "Invoice",
                JobEntityComponent.History => "History",
                _ => value.ToString(),
            };
        }
    }
}
