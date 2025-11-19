using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Notifications;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.Validation;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.Workflow;
using DentalDrill.CRM.Services.Data;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Services.Workflow
{
    public class SaleRepairWorkflow : BaseRepairWorkflow
    {
        private readonly IRepository repository;
        private readonly IUrlHelper urlHelper;
        private readonly UserEntityResolver userResolver;
        private readonly NotificationsService notificationsService;

        public SaleRepairWorkflow(
            IRepositoryFactory repositoryFactory,
            IRepository repository,
            IDataTransactionService dataTransactionService,
            IClientManager clientManager,
            IHandpieceManager handpieceManager,
            IUrlHelper urlHelper,
            ClientEmailsService emailService,
            UserEntityResolver userResolver,
            NotificationsService notificationsService,
            CalendarService calendarService,
            IDateTimeService dateTimeService,
            IChangeTrackingService<Handpiece, HandpieceChange> handpieceChangeTrackingService,
            IChangeTrackingService<Job, JobChange> jobChangeTrackingService)
            : base(
                repositoryFactory,
                repository,
                dataTransactionService,
                clientManager,
                handpieceManager,
                urlHelper,
                emailService,
                userResolver,
                notificationsService,
                calendarService,
                dateTimeService,
                handpieceChangeTrackingService,
                jobChangeTrackingService)
        {
            this.repository = repository;
            this.urlHelper = urlHelper;
            this.userResolver = userResolver;
            this.notificationsService = notificationsService;
        }

        public override Task<Job> HandleJobEvent(Guid jobId, JobWorkflowEvent jobEvent)
        {
            return jobEvent switch
            {
                JobWorkflowEvent.Created => HandleJobCreate(),
                JobWorkflowEvent.Open => HandleJobOpen(),
                _ => throw new ArgumentOutOfRangeException(nameof(jobEvent), jobEvent, null),
            };

            async Task<Job> HandleJobCreate()
            {
                var job = await this.repository.Query<Job>("Client").SingleAsync(x => x.Id == jobId);
                return job;
            }

            async Task<Job> HandleJobOpen()
            {
                var job = await this.repository.Query<Job>("Client").SingleAsync(x => x.Id == jobId);
                return job;
            }
        }

        public override Task<Handpiece> HandleHandpieceEvent(Guid handpieceId, HandpieceWorkflowEvent handpieceEvent)
        {
            return handpieceEvent switch
            {
                HandpieceWorkflowEvent.Open => HandleHandpieceOpen(),
                _ => throw new ArgumentOutOfRangeException(nameof(handpieceEvent), handpieceEvent, null),
            };

            async Task<Handpiece> HandleHandpieceOpen()
            {
                var handpiece = await this.repository.Query<Handpiece>("Job").SingleAsync(x => x.Id == handpieceId);
                return handpiece;
            }
        }

        public override Task<JobStatus> GetNewJobStatusAsync()
        {
            return Task.FromResult(JobStatus.SentComplete);
        }

        public override Task<HandpieceStatus> GetNewHandpieceStatusAsync()
        {
            return Task.FromResult(HandpieceStatus.SentComplete);
        }

        public override Task<HandpieceStatus> GetNewHandpieceStatusAsync(Job existingJob)
        {
            return existingJob.Status switch
            {
                JobStatus.SentComplete => Task.FromResult(HandpieceStatus.SentComplete),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public override async Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlForNewHandpieceAsync(Job job)
        {
            var employeeAccess = await this.userResolver.GetEmployeeAccessAsync();
            var jobTypeAccess = employeeAccess.Workshops[job.WorkshopId].JobTypes[job.JobTypeId];

            var list = new PropertiesAccessControlList<Handpiece>();
            list.Set(x => x.Brand, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Brand), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Brand));
            list.Set(x => x.MakeAndModel, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Model), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Model));
            list.Set(x => x.Serial, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Serial), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Serial));
            list.Set(x => x.CostOfRepair, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.CostOfRepair), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.CostOfRepair));
            list.Set(x => x.InternalComment, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.InternalComment), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.InternalComment));
            list.Set(x => x.PublicComment, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PublicComment), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PublicComment));

            list.Set(x => x.ServiceLevelId, true, false);
            list.Set(x => x.Rating, true, false);
            list.Set(x => x.ProblemDescription, true, false);
            list.Set(x => x.DiagnosticReport, true, false);
            list.Set(x => x.Parts, true, false);
            list.Set(x => x.PartsOutOfStock, true, false);
            list.Set(x => x.PartsComment, true, false);
            list.Set(x => x.PartsOrdered, true, false);
            list.Set(x => x.PartsRestocked, true, false);
            list.Set(x => x.ReturnById, true, false);
            list.Set(x => x.SpeedType, true, false);
            list.Set(x => x.Speed, true, false);

            return list;
        }

        public override async Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlAsync(Handpiece handpiece)
        {
            var employeeAccess = await this.userResolver.GetEmployeeAccessAsync();
            var jobTypeAccess = employeeAccess.Workshops[handpiece.Job.WorkshopId].JobTypes[handpiece.Job.JobTypeId];

            var list = new PropertiesAccessControlList<Handpiece>();
            list.Set(
                x => x.Brand,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Brand),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Brand));
            list.Set(
                x => x.MakeAndModel,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Model),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Model));
            list.Set(
                x => x.Serial,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Serial),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Serial));
            list.Set(
                x => x.CostOfRepair,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.CostOfRepair),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.CostOfRepair));
            list.Set(
                x => x.InternalComment,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.InternalComment),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.InternalComment));
            list.Set(
                x => x.PublicComment,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PublicComment),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PublicComment));

            list.Set(x => x.ServiceLevelId, true, false);
            list.Set(x => x.Rating, true, false);
            list.Set(x => x.ProblemDescription, true, false);
            list.Set(x => x.DiagnosticReport, true, false);
            list.Set(x => x.Parts, true, true);
            list.Set(x => x.PartsOutOfStock, true, false);
            list.Set(x => x.PartsComment, true, false);
            list.Set(x => x.PartsOrdered, true, false);
            list.Set(x => x.PartsRestocked, true, false);
            list.Set(x => x.ReturnById, true, false);
            list.Set(x => x.SpeedType, true, false);
            list.Set(x => x.Speed, true, false);

            return list;
        }

        public override async Task<ModelValidatorCollection<JobHandpieceEditModel>> GetHandpieceUpdateValidators(Handpiece handpiece, JobHandpieceEditModel model)
        {
            var acl = await this.GetPropertiesAccessControlAsync(handpiece);
            var collection = new ModelValidatorCollection<JobHandpieceEditModel>();

            if (acl.For(x => x.Brand).CanUpdate)
            {
                collection.Require(x => x.Brand);
            }

            if (acl.For(x => x.MakeAndModel).CanUpdate)
            {
                collection.Require(x => x.MakeAndModel);
            }

            if (acl.For(x => x.Serial).CanUpdate)
            {
                collection.Require(x => x.Serial);
            }

            if ((handpiece.HandpieceStatus is HandpieceStatus.Cancelled && model.ChangeStatus is null or HandpieceStatus.Cancelled) || model.ChangeStatus is HandpieceStatus.Cancelled)
            {
                // Short-circuiting if status is already cancelled or is changed to cancelled
                return collection;
            }

            if (acl.For(x => x.CostOfRepair).CanUpdate)
            {
                collection.Require(x => x.CostOfRepair);
            }

            return collection;
        }

        public override async Task<JobActionsList> GetJobActionsListAsync(Job job)
        {
            var employeeAccess = await this.userResolver.GetEmployeeAccessAsync();
            var jobTypeAccess = employeeAccess.Workshops[job.WorkshopId].JobTypes[job.JobTypeId];

            var handpieces = await this.repository.Query<Handpiece>().Where(x => x.JobId == job.Id).ToListAsync();

            switch (job.Status)
            {
                case JobStatus.Unknown:
                case JobStatus.Received:
                case JobStatus.BeingEstimated:
                case JobStatus.WaitingForApproval:
                case JobStatus.EstimateSent:
                case JobStatus.BeingRepaired:
                case JobStatus.ReadyToReturn:
                    return JobActionsList.Create(builder => { });
                case JobStatus.SentComplete:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.BeingEstimated)
                        .Action(
                            "Cancel",
                            JobStatus.Cancelled,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.Cancelled }),
                            handpieces.All(x => x.HandpieceStatus == HandpieceStatus.Cancelled))
                        .CanBulkChangeStatus(false)
                        .CanSetStatusFrom(
                            HandpieceStatus.SentComplete,
                            HandpieceStatus.SentComplete, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.Cancelled,
                            HandpieceStatus.Cancelled, HandpieceStatus.SentComplete));

                case JobStatus.Cancelled:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.BeingEstimated)
                        .Action(
                            "Restore",
                            JobStatus.SentComplete,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.SentComplete }),
                            handpieces.Any()));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override Task<Boolean> CanChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus)
        {
            return Task.FromResult(true);
        }

        public override async Task ChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus)
        {
            handpiece.HandpieceStatus = newStatus;

            await this.repository.UpdateAsync(handpiece);
            await this.repository.SaveChangesAsync();
        }

        public override async Task ChangeJobStatusAsync(Job job, JobStatus status)
        {
            job.Status = status;

            await this.repository.UpdateAsync(job);
            await this.repository.SaveChangesAsync();
        }
    }
}
