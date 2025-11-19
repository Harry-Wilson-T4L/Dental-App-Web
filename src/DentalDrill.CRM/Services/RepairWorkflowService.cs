using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.Validation;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.Workflow;
using DentalDrill.CRM.Services.Workflow;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Services
{
    public class RepairWorkflowService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IRepository repository;

        public RepairWorkflowService(
            IServiceProvider serviceProvider,
            IRepository repository)
        {
            this.serviceProvider = serviceProvider;
            this.repository = repository;
        }

        public async Task HandleJobCreate(Guid jobId)
        {
            var workflow = await this.GetWorkflowByJob(jobId);
            await workflow.HandleJobEvent(jobId, JobWorkflowEvent.Created);
        }

        public async Task HandleJobOpenAsync(Job job)
        {
            var workflow = await this.GetWorkflow(job);
            await workflow.HandleJobEvent(job.Id, JobWorkflowEvent.Open);
        }

        public async Task<Handpiece> HandleHandpieceOpenAsync(Guid handpieceId)
        {
            var workflow = await this.GetWorkflowByHandpiece(handpieceId);
            return await workflow.HandleHandpieceEvent(handpieceId, HandpieceWorkflowEvent.Open);
        }

        public async Task<JobStatus> GetNewJobStatusAsync(String jobType)
        {
            var workflow = await this.GetWorkflow(jobType);
            return await workflow.GetNewJobStatusAsync();
        }

        public async Task<HandpieceStatus> GetNewHandpieceStatusAsync(String jobType)
        {
            var workflow = await this.GetWorkflow(jobType);
            return await workflow.GetNewHandpieceStatusAsync();
        }

        public async Task<HandpieceStatus> GetNewHandpieceStatusAsync(Job existingJob)
        {
            var workflow = await this.GetWorkflow(existingJob);
            return await workflow.GetNewHandpieceStatusAsync(existingJob);
        }

        public async Task<Boolean> CanAddHandpiecesAsync(Job existingJob)
        {
            var workflow = await this.GetWorkflow(existingJob);
            return await workflow.CanAddNewHandpiecesAsync(existingJob);
        }

        public async Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlForNewEntityAsync(Job job)
        {
            var workflow = await this.GetWorkflow(job);
            return await workflow.GetPropertiesAccessControlForNewHandpieceAsync(job);
        }

        public async Task<ModelValidatorCollection<JobHandpieceEditModel>> GetUpdateValidators(Handpiece handpiece, JobHandpieceEditModel model)
        {
            var workflow = await this.GetWorkflow(handpiece);
            return await workflow.GetHandpieceUpdateValidators(handpiece, model);
        }

        public async Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlAsync(Handpiece handpiece)
        {
            var workflow = await this.GetWorkflow(handpiece);
            return await workflow.GetPropertiesAccessControlAsync(handpiece);
        }

        public async Task<PropertiesAccessControlList<Job>> GetPropertiesAccessControlAsync(Job job)
        {
            var workflow = await this.GetWorkflow(job);
            return await workflow.GetPropertiesAccessControlAsync(job);
        }

        public async Task<IReadOnlyList<HandpieceStatus>> GetAvailableStatusChangesAsync(Handpiece handpiece)
        {
            var workflow = await this.GetWorkflow(handpiece);
            return await workflow.GetAvailableStatusChangesAsync(handpiece);
        }

        public async Task<JobActionsList> GetJobActionsListAsync(Job job)
        {
            var workflow = await this.GetWorkflow(job);
            return await workflow.GetJobActionsListAsync(job);
        }

        public async Task<Boolean> CanChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus)
        {
            var workflow = await this.GetWorkflow(handpiece);
            return await workflow.CanChangeHandpieceStatusAsync(handpiece, newStatus);
        }

        public async Task ChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus)
        {
            var workflow = await this.GetWorkflow(handpiece);
            await workflow.ChangeHandpieceStatusAsync(handpiece, newStatus);
        }

        public async Task ChangeJobStatusAsync(Job job, JobStatus status)
        {
            var workflow = await this.GetWorkflow(job);
            await workflow.ChangeJobStatusAsync(job, status);
        }

        private async Task<IRepairWorkflow> GetWorkflowByHandpiece(Guid handpieceId)
        {
            var handpiece = await this.repository.QueryWithoutTracking<Handpiece>()
                .Include(x => x.Job)
                .SingleAsync(x => x.Id == handpieceId);
            var workflow = await this.GetWorkflow(handpiece.Job.JobTypeId);
            return workflow;
        }

        private async Task<IRepairWorkflow> GetWorkflowByJob(Guid jobId)
        {
            var job = await this.repository.QueryWithoutTracking<Job>().SingleAsync(x => x.Id == jobId);
            var workflow = await this.GetWorkflow(job.JobTypeId);
            return workflow;
        }

        private async Task<IRepairWorkflow> GetWorkflow(Handpiece handpiece)
        {
            var job = await this.repository.QueryWithoutTracking<Job>().SingleAsync(x => x.Id == handpiece.JobId);
            var workflow = await this.GetWorkflow(job.JobTypeId);
            return workflow;
        }

        private Task<IRepairWorkflow> GetWorkflow(Job job)
        {
            return this.GetWorkflow(job.JobTypeId);
        }

        private Task<IRepairWorkflow> GetWorkflow(String typeId)
        {
            switch (typeId)
            {
                case JobTypes.Estimate:
                {
                    var factory = ActivatorUtilities.CreateInstance<EstimateRepairWorkflowFactory>(this.serviceProvider);
                    return Task.FromResult<IRepairWorkflow>(factory.Create());
                }

                case JobTypes.Sale:
                {
                    var factory = ActivatorUtilities.CreateInstance<SaleRepairWorkflowFactory>(this.serviceProvider);
                    return Task.FromResult<IRepairWorkflow>(factory.Create());
                }

                default:
                {
                    return Task.FromException<IRepairWorkflow>(new InvalidOperationException($"Job Type {typeId} is not supported"));
                }
            }
        }
    }
}
