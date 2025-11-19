using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.Validation;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.Workflow;

namespace DentalDrill.CRM.Services.Workflow
{
    public interface IRepairWorkflow
    {
        Task<Job> HandleJobEvent(Guid jobId, JobWorkflowEvent jobEvent);

        Task<Handpiece> HandleHandpieceEvent(Guid handpieceId, HandpieceWorkflowEvent handpieceEvent);

        Task<JobStatus> GetNewJobStatusAsync();

        Task<HandpieceStatus> GetNewHandpieceStatusAsync();

        Task<HandpieceStatus> GetNewHandpieceStatusAsync(Job existingJob);

        Task<Boolean> CanAddNewHandpiecesAsync(Job existingJob);

        Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlForNewHandpieceAsync(Job job);

        Task<PropertiesAccessControlList<Handpiece>> GetPropertiesAccessControlAsync(Handpiece handpiece);

        Task<PropertiesAccessControlList<Job>> GetPropertiesAccessControlAsync(Job job);

        Task<ModelValidatorCollection<JobHandpieceEditModel>> GetHandpieceUpdateValidators(Handpiece handpiece, JobHandpieceEditModel model);

        Task<IReadOnlyList<HandpieceStatus>> GetAvailableStatusChangesAsync(Handpiece handpiece);

        Task<JobActionsList> GetJobActionsListAsync(Job job);

        Task<Boolean> CanChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus);

        Task ChangeHandpieceStatusAsync(Handpiece handpiece, HandpieceStatus newStatus);

        Task ChangeJobStatusAsync(Job job, JobStatus status);
    }
}
