using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class CorporateAccessController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ClientEmailsService clientEmailsService;

        public CorporateAccessController(IEntityControllerServices controllerServices, UserManager<ApplicationUser> userManager, ClientEmailsService clientEmailsService)
        {
            this.userManager = userManager;
            this.clientEmailsService = clientEmailsService;
            this.ControllerServices = controllerServices;
            this.PermissionsValidator = new DefaultEntityPermissionsValidator<Corporate>(this.ControllerServices.PermissionsHub, null, null, null);

            this.CreateUserHandler = new BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessCreateUserOperationModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ManageUserHandler = new BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessManageUserOperationModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.DeleteUserHandler = new BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessDeleteUserOperationModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ConfigurePageUrlHandler = new BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessConfigurePageUrlOperationModel>(this, this.ControllerServices, this.PermissionsValidator);

            this.CreateUserHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.CreateUserHandler.Overrides.InitializeOperationModel = this.InitializeCreateUserModel;
            this.CreateUserHandler.Overrides.ValidateOperationModel = this.ValidateCreateUserModel;
            this.CreateUserHandler.Overrides.ExecuteOperation = this.ExecuteCreateUserOperation;
            this.CreateUserHandler.Overrides.GetOperationSuccessResult = this.GetCreateUserSuccessResult;

            this.ManageUserHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ManageUserHandler.Overrides.InitializeOperationModel = this.InitializeManageUserModel;
            this.ManageUserHandler.Overrides.ValidateOperationModel = this.ValidateManageUserModel;
            this.ManageUserHandler.Overrides.ExecuteOperation = this.ExecuteManageUserOperation;
            this.ManageUserHandler.Overrides.GetOperationSuccessResult = this.GetManageUserSuccessResult;

            this.DeleteUserHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteUserHandler.Overrides.InitializeOperationModel = this.InitializeDeleteUserModel;
            this.DeleteUserHandler.Overrides.ExecuteOperation = this.ExecuteDeleteUserOperation;
            this.DeleteUserHandler.Overrides.GetOperationSuccessResult = this.GetDeleteUserSuccessResult;

            this.ConfigurePageUrlHandler.Overrides.InitializeOperationModel = this.InitializeConfigurePageUrlModel;
            this.ConfigurePageUrlHandler.Overrides.ValidateOperationModel = this.ValidateConfigurePageUrlModel;
            this.ConfigurePageUrlHandler.Overrides.ExecuteOperation = this.ExecuteConfigurePageUrlOperation;
            this.ConfigurePageUrlHandler.Overrides.GetOperationSuccessResult = this.GetConfigurePageUrlSuccessResult;
        }

        protected IEntityControllerServices ControllerServices { get; }

        protected IEntityPermissionsValidator<Corporate> PermissionsValidator { get; }

        protected IRepository Repository => this.ControllerServices.Repository;

        protected BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessCreateUserOperationModel> CreateUserHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessManageUserOperationModel> ManageUserHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessDeleteUserOperationModel> DeleteUserHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Corporate, CorporateAccessConfigurePageUrlOperationModel> ConfigurePageUrlHandler { get; }

        public Task<IActionResult> CreateUser(Guid id) => this.CreateUserHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateUser(Guid id, CorporateAccessCreateUserOperationModel model) => this.CreateUserHandler.Execute(id, model);

        public Task<IActionResult> ManageUser(Guid id) => this.ManageUserHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ManageUser(Guid id, CorporateAccessManageUserOperationModel model) => this.ManageUserHandler.Execute(id, model);

        public Task<IActionResult> DeleteUser(Guid id) => this.DeleteUserHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteUser(Guid id, CorporateAccessDeleteUserOperationModel model) => this.DeleteUserHandler.Execute(id, model);

        public Task<IActionResult> ConfigurePageUrl(Guid id) => this.ConfigurePageUrlHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ConfigurePageUrl(Guid id, CorporateAccessConfigurePageUrlOperationModel model) => this.ConfigurePageUrlHandler.Execute(id, model);

        private Task<Corporate> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<Corporate>("User").SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task InitializeCreateUserModel(Guid id, Corporate entity, CorporateAccessCreateUserOperationModel model, Boolean initial)
        {
            if (initial)
            {
                model.UserName = $"{entity.CorporateNo}{ClientUrlPathHelper.ConvertToUsername(entity.Name)}";
                model.Password = "Hub2153";
            }

            model.Corporate = entity;
            return Task.CompletedTask;
        }

        private async Task<Boolean> ValidateCreateUserModel(Guid id, Corporate entity, CorporateAccessCreateUserOperationModel model)
        {
            if (entity.UserId != null)
            {
                this.ModelState.AddModelError(String.Empty, "User already created");
                return false;
            }

            var validatedUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                LockoutEnabled = true,
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

            return this.ModelState.IsValid;
        }

        private async Task ExecuteCreateUserOperation(Guid id, Corporate entity, CorporateAccessCreateUserOperationModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                LockoutEnabled = true,
                MustChangePasswordAtNextLogin = true,
            };

            await this.userManager.CreateAsync(user, model.Password).ThrowOnErrorsAsync();
            await this.userManager.AddToRoleAsync(user, ApplicationRoles.Corporate).ThrowOnErrorsAsync();

            entity.UserId = user.Id;
            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetCreateUserSuccessResult(Guid id, Corporate entity, CorporateAccessCreateUserOperationModel model)
        {
            return this.HybridFormResultAsync("CorporateAccessCreateUser", this.RedirectToAction("Details", "Corporates", new { id = entity.Id, Tab = "Access" }));
        }

        private Task InitializeManageUserModel(Guid id, Corporate entity, CorporateAccessManageUserOperationModel model, Boolean initial)
        {
            if (initial)
            {
                model.UserName = entity.User.UserName;
                model.Email = entity.User.Email;
            }

            model.Corporate = entity;
            return Task.CompletedTask;
        }

        private async Task<Boolean> ValidateManageUserModel(Guid id, Corporate entity, CorporateAccessManageUserOperationModel model)
        {
            var validatedUser = await this.Repository.QueryWithoutTracking<ApplicationUser>().SingleAsync(x => x.Id == entity.UserId);
            validatedUser.UserName = model.UserName;

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

        private async Task ExecuteManageUserOperation(Guid id, Corporate entity, CorporateAccessManageUserOperationModel model)
        {
            entity.User.Email = model.Email;
            entity.User.UserName = model.UserName;
            await this.userManager.UpdateAsync(entity.User).ThrowOnErrorsAsync();

            if (model.ShouldUpdatePassword())
            {
                entity.User.PasswordHash = this.userManager.PasswordHasher.HashPassword(entity.User, model.Password);
                await this.userManager.UpdateSecurityStampAsync(entity.User).ThrowOnErrorsAsync();
            }
        }

        private Task<IActionResult> GetManageUserSuccessResult(Guid id, Corporate entity, CorporateAccessManageUserOperationModel model)
        {
            return this.HybridFormResultAsync("CorporateAccessManageUser", this.RedirectToAction("Details", "Corporates", new { id = entity.Id, Tab = "Access" }));
        }

        private Task InitializeDeleteUserModel(Guid id, Corporate entity, CorporateAccessDeleteUserOperationModel model, Boolean initial)
        {
            model.Corporate = entity;
            return Task.CompletedTask;
        }

        private async Task ExecuteDeleteUserOperation(Guid id, Corporate entity, CorporateAccessDeleteUserOperationModel model)
        {
            var userId = entity.UserId;

            entity.UserId = null;
            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();

            var user = await this.Repository.Query<ApplicationUser>().SingleAsync(x => x.Id == userId);
            await this.userManager.DeleteAsync(user).ThrowOnErrorsAsync();
        }

        private Task<IActionResult> GetDeleteUserSuccessResult(Guid id, Corporate entity, CorporateAccessDeleteUserOperationModel model)
        {
            return this.HybridFormResultAsync("CorporateAccessDeleteUser", this.RedirectToAction("Details", "Corporates", new { id = entity.Id, Tab = "Access" }));
        }

        private Task InitializeConfigurePageUrlModel(Guid id, Corporate entity, CorporateAccessConfigurePageUrlOperationModel model, Boolean initial)
        {
            if (initial)
            {
                model.UrlPath = entity.UrlPath;
            }

            model.Corporate = entity;
            return Task.CompletedTask;
        }

        private Task<Boolean> ValidateConfigurePageUrlModel(Guid id, Corporate entity, CorporateAccessConfigurePageUrlOperationModel model)
        {
            if (!String.IsNullOrEmpty(model.UrlPath))
            {
                var defaultRegex = new Regex(@"^Corporate(?<no>\d+)$");
                var defaultMatch = defaultRegex.Match(model.UrlPath);
                if (defaultMatch.Success)
                {
                    var no = Int32.Parse(defaultMatch.Groups["no"].Value);
                    if (no != entity.CorporateNo)
                    {
                        this.ModelState.AddModelError(nameof(model.UrlPath), "CorporateX path is reserved and is only valid when X is Corporate No");
                        return Task.FromResult(false);
                    }
                }
            }

            return Task.FromResult(true);
        }

        private async Task ExecuteConfigurePageUrlOperation(Guid id, Corporate entity, CorporateAccessConfigurePageUrlOperationModel model)
        {
            entity.UrlPath = model.UrlPath;
            if (String.IsNullOrEmpty(entity.UrlPath))
            {
                entity.UrlPath = $"Corporate{entity.CorporateNo}";
            }

            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetConfigurePageUrlSuccessResult(Guid id, Corporate entity, CorporateAccessConfigurePageUrlOperationModel model)
        {
            return this.HybridFormResultAsync("CorporateAccessConfigurePageUrl", this.RedirectToAction("Details", "Corporates", new { id = entity.Id, Tab = "Access" }));
        }
    }
}
