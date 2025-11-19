using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeHandpieceExceptions;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/WorkshopRoleJobTypeHandpieceException")]
    [PermissionsManager("Entity", "/Domain/WorkshopRoleJobTypeHandpieceException/Entities/{entity}")]
    public class WorkshopRoleJobTypeHandpieceExceptionsController : BaseTelerikIndexlessDependentBasicCrudController<Guid, WorkshopRoleJobTypeHandpieceException, Guid, WorkshopRoleJobType, WorkshopRoleJobTypeHandpieceExceptionReadModel, WorkshopRoleJobTypeHandpieceExceptionEditModel, WorkshopRoleJobTypeHandpieceExceptionEditModel, WorkshopRoleJobTypeHandpieceExceptionEditModel, WorkshopRoleJobTypeHandpieceExceptionEditModel>
    {
        public WorkshopRoleJobTypeHandpieceExceptionsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = (p, e, m, a) => this.HybridFormResultAsync("WorkshopRoleJobTypeHandpieceExceptionsCreate", this.RedirectToAction("Details", "WorkshopRoleJobTypes", new { id = e.WorkshopRoleJobTypeId }));

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = (e, m, a) => this.HybridFormResultAsync("WorkshopRoleJobTypeHandpieceExceptionsEdit", this.RedirectToAction("Details", "WorkshopRoleJobTypes", new { id = e.WorkshopRoleJobTypeId }));

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = (e, a) => this.HybridFormResultAsync("WorkshopRoleJobTypeHandpieceExceptionsDelete", this.RedirectToAction("Details", "WorkshopRoleJobTypes", new { id = e.WorkshopRoleJobTypeId }));
        }

        private Task<IQueryable<WorkshopRoleJobTypeHandpieceException>> PrepareReadQuery(WorkshopRoleJobType parent)
        {
            var query = this.Repository.Query<WorkshopRoleJobTypeHandpieceException>()
                .Include(x => x.WorkshopRoleJobType)
                .ThenInclude(x => x.JobType)
                .Where(x => x.WorkshopRoleJobTypeId == parent.Id);

            return Task.FromResult(query);
        }

        private WorkshopRoleJobTypeHandpieceExceptionReadModel ConvertEntityToViewModel(WorkshopRoleJobType parent, WorkshopRoleJobTypeHandpieceException entity, String[] _)
        {
            return new WorkshopRoleJobTypeHandpieceExceptionReadModel
            {
                Id = entity.Id,
                JobTypeId = entity.WorkshopRoleJobType.JobTypeId,
                JobTypeName = entity.WorkshopRoleJobType.JobType.Name,
                WhenJobStatus = entity.WhenJobStatus.ToDisplayString(),
                WhenHandpieceStatus = entity.WhenHandpieceStatus.ToDisplayString(),
                ReadOnlyFields = entity.ReadOnlyFields.ToDisplayString(),
                HiddenFields = entity.HiddenFields.ToDisplayString(),
            };
        }

        private Task<WorkshopRoleJobTypeHandpieceException> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<WorkshopRoleJobTypeHandpieceException>()
                .Include(x => x.WorkshopRoleJobType)
                .ThenInclude(x => x.JobType)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<WorkshopRoleJobType> QuerySingleParentEntity(Guid id)
        {
            return this.Repository.Query<WorkshopRoleJobType>()
                .Include(x => x.JobType)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<WorkshopRoleJobTypeHandpieceExceptionEditModel> ConvertToDetailsModel(WorkshopRoleJobTypeHandpieceException entity)
        {
            var model = new WorkshopRoleJobTypeHandpieceExceptionEditModel
            {
                Original = entity,
                Parent = entity.WorkshopRoleJobType,
                WhenJobStatus = JobStatusFlagsHelper.GetSupportedFlags(entity.WorkshopRoleJobType.JobTypeId)
                    .Select(x => new PermissionEditModel<JobStatusFlags>(x, entity.WhenJobStatus.HasFlag(x)))
                    .ToList(),
                WhenHandpieceStatus = HandpieceStatusFlagsHelper.GetSupportedFlags(entity.WorkshopRoleJobType.JobTypeId)
                    .Select(x => new PermissionEditModel<HandpieceStatusFlags>(x, entity.WhenHandpieceStatus.HasFlag(x)))
                    .ToList(),
                Fields = HandpieceEntityFieldHelper.GetSupportedFlags(entity.WorkshopRoleJobType.JobTypeId)
                    .Select(x => new PermissionReadWriteEditModel<HandpieceEntityField>(x, entity.HiddenFields.HasFlag(x), entity.ReadOnlyFields.HasFlag(x)))
                    .ToList(),
            };

            return Task.FromResult(model);
        }

        private Task InitializeCreateModel(WorkshopRoleJobType parent, WorkshopRoleJobTypeHandpieceExceptionEditModel model, Boolean initial)
        {
            if (initial)
            {
                model.WhenJobStatus = JobStatusFlagsHelper.GetSupportedFlags(parent.JobTypeId)
                    .Select(x => new PermissionEditModel<JobStatusFlags>(x))
                    .ToList();
                model.WhenHandpieceStatus = HandpieceStatusFlagsHelper.GetSupportedFlags(parent.JobTypeId)
                    .Select(x => new PermissionEditModel<HandpieceStatusFlags>(x))
                    .ToList();
                model.Fields = HandpieceEntityFieldHelper.GetSupportedFlags(parent.JobTypeId)
                    .Select(x => new PermissionReadWriteEditModel<HandpieceEntityField>(x))
                    .ToList();
            }

            model.Parent = parent;

            return Task.CompletedTask;
        }

        private Task InitializeNewEntity(WorkshopRoleJobType parent, WorkshopRoleJobTypeHandpieceException entity, WorkshopRoleJobTypeHandpieceExceptionEditModel model)
        {
            entity.Id = Guid.NewGuid();
            entity.WorkshopRoleJobTypeId = parent.Id;
            entity.WhenJobStatus = model.WhenJobStatus.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();
            entity.WhenHandpieceStatus = model.WhenHandpieceStatus.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();
            entity.ReadOnlyFields = model.Fields.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.HiddenFields = model.Fields.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();

            return Task.CompletedTask;
        }

        private Task InitializeEditModelWithEntity(WorkshopRoleJobTypeHandpieceException entity, WorkshopRoleJobTypeHandpieceExceptionEditModel model)
        {
            model.WhenJobStatus = JobStatusFlagsHelper.GetSupportedFlags(entity.WorkshopRoleJobType.JobTypeId)
                .Select(x => new PermissionEditModel<JobStatusFlags>(x, entity.WhenJobStatus.HasFlag(x)))
                .ToList();
            model.WhenHandpieceStatus = HandpieceStatusFlagsHelper.GetSupportedFlags(entity.WorkshopRoleJobType.JobTypeId)
                .Select(x => new PermissionEditModel<HandpieceStatusFlags>(x, entity.WhenHandpieceStatus.HasFlag(x)))
                .ToList();
            model.Fields = HandpieceEntityFieldHelper.GetSupportedFlags(entity.WorkshopRoleJobType.JobTypeId)
                .Select(x => new PermissionReadWriteEditModel<HandpieceEntityField>(x, entity.HiddenFields.HasFlag(x), entity.ReadOnlyFields.HasFlag(x)))
                .ToList();

            return Task.CompletedTask;
        }

        private Task InitializeEditModel(WorkshopRoleJobTypeHandpieceException entity, WorkshopRoleJobTypeHandpieceExceptionEditModel model, Boolean initial)
        {
            model.Original = entity;
            model.Parent = entity.WorkshopRoleJobType;

            return Task.CompletedTask;
        }

        private Task UpdateExistingEntity(WorkshopRoleJobTypeHandpieceException entity, WorkshopRoleJobTypeHandpieceExceptionEditModel model)
        {
            entity.WhenJobStatus = model.WhenJobStatus.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();
            entity.WhenHandpieceStatus = model.WhenHandpieceStatus.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();
            entity.ReadOnlyFields = model.Fields.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.HiddenFields = model.Fields.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();

            return Task.CompletedTask;
        }
    }
}
