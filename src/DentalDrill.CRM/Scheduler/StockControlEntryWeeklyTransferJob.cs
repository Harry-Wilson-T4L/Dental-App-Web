using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DentalDrill.CRM.Scheduler
{
    public class StockControlEntryWeeklyTransferJob : IJob
    {
        private readonly CalendarService calendarService;
        private readonly IRepository repository;
        private readonly ILogger logger;

        public StockControlEntryWeeklyTransferJob(CalendarService calendarService, IRepository repository, ILogger<StockControlEntryWeeklyTransferJob> logger)
        {
            this.calendarService = calendarService;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var currentWeek = await this.calendarService.GetCurrentWeekAsync();
                var previousActive = await this.repository.Query<StockControlEntry>()
                    .Where(x => x.Status == StockControlEntryStatus.Active && x.WeekId != currentWeek.Id && x.Week.RangeStart < currentWeek.RangeStart)
                    .ToListAsync();

                foreach (var entry in previousActive)
                {
                    this.logger.LogInformation($"Automatically transferring StockControlEntry('{entry.Id}') to Week('{currentWeek.Id}')");
                    entry.WeekId = currentWeek.Id;
                    await this.repository.UpdateAsync(entry);
                    await this.repository.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
            }
        }
    }
}
