using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Extensions;
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
    public abstract class BaseRepairWorkflow : IRepairWorkflow
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IRepository repository;
        private readonly IDataTransactionService dataTransactionService;
        private readonly IClientManager clientManager;
        private readonly IHandpieceManager handpieceManager;
        private readonly IUrlHelper urlHelper;
        private readonly ClientEmailsService emailService;
        private readonly UserEntityResolver userResolver;
        private readonly NotificationsService notificationsService;
        private readonly CalendarService calendarService;
        private readonly IDateTimeService dateTimeService;
        private readonly IChangeTrackingService<Handpiece, HandpieceChange> handpieceChangeTrackingService;
        private readonly IChangeTrackingService<Job, JobChange> jobChangeTrackingService;

        protected BaseRepairWorkflow(
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
        {
            this.repositoryFactory = repositoryFactory;
            this.repository = repository;
            this.dataTransactionService = dataTransactionService;
            this.clientManager = clientManager;
            this.handpieceManager = handpieceManager;
            this.urlHelper = urlHelper;
            this.emailService = emailService;
            this.userResolver = userResolver;
            this.notificationsService = notificationsService;
            this.calendarService = calendarService;
            this.dateTimeService = dateTimeService;
            this.handpieceChangeTrackingService = handpieceChangeTrackingService;
            this.jobChangeTrackingService = jobChangeTrackingService;
        }

        public virtual Task<Job> HandleJobEvent(Guid jobId, JobWorkflowEvent jobEvent)
        {
            return jobEvent switch
            {
                JobWorkflowEvent.Created => HandleJobCreate(),
                JobWorkflowEvent.Open => HandleJobOpen(),
                _ => throw new ArgumentOutOfRangeException(nameof(jobEvent), jobEvent, null),
            };

            async Task<Job> HandleJobCreate()
            {
                var user = await this.userResolver.ResolveCurrentUserEntity();
                var access = await this.userResolver.GetEmployeeAccessAsync();
                var job = await this.repository.Query<Job>("Client").SingleAsync(x => x.Id == jobId);

                await this.notificationsService.CreateJobNotification(new JobCreatedPayload(job), NotificationScope.Workshop, job);

                if (!String.IsNullOrEmpty(job.Client.Email))
                {
                    var email = new JobCreatedEmail
                    {
                        Job = job,
                        Client = job.Client,
                        Employee = user is Employee employee && access.Workshops[job.WorkshopId].LegacyRole.IsOneOf(EmployeeType.CompanyAdministrator, EmployeeType.OfficeAdministrator)
                            ? employee
                            : null,
                    };

                    await this.emailService.SendClientEmail(job.Client, email, ClientNotificationsType.HandpieceNotification);
                }

                return job;
            }

            async Task<Job> HandleJobOpen()
            {
                var job = await this.repository.Query<Job>("Client").SingleAsync(x => x.Id == jobId);
                await this.notificationsService.ReadJobNotifications(job);

                return job;
            }
        }

        public virtual Task<Handpiece> HandleHandpieceEvent(Guid handpieceId, HandpieceWorkflowEvent handpieceEvent)
        {
            return handpieceEvent switch
            {
                HandpieceWorkflowEvent.Open => HandleHandpieceOpen(),
                _ => throw new ArgumentOutOfRangeException(nameof(handpieceEvent), handpieceEvent, null),
            };

            async Task<Handpiece> HandleHandpieceOpen()
            {
                using (var localRepository = this.repositoryFactory.CreateRepository())
                {
                    var handpiece = await localRepository.Query<Handpiece>("Job").SingleAsync(x => x.Id == handpieceId);
                    if (handpiece.HandpieceStatus == HandpieceStatus.Received && handpiece.EstimatedById == null)
                    {
                        var user = await this.userResolver.ResolveCurrentUserEntity();
                        var access = await this.userResolver.GetEmployeeAccessAsync();
                        if (user is Employee employee && access.Workshops[handpiece.Job.WorkshopId].LegacyRole.IsOneOf(EmployeeType.WorkshopTechnician, EmployeeType.CompanyAdministrator))
                        {
                            handpiece.HandpieceStatus = HandpieceStatus.BeingEstimated;
                            handpiece.EstimatedById = employee.Id;

                            JobChange jobChange = null;
                            if (handpiece.Job.Status == JobStatus.Received)
                            {
                                jobChange = await this.jobChangeTrackingService.CaptureEntityForUpdate(handpiece.Job);
                                handpiece.Job.Status = JobStatus.BeingEstimated;
                                await localRepository.UpdateAsync(handpiece.Job);
                            }

                            var handpieceChange = await this.handpieceChangeTrackingService.CaptureEntityForUpdate(handpiece);
                            await localRepository.UpdateAsync(handpiece);
                            await localRepository.SaveChangesAsync();

                            if (jobChange != null)
                            {
                                await this.jobChangeTrackingService.TrackModifyEntityAsync(handpiece.Job, jobChange);
                            }

                            await this.handpieceChangeTrackingService.TrackModifyEntityAsync(handpiece, handpieceChange);

                            await localRepository.SaveChangesAsync();
                        }
                    }

                    return handpiece;
                }
            }
        }

        public virtual Task<JobStatus> GetNewJobStatusAsync()
        {
            return Task.FromResult(JobStatus.Received);
        }

        public virtual Task<HandpieceStatus> GetNewHandpieceStatusAsync()
        {
            return Task.FromResult(HandpieceStatus.Received);
        }

        public virtual Task<HandpieceStatus> GetNewHandpieceStatusAsync(Job existingJob)
        {
            return existingJob.Status switch
            {
                JobStatus.Received => Task.FromResult(HandpieceStatus.Received),
                JobStatus.WaitingForApproval => Task.FromResult(HandpieceStatus.WaitingForApproval),
                JobStatus.EstimateSent => Task.FromResult(HandpieceStatus.EstimateSent),
                JobStatus.BeingRepaired => Task.FromResult(HandpieceStatus.BeingRepaired),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public virtual Task<Boolean> CanAddNewHandpiecesAsync(Job existingJob)
        {
            return existingJob.Status switch
            {
                JobStatus.Received => Task.FromResult(true),
                JobStatus.WaitingForApproval => Task.FromResult(true),
                JobStatus.EstimateSent => Task.FromResult(true),
                JobStatus.BeingRepaired => Task.FromResult(true),
                _ => Task.FromResult(false),
            };
        }

        public virtual async Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlForNewHandpieceAsync(Job job)
        {
            var employeeAccess = await this.userResolver.GetEmployeeAccessAsync();
            var jobTypeAccess = employeeAccess.Workshops[job.WorkshopId].JobTypes[job.JobTypeId];

            var user = await this.userResolver.ResolveCurrentUserEntity();
            var list = new PropertiesAccessControlList<Handpiece>();
            list.Set(x => x.Brand, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Brand), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Brand));
            list.Set(x => x.MakeAndModel, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Model), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Model));
            list.Set(x => x.Serial, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Serial), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Serial));
            list.Set(x => x.ServiceLevelId, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.ServiceLevel), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.ServiceLevel));
            list.Set(x => x.Rating, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Rating), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Rating));
            list.Set(x => x.ProblemDescription, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.ProblemDescription), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.ProblemDescription));
            list.Set(x => x.DiagnosticReport, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.DiagnosticReport), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.DiagnosticReport));
            list.Set(x => x.Parts, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Parts), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Parts));
            list.Set(x => x.PartsOutOfStock, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsOutOfStock), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsOutOfStock));
            list.Set(x => x.PartsComment, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsComment), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsComment));
            list.Set(x => x.PartsOrdered, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsOrdered), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsOrdered));
            list.Set(x => x.PartsRestocked, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsRestocked), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PartsRestocked));
            list.Set(x => x.ReturnById, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.ReturnBy), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.ReturnBy));
            list.Set(x => x.CostOfRepair, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.CostOfRepair), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.CostOfRepair));
            list.Set(x => x.SpeedType, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.SpeedType), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.SpeedType));
            list.Set(x => x.Speed, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Speed), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.Speed));
            list.Set(x => x.InternalComment, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.InternalComment), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.InternalComment));
            list.Set(x => x.PublicComment, jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PublicComment), jobTypeAccess.CanInitHandpieceField(HandpieceEntityField.PublicComment));

            ////if (user is Employee employee)
            ////{
            ////    if (employee.Type == EmployeeType.OfficeAdministrator)
            ////    {
            ////        list.Update(x => x.Rating, false, false);
            ////        list.Update(x => x.ServiceLevelId, canUpdate: false);
            ////        list.Update(x => x.EstimatedById, false, false);
            ////        list.Update(x => x.ReturnById, canUpdate: false);
            ////        list.Update(x => x.CostOfRepair, canUpdate: false);
            ////        list.Update(x => x.SpeedType, canUpdate: false);
            ////        list.Update(x => x.Speed, canUpdate: false);
            ////        list.Update(x => x.DiagnosticReport, canUpdate: false);
            ////        list.Update(x => x.Parts, canUpdate: false);
            ////        list.Update(x => x.PartsOutOfStock, canUpdate: false);
            ////    }
            ////    else if (employee.Type == EmployeeType.WorkshopTechnician)
            ////    {
            ////        list.Update(x => x.PartsOrdered, canUpdate: false);
            ////        list.Update(x => x.PartsRestocked, canUpdate: false);
            ////    }
            ////}

            return list;
        }

        public virtual async Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlAsync(Handpiece handpiece)
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
                x => x.ServiceLevelId,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.ServiceLevel),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.ServiceLevel));
            list.Set(
                x => x.EstimatedById,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.EstimatedBy),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.EstimatedBy));
            list.Set(
                x => x.Rating,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Rating),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Rating));
            list.Set(
                x => x.ProblemDescription,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.ProblemDescription),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.ProblemDescription));
            list.Set(
                x => x.DiagnosticReport,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.DiagnosticReport),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.DiagnosticReport));
            list.Set(
                x => x.Parts,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Parts),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Parts));
            list.Set(
                x => x.PartsOutOfStock,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsOutOfStock),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsOutOfStock));
            list.Set(
                x => x.PartsComment,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsComment),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsComment));
            list.Set(
                x => x.PartsOrdered,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsOrdered),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsOrdered));
            list.Set(
                x => x.PartsRestocked,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsRestocked),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PartsRestocked));
            list.Set(
                x => x.ReturnById,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.ReturnBy),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.ReturnBy));
            list.Set(
                x => x.CostOfRepair,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.CostOfRepair),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.CostOfRepair));
            list.Set(
                x => x.SpeedType,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.SpeedType),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.SpeedType));
            list.Set(
                x => x.Speed,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Speed),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.Speed));
            list.Set(
                x => x.InternalComment,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.InternalComment),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.InternalComment));
            list.Set(
                x => x.PublicComment,
                jobTypeAccess.CanReadHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PublicComment),
                jobTypeAccess.CanWriteHandpieceField(handpiece.Job.Status, handpiece.HandpieceStatus, HandpieceEntityField.PublicComment));

            ////if (user is Employee employee)
            ////{
            ////    if (employee.Type == EmployeeType.OfficeAdministrator)
            ////    {
            ////        list.Update(x => x.Rating, false, false);
            ////        list.Update(x => x.ServiceLevelId, canUpdate: false);
            ////        list.Update(x => x.EstimatedById, false, false);
            ////        list.Update(x => x.ReturnById, canUpdate: false);
            ////        list.Update(x => x.CostOfRepair, canUpdate: false);
            ////        list.Update(x => x.SpeedType, canUpdate: false);
            ////        list.Update(x => x.Speed, canUpdate: false);
            ////        list.Update(x => x.DiagnosticReport, canUpdate: false);
            ////        list.Update(x => x.PartsOrdered, canUpdate: false);
            ////        list.Update(x => x.PartsRestocked, canUpdate: false);

            ////        if (handpiece.HandpieceStatus.IsNotOneOf(HandpieceStatus.Received))
            ////        {
            ////            list.Update(x => x.Brand, canUpdate: false);
            ////            list.Update(x => x.MakeAndModel, canUpdate: false);
            ////            list.Update(x => x.Serial, canUpdate: false);
            ////            list.Update(x => x.ProblemDescription, canUpdate: false);
            ////        }

            ////        if (handpiece.HandpieceStatus.IsOneOf(HandpieceStatus.BeingEstimated, HandpieceStatus.BeingRepaired))
            ////        {
            ////            list.Update(x => x.Parts, canUpdate: false);
            ////            list.Update(x => x.PartsOutOfStock, canUpdate: false);
            ////            list.Update(x => x.PartsComment, canUpdate: false);
            ////            list.Update(x => x.PublicComment, canUpdate: false);
            ////        }

            ////        if (handpiece.HandpieceStatus.IsOneOf(HandpieceStatus.SentComplete, HandpieceStatus.Cancelled))
            ////        {
            ////            list.Update(x => x.Parts, canUpdate: false);
            ////        }
            ////    }
            ////    else if (employee.Type == EmployeeType.WorkshopTechnician)
            ////    {
            ////        list.Update(x => x.ProblemDescription, canUpdate: false);
            ////        list.Update(x => x.PartsOrdered, canUpdate: false);
            ////        list.Update(x => x.PartsRestocked, canUpdate: false);
            ////        if (handpiece.HandpieceStatus.IsNotOneOf(HandpieceStatus.Received, HandpieceStatus.BeingEstimated, HandpieceStatus.WaitingForApproval))
            ////        {
            ////            list.Update(x => x.Brand, canUpdate: false);
            ////            list.Update(x => x.MakeAndModel, canUpdate: false);
            ////            list.Update(x => x.Serial, canUpdate: false);
            ////        }

            ////        if (handpiece.HandpieceStatus.IsNotOneOf(HandpieceStatus.Received, HandpieceStatus.BeingEstimated, HandpieceStatus.WaitingForParts, HandpieceStatus.BeingRepaired) &&
            ////            !(handpiece.Job.Status == JobStatus.BeingEstimated && handpiece.HandpieceStatus == HandpieceStatus.WaitingForApproval))
            ////        {
            ////            list.Update(x => x.Parts, canUpdate: false);
            ////            list.Update(x => x.PartsOutOfStock, canUpdate: false);
            ////            list.Update(x => x.PartsComment, canUpdate: false);
            ////        }

            ////        if (handpiece.HandpieceStatus.IsNotOneOf(HandpieceStatus.Received, HandpieceStatus.BeingEstimated, HandpieceStatus.BeingRepaired) &&
            ////            !(handpiece.Job.Status == JobStatus.BeingEstimated && handpiece.HandpieceStatus == HandpieceStatus.WaitingForApproval))
            ////        {
            ////            list.Update(x => x.Brand, canUpdate: false);
            ////            list.Update(x => x.MakeAndModel, canUpdate: false);
            ////            list.Update(x => x.Serial, canUpdate: false);
            ////            list.Update(x => x.ServiceLevelId, canUpdate: false);
            ////            list.Update(x => x.EstimatedById, canUpdate: false);
            ////            list.Update(x => x.Rating, canUpdate: false);
            ////            list.Update(x => x.DiagnosticReport, canUpdate: false);
            ////            list.Update(x => x.ReturnById, canUpdate: false);
            ////            list.Update(x => x.CostOfRepair, canUpdate: false);
            ////            list.Update(x => x.SpeedType, canUpdate: false);
            ////            list.Update(x => x.Speed, canUpdate: false);
            ////            list.Update(x => x.PublicComment, canUpdate: false);
            ////        }

            ////        if (handpiece.HandpieceStatus.IsOneOf(HandpieceStatus.SentComplete, HandpieceStatus.Cancelled))
            ////        {
            ////            list.Update(x => x.Parts, canUpdate: false);
            ////        }
            ////    }
            ////}

            return list;
        }

        public virtual async Task<PropertiesAccessControlList<Job>> GetPropertiesAccessControlAsync(Job job)
        {
            var employeeAccess = await this.userResolver.GetEmployeeAccessAsync();
            var jobTypeAccess = employeeAccess.Workshops[job.WorkshopId].JobTypes[job.JobTypeId];

            var list = new PropertiesAccessControlList<Job>();
            list.Set(
                x => x.ClientId,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.Client),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.Client));
            list.Set(
                x => x.HasWarning,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.HasWarning),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.HasWarning));
            list.Set(
                x => x.Received,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.Received),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.Received));
            list.Set(
                x => x.CreatorId,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.Creator),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.Creator));
            list.Set(
                x => x.ApprovedOn,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.ApprovedOn),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.ApprovedOn));
            list.Set(
                x => x.ApprovedBy,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.ApprovedBy),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.ApprovedBy));
            list.Set(
                x => x.RatePlan,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.RatePlan),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.RatePlan));
            list.Set(
                x => x.Comment,
                jobTypeAccess.CanReadJobField(job.Status, JobEntityField.Comment),
                jobTypeAccess.CanWriteJobField(job.Status, JobEntityField.Comment));

            ////if (user is Employee employee)
            ////{
            ////    if (employee.Type == EmployeeType.CompanyAdministrator)
            ////    {
            ////        list.Update(x => x.ClientId, canUpdate: true);
            ////        list.Update(x => x.Received, canUpdate: true);
            ////    }

            ////    if (employee.Type == EmployeeType.CompanyAdministrator || employee.Type == EmployeeType.OfficeAdministrator)
            ////    {
            ////        list.Update(x => x.ApprovedOn, canUpdate: true);
            ////        list.Update(x => x.ApprovedBy, canUpdate: true);
            ////        list.Update(x => x.RatePlan, canUpdate: true);
            ////    }
            ////}

            return list;
        }

        public virtual async Task<ModelValidatorCollection<JobHandpieceEditModel>> GetHandpieceUpdateValidators(Handpiece handpiece, JobHandpieceEditModel model)
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

            if (!String.IsNullOrEmpty(handpiece.ImportKey))
            {
                // If handpiece was imported - do not add any additional requirements
                return collection;
            }

            if ((handpiece.HandpieceStatus is HandpieceStatus.Cancelled && model.ChangeStatus is null or HandpieceStatus.Cancelled) || model.ChangeStatus is HandpieceStatus.Cancelled)
            {
                // Short-circuiting if status is already cancelled or is changed to cancelled
                return collection;
            }

            if (handpiece.HandpieceStatus.IsNotOneOf(HandpieceStatus.Received))
            {
                if (acl.For(x => x.CostOfRepair).CanUpdate)
                {
                    collection.Require(x => x.CostOfRepair);
                }

                if (acl.For(x => x.ReturnById).CanUpdate)
                {
                    collection.Require(x => x.ReturnById);
                }

                if (acl.For(x => x.ServiceLevelId).CanUpdate)
                {
                    collection.Require(x => x.ServiceLevelId);
                }

                if (acl.For(x => x.DiagnosticReport).CanUpdate)
                {
                    collection.Custom(
                        x => !String.IsNullOrEmpty(x.DiagnosticReportOther) || (x.DiagnosticReportChecked != null && x.DiagnosticReportChecked.Count > 0),
                        "DiagnosticReportChecked",
                        "Field is required");
                }

                if (acl.For(x => x.Parts).CanUpdate)
                {
                    if (handpiece.PartsVersion == HandpiecePartsVersion.InventorySKUv1)
                    {
                        collection.CustomAsync(
                            async model =>
                            {
                                var handpieceDomain = await this.handpieceManager.GetByIdAsync(handpiece.Id);
                                var parts = await handpieceDomain.Parts.GetRequiredPartsAsync();
                                return parts.Count > 0;
                            },
                            "RequiredParts",
                            "Parts are required");
                    }
                }
            }

            if (handpiece.HandpieceStatus.IsNotOneOf(
                HandpieceStatus.Received,
                HandpieceStatus.BeingEstimated,
                HandpieceStatus.WaitingForApproval,
                HandpieceStatus.TbcHoldOn,
                HandpieceStatus.EstimateSent))
            {
                if (acl.For(x => x.Parts).CanUpdate)
                {
                    collection.Require(x => x.Parts);
                }

                if (acl.For(x => x.Speed).CanUpdate)
                {
                    collection.Require(x => x.Speed);
                }

                if (acl.For(x => x.PublicComment).CanUpdate)
                {
                    collection.Require(x => x.PublicComment);
                }
            }

            if (handpiece.HandpieceStatus.IsOneOf(HandpieceStatus.ReadyToReturn, HandpieceStatus.SentComplete) ||
                (model?.ChangeStatus?.IsOneOf(HandpieceStatus.ReadyToReturn, HandpieceStatus.SentComplete) ?? false))
            {
                if (acl.For(x => x.Rating).CanUpdate)
                {
                    collection.Custom(x => x.Rating, x => x > 0, "Field is required");
                }
                else
                {
                    collection.Fact(handpiece.Rating > 0, "Rating", "Field is required");
                }
            }

            if (model?.ChangeStatus?.IsOneOf(HandpieceStatus.BeingRepaired, HandpieceStatus.ReadyToReturn, HandpieceStatus.SentComplete) ?? false)
            {
                if (handpiece.PartsVersion == HandpiecePartsVersion.InventorySKUv1)
                {
                    collection.CustomAsync(
                        async model =>
                        {
                            var handpieceDomain = await this.handpieceManager.GetByIdAsync(handpiece.Id);
                            return await handpieceDomain.Parts.CanCompleteAsync();
                        },
                        "RequiredParts",
                        "Required parts are missing");
                }
            }

            return collection;
        }

        public virtual async Task<IReadOnlyList<HandpieceStatus>> GetAvailableStatusChangesAsync(Handpiece handpiece)
        {
            var jobActions = await this.GetJobActionsListAsync(handpiece.Job);
            return jobActions.GetPossibleStatuses(handpiece.HandpieceStatus);
        }

        public virtual async Task<JobActionsList> GetJobActionsListAsync(Job job)
        {
            var employeeAccess = await this.userResolver.GetEmployeeAccessAsync();
            var jobTypeAccess = employeeAccess.Workshops[job.WorkshopId].JobTypes[job.JobTypeId];

            var handpieces = await this.repository.Query<Handpiece>().Where(x => x.JobId == job.Id).ToListAsync();

            switch (job.Status)
            {
                case JobStatus.Unknown:
                    return JobActionsList.Create(builder => { });
                case JobStatus.Received:
                    return JobActionsList.Create(builder => { });
                case JobStatus.BeingEstimated:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.BeingEstimated)
                        .Action(
                            "Estimate complete",
                            JobStatus.WaitingForApproval,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.WaitingForApproval }),
                            handpieces.All(x => x.HandpieceStatus == HandpieceStatus.WaitingForApproval))
                        .CanSetStatusFrom(HandpieceStatus.BeingEstimated, HandpieceStatus.BeingEstimated, HandpieceStatus.WaitingForApproval)
                        .CanSetStatusFrom(HandpieceStatus.WaitingForApproval, HandpieceStatus.BeingEstimated, HandpieceStatus.WaitingForApproval));

                case JobStatus.WaitingForApproval:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.WaitingForApproval)
                        .Action(
                            "Estimate sent",
                            JobStatus.EstimateSent,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.EstimateSent }),
                            handpieces.All(x => x.HandpieceStatus != HandpieceStatus.WaitingForApproval) && handpieces.Any(x => x.HandpieceStatus == HandpieceStatus.EstimateSent))
                        .Action(
                            "Being repaired",
                            JobStatus.BeingRepaired,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.BeingRepaired }),
                            handpieces.All(x => x.HandpieceStatus != HandpieceStatus.WaitingForApproval && x.HandpieceStatus != HandpieceStatus.EstimateSent) &&
                            handpieces.Any(x => x.HandpieceStatus is HandpieceStatus.BeingRepaired or HandpieceStatus.WaitingForParts))
                        .Action(
                            "Ready to return",
                            JobStatus.ReadyToReturn,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.ReadyToReturn }),
                            handpieces.All(x => x.HandpieceStatus != HandpieceStatus.WaitingForApproval && x.HandpieceStatus != HandpieceStatus.BeingRepaired) &&
                            handpieces.Any(x => x.HandpieceStatus == HandpieceStatus.ReturnUnrepaired || x.HandpieceStatus == HandpieceStatus.TradeIn))
                        .Action(
                            "Cancel",
                            JobStatus.Cancelled,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.Cancelled }),
                            handpieces.All(x => x.HandpieceStatus == HandpieceStatus.Cancelled))
                        .CanBulkChangeStatus(false)
                        .CanSetStatusFrom(
                            HandpieceStatus.WaitingForApproval,
                            HandpieceStatus.WaitingForApproval, HandpieceStatus.EstimateSent, HandpieceStatus.BeingRepaired, HandpieceStatus.WaitingForParts, HandpieceStatus.TbcHoldOn, HandpieceStatus.Cancelled, HandpieceStatus.ReturnUnrepaired, HandpieceStatus.TradeIn)
                        .CanSetStatusFrom(
                            HandpieceStatus.EstimateSent,
                            HandpieceStatus.EstimateSent, HandpieceStatus.WaitingForApproval)
                        .CanSetStatusFrom(
                            HandpieceStatus.BeingRepaired,
                            HandpieceStatus.BeingRepaired, HandpieceStatus.WaitingForApproval)
                        .CanSetStatusFrom(
                            HandpieceStatus.WaitingForParts,
                            HandpieceStatus.WaitingForParts, HandpieceStatus.WaitingForApproval)
                        .CanSetStatusFrom(
                            HandpieceStatus.TbcHoldOn,
                            HandpieceStatus.TbcHoldOn, HandpieceStatus.WaitingForApproval, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReturnUnrepaired,
                            HandpieceStatus.ReturnUnrepaired, HandpieceStatus.WaitingForApproval)
                        .CanSetStatusFrom(
                            HandpieceStatus.TradeIn,
                            HandpieceStatus.TradeIn, HandpieceStatus.WaitingForApproval)
                        .CanSetStatusFrom(
                            HandpieceStatus.Cancelled,
                            HandpieceStatus.Cancelled, HandpieceStatus.WaitingForApproval));

                case JobStatus.EstimateSent:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.EstimateSent)
                        .Action(
                            "Being repaired",
                            JobStatus.BeingRepaired,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.BeingRepaired }),
                            handpieces.All(x => x.HandpieceStatus != HandpieceStatus.EstimateSent) && handpieces.Any(x => x.HandpieceStatus is HandpieceStatus.BeingRepaired or HandpieceStatus.WaitingForParts))
                        .Action(
                            "Ready to return",
                            JobStatus.ReadyToReturn,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.ReadyToReturn }),
                            handpieces.All(x => x.HandpieceStatus != HandpieceStatus.EstimateSent && x.HandpieceStatus != HandpieceStatus.BeingRepaired) &&
                            handpieces.Any(x => x.HandpieceStatus == HandpieceStatus.ReturnUnrepaired || x.HandpieceStatus == HandpieceStatus.TradeIn))
                        .Action(
                            "Cancel",
                            JobStatus.Cancelled,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.Cancelled }),
                            handpieces.All(x => x.HandpieceStatus == HandpieceStatus.Cancelled))
                        .CanSetStatusFrom(
                            HandpieceStatus.EstimateSent,
                            HandpieceStatus.EstimateSent, HandpieceStatus.BeingRepaired, HandpieceStatus.WaitingForParts, HandpieceStatus.TbcHoldOn, HandpieceStatus.Cancelled, HandpieceStatus.ReturnUnrepaired, HandpieceStatus.TradeIn)
                        .CanSetStatusFrom(
                            HandpieceStatus.BeingRepaired,
                            HandpieceStatus.BeingRepaired, HandpieceStatus.EstimateSent)
                        .CanSetStatusFrom(
                            HandpieceStatus.WaitingForParts,
                            HandpieceStatus.WaitingForParts, HandpieceStatus.EstimateSent)
                        .CanSetStatusFrom(
                            HandpieceStatus.TbcHoldOn,
                            HandpieceStatus.TbcHoldOn, HandpieceStatus.EstimateSent, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReturnUnrepaired,
                            HandpieceStatus.ReturnUnrepaired, HandpieceStatus.EstimateSent)
                        .CanSetStatusFrom(
                            HandpieceStatus.TradeIn,
                            HandpieceStatus.TradeIn, HandpieceStatus.EstimateSent)
                        .CanSetStatusFrom(
                            HandpieceStatus.Cancelled,
                            HandpieceStatus.Cancelled, HandpieceStatus.EstimateSent));

                case JobStatus.BeingRepaired:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.BeingRepaired)
                        .Action(
                            "Ready to return",
                            JobStatus.ReadyToReturn,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.ReadyToReturn }),
                            handpieces.All(x => x.HandpieceStatus != HandpieceStatus.BeingRepaired && x.HandpieceStatus != HandpieceStatus.NeedsReApproval) &&
                            handpieces.Any(x => x.HandpieceStatus == HandpieceStatus.ReadyToReturn || x.HandpieceStatus == HandpieceStatus.Unrepairable))
                        .Action(
                            "Cancel",
                            JobStatus.Cancelled,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.Cancelled }),
                            handpieces.All(x => x.HandpieceStatus == HandpieceStatus.Cancelled))
                        .CanSetStatusFrom(
                            HandpieceStatus.BeingRepaired,
                            HandpieceStatus.BeingRepaired, HandpieceStatus.ReadyToReturn, HandpieceStatus.NeedsReApproval, HandpieceStatus.WaitingForParts, HandpieceStatus.Unrepairable, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.NeedsReApproval,
                            HandpieceStatus.NeedsReApproval, HandpieceStatus.BeingRepaired, HandpieceStatus.ReadyToReturn, HandpieceStatus.WaitingForParts, HandpieceStatus.Unrepairable, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReadyToReturn,
                            HandpieceStatus.ReadyToReturn, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.WaitingForParts,
                            HandpieceStatus.WaitingForParts, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.Unrepairable,
                            HandpieceStatus.Unrepairable, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.TbcHoldOn,
                            HandpieceStatus.TbcHoldOn, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.TradeIn,
                            HandpieceStatus.TradeIn, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReturnUnrepaired,
                            HandpieceStatus.ReturnUnrepaired, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.Cancelled,
                            HandpieceStatus.Cancelled, HandpieceStatus.NeedsReApproval, HandpieceStatus.BeingRepaired, HandpieceStatus.WaitingForParts));

                case JobStatus.ReadyToReturn:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.ReadyToReturn)
                        .Action(
                            "Send Complete",
                            JobStatus.SentComplete,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.SentComplete }),
                            handpieces.Any(x => x.HandpieceStatus != HandpieceStatus.Cancelled) &&
                            handpieces.All(x => x.HandpieceStatus == HandpieceStatus.SentComplete || x.HandpieceStatus == HandpieceStatus.ReadyToReturn || x.HandpieceStatus == HandpieceStatus.TradeIn || x.HandpieceStatus == HandpieceStatus.ReturnUnrepaired || x.HandpieceStatus == HandpieceStatus.Unrepairable || x.HandpieceStatus == HandpieceStatus.Cancelled))
                        .Action(
                            "Cancel",
                            JobStatus.Cancelled,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.Cancelled }),
                            handpieces.All(x => x.HandpieceStatus == HandpieceStatus.Cancelled))
                        .CanSetStatusFrom(
                            HandpieceStatus.BeingRepaired,
                            HandpieceStatus.BeingRepaired, HandpieceStatus.ReadyToReturn, HandpieceStatus.NeedsReApproval, HandpieceStatus.WaitingForParts, HandpieceStatus.Unrepairable, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.NeedsReApproval,
                            HandpieceStatus.NeedsReApproval, HandpieceStatus.BeingRepaired, HandpieceStatus.WaitingForParts, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReadyToReturn,
                            HandpieceStatus.ReadyToReturn, HandpieceStatus.BeingRepaired, HandpieceStatus.SentComplete, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.WaitingForParts,
                            HandpieceStatus.WaitingForParts, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.Unrepairable,
                            HandpieceStatus.Unrepairable, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.TbcHoldOn,
                            HandpieceStatus.TbcHoldOn, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.TradeIn,
                            HandpieceStatus.TradeIn, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReturnUnrepaired,
                            HandpieceStatus.ReturnUnrepaired, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.Cancelled,
                            HandpieceStatus.Cancelled, HandpieceStatus.NeedsReApproval, HandpieceStatus.BeingRepaired, HandpieceStatus.WaitingForParts, HandpieceStatus.ReadyToReturn));

                case JobStatus.SentComplete:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.SentComplete)
                        .CanSetStatusFrom(
                            HandpieceStatus.BeingRepaired,
                            HandpieceStatus.BeingRepaired, HandpieceStatus.ReadyToReturn, HandpieceStatus.WaitingForParts, HandpieceStatus.Unrepairable)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReadyToReturn,
                            HandpieceStatus.ReadyToReturn, HandpieceStatus.BeingRepaired, HandpieceStatus.SentComplete, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.WaitingForParts,
                            HandpieceStatus.WaitingForParts, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.Unrepairable,
                            HandpieceStatus.Unrepairable, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.TbcHoldOn,
                            HandpieceStatus.TbcHoldOn, HandpieceStatus.BeingRepaired, HandpieceStatus.Cancelled)
                        .CanSetStatusFrom(
                            HandpieceStatus.TradeIn,
                            HandpieceStatus.TradeIn, HandpieceStatus.BeingRepaired)
                        .CanSetStatusFrom(
                            HandpieceStatus.ReturnUnrepaired,
                            HandpieceStatus.ReturnUnrepaired, HandpieceStatus.BeingRepaired)
                        .CanSetStatusFrom(
                            HandpieceStatus.Cancelled,
                            HandpieceStatus.Cancelled, HandpieceStatus.BeingRepaired));

                case JobStatus.Cancelled:
                    return JobActionsList.Create(builder => builder
                        .UseAccessControl(jobTypeAccess, JobStatus.Cancelled)
                        .Action(
                            "Restore",
                            JobStatus.WaitingForApproval,
                            this.urlHelper.Action("SetStatus", "Jobs", new { id = job.Id, status = JobStatus.WaitingForApproval }),
                            handpieces.Any()));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual async Task<Boolean> CanChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus)
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                return false;
            }

            if (newStatus == HandpieceStatus.SentComplete || newStatus == HandpieceStatus.ReadyToReturn)
            {
                var handpieceDomain = await this.handpieceManager.GetByIdAsync(handpiece.Id);
                if (!await handpieceDomain.Parts.CanCompleteAsync())
                {
                    return false;
                }
            }

            if (newStatus.IsOneOf(HandpieceStatus.ReadyToReturn, HandpieceStatus.SentComplete))
            {
                if (handpiece.Rating == 0)
                {
                    return false;
                }

                if (String.IsNullOrEmpty(handpiece.PublicComment))
                {
                    return false;
                }
            }

            return true;
        }

        public virtual async Task ChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus)
        {
            var oldStatus = handpiece.HandpieceStatus;

            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                throw new InvalidOperationException();
            }

            if (handpiece.HandpieceStatus == HandpieceStatus.BeingEstimated &&
                (Int32)newStatus >= (Int32)HandpieceStatus.WaitingForApproval &&
                newStatus.IsNotOneOf(HandpieceStatus.Cancelled))
            {
                handpiece.EstimatedOn = this.dateTimeService.CurrentUtcTime;
            }

            if ((handpiece.HandpieceStatus == HandpieceStatus.BeingRepaired && (newStatus == HandpieceStatus.ReadyToReturn || newStatus == HandpieceStatus.Unrepairable)) ||
                (handpiece.HandpieceStatus == HandpieceStatus.NeedsReApproval && newStatus == HandpieceStatus.ReadyToReturn))
            {
                handpiece.RepairedById = employee.Id;
                handpiece.RepairedOn = this.dateTimeService.CurrentUtcTime;
            }

            if ((Int32)handpiece.HandpieceStatus < (Int32)HandpieceStatus.BeingRepaired &&
                (Int32)newStatus >= (Int32)HandpieceStatus.BeingRepaired &&
                newStatus.IsNotOneOf(HandpieceStatus.Cancelled))
            {
                handpiece.ApprovedById = employee.Id;
                handpiece.ApprovedOn = this.dateTimeService.CurrentUtcTime;
            }

            if (handpiece.HandpieceStatus != HandpieceStatus.SentComplete && newStatus == HandpieceStatus.SentComplete)
            {
                handpiece.CompletedOn = DateTime.UtcNow;
                var stockControlEntry = await this.repository.Query<StockControlEntry>().SingleOrDefaultAsync(x => x.HandpieceId == handpiece.Id);
                var week = await this.calendarService.GetCurrentWeekAsync();
                if (stockControlEntry == null && week != null)
                {
                    stockControlEntry = new StockControlEntry
                    {
                        HandpieceId = handpiece.Id,
                        WeekId = week.Id,
                        CompletedAt = DateTimeOffset.UtcNow,
                        Status = StockControlEntryStatus.Active,
                    };

                    await this.repository.InsertAsync(stockControlEntry);
                }
            }

            handpiece.HandpieceStatus = newStatus;

            await this.repository.UpdateAsync(handpiece);
            await this.repository.SaveChangesAsync();

            switch (newStatus)
            {
                case HandpieceStatus.BeingRepaired when (Int32)oldStatus < (UInt32)newStatus:
                case HandpieceStatus.WaitingForParts when (Int32)oldStatus < (UInt32)newStatus:
                    {
                        await using (var transaction = await this.dataTransactionService.BeginTransactionAsync())
                        {
                            var handpieceDomain = await this.handpieceManager.GetByIdAsync(handpiece.Id);
                            var requiredParts = await handpieceDomain.Parts.GetRequiredPartsAsync();
                            foreach (var requiredPart in requiredParts)
                            {
                                var orderMovements = await requiredPart.SKU.Movements.GetMovementsLinkedToPartAsync<IInventoryOrderMovement>(requiredPart.Id);
                                foreach (var order in orderMovements.Where(x => x.Status == InventoryMovementStatus.Requested))
                                {
                                    await order.ApproveAsync();
                                }

                                await requiredPart.SKU.TryProcessMovementsChangesAsync(handpieceDomain.Job.Workshop);
                            }

                            await transaction.CommitAsync();
                        }

                        break;
                    }

                case HandpieceStatus.SentComplete:
                    {
                        await using (var transaction = await this.dataTransactionService.BeginTransactionAsync())
                        {
                            var handpieceDomain = await this.handpieceManager.GetByIdAsync(handpiece.Id);
                            var requiredParts = await handpieceDomain.Parts.GetRequiredPartsAsync();
                            var skus = requiredParts.Select(x => x.SKU).ToList();
                            await handpieceDomain.Parts.CompleteAsync();
                            foreach (var sku in skus)
                            {
                                await sku.TryProcessMovementsChangesAsync(handpieceDomain.Job.Workshop);
                            }

                            await transaction.CommitAsync();
                        }

                        break;
                    }

                case HandpieceStatus.Cancelled:
                    {
                        await using (var transaction = await this.dataTransactionService.BeginTransactionAsync())
                        {
                            var handpieceDomain = await this.handpieceManager.GetByIdAsync(handpiece.Id);
                            var requiredParts = await handpieceDomain.Parts.GetRequiredPartsAsync();
                            var skus = requiredParts.Select(x => x.SKU).ToList();
                            await handpieceDomain.Parts.CancelAsync();
                            foreach (var sku in skus)
                            {
                                await sku.TryProcessMovementsChangesAsync(handpieceDomain.Job.Workshop);
                            }

                            await transaction.CommitAsync();
                        }

                        break;
                    }
            }
        }

        public virtual async Task ChangeJobStatusAsync(Job job, JobStatus status)
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                throw new InvalidOperationException();
            }

            if ((job.Status == JobStatus.Received || job.Status == JobStatus.BeingEstimated) && status == JobStatus.WaitingForApproval)
            {
                await this.notificationsService.CreateJobNotification(new JobEstimatedPayload(job), NotificationScope.Office, job);
            }

            if ((job.Status == JobStatus.WaitingForApproval || job.Status == JobStatus.EstimateSent) && status == JobStatus.BeingRepaired)
            {
                await this.notificationsService.CreateJobNotification(new JobApprovedForRepairPayload(job), NotificationScope.Workshop, job);
            }

            if ((job.Status == JobStatus.BeingRepaired) && status == JobStatus.ReadyToReturn)
            {
                await this.notificationsService.CreateJobNotification(new JobRepairCompletePayload(job), NotificationScope.Office, job);
            }

            if ((job.Status != JobStatus.SentComplete) && status == JobStatus.SentComplete)
            {
                var clientDomain = await this.clientManager.GetByIdAsync(job.ClientId);
                await clientDomain.Feedback.ProcessJobCompletionAsync();
            }

            job.Status = status;
            await this.repository.UpdateAsync(job);
            await this.repository.SaveChangesAsync();
        }
    }
}
