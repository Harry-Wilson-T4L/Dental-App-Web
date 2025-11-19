using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ReportDateRange
    {
        public ReportDateRange(DateTime from, DateTime to, String title)
        {
            this.From = from;
            this.To = to;
            this.Title = title;
        }

        public DateTime From { get; }

        public DateTime To { get; }

        public String Title { get; }

        public static ReportDateRange SingleDay(DateTime day, String title)
        {
            return new ReportDateRange(day, day, title);
        }

        public static ReportDateRange SpecificNumberOfPreviousDays(DateTime day, Int32 numberOfDays, String title)
        {
            return new ReportDateRange(day.AddDays(-numberOfDays), day, title);
        }

        public static ReportDateRange SingleWeekUpTo(DateTime day, String title)
        {
            var offset = (Int32)day.DayOfWeek;
            offset = offset == 0 ? 6 : offset - 1;
            return new ReportDateRange(day.AddDays(-offset), day, title);
        }

        public static ReportDateRange SingleMonthUpTo(DateTime day, String title)
        {
            return new ReportDateRange(new DateTime(day.Year, day.Month, 1), day, title);
        }

        public static ReportDateRange SingleMonth(Int32 year, Int32 month, String title)
        {
            while (month > 12)
            {
                month -= 12;
                year += 1;
            }

            while (month < 1)
            {
                month += 12;
                year -= 1;
            }

            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            return new ReportDateRange(start, end, title);
        }

        public static ReportDateRange QuarterUpTo(DateTime day, String title)
        {
            var month = (day.Month - 1) / 3 * 3 + 1;
            return new ReportDateRange(new DateTime(day.Year, month, 1), day, title);
        }

        public static ReportDateRange YearUpTo(DateTime day, String title)
        {
            return new ReportDateRange(new DateTime(day.Year, 1, 1), day, title);
        }
    }
}
