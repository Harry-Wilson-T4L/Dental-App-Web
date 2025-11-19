using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class FeedbackConfigurationPeriodTypeHelper
    {
        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(new[]
            {
                Tuple.Create(FeedbackConfigurationPeriodType.Day, "Day"),
                Tuple.Create(FeedbackConfigurationPeriodType.Week, "Week"),
                Tuple.Create(FeedbackConfigurationPeriodType.Month, "Month"),
                Tuple.Create(FeedbackConfigurationPeriodType.Quarter, "Quarter"),
                Tuple.Create(FeedbackConfigurationPeriodType.Year, "Year"),
            }, "Item1", "Item2");
        }

        public static String FormatNameForLength(this FeedbackConfigurationPeriodType value, Int32 length)
        {
            if (length == 1)
            {
                return value switch
                {
                    FeedbackConfigurationPeriodType.Day => "day",
                    FeedbackConfigurationPeriodType.Week => "week",
                    FeedbackConfigurationPeriodType.Month => "month",
                    FeedbackConfigurationPeriodType.Quarter => "quarter",
                    FeedbackConfigurationPeriodType.Year => "year",
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
                };
            }
            else
            {
                return value switch
                {
                    FeedbackConfigurationPeriodType.Day => "days",
                    FeedbackConfigurationPeriodType.Week => "weeks",
                    FeedbackConfigurationPeriodType.Month => "months",
                    FeedbackConfigurationPeriodType.Quarter => "quarters",
                    FeedbackConfigurationPeriodType.Year => "years",
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
                };
            }
        }
    }
}
