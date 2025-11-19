using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services.Data;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Services.Workflow
{
    public class EstimateRepairWorkflow : BaseRepairWorkflow
    {
        public EstimateRepairWorkflow(
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
        }
    }
}
