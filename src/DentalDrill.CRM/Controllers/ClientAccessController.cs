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
using DevGuild.AspNetCore.Services.Mail;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientAccessController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ClientEmailsService clientEmailsService;
        private readonly UserEntityResolver userEntityResolver;

        public ClientAccessController(IEntityControllerServices controllerServices, UserManager<ApplicationUser> userManager, ClientEmailsService clientEmailsService, UserEntityResolver userEntityResolver)
        {
            this.userManager = userManager;
            this.clientEmailsService = clientEmailsService;
            this.userEntityResolver = userEntityResolver;

            this.ControllerServices = controllerServices;
            this.PermissionsValidator = new DefaultEntityPermissionsValidator<Client>(this.ControllerServices.PermissionsHub, null, null, null);

            this.CreateUserHandler = new BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessCreateUserOperationModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ManageUserHandler = new BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessManageUserOperationModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.DeleteUserHandler = new BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessDeleteUserOperationModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ConfigurePageUrlHandler = new BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessConfigurePageUrlOperationModel>(this, this.ControllerServices, this.PermissionsValidator);

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

        protected IEntityPermissionsValidator<Client> PermissionsValidator { get; }

        protected IRepository Repository => this.ControllerServices.Repository;

        protected BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessCreateUserOperationModel> CreateUserHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessManageUserOperationModel> ManageUserHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessDeleteUserOperationModel> DeleteUserHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Client, ClientAccessConfigurePageUrlOperationModel> ConfigurePageUrlHandler { get; }

        public Task<IActionResult> CreateUser(Guid id) => this.CreateUserHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateUser(Guid id, ClientAccessCreateUserOperationModel model) => this.CreateUserHandler.Execute(id, model);

        public Task<IActionResult> ManageUser(Guid id) => this.ManageUserHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ManageUser(Guid id, ClientAccessManageUserOperationModel model) => this.ManageUserHandler.Execute(id, model);

        public Task<IActionResult> DeleteUser(Guid id) => this.DeleteUserHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteUser(Guid id, ClientAccessDeleteUserOperationModel model) => this.DeleteUserHandler.Execute(id, model);

        public Task<IActionResult> ConfigurePageUrl(Guid id) => this.ConfigurePageUrlHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ConfigurePageUrl(Guid id, ClientAccessConfigurePageUrlOperationModel model) => this.ConfigurePageUrlHandler.Execute(id, model);

        private Task<Client> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<Client>("User").SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task InitializeCreateUserModel(Guid id, Client entity, ClientAccessCreateUserOperationModel model, Boolean initial)
        {
            if (initial)
            {
                model.UserName = $"{entity.ClientNo}{ClientUrlPathHelper.ConvertToUsername(entity.Name)}";
                model.Password = "Hub2153";
                model.SendEmail = true;
            }

            model.Client = entity;
            return Task.CompletedTask;
        }

        private async Task<Boolean> ValidateCreateUserModel(Guid id, Client entity, ClientAccessCreateUserOperationModel model)
        {
            if (entity.UserId != null)
            {
                this.ModelState.AddModelError(String.Empty, "User already created");
                return false;
            }

            var validatedUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = null,
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

        private async Task ExecuteCreateUserOperation(Guid id, Client entity, ClientAccessCreateUserOperationModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = !String.IsNullOrEmpty(entity.Email) ? entity.Email : null,
                LockoutEnabled = true,
                MustChangePasswordAtNextLogin = true,
            };

            await this.userManager.CreateAsync(user, model.Password).ThrowOnErrorsAsync();
            await this.userManager.AddToRoleAsync(user, ApplicationRoles.Client).ThrowOnErrorsAsync();

            entity.UserId = user.Id;
            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();

            if (model.SendEmail)
            {
                var employee = await this.userEntityResolver.ResolveCurrentUserEntity() as Employee;
                var client = await this.Repository.QueryWithoutTracking<Client>("User").SingleOrDefaultAsync(x => x.Id == entity.Id);
                client.NotificationsOptions |= ClientNotificationsOptions.Enabled;
                var email = new WelcomeEmail
                {
                    Employee = employee,
                    RecipientName = null,
                    RecipientEmail = client.Email,
                    UserName = client.User.UserName,
                    Password = model.Password,
                    Attachments = new List<(UploadedFile FileInfo, Byte[] FileBytes)>(),
                };

                await this.clientEmailsService.SendClientEmail(client, email, ClientNotificationsType.ManualEmail);
            }
        }

        private Task<IActionResult> GetCreateUserSuccessResult(Guid id, Client entity, ClientAccessCreateUserOperationModel model)
        {
            return this.HybridFormResultAsync("ClientAccessCreateUser", this.RedirectToAction("Details", "Clients", new { id = entity.Id, Tab = "Access" }));
        }

        private Task InitializeManageUserModel(Guid id, Client entity, ClientAccessManageUserOperationModel model, Boolean initial)
        {
            if (initial)
            {
                model.UserName = entity.User.UserName;
            }

            model.Client = entity;
            return Task.CompletedTask;
        }

        private async Task<Boolean> ValidateManageUserModel(Guid id, Client entity, ClientAccessManageUserOperationModel model)
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

        private async Task ExecuteManageUserOperation(Guid id, Client entity, ClientAccessManageUserOperationModel model)
        {
            entity.User.UserName = model.UserName;
            entity.User.Email = !String.IsNullOrEmpty(entity.Email) ? entity.Email : null;
            await this.userManager.UpdateAsync(entity.User).ThrowOnErrorsAsync();

            if (model.ShouldUpdatePassword())
            {
                entity.User.PasswordHash = this.userManager.PasswordHasher.HashPassword(entity.User, model.Password);
                await this.userManager.UpdateSecurityStampAsync(entity.User).ThrowOnErrorsAsync();
            }
        }

        private Task<IActionResult> GetManageUserSuccessResult(Guid id, Client entity, ClientAccessManageUserOperationModel model)
        {
            return this.HybridFormResultAsync("ClientAccessManageUser", this.RedirectToAction("Details", "Clients", new { id = entity.Id, Tab = "Access" }));
        }

        private Task InitializeDeleteUserModel(Guid id, Client entity, ClientAccessDeleteUserOperationModel model, Boolean initial)
        {
            model.Client = entity;
            return Task.CompletedTask;
        }

        private async Task ExecuteDeleteUserOperation(Guid id, Client entity, ClientAccessDeleteUserOperationModel model)
        {
            var userId = entity.UserId;

            entity.UserId = null;
            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();

            var user = await this.Repository.Query<ApplicationUser>().SingleAsync(x => x.Id == userId);
            await this.userManager.DeleteAsync(user).ThrowOnErrorsAsync();
        }

        private Task<IActionResult> GetDeleteUserSuccessResult(Guid id, Client entity, ClientAccessDeleteUserOperationModel model)
        {
            return this.HybridFormResultAsync("ClientAccessDeleteUser", this.RedirectToAction("Details", "Clients", new { id = entity.Id, Tab = "Access" }));
        }

        private Task InitializeConfigurePageUrlModel(Guid id, Client entity, ClientAccessConfigurePageUrlOperationModel model, Boolean initial)
        {
            if (initial)
            {
                model.UrlPath = entity.UrlPath;
            }

            model.Client = entity;
            return Task.CompletedTask;
        }

        private Task<Boolean> ValidateConfigurePageUrlModel(Guid id, Client entity, ClientAccessConfigurePageUrlOperationModel model)
        {
            if (!String.IsNullOrEmpty(model.UrlPath))
            {
                var defaultRegex = new Regex(@"^Client(?<no>\d+)$");
                var defaultMatch = defaultRegex.Match(model.UrlPath);
                if (defaultMatch.Success)
                {
                    var no = Int32.Parse(defaultMatch.Groups["no"].Value);
                    if (no != entity.ClientNo)
                    {
                        this.ModelState.AddModelError(nameof(model.UrlPath), "ClientX path is reserved and is only valid when X is Client No");
                        return Task.FromResult(false);
                    }
                }
            }

            return Task.FromResult(true);
        }

        private async Task ExecuteConfigurePageUrlOperation(Guid id, Client entity, ClientAccessConfigurePageUrlOperationModel model)
        {
            entity.UrlPath = model.UrlPath;
            if (String.IsNullOrEmpty(entity.UrlPath))
            {
                entity.UrlPath = $"Client{entity.ClientNo}";
            }

            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetConfigurePageUrlSuccessResult(Guid id, Client entity, ClientAccessConfigurePageUrlOperationModel model)
        {
            return this.HybridFormResultAsync("ClientAccessConfigurePageUrl", this.RedirectToAction("Details", "Clients", new { id = entity.Id, Tab = "Access" }));
        }
    }
}