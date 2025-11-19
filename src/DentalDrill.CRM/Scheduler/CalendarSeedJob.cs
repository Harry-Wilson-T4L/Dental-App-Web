using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DentalDrill.CRM.Scheduler
{
    [DisallowConcurrentExecution]
    public class CalendarSeedJob : IJob
    {
        private readonly CalendarService calendarService;
        private readonly ILogger logger;

        public CalendarSeedJob(CalendarService calendarService, ILogger<CalendarSeedJob> logger)
        {
            this.calendarService = calendarService;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var date = DateTime.UtcNow;
                await this.calendarService.SeedYearAsync(date.Year);
                if (date.Month >= 10)
                {
                    await this.calendarService.SeedYearAsync(date.Year + 1);
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
            }
        }
    }
}
