using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
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
    [PermissionsManager("Type", "/Domain/User")]
    [PermissionsManager("Entity", "/Domain/User/Entities/{entity}")]
    public class UsersController : BaseTelerikIndexBasicCrudController<Guid, ApplicationUser, EmptyEmployeeIndexViewModel, UserReadViewModel, UserDetailsViewModel, UserCreateViewModel, UserEditViewModel, UserDetailsViewModel>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IEntityControllerServices controllerServices, UserManager<ApplicationUser> userManager)
            : base(controllerServices)
        {
            this.userManager = userManager;

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.ValidateCreateModel = this.ValidateCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.InsertEntity = this.InsertEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;
            this.EditHandler.Overrides.ValidateEditModel = this.ValidateEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.UpdateEntity = this.UpdateEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;
            this.DeleteHandler.Overrides.DeleteEntity = this.DeleteEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private async Task<IQueryable<ApplicationUser>> PrepareReadQuery()
        {
            var usersRolesQuery = from userRoles in this.Repository.Query<IdentityUserRole<Guid>>()
                join roles in this.Repository.Query<Role>() on userRoles.RoleId equals roles.Id
                select new { UserId = userRoles.UserId, Role = roles };

            var usersRoles = await usersRolesQuery.ToListAsync();
            var usersWithRoles = usersRoles.GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.Select(y => y.Role).ToList());

            this.HttpContext.StoreTemporaryData("UsersRolesMap", usersWithRoles);

            var usersQuery = this.Repository.Query<ApplicationUser>();
            return usersQuery;
        }

        private UserReadViewModel ConvertEntityToViewModel(ApplicationUser entity, String[] allowedProperties)
        {
            var usersRolesMap = this.HttpContext.PeekTemporaryData<Dictionary<Guid, List<Role>>>("UsersRolesMap");

            if (!usersRolesMap.TryGetValue(entity.Id, out var roles))
            {
                roles = new List<Role>();
            }

            return new UserReadViewModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                Roles = String.Join(", ", roles.Select(x => x.Name)),
            };
        }

        private async Task<UserDetailsViewModel> ConvertToDetailsModel(ApplicationUser entity)
        {
            var usersRolesQuery = from userRoles in this.Repository.Query<IdentityUserRole<Guid>>()
                join roles in this.Repository.Query<Role>() on userRoles.RoleId equals roles.Id
                where userRoles.UserId == entity.Id
                select roles;

            return new UserDetailsViewModel
            {
                User = entity,
                Roles = await usersRolesQuery.ToListAsync(),
            };
        }

        private async Task InitializeCreateModel(UserCreateViewModel model, Boolean initial)
        {
            model.Roles = await this.GetCreatableRolesAsync();
        }

        private async Task<Boolean> ValidateCreateModel(UserCreateViewModel model)
        {
            var validatedUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = String.IsNullOrEmpty(model.Email) ? null : model.Email,
                LockoutEnabled = model.LockoutEnabled,
            };

            foreach (var userValidator in this.userManager.UserValidators)
            {
                var validationResult = await userValidator.ValidateAsync(this.userManager, validatedUser);
                validationResult.AddErrorsToModelState(this.ModelState);
            }

            foreach (var passwordValidator in this.userManager.PasswordValidators)
            {
                var validationResult = await passwordValidator.ValidateAsync(this.userManager, validatedUser, model.Password);
                validationResult.AddErrorsToModelState(this.ModelState);
            }

            var validRoles = await this.GetCreatableRolesAsync();
            if (validRoles.All(x => x.Id != model.RoleId))
            {
                this.ModelState.AddModelError(nameof(model.RoleId), "Invalid Role");
            }

            return this.ModelState.IsValid;
        }

        private Task InitializeNewEntity(ApplicationUser entity, UserCreateViewModel model)
        {
            entity.UserName = model.UserName;
            entity.Email = String.IsNullOrEmpty(model.Email) ? null : model.Email;
            entity.LockoutEnabled = model.LockoutEnabled;
            return Task.CompletedTask;
        }

        private async Task InsertEntity(ApplicationUser entity, UserCreateViewModel model)
        {
            var role = await this.Repository.QueryWithoutTracking<Role>().SingleAsync(x => x.Id == model.RoleId);

            await this.userManager.CreateAsync(entity, model.Password).ThrowOnErrorsAsync();
            await this.userManager.AddToRoleAsync(entity, role.Name).ThrowOnErrorsAsync();
        }

        private Task<IActionResult> GetCreateSuccessResult(ApplicationUser entity, UserCreateViewModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("UsersCreate", this.RedirectToAction("Index"));
        }

        private Task InitializeEditModelWithEntity(ApplicationUser entity, UserEditViewModel model)
        {
            model.UserName = entity.UserName;
            model.Email = entity.Email;
            model.LockoutEnabled = entity.LockoutEnabled;
            model.MustChangePasswordAtNextLogin = entity.MustChangePasswordAtNextLogin;
            return Task.CompletedTask;
        }

        private async Task<Boolean> ValidateEditModel(ApplicationUser entity, UserEditViewModel model)
        {
            var validatedUser = await this.Repository.QueryWithoutTracking<ApplicationUser>().SingleAsync(x => x.Id == entity.Id);
            validatedUser.UserName = model.UserName;
            validatedUser.Email = String.IsNullOrEmpty(model.Email) ? null : model.Email;
            validatedUser.LockoutEnabled = model.LockoutEnabled;

            foreach (var userValidator in this.userManager.UserValidators)
            {
                var validationResult = await userValidator.ValidateAsync(this.userManager, validatedUser);
                validationResult.AddErrorsToModelState(this.ModelState);
            }

            if (model.ShouldUpdatePassword())
            {
                foreach (var passwordValidator in this.userManager.PasswordValidators)
                {
                    var validationResult = await passwordValidator.ValidateAsync(this.userManager, validatedUser, model.Password);
                    validationResult.AddErrorsToModelState(this.ModelState);
                }
            }

            return this.ModelState.IsValid;
        }

        private Task UpdateExistingEntity(ApplicationUser entity, UserEditViewModel model)
        {
            entity.UserName = model.UserName;
            entity.Email = model.Email;
            entity.LockoutEnabled = model.LockoutEnabled;
            entity.MustChangePasswordAtNextLogin = model.MustChangePasswordAtNextLogin;

            if (model.ShouldUpdatePassword())
            {
                entity.PasswordHash = this.userManager.PasswordHasher.HashPassword(entity, model.Password);
            }

            return Task.CompletedTask;
        }

        private async Task UpdateEntity(ApplicationUser entity, UserEditViewModel model)
        {
            await this.userManager.UpdateAsync(entity).ThrowOnErrorsAsync();

            if (model.ShouldUpdatePassword())
            {
                await this.userManager.UpdateSecurityStampAsync(entity).ThrowOnErrorsAsync();
            }
        }

        private Task<IActionResult> GetEditSuccessResult(ApplicationUser entity, UserEditViewModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("UsersEdit", this.RedirectToAction("Index"));
        }

        private async Task DeleteEntity(ApplicationUser entity, Dictionary<String, Object> model)
        {
            var userRoles = await this.userManager.GetRolesAsync(entity);
            var validRoles = await this.GetCreatableRolesAsync();

            if (userRoles.Contains(ApplicationRoles.Client) && await this.Repository.QueryWithoutTracking<Client>().AnyAsync(x => x.UserId == entity.Id))
            {
                throw new InvalidOperationException("Existing Client prevents deletion of this user");
            }

            if (userRoles.Contains(ApplicationRoles.Corporate) && await this.Repository.QueryWithoutTracking<Corporate>().AnyAsync(x => x.UserId == entity.Id))
            {
                throw new InvalidOperationException("Existing Corporate prevents deletion of this user");
            }

            if (userRoles.Contains(ApplicationRoles.WorkshopTechnician) && await this.Repository.QueryWithoutTracking<Employee>().AnyAsync(x => x.ApplicationUserId == entity.Id))
            {
                throw new InvalidOperationException("Existing Employee prevents deletion of this user");
            }

            if (userRoles.Contains(ApplicationRoles.OfficeAdministrator) && await this.Repository.QueryWithoutTracking<Employee>().AnyAsync(x => x.ApplicationUserId == entity.Id))
            {
                throw new InvalidOperationException("Existing Employee prevents deletion of this user");
            }

            if (userRoles.Contains(ApplicationRoles.CompanyAdministrator) && await this.Repository.QueryWithoutTracking<Employee>().AnyAsync(x => x.ApplicationUserId == entity.Id))
            {
                throw new InvalidOperationException("Existing Employee prevents deletion of this user");
            }

            await this.userManager.DeleteAsync(entity).ThrowOnErrorsAsync();
        }

        private Task<IActionResult> GetDeleteSuccessResult(ApplicationUser entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("UsersDelete", this.RedirectToAction("Index"));
        }

        private Task<List<Role>> GetCreatableRolesAsync()
        {
            return this.Repository.QueryWithoutTracking<Role>()
                .Where(x => x.Name == ApplicationRoles.Administrator)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
