using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeStatuses;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/WorkshopRoleJobTypeStatus")]
    [PermissionsManager("Entity", "/Domain/WorkshopRoleJobTypeStatus/Entities/{entity}")]
    public class WorkshopRoleJobTypeStatusesController : BaseTelerikIndexlessDependentBasicCrudController<Guid, WorkshopRoleJobTypeStatus, Guid, WorkshopRoleJobType, WorkshopRoleJobTypeStatusReadModel, WorkshopRoleJobTypeStatusEditModel, WorkshopRoleJobTypeStatusEditModel, WorkshopRoleJobTypeStatusEditModel, WorkshopRoleJobTypeStatusEditModel>
    {
        public WorkshopRoleJobTypeStatusesController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = (e, m, a) => this.HybridFormResultAsync("WorkshopRoleJobTypeStatusesEdit", this.RedirectToAction("Details", "WorkshopRoleJobTypes", new { id = e.WorkshopRoleJobTypeId }));
        }

        [NonAction]
        public override Task<IActionResult> Create(Guid parentId) => throw new NotSupportedException();

        [NonAction]
        public override Task<IActionResult> Create(Guid parentId, WorkshopRoleJobTypeStatusEditModel model) => throw new NotSupportedException();

        [NonAction]
        public override Task<IActionResult> Delete(Guid id) => throw new NotSupportedException();

        [NonAction]
        public override Task<IActionResult> DeleteConfirmed(Guid id) => throw new NotSupportedException();

        private Task<IQueryable<WorkshopRoleJobTypeStatus>> PrepareReadQuery(WorkshopRoleJobType parent)
        {
            var query = this.Repository.Query<WorkshopRoleJobTypeStatus>()
                .Include(x => x.WorkshopRoleJobType)
                .ThenInclude(x => x.JobType)
                .Where(x => x.WorkshopRoleJobTypeId == parent.Id);

            return Task.FromResult(query);
        }

        private WorkshopRoleJobTypeStatusReadModel ConvertEntityToViewModel(WorkshopRoleJobType parent, WorkshopRoleJobTypeStatus entity, String[] _)
        {
            return new WorkshopRoleJobTypeStatusReadModel
            {
                Id = entity.Id,
                JobTypeId = entity.WorkshopRoleJobType.JobTypeId,
                JobTypeName = entity.WorkshopRoleJobType.JobType.Name,
                JobStatus = entity.JobStatus.ToDisplayString(),
                JobTransitions = entity.JobTransitions.ToDisplayString(),
                HandpieceTransitionsFrom = entity.HandpieceTransitionsFrom.ToDisplayString(),
                HandpieceTransitionsTo = entity.HandpieceTransitionsTo.ToDisplayString(),
            };
        }

        private Task<WorkshopRoleJobTypeStatus> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<WorkshopRoleJobTypeStatus>()
                .Include(x => x.WorkshopRoleJobType)
                .ThenInclude(x => x.JobType)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<WorkshopRoleJobTypeStatusEditModel> ConvertToDetailsModel(WorkshopRoleJobTypeStatus entity)
        {
            var model = new WorkshopRoleJobTypeStatusEditModel
            {
                Original = entity,
                Parent = entity.WorkshopRoleJobType,
                JobTransitions = JobStatusFlagsHelper.GetSupportedJobTransitions(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus)
                    .SplitValue()
                    .Select(x => new PermissionEditModel<JobStatusFlags>(x, entity.JobTransitions.HasFlag(x)))
                    .ToList(),
                HandpieceTransitionsFrom = HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsFrom(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus)
                    .SplitValue()
                    .Select(x => new PermissionEditModel<HandpieceStatusFlags>(x, entity.HandpieceTransitionsFrom.HasFlag(x)))
                    .ToList(),
                HandpieceTransitionsTo = HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsTo(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus)
                    .SplitValue()
                    .Select(x => new PermissionEditModel<HandpieceStatusFlags>(x, entity.HandpieceTransitionsTo.HasFlag(x)))
                    .ToList(),
            };

            return Task.FromResult(model);
        }

        private Task InitializeEditModelWithEntity(WorkshopRoleJobTypeStatus entity, WorkshopRoleJobTypeStatusEditModel model)
        {
            model.JobTransitions = JobStatusFlagsHelper.GetSupportedJobTransitions(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus)
                .SplitValue()
                .Select(x => new PermissionEditModel<JobStatusFlags>(x, entity.JobTransitions.HasFlag(x)))
                .ToList();
            model.HandpieceTransitionsFrom = HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsFrom(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus)
                .SplitValue()
                .Select(x => new PermissionEditModel<HandpieceStatusFlags>(x, entity.HandpieceTransitionsFrom.HasFlag(x)))
                .ToList();
            model.HandpieceTransitionsTo = HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsTo(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus)
                .SplitValue()
                .Select(x => new PermissionEditModel<HandpieceStatusFlags>(x, entity.HandpieceTransitionsTo.HasFlag(x)))
                .ToList();

            return Task.CompletedTask;
        }

        private Task InitializeEditModel(WorkshopRoleJobTypeStatus entity, WorkshopRoleJobTypeStatusEditModel model, Boolean initial)
        {
            model.Original = entity;
            model.Parent = entity.WorkshopRoleJobType;
            return Task.CompletedTask;
        }

        private Task UpdateExistingEntity(WorkshopRoleJobTypeStatus entity, WorkshopRoleJobTypeStatusEditModel model)
        {
            entity.JobTransitions = model.JobTransitions.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue()
                                    & JobStatusFlagsHelper.GetSupportedJobTransitions(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus);
            entity.HandpieceTransitionsFrom = model.HandpieceTransitionsFrom.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue()
                                              & HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsFrom(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus);
            entity.HandpieceTransitionsTo = model.HandpieceTransitionsTo.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue()
                                            & HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsTo(entity.WorkshopRoleJobType.JobTypeId, entity.JobStatus);

            return Task.CompletedTask;
        }
    }
}
