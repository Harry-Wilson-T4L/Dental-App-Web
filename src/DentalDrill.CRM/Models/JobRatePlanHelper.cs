using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class JobRatePlanHelper
    {
        public static String ToDisplayString(this JobRatePlan value)
        {
            switch (value)
            {
                case JobRatePlan.Default:
                    return "Default";
                case JobRatePlan.Discount5:
                    return "5% discount";
                case JobRatePlan.Discount10:
                    return "10% discount";
                case JobRatePlan.Custom:
                    return "Custom";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(JobRatePlan.Default, "Default"),
                    Tuple.Create(JobRatePlan.Discount5, "5% discount"),
                    Tuple.Create(JobRatePlan.Discount10, "10% discount"),
                    Tuple.Create(JobRatePlan.Custom, "Custom"),
                }, "Item1", "Item2");
        }
    }
}
