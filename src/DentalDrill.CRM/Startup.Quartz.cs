using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DentalDrill.CRM.Scheduler;
using DevGuild.AspNetCore.Contracts;
using DevGuild.AspNetCore.Services.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace DentalDrill.CRM
{
    public partial class Startup
    {
        private class QuartzJobFactory : IJobFactory
        {
            private readonly IServiceScopeFactory scopeFactory;
            private readonly List<(IJob Job, IServiceScope Scope)> jobsWithScopes = new List<(IJob Job, IServiceScope Scope)>();

            public QuartzJobFactory(IServiceScopeFactory scopeFactory)
            {
                this.scopeFactory = scopeFactory;
            }

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                var scope = this.scopeFactory.CreateScope();
                scope.OverrideRequestInformation(new RequestInformation(
                    "QuartzScheduler",
                    "EXEC",
                    bundle.JobDetail.JobType.FullName,
                    "::1",
                    "Quartz",
                    null,
                    $"Quartz-{Guid.NewGuid()}"));
                var job = ActivatorUtilities.CreateInstance(scope.ServiceProvider, bundle.JobDetail.JobType) as IJob;

                lock (this.jobsWithScopes)
                {
                    this.jobsWithScopes.Add((job, scope));
                }

                return job;
            }

            public void ReturnJob(IJob job)
            {
                IServiceScope scope = null;
                lock (this.jobsWithScopes)
                {
                    for (var i = 0; i < this.jobsWithScopes.Count; i++)
                    {
                        if (Object.ReferenceEquals(this.jobsWithScopes[i].Job, job))
                        {
                            scope = this.jobsWithScopes[i].Scope;
                            this.jobsWithScopes.RemoveAt(i);
                            break;
                        }
                    }
                }

                (job as IDisposable)?.Dispose();
                scope?.Dispose();
            }
        }

        private class QuartzScheduler : IHostedService
        {
            private readonly IServiceScopeFactory serviceScopeFactory;
            private readonly IConfiguration configuration;

            private IScheduler scheduler;

            public QuartzScheduler(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
            {
                this.serviceScopeFactory = serviceScopeFactory;
                this.configuration = configuration;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                Ensure.State.Null(this.scheduler);

                var factory = new StdSchedulerFactory();
                this.scheduler = await factory.GetScheduler(cancellationToken);
                this.scheduler.JobFactory = new QuartzJobFactory(this.serviceScopeFactory);
                await this.ConfigureSchedulerAsync(this.scheduler, cancellationToken);
                await this.scheduler.Start(cancellationToken);
            }

            public async Task StopAsync(CancellationToken cancellationToken)
            {
                if (this.scheduler == null)
                {
                    return;
                }

                await this.scheduler.Shutdown(waitForJobsToComplete: true, cancellationToken);
                this.scheduler = null;
            }

            private async Task ConfigureSchedulerAsync(IScheduler scheduler, CancellationToken cancellationToken)
            {
                {
                    var job = JobBuilder.Create<PickupRequestsEmailSendingJob>().Build();
                    var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInMinutes(1).RepeatForever()).StartNow().Build();
                    await scheduler.ScheduleJob(job, trigger);
                }

                {
                    var job = JobBuilder.Create<PickupRequestSmartFreightSubmitJob>().Build();
                    var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInMinutes(1).RepeatForever()).StartNow().Build();
                    await scheduler.ScheduleJob(job, trigger);
                }

                {
                    var job = JobBuilder.Create<CalendarSeedJob>().Build();
                    var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInHours(1).RepeatForever()).StartNow().Build();
                    await scheduler.ScheduleJob(job, trigger);
                }

                {
                    var job = JobBuilder.Create<StockControlEntryWeeklyTransferJob>().Build();
                    var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInHours(1).RepeatForever()).StartNow().Build();
                    await scheduler.ScheduleJob(job, trigger);
                }

                {
                    var job = JobBuilder.Create<CallbackNotificationActivationJob>().Build();
                    var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInMinutes(1).RepeatForever()).StartNow().Build();
                    await scheduler.ScheduleJob(job, trigger);
                }

                {
                    var job = JobBuilder.Create<FeedbackFormSendingJob>().Build();
                    var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInMinutes(5).RepeatForever()).StartNow().Build();
                    await scheduler.ScheduleJob(job, trigger);
                }

                if (this.configuration.GetValue<Boolean>("MaintenanceReminder:Enabled", false))
                {
                    var cronSchedule = this.configuration.GetValue<String>("MaintenanceReminder:Schedule", "0 0 12 * * ?");
                    var job = JobBuilder.Create<MonthlyMaintenanceReminderJob>().Build();
                    var trigger = TriggerBuilder.Create().WithCronSchedule(cronSchedule, cron => cron.InTimeZone(TimeZoneInfo.Utc)).StartNow().Build();
                    await scheduler.ScheduleJob(job, trigger);
                }
            }
        }
    }
}
