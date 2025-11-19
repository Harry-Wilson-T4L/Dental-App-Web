using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.EmployeeRoleWorkshops;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/EmployeeRoleWorkshop")]
    public class EmployeeRoleWorkshopsController : BaseTelerikIndexlessDependentBasicCrudController<Guid, EmployeeRoleWorkshop, Guid, EmployeeRole, EmployeeRoleWorkshopReadModel, EmployeeRoleWorkshopEditModel, EmployeeRoleWorkshopEditModel, EmployeeRoleWorkshopEditModel, EmployeeRoleWorkshopEditModel>
    {
        public EmployeeRoleWorkshopsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = (p, e, m, a) => this.HybridFormResultAsync("EmployeeRoleWorkshopsCreate", this.RedirectToAction("Details", "EmployeeRoles", new { id = e.EmployeeRoleId }));

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = (e, m, a) => this.HybridFormResultAsync("EmployeeRoleWorkshopsEdit", this.RedirectToAction("Details", "EmployeeRoles", new { id = e.EmployeeRoleId }));

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = (e, a) => this.HybridFormResultAsync("EmployeeRoleWorkshopsDelete", this.RedirectToAction("Details", "EmployeeRoles", new { id = e.EmployeeRoleId }));
        }

        private Task<IQueryable<EmployeeRoleWorkshop>> PrepareReadQuery(EmployeeRole parent)
        {
            var query = this.Repository.Query<EmployeeRoleWorkshop>()
                .Include(x => x.EmployeeRole)
                .Include(x => x.Workshop)
                .Include(x => x.WorkshopRole)
                .Where(x => x.EmployeeRoleId == parent.Id);

            return Task.FromResult(query);
        }

        private EmployeeRoleWorkshopReadModel ConvertEntityToViewModel(EmployeeRole parent, EmployeeRoleWorkshop entity, String[] _)
        {
            return new EmployeeRoleWorkshopReadModel
            {
                Id = entity.Id,
                WorkshopId = entity.WorkshopId,
                WorkshopName = entity.Workshop.Name,
                WorkshopRoleId = entity.WorkshopRoleId,
                WorkshopRoleName = entity.WorkshopRole.Name,
                EmployeeType = entity.EmployeeType,
                EmployeeTypeName = entity.EmployeeType.ToDisplayString(),
            };
        }

        private Task<EmployeeRoleWorkshop> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<EmployeeRoleWorkshop>()
                .Include(x => x.EmployeeRole)
                .Include(x => x.Workshop)
                .Include(x => x.WorkshopRole)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<EmployeeRoleWorkshopEditModel> ConvertToDetailsModel(EmployeeRoleWorkshop entity)
        {
            var model = new EmployeeRoleWorkshopEditModel
            {
                Original = entity,
                Parent = entity.EmployeeRole,
                WorkshopId = entity.WorkshopId,
                WorkshopRoleId = entity.WorkshopRoleId,
                EmployeeType = entity.EmployeeType,
                Workshops = await this.Repository.QueryWithoutTracking<Workshop>().OrderBy(x => x.OrderNo).ToListAsync(),
                WorkshopRoles = await this.Repository.QueryWithoutTracking<WorkshopRole>().OrderBy(x => x.Name).ToListAsync(),
            };

            return model;
        }

        private async Task InitializeCreateModel(EmployeeRole parent, EmployeeRoleWorkshopEditModel model, Boolean initial)
        {
            if (initial)
            {
            }

            model.Parent = parent;
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>().OrderBy(x => x.OrderNo).ToListAsync();
            model.WorkshopRoles = await this.Repository.QueryWithoutTracking<WorkshopRole>().OrderBy(x => x.Name).ToListAsync();
        }

        private Task InitializeNewEntity(EmployeeRole parent, EmployeeRoleWorkshop entity, EmployeeRoleWorkshopEditModel model)
        {
            entity.Id = Guid.NewGuid();
            entity.EmployeeRoleId = parent.Id;
            entity.WorkshopId = model.WorkshopId;
            entity.WorkshopRoleId = model.WorkshopRoleId;
            entity.EmployeeType = model.EmployeeType;

            return Task.CompletedTask;
        }

        private Task InitializeEditModelWithEntity(EmployeeRoleWorkshop entity, EmployeeRoleWorkshopEditModel model)
        {
            model.WorkshopId = entity.WorkshopId;
            model.WorkshopRoleId = entity.WorkshopRoleId;
            model.EmployeeType = entity.EmployeeType;

            return Task.CompletedTask;
        }

        private async Task InitializeEditModel(EmployeeRoleWorkshop entity, EmployeeRoleWorkshopEditModel model, Boolean initial)
        {
            model.Original = entity;
            model.Parent = entity.EmployeeRole;
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>().OrderBy(x => x.OrderNo).ToListAsync();
            model.WorkshopRoles = await this.Repository.QueryWithoutTracking<WorkshopRole>().OrderBy(x => x.Name).ToListAsync();
        }

        private Task UpdateExistingEntity(EmployeeRoleWorkshop entity, EmployeeRoleWorkshopEditModel model)
        {
            entity.WorkshopRoleId = model.WorkshopRoleId;
            entity.EmployeeType = model.EmployeeType;

            return Task.CompletedTask;
        }
    }
}
