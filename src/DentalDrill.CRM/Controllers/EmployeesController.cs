using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Theming;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Identity.Data;
using DevGuild.AspNetCore.Services.Identity.Models;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/Employee")]
    [PermissionsManager("Entity", "/Domain/Employee/Entities/{entity}")]
    public class EmployeesController : BaseTelerikIndexBasicCrudController<Guid, Employee, EmptyEmployeeIndexViewModel, EmployeeReadModel, Employee, EmployeeCreateModel, EmployeeEditModel, Employee>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IThemesService themesService;

        public EmployeesController(IEntityControllerServices controllerServices, UserManager<ApplicationUser> userManager, IThemesService themesService)
            : base(controllerServices)
        {
            this.userManager = userManager;
            this.themesService = themesService;

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.ValidateCreateModel = this.ValidateCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.InsertEntity = this.InsertEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;
            this.EditHandler.Overrides.UpdateEntity = this.UpdateEntity;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.DeleteEntity = this.DeleteEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<IQueryable<Employee>> PrepareReadQuery()
        {
            var query = this.Repository.Query<Employee>("ApplicationUser", "Role", "AppearanceBackground");
            if (!(this.Request.Query.TryGetValue("ShowDeleted", out var deletedValues) && deletedValues.Equals("true")))
            {
                query = query.Where(x => x.DeletionStatus == DeletionStatus.Normal);
            }

            return Task.FromResult(query);
        }

        private EmployeeReadModel ConvertEntityToViewModel(Employee entity, String[] allowedProperties)
        {
            return new EmployeeReadModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.ApplicationUser.UserName,
                RoleId = entity.RoleId,
                RoleName = entity.Role.Name,
                Type = entity.Type,
                TypeName = entity.Type.ToDisplayString(),
                DeletionStatus = entity.DeletionStatus.ToString(),
            };
        }

        private Task<Employee> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<Employee>("ApplicationUser", "Role", "AppearanceBackground").SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task InitializeCreateModel(EmployeeCreateModel model, Boolean initial)
        {
            if (initial)
            {
                model.AppearanceTheme = "light2";
            }

            model.AvailableThemes = this.themesService.GetAvailableThemes();
            model.AvailableRoles = await this.Repository.QueryWithoutTracking<EmployeeRole>().OrderBy(x => x.Name).ToListAsync();
        }

        private async Task<Boolean> ValidateCreateModel(EmployeeCreateModel model)
        {
            var validatedUser = new ApplicationUser
            {
                UserName = model.UserName ?? String.Empty,
            };

            foreach (var userValidator in this.userManager.UserValidators)
            {
                var validationResult = await userValidator.ValidateAsync(this.userManager, validatedUser);
                validationResult.AddErrorsToModelState(this.ModelState);
            }

            foreach (var passwordValidator in this.userManager.PasswordValidators)
            {
                var validationResult = await passwordValidator.ValidateAsync(this.userManager, validatedUser, model.Password ?? String.Empty);
                validationResult.AddErrorsToModelState(this.ModelState);
            }

            return this.ModelState.IsValid;
        }

        private Task InitializeNewEntity(Employee entity, EmployeeCreateModel model)
        {
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.RoleId = model.RoleId;
            entity.Type = model.Type;
            entity.AppearanceTheme = model.AppearanceTheme;
            entity.AppearanceBackgroundId = model.AppearanceBackgroundId;
            entity.AppearanceOpacity = model.AppearanceOpacity;
            return Task.CompletedTask;
        }

        private async Task InsertEntity(Employee entity, EmployeeCreateModel model)
        {
            String roleName;
            switch (model.Type)
            {
                case EmployeeType.WorkshopTechnician:
                    roleName = ApplicationRoles.WorkshopTechnician;
                    break;
                case EmployeeType.OfficeAdministrator:
                    roleName = ApplicationRoles.OfficeAdministrator;
                    break;
                case EmployeeType.CompanyAdministrator:
                    roleName = ApplicationRoles.CompanyAdministrator;
                    break;
                case EmployeeType.CompanyManager:
                    roleName = ApplicationRoles.CompanyManager;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
            };

            await this.userManager.CreateAsync(user, model.Password).ThrowOnErrorsAsync();
            await this.userManager.AddToRoleAsync(user, roleName).ThrowOnErrorsAsync();

            entity.ApplicationUserId = user.Id;
            await this.Repository.InsertAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private async Task InitializeEditModel(Employee entity, EmployeeEditModel model, Boolean initial)
        {
            model.AvailableThemes = this.themesService.GetAvailableThemes();
            model.AvailableRoles = await this.Repository.QueryWithoutTracking<EmployeeRole>().OrderBy(x => x.Name).ToListAsync();
            model.Original = entity;
        }

        private async Task UpdateEntity(Employee entity, EmployeeEditModel model)
        {
            String roleName;
            switch (model.Type)
            {
                case EmployeeType.WorkshopTechnician:
                    roleName = ApplicationRoles.WorkshopTechnician;
                    break;
                case EmployeeType.OfficeAdministrator:
                    roleName = ApplicationRoles.OfficeAdministrator;
                    break;
                case EmployeeType.CompanyAdministrator:
                    roleName = ApplicationRoles.CompanyAdministrator;
                    break;
                case EmployeeType.CompanyManager:
                    roleName = ApplicationRoles.CompanyManager;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var existingRoles = await this.userManager.GetRolesAsync(entity.ApplicationUser);
            if (existingRoles.Count != 1 || existingRoles[0] != roleName)
            {
                await this.userManager.RemoveFromRolesAsync(entity.ApplicationUser, existingRoles).ThrowOnErrorsAsync();
                await this.userManager.AddToRoleAsync(entity.ApplicationUser, roleName).ThrowOnErrorsAsync();
            }

            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetCreateSuccessResult(Employee entity, EmployeeCreateModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("EmployeesCreate", this.RedirectToAction("Index"));
        }

        private Task<IActionResult> GetEditSuccessResult(Employee entity, EmployeeEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("EmployeesEdit", this.RedirectToAction("Index"));
        }

        private async Task DeleteEntity(Employee entity, Dictionary<String, Object> additionalData)
        {
            if (entity.DeletionStatus == DeletionStatus.Normal)
            {
                entity.DeletionStatus = DeletionStatus.Deleted;

                await this.Repository.UpdateAsync(entity);
                await this.Repository.SaveChangesAsync();

                var user = await this.Repository.Query<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == entity.ApplicationUserId);
                if (user != null)
                {
                    await this.userManager.RemovePasswordAsync(user).ThrowOnErrorsAsync();
                    await this.userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(2099, 12, 31, 0, 0, 0, TimeSpan.Zero)).ThrowOnErrorsAsync();
                    await this.Repository.SaveChangesAsync();
                }
            }
            else
            {
                entity.DeletionStatus = DeletionStatus.Normal;

                await this.Repository.UpdateAsync(entity);
                await this.Repository.SaveChangesAsync();

                var user = await this.Repository.Query<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == entity.ApplicationUserId);
                if (user != null)
                {
                    await this.userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).ThrowOnErrorsAsync();
                    await this.Repository.SaveChangesAsync();
                }
            }
        }

        private Task<IActionResult> GetDeleteSuccessResult(Employee entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("EmployeesDelete", this.RedirectToAction("Index"));
        }
    }
}
