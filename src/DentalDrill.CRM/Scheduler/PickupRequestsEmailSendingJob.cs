using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;

namespace DentalDrill.CRM.Scheduler
{
    [DisallowConcurrentExecution]
    public class PickupRequestsEmailSendingJob : IJob
    {
        private readonly IRepository repository;
        private readonly IEmailService emailService;
        private readonly PickupRequestEmailOptions options;

        public PickupRequestsEmailSendingJob(IRepository repository, IEmailService emailService, IOptions<PickupRequestEmailOptions> options)
        {
            this.repository = repository;
            this.emailService = emailService;
            this.options = options.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (!this.options.Enabled)
            {
                return;
            }

            var createdRequests = await this.repository.Query<PickupRequest>("CreatedBy", "Employee", "Client", "Corporate")
                .Where(x => x.Status == PickupRequestStatus.Created)
                .ToListAsync();

            foreach (var request in createdRequests)
            {
                var typeName = request.Type.ToString();
                var typeOptions = this.options.Types?.GetValueOrDefault(typeName);

                if (typeOptions?.Enabled == false)
                {
                    continue;
                }

                var email = new PickupRequestReceivedEmail
                {
                    Recipient = typeOptions?.Recipient ?? this.options.Recipient,
                    Link = $"{typeOptions?.BaseUrl ?? this.options.BaseUrl}PickupRequests/Details/{request.Id:D}", // TODO: Build from options
                    Request = request,
                    SubjectPrefix = typeOptions?.SubjectPrefix ?? this.options.SubjectPrefix,
                };

                var message = await this.emailService.SendAsync(email);
                request.Status = PickupRequestStatus.EmailSent;
                await this.repository.UpdateAsync(request);
                await this.repository.SaveChangesAsync();
            }
        }
    }
}
