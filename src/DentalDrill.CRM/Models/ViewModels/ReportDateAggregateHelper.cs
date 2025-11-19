using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.ViewModels
{
    public static class ReportDateAggregateHelper
    {
        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(ReportDateAggregate.EntirePeriod, "Entire period"),
                    Tuple.Create(ReportDateAggregate.Yearly, "Yearly"),
                    Tuple.Create(ReportDateAggregate.Quarterly, "Quarterly"),
                    Tuple.Create(ReportDateAggregate.Monthly, "Monthly"),
                    Tuple.Create(ReportDateAggregate.Weekly, "Weekly"),
                    Tuple.Create(ReportDateAggregate.Daily, "Daily"),
                },
                "Item1",
                "Item2");
        }

        public static IReadOnlyList<String> GenerateDateRanges(DateTime from, DateTime to, ReportDateAggregate dateAggregate)
        {
            DateTime GetWeekFirstDay(DateTime dateTime)
            {
                switch (dateTime.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return dateTime;
                    case DayOfWeek.Tuesday:
                        return dateTime.AddDays(-1);
                    case DayOfWeek.Wednesday:
                        return dateTime.AddDays(-2);
                    case DayOfWeek.Thursday:
                        return dateTime.AddDays(-3);
                    case DayOfWeek.Friday:
                        return dateTime.AddDays(-4);
                    case DayOfWeek.Saturday:
                        return dateTime.AddDays(-5);
                    case DayOfWeek.Sunday:
                        return dateTime.AddDays(-6);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            switch (dateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                    return new[] { "*" };

                case ReportDateAggregate.Yearly:
                {
                    var result = new List<String>();
                    for (var year = from.Year; year <= to.Year; year++)
                    {
                        result.Add($"{year}");
                    }

                    return result;
                }

                case ReportDateAggregate.Quarterly:
                {
                    var result = new List<String>();
                    var fromQuarterNo = (from.Month - 1) / 3;
                    var quarterStart = new DateTime(from.Year, fromQuarterNo * 3 + 1, 1);
                    for (; quarterStart <= to; quarterStart = quarterStart.AddMonths(3))
                    {
                        result.Add($"{quarterStart.Year}-Q{(quarterStart.Month - 1) / 3 + 1}");
                    }

                    return result;
                }

                case ReportDateAggregate.Monthly:
                {
                    var result = new List<String>();
                    for (var monthStart = new DateTime(from.Year, from.Month, 1); monthStart <= to; monthStart = monthStart.AddMonths(1))
                    {
                        result.Add($"{monthStart.Year}-{monthStart.Month}");
                    }

                    return result;
                }

                case ReportDateAggregate.Weekly:
                {
                    var result = new List<String>();
                    for (var weekStart = GetWeekFirstDay(from); weekStart <= to; weekStart = weekStart.AddDays(7))
                    {
                        var thursday = weekStart.AddDays(3);
                        var weekNo = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(thursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                        result.Add($"{thursday.Year}-W{weekNo}");
                    }

                    return result;
                }

                case ReportDateAggregate.Daily:
                {
                    var result = new List<String>();
                    for (var dateStart = from; dateStart <= to; dateStart = dateStart.AddDays(1))
                    {
                        result.Add($"{dateStart.Year}-{dateStart.Month}-{dateStart.Day}");
                    }

                    return result;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
