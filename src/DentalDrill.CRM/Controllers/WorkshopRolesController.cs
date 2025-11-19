using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using DentalDrill.CRM.Models.ViewModels.WorkshopRoles;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/WorkshopRole")]
    [PermissionsManager("Entity", "/Domain/WorkshopRole/Entities/{entity}")]
    public class WorkshopRolesController : BaseTelerikIndexBasicCrudController<Guid, WorkshopRole, EmptyEmployeeIndexViewModel, WorkshopRole, WorkshopRoleEditModel, WorkshopRoleEditModel, WorkshopRoleEditModel, WorkshopRoleEditModel>
    {
        public WorkshopRolesController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.DetailsTabJobTypesHandler = new BasicCrudDetailsActionHandler<Guid, WorkshopRole, WorkshopRoleDetailsModel>(this, this.ControllerServices, this.PermissionsValidator);

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = (e, m, a) => this.HybridFormResultAsync("WorkshopRolesCreate", this.RedirectToAction("Index"));

            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = (e, m, a) => this.HybridFormResultAsync("WorkshopRolesEdit", this.RedirectToAction("Index"));

            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = (e, a) => this.HybridFormResultAsync("WorkshopRolesDelete", this.RedirectToAction("Index"));

            this.DetailsTabJobTypesHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsTabJobTypesModel;
        }

        protected BasicCrudDetailsActionHandler<Guid, WorkshopRole, WorkshopRoleDetailsModel> DetailsTabJobTypesHandler { get; }

        public Task<IActionResult> DetailsTabJobTypes(Guid parentId) => this.DetailsTabJobTypesHandler.Details(parentId);

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<WorkshopRoleEditModel> ConvertToDetailsModel(WorkshopRole entity)
        {
            var model = new WorkshopRoleEditModel
            {
                Original = entity,
                Name = entity.Name,
                WorkshopAccess = WorkshopPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<WorkshopPermissions>(
                    x,
                    entity.WorkshopAccess.HasFlag(x))).ToList(),
                InventoryAccess = InventoryMovementPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<InventoryMovementPermissions>(
                    x,
                    entity.InventoryAccess.HasFlag(x))).ToList(),
            };

            return Task.FromResult(model);
        }

        private Task InitializeCreateModel(WorkshopRoleEditModel model, Boolean initial)
        {
            if (initial)
            {
                model.WorkshopAccess = WorkshopPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<WorkshopPermissions>(x)).ToList();
                model.InventoryAccess = InventoryMovementPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<InventoryMovementPermissions>(x)).ToList();
            }

            return Task.CompletedTask;
        }

        private Task InitializeNewEntity(WorkshopRole entity, WorkshopRoleEditModel model)
        {
            entity.Id = Guid.NewGuid();
            entity.Name = model.Name;
            entity.WorkshopAccess = model.WorkshopAccess.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();
            entity.InventoryAccess = model.InventoryAccess.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();

            return Task.CompletedTask;
        }

        private Task InitializeEditModelWithEntity(WorkshopRole entity, WorkshopRoleEditModel model)
        {
            model.Name = entity.Name;
            model.WorkshopAccess = WorkshopPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<WorkshopPermissions>(
                x,
                entity.WorkshopAccess.HasFlag(x))).ToList();
            model.InventoryAccess = InventoryMovementPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<InventoryMovementPermissions>(
                x,
                entity.InventoryAccess.HasFlag(x))).ToList();

            return Task.CompletedTask;
        }

        private Task InitializeEditModel(WorkshopRole entity, WorkshopRoleEditModel model, Boolean initial)
        {
            model.Original = entity;
            return Task.CompletedTask;
        }

        private Task UpdateExistingEntity(WorkshopRole entity, WorkshopRoleEditModel model)
        {
            entity.Name = model.Name;
            entity.WorkshopAccess = model.WorkshopAccess.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();
            entity.InventoryAccess = model.InventoryAccess.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();

            return Task.CompletedTask;
        }

        private async Task<WorkshopRoleDetailsModel> ConvertToDetailsTabJobTypesModel(WorkshopRole entity)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            return new WorkshopRoleDetailsModel
            {
                Entity = entity,
                Access = employeeAccess,
            };
        }
    }
}
