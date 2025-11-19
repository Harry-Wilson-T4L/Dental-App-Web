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
    [DisallowConcurrentExecution]
    public class CallbackNotificationActivationJob : IJob
    {
        private readonly IRepository repository;
        private readonly CalendarService calendarService;
        private readonly CallbackService callbackService;
        private readonly ILogger logger;

        public CallbackNotificationActivationJob(IRepository repository, CalendarService calendarService, CallbackService callbackService, ILogger<CallbackNotificationActivationJob> logger)
        {
            this.repository = repository;
            this.calendarService = calendarService;
            this.callbackService = callbackService;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.calendarService.TimeZone);
                var callbacksToActivate = await this.repository.Query<ClientCallbackNotification>()
                    .Where(x => x.Status == ClientCallbackNotificationStatus.New && (x.CallDateTime != null && x.CallDateTime < date))
                    .ToListAsync();

                foreach (var callback in callbacksToActivate)
                {
                    callback.Status = ClientCallbackNotificationStatus.Active;
                    await this.repository.UpdateAsync(callback);
                    await this.repository.SaveChangesAsync();

                    if (callback.AssignedToId.HasValue)
                    {
                        await this.callbackService.NotifyAssignedEmployee(callback.AssignedToId.Value);
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
            }
        }
    }
}
