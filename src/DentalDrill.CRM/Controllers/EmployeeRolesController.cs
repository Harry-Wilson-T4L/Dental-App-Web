using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.EmployeeRoles;
using DentalDrill.CRM.Models.ViewModels.Permissions;
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
    [PermissionsManager("Type", "/Domain/EmployeeRole")]
    public class EmployeeRolesController : BaseTelerikIndexBasicCrudController<Guid, EmployeeRole, EmptyEmployeeIndexViewModel, EmployeeRole, EmployeeRoleEditModel, EmployeeRoleEditModel, EmployeeRoleEditModel, EmployeeRoleEditModel>
    {
        public EmployeeRolesController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.DetailsTabWorkshopsHandler = new BasicCrudDetailsActionHandler<Guid, EmployeeRole, EmployeeRoleDetailsModel>(this, this.ControllerServices, this.PermissionsValidator);

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = (e, m, a) => this.HybridFormResultAsync("EmployeeRolesCreate", this.RedirectToAction("Index"));

            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = (e, m, a) => this.HybridFormResultAsync("EmployeeRolesEdit", this.RedirectToAction("Index"));

            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = (e, a) => this.HybridFormResultAsync("EmployeeRolesDelete", this.RedirectToAction("Index"));

            this.DetailsTabWorkshopsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsTabWorkshopsModel;
        }

        protected BasicCrudDetailsActionHandler<Guid, EmployeeRole, EmployeeRoleDetailsModel> DetailsTabWorkshopsHandler { get; }

        public Task<IActionResult> DetailsTabWorkshops(Guid parentId) => this.DetailsTabWorkshopsHandler.Details(parentId);

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<EmployeeRoleEditModel> ConvertToDetailsModel(EmployeeRole entity)
        {
            var model = new EmployeeRoleEditModel
            {
                Original = entity,
                Name = entity.Name,
                Global = GlobalComponentHelper.GetAllFlags().Select(x => new PermissionReadWriteEditModel<GlobalComponent>(
                    x,
                    entity.GlobalComponentRead.HasFlag(x),
                    entity.GlobalComponentWrite.HasFlag(x))).ToList(),
                ClientComponents = ClientEntityComponentHelper.GetAllFlags().Select(x => new PermissionReadWriteEditModel<ClientEntityComponent>(
                    x,
                    entity.ClientComponentRead.HasFlag(x),
                    entity.ClientComponentWrite.HasFlag(x))).ToList(),
                ClientFields = ClientEntityFieldHelper.GetAllFlags().Select(x => new PermissionReadWriteInitEditModel<ClientEntityField>(
                    x,
                    entity.ClientFieldRead.HasFlag(x),
                    entity.ClientFieldWrite.HasFlag(x),
                    entity.ClientFieldInit.HasFlag(x))).ToList(),
                Inventory = InventoryPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<InventoryPermissions>(
                    x,
                    entity.InventoryAccess.HasFlag(x))).ToList(),
            };

            return Task.FromResult(model);
        }

        private Task InitializeCreateModel(EmployeeRoleEditModel model, Boolean initial)
        {
            if (initial)
            {
                model.Global = GlobalComponentHelper.GetAllFlags().Select(x => new PermissionReadWriteEditModel<GlobalComponent>(x)).ToList();
                model.ClientComponents = ClientEntityComponentHelper.GetAllFlags().Select(x => new PermissionReadWriteEditModel<ClientEntityComponent>(x)).ToList();
                model.ClientFields = ClientEntityFieldHelper.GetAllFlags().Select(x => new PermissionReadWriteInitEditModel<ClientEntityField>(x)).ToList();
                model.Inventory = InventoryPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<InventoryPermissions>(x)).ToList();
            }

            return Task.CompletedTask;
        }

        private Task InitializeNewEntity(EmployeeRole entity, EmployeeRoleEditModel model)
        {
            entity.Id = Guid.NewGuid();
            entity.Name = model.Name;
            entity.GlobalComponentRead = model.Global.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();
            entity.GlobalComponentWrite = model.Global.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.ClientComponentRead = model.ClientComponents.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();
            entity.ClientComponentWrite = model.ClientComponents.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.ClientFieldRead = model.ClientFields.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();
            entity.ClientFieldWrite = model.ClientFields.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.ClientFieldInit = model.ClientFields.Where(x => x.Init).Select(x => x.Object).ToList().CombineValue();
            entity.InventoryAccess = model.Inventory.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();

            return Task.CompletedTask;
        }

        private Task InitializeEditModelWithEntity(EmployeeRole entity, EmployeeRoleEditModel model)
        {
            model.Name = entity.Name;
            model.Global = GlobalComponentHelper.GetAllFlags().Select(x => new PermissionReadWriteEditModel<GlobalComponent>(
                x,
                entity.GlobalComponentRead.HasFlag(x),
                entity.GlobalComponentWrite.HasFlag(x))).ToList();
            model.ClientComponents = ClientEntityComponentHelper.GetAllFlags().Select(x => new PermissionReadWriteEditModel<ClientEntityComponent>(
                x,
                entity.ClientComponentRead.HasFlag(x),
                entity.ClientComponentWrite.HasFlag(x))).ToList();
            model.ClientFields = ClientEntityFieldHelper.GetAllFlags().Select(x => new PermissionReadWriteInitEditModel<ClientEntityField>(
                x,
                entity.ClientFieldRead.HasFlag(x),
                entity.ClientFieldWrite.HasFlag(x),
                entity.ClientFieldInit.HasFlag(x))).ToList();
            model.Inventory = InventoryPermissionsHelper.GetAllFlags().Select(x => new PermissionEditModel<InventoryPermissions>(
                x,
                entity.InventoryAccess.HasFlag(x))).ToList();

            return Task.CompletedTask;
        }

        private Task InitializeEditModel(EmployeeRole entity, EmployeeRoleEditModel model, Boolean initial)
        {
            model.Original = entity;
            return Task.CompletedTask;
        }

        private Task UpdateExistingEntity(EmployeeRole entity, EmployeeRoleEditModel model)
        {
            entity.Name = model.Name;
            entity.GlobalComponentRead = model.Global.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();
            entity.GlobalComponentWrite = model.Global.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.ClientComponentRead = model.ClientComponents.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();
            entity.ClientComponentWrite = model.ClientComponents.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.ClientFieldRead = model.ClientFields.Where(x => x.Read).Select(x => x.Object).ToList().CombineValue();
            entity.ClientFieldWrite = model.ClientFields.Where(x => x.Write).Select(x => x.Object).ToList().CombineValue();
            entity.ClientFieldInit = model.ClientFields.Where(x => x.Init).Select(x => x.Object).ToList().CombineValue();
            entity.InventoryAccess = model.Inventory.Where(x => x.Enabled).Select(x => x.Object).ToList().CombineValue();

            return Task.CompletedTask;
        }

        private async Task<EmployeeRoleDetailsModel> ConvertToDetailsTabWorkshopsModel(EmployeeRole entity)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            return new EmployeeRoleDetailsModel
            {
                Entity = entity,
                Access = employeeAccess,
            };
        }
    }
}
