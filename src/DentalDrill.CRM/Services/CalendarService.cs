using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DentalDrill.CRM.Services
{
    public class CalendarService
    {
        private readonly IRepository repository;
        private readonly CalendarOptions options;
        private readonly ILogger logger;

        public CalendarService(IRepository repository, IOptions<CalendarOptions> options, ILogger<CalendarService> logger)
        {
            this.repository = repository;
            this.options = options.Value;
            this.logger = logger;
            this.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(this.options.TimeZone);
        }

        public TimeZoneInfo TimeZone { get; }

        public Task<CalendarWeek> GetCurrentWeekAsync()
        {
            return this.GetWeekAsync(DateTimeOffset.UtcNow);
        }

        public async Task<CalendarWeek> GetWeekAsync(Guid id)
        {
            var week = await this.repository.Query<CalendarWeek>("Year")
                .SingleOrDefaultAsync(x => x.Id == id);
            return week;
        }

        public async Task<CalendarWeek> GetWeekAsync(DateTimeOffset dateTime)
        {
            var weeks = await this.repository.Query<CalendarWeek>("Year")
                .Where(x => x.RangeStart <= dateTime && dateTime < x.RangeEnd)
                .ToListAsync();

            if (weeks.Count == 0)
            {
                this.logger.LogInformation($"No week found for date {dateTime:O}.");
                return null;
            }

            if (weeks.Count > 1)
            {
                this.logger.LogWarning($"Multiple weeks found for date {dateTime:O}.");
                return weeks[0];
            }

            return weeks[0];
        }

        public async Task<CalendarWeek> GetPreviousWeekAsync(CalendarWeek week)
        {
            return await this.repository.Query<CalendarWeek>("Year")
                .Where(x => x.RangeStart < week.RangeStart)
                .OrderByDescending(x => x.RangeStart)
                .FirstOrDefaultAsync();
        }

        public async Task<CalendarWeek> GetNextWeekAsync(CalendarWeek week)
        {
            return await this.repository.Query<CalendarWeek>("Year")
                .Where(x => x.RangeStart > week.RangeStart)
                .OrderBy(x => x.RangeStart)
                .FirstOrDefaultAsync();
        }

        public async Task SeedYearAsync(Int32 year)
        {
            var calendarYear = await this.repository.Query<CalendarYear>().SingleOrDefaultAsync(x => x.Year == year);
            if (calendarYear == null)
            {
                var yearStart = new DateTime(year, 1, 1, 0, 0, 0);
                var yearEnd = new DateTime(year + 1, 1, 1, 0, 0, 0);

                calendarYear = new CalendarYear
                {
                    Id = Guid.NewGuid(),
                    Year = year,
                    RangeStart = this.ConvertToOffset(yearStart),
                    RangeEnd = this.ConvertToOffset(yearEnd),
                };

                await this.repository.InsertAsync(calendarYear);

                var weeks = this.GenerateWeeks(year);
                foreach (var week in weeks)
                {
                    week.YearId = calendarYear.Id;
                    await this.repository.InsertAsync(week);
                }

                await this.repository.SaveChangesAsync();
            }
        }

        private List<CalendarWeek> GenerateWeeks(Int32 year)
        {
            // Looking up 1st Thu of the year
            var start = new DateTime(year, 1, 1, 0, 0, 0);
            start = GetClosestDayOfWeek(start, DayOfWeek.Thursday);

            var result = new List<CalendarWeek>();
            var weekNumber = 1;
            while (start.Year == year)
            {
                var weekStart = start.AddDays(-3);
                var weekEnd = start.AddDays(4);

                result.Add(new CalendarWeek
                {
                    Id = Guid.NewGuid(),
                    Week = weekNumber,
                    RangeStart = this.ConvertToOffset(weekStart),
                    RangeEnd = this.ConvertToOffset(weekEnd),
                });

                start = start.AddDays(7);
                weekNumber++;
            }

            return result;

            DateTime GetClosestDayOfWeek(DateTime startingDateTime, DayOfWeek dayOfWeek)
            {
                while (startingDateTime.DayOfWeek != dayOfWeek)
                {
                    startingDateTime = startingDateTime.AddDays(1);
                }

                return startingDateTime;
            }
        }

        private DateTimeOffset ConvertToOffset(DateTime dateTime)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, this.TimeZone);
            var offset = dateTime - utcTime;

            return new DateTimeOffset(dateTime, offset);
        }
    }
}
