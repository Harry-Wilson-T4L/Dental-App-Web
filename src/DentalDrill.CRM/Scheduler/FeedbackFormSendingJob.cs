using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DentalDrill.CRM.Scheduler
{
    [DisallowConcurrentExecution]
    public class FeedbackFormSendingJob : IJob
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IRepository repository;
        private readonly ILogger logger;

        public FeedbackFormSendingJob(IServiceProvider serviceProvider, IRepository repository, ILogger<FeedbackFormSendingJob> logger)
        {
            this.serviceProvider = serviceProvider;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var date = DateTime.UtcNow;
                var qualifyingForms = await this.repository.Query<FeedbackForm>("Client")
                    .Where(x => x.Client.Email != null &&
                                (x.Client.NotificationsOptions & ClientNotificationsOptions.Enabled) == ClientNotificationsOptions.Enabled &&
                                (x.Client.NotificationsOptions & ClientNotificationsOptions.DisabledFeedbackRequests) != ClientNotificationsOptions.DisabledFeedbackRequests)
                    .Where(x => x.ScheduledOn != null && x.ScheduledOn < date)
                    .Where(x => x.SentOn == null)
                    .ToListAsync();

                foreach (var qualifyingForm in qualifyingForms)
                {
                    var formDomain = await FeedbackFormDomainModel.GetByIdAsync(this.serviceProvider, qualifyingForm.Id);
                    await formDomain.EmailFormAsync(qualifyingForm.Client.Email);
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
            }
        }
    }
}
