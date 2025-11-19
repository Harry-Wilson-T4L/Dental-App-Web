using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DentalDrill.CRM.Services
{
    public class JobChangeTrackingService : ChangeTrackingService<Job, Guid, JobChange>
    {
        public JobChangeTrackingService(IRepositoryFactory repositoryFactory, UserEntityResolver userResolver, IRepository repository)
            : base(repositoryFactory, userResolver, repository)
        {
        }

        protected override Task FillOldVersion(JobChange change, Job entity)
        {
            change.OldStatus = entity.Status;
            change.OldContent = this.CaptureContent(entity);
            return Task.CompletedTask;
        }

        protected override Task FillNewVersion(JobChange change, Job entity)
        {
            change.NewStatus = entity.Status;
            change.NewContent = this.CaptureContent(entity);
            return Task.CompletedTask;
        }

        protected override async Task<Job> ReloadEntity(Job entity)
        {
            var reloaded = await this.Repository.QueryWithoutTracking<Job>()
                .SingleOrDefaultAsync(x => x.Id == entity.Id);

            if (reloaded == null)
            {
                throw new InvalidOperationException("Unable to reload the entity");
            }

            return reloaded;
        }

        protected override Task<JobChange> CreateChange(Job entity)
        {
            var change = new JobChange
            {
                JobId = entity.Id,
            };

            return Task.FromResult(change);
        }

        private String CaptureContent(Job entity)
        {
            return JsonConvert.SerializeObject(new
            {
                Id = entity.Id,
                JobNumber = entity.JobNumber,
                Status = entity.Status,
                Received = entity.Received,

                ClientId = entity.ClientId,
                CreatorId = entity.CreatorId,

                ApprovedBy = entity.ApprovedBy,
                ApprovedOn = entity.ApprovedOn,

                Comment = entity.Comment,
                HasWarning = entity.HasWarning,
                RatePlan = entity.RatePlan,
            });
        }
    }
}