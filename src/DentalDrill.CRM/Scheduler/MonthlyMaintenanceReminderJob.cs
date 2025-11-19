using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DentalDrill.CRM.Scheduler
{
    [DisallowConcurrentExecution]
    public class MonthlyMaintenanceReminderJob : Quartz.IJob
    {
        private readonly IClientManager clientManager;
        private readonly IRepository repository;
        private readonly ILogger logger;

        public MonthlyMaintenanceReminderJob(IClientManager clientManager, IRepository repository, ILogger<MonthlyMaintenanceReminderJob> logger)
        {
            this.clientManager = clientManager;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                this.logger.LogInformation($"Starting monthly maintenance reminder job");
                var reminders = await this.clientManager.RepairHistory.GetPendingRemindersAsync();
                this.logger.LogInformation($"{reminders} client(s) have pending reminders");

                foreach (var reminder in reminders)
                {
                    this.logger.LogInformation($"Processing reminder for client {reminder.Client.Entity.ClientNo}: {reminder.Client.Entity.FullName}");

                    try
                    {
                        await reminder.SendEmailAsync();
                    }
                    catch (Exception sendException)
                    {
                        this.logger.LogError(sendException, $"Failed to send reminder email to client {reminder.Client.Entity.ClientNo}: {reminder.Client.Entity.FullName}. {sendException.Message}");
                    }

                    this.logger.LogInformation($"Reminder sent to client {reminder.Client.Entity.ClientNo}: {reminder.Client.Entity.FullName}");
                    await this.repository.SaveChangesAsync();
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }

                this.logger.LogInformation($"Finished monthly maintenance reminder job");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
            }
        }
    }
}
