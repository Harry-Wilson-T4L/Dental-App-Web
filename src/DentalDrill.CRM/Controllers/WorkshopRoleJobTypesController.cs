using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypes;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/WorkshopRoleJobType")]
    [PermissionsManager("Entity", "/Domain/WorkshopRoleJobType/Entities/{entity}")]
    public class WorkshopRoleJobTypesController : BaseTelerikIndexlessDependentBasicCrudController<Guid, WorkshopRoleJobType, Guid, WorkshopRole, WorkshopRoleJobTypeReadModel, WorkshopRoleJobTypeEditModel, WorkshopRoleJobTypeCreateModel, WorkshopRoleJobTypeEditModel, WorkshopRoleJobTypeEditModel>
    {
        public WorkshopRoleJobTypesController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.DetailsTabStatusesHandler = new BasicCrudDetailsActionHandler<Guid, WorkshopRoleJobType, WorkshopRoleJobType>(this, this.ControllerServices, this.PermissionsValidator);
            this.DetailsTabJobExceptionsHandler = new BasicCrudDetailsActionHandler<Guid, WorkshopRoleJobType, WorkshopRoleJobType>(this, this.ControllerServices, this.PermissionsValidator);
            this.DetailsTabHandpieceExceptionsHandler = new BasicCrudDetailsActionHandler<Guid, WorkshopRoleJobType, WorkshopRoleJobType>(this, this.ControllerServices, this.PermissionsValidator);

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = (p, e, m, a) => this.HybridFormResultAsync("WorkshopRoleJobTypesCreate", this.RedirectToAction("Details", "WorkshopRoles", new { id = e.WorkshopRoleId }));

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = (e, m, a) => this.HybridFormResultAsync("WorkshopRoleJobTypesEdit", this.RedirectToAction("Details", "WorkshopRoles", new { id = e.WorkshopRoleId }));

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = (e, a) => this.HybridFormResultAsync("WorkshopRoleJobTypesDelete", this.RedirectToAction("Details", "WorkshopRoles", new { id = e.WorkshopRoleId }));
        }

        protected BasicCrudDetailsActionHandler<Guid, WorkshopRoleJobType, WorkshopRoleJobType> DetailsTabStatusesHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, WorkshopRoleJobType, WorkshopRoleJobType> DetailsTabJobExceptionsHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, WorkshopRoleJobType, WorkshopRoleJobType> DetailsTabHandpieceExceptionsHandler { get; }

        public Task<IActionResult> DetailsTabStatuses(Guid parentId) => this.DetailsTabStatusesHandler.Details(parentId);

        public Task<IActionResult> DetailsTabJobExceptions(Guid parentId) => this.DetailsTabJobExceptionsHandler.Details(parentId);

        public Task<IActionResult> DetailsTabHandpieceExceptions(Guid parentId) => this.DetailsTabHandpieceExceptionsHandler.Details(parentId);

        private Task<IQueryable<WorkshopRoleJobType>> PrepareReadQuery(WorkshopRole parent)
        {
            var query = this.Repository.Query<WorkshopRoleJobType>()
                .Include(x => x.WorkshopRole)
                .Include(x => x.JobType)
                .Where(x => x.WorkshopRoleId == parent.Id);

            return Task.FromResult(query);
        }

        private WorkshopRoleJobTypeReadModel ConvertEntityToViewModel(WorkshopRole parent, WorkshopRoleJobType entity, String[] _)
        {
            return new WorkshopRoleJobTypeReadModel
            {
                Id = entity.Id,
                WorkshopRoleId = entity.WorkshopRoleId,
                WorkshopRoleName = entity.WorkshopRole.Name,
                JobTypeId = entity.JobTypeId,
                JobTypeName = entity.JobType.Name,
                JobComponentRead = entity.JobComponentRead,
                JobComponentWrite = entity.JobComponentWrite,
                JobFieldRead = entity.JobFieldRead,
                JobFieldWrite = entity.JobFieldWrite,
                JobFieldInit = entity.JobFieldInit,
                HandpieceComponentRead = entity.HandpieceComponentRead,
                HandpieceComponentWrite = entity.HandpieceComponentWrite,
                HandpieceFieldRead = entity.HandpieceFieldRead,
                HandpieceFieldWrite = entity.HandpieceFieldWrite,
                HandpieceFieldInit = entity.HandpieceFieldInit,
            };
        }

        private Task<WorkshopRoleJobType> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<WorkshopRoleJobType>()
                .Include(x => x.WorkshopRole)
                .Include(x => x.JobType)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<WorkshopRoleJobTypeEditModel> ConvertToDetailsModel(WorkshopRoleJobType entity)
        {
            var model = new WorkshopRoleJobTypeEditModel
            {
                Original = entity,
                Parent = entity.WorkshopRole,
                JobComponents = JobEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteEditModel<JobEntityComponent>(
                    x,
                    entity.JobComponentRead.HasFlag(x),
                    entity.JobComponentWrite.HasFlag(x))).ToList(),
                JobFields = JobEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteInitEditModel<JobEntityField>(
                    x,
                    entity.JobFieldRead.HasFlag(x),
                    entity.JobFieldWrite.HasFlag(x),
                    entity.JobFieldInit.HasFlag(x))).ToList(),
                HandpieceComponents = HandpieceEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteEditModel<HandpieceEntityComponent>(
                    x,
                    entity.HandpieceComponentRead.HasFlag(x),
                    entity.HandpieceComponentWrite.HasFlag(x))).ToList(),
                HandpieceFields = HandpieceEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteInitEditModel<HandpieceEntityField>(
                    x,
                    entity.HandpieceFieldRead.HasFlag(x),
                    entity.HandpieceFieldWrite.HasFlag(x),
                    entity.HandpieceFieldInit.HasFlag(x))).ToList(),
            };

            return Task.FromResult(model);
        }

        private async Task InitializeCreateModel(WorkshopRole parent, WorkshopRoleJobTypeCreateModel model, Boolean initial)
        {
            model.Parent = parent;
            model.JobTypes = await this.Repository.Query<JobType>().OrderBy(x => x.Name).ToListAsync();
        }

        private Task InitializeNewEntity(WorkshopRole parent, WorkshopRoleJobType entity, WorkshopRoleJobTypeCreateModel model)
        {
            entity.Id = Guid.NewGuid();
            entity.WorkshopRoleId = parent.Id;
            entity.JobTypeId = model.JobTypeId;
            entity.JobComponentRead = JobEntityComponentHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.JobComponentWrite = JobEntityComponentHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.JobFieldRead = JobEntityFieldHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.JobFieldWrite = JobEntityFieldHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.JobFieldInit = JobEntityFieldHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.HandpieceComponentRead = HandpieceEntityComponentHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.HandpieceComponentWrite = HandpieceEntityComponentHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.HandpieceFieldRead = HandpieceEntityFieldHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.HandpieceFieldWrite = HandpieceEntityFieldHelper.GetSupportedFlags(model.JobTypeId).CombineValue();
            entity.HandpieceFieldInit = HandpieceEntityFieldHelper.GetSupportedFlags(model.JobTypeId).CombineValue();

            entity.StatusPermissions = JobStatusHelper.GetSupportedStatuses(model.JobTypeId)
                .Select(x => new WorkshopRoleJobTypeStatus
                {
                    WorkshopRoleJobTypeId = entity.Id,
                    JobStatus = x,
                    JobTransitions = JobStatusFlagsHelper.GetSupportedJobTransitions(model.JobTypeId, x),
                    HandpieceTransitionsFrom = HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsFrom(model.JobTypeId, x),
                    HandpieceTransitionsTo = HandpieceStatusFlagsHelper.GetSupportedHandpieceTransitionsTo(model.JobTypeId, x),
                })
                .ToList();

            return Task.CompletedTask;
        }

        private Task InitializeEditModelWithEntity(WorkshopRoleJobType entity, WorkshopRoleJobTypeEditModel model)
        {
            model.JobComponents = JobEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteEditModel<JobEntityComponent>(
                x,
                entity.JobComponentRead.HasFlag(x),
                entity.JobComponentWrite.HasFlag(x))).ToList();
            model.JobFields = JobEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteInitEditModel<JobEntityField>(
                x,
                entity.JobFieldRead.HasFlag(x),
                entity.JobFieldWrite.HasFlag(x),
                entity.JobFieldInit.HasFlag(x))).ToList();
            model.HandpieceComponents = HandpieceEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteEditModel<HandpieceEntityComponent>(
                x,
                entity.HandpieceComponentRead.HasFlag(x),
                entity.HandpieceComponentWrite.HasFlag(x))).ToList();
            model.HandpieceFields = HandpieceEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).Select(x => new PermissionReadWriteInitEditModel<HandpieceEntityField>(
                x,
                entity.HandpieceFieldRead.HasFlag(x),
                entity.HandpieceFieldWrite.HasFlag(x),
                entity.HandpieceFieldInit.HasFlag(x))).ToList();

            return Task.CompletedTask;
        }

        private Task InitializeEditModel(WorkshopRoleJobType entity, WorkshopRoleJobTypeEditModel model, Boolean initial)
        {
            model.Original = entity;
            model.Parent = entity.WorkshopRole;
            return Task.CompletedTask;
        }

        private Task UpdateExistingEntity(WorkshopRoleJobType entity, WorkshopRoleJobTypeEditModel model)
        {
            entity.JobComponentRead = model.JobComponents.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue() & JobEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.JobComponentWrite = model.JobComponents.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue() & JobEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.JobFieldRead = model.JobFields.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue() & JobEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.JobFieldWrite = model.JobFields.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue() & JobEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.JobFieldInit = model.JobFields.Where(x => x.Init).Select(x => x.Object).ToList().CombineValue() & JobEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.HandpieceComponentRead = model.HandpieceComponents.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue() & HandpieceEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.HandpieceComponentWrite = model.HandpieceComponents.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue() & HandpieceEntityComponentHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.HandpieceFieldRead = model.HandpieceFields.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue() & HandpieceEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.HandpieceFieldWrite = model.HandpieceFields.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue() & HandpieceEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();
            entity.HandpieceFieldInit = model.HandpieceFields.Where(x => x.Init).Select(x => x.Object).ToList().CombineValue() & HandpieceEntityFieldHelper.GetSupportedFlags(entity.JobTypeId).CombineValue();

            return Task.CompletedTask;
        }
    }
}
