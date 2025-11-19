using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.Identity.Manage;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Theming;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Mail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private const String AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const String RecoveryCodesKey = nameof(ManageController.RecoveryCodesKey);

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailService emailService;
        private readonly ILogger logger;
        private readonly UrlEncoder urlEncoder;
        private readonly IRepository repository;
        private readonly UserEntityResolver userResolver;
        private readonly IThemesService themesService;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailService emailService,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder,
          IRepository repository,
          UserEntityResolver userResolver,
          IThemesService themesService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.logger = logger;
            this.urlEncoder = urlEncoder;
            this.repository = repository;
            this.userResolver = userResolver;
            this.themesService = themesService;
        }

        [TempData]
        public String StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = this.StatusMessage,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await this.userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = this.Url.Action(
                action: "ConfirmEmail",
                controller: "Account",
                values: new { userId = user.Id, code = code },
                protocol: this.Request.Scheme);

            await this.emailService.SendAsync(new EmailConfirmationEmail
            {
                Recipient = user.Email,
                Link = callbackUrl,
            });

            this.StatusMessage = "Verification email sent. Please check your email.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = this.StatusMessage };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                changePasswordResult.AddErrorsToModelState(this.ModelState);
                return this.View(model);
            }

            if (user.MustChangePasswordAtNextLogin)
            {
                user.MustChangePasswordAtNextLogin = false;
                await this.userManager.UpdateAsync(user);
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);
            this.logger.LogInformation("User changed their password successfully.");
            this.StatusMessage = "Your password has been changed.";

            return this.RedirectToAction(nameof(this.ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return this.RedirectToAction(nameof(this.ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = this.StatusMessage };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var addPasswordResult = await this.userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                addPasswordResult.AddErrorsToModelState(this.ModelState);
                return this.View(model);
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);
            this.StatusMessage = "Your password has been set.";

            return this.RedirectToAction(nameof(this.SetPassword));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var model = new ExternalLoginsViewModel { CurrentLogins = await this.userManager.GetLoginsAsync(user) };
            model.OtherLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            model.ShowRemoveButton = await this.userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
            model.StatusMessage = this.StatusMessage;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(String provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = this.Url.Action(nameof(this.LinkLoginCallback));
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, this.userManager.GetUserId(this.User));
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync(user.Id.ToString());
            if (info == null)
            {
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await this.userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.StatusMessage = "The external login was added.";
            return this.RedirectToAction(nameof(this.ExternalLogins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var result = await this.userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);
            this.StatusMessage = "The external login was removed.";
            return this.RedirectToAction(nameof(this.ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await this.userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await this.userManager.CountRecoveryCodesAsync(user),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            return this.View(nameof(this.Disable2fa));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var disable2faResult = await this.userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            this.logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
            return this.RedirectToAction(nameof(this.TwoFactorAuthentication));
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var model = new EnableAuthenticatorViewModel();
            await this.LoadSharedKeyAndQrCodeUriAsync(user, model);

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return this.View(model);
            }

            // Strip spaces and hyphens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await this.userManager.VerifyTwoFactorTokenAsync(
                user, this.userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                this.ModelState.AddModelError("Code", "Verification code is invalid.");
                await this.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return this.View(model);
            }

            await this.userManager.SetTwoFactorEnabledAsync(user, true);
            this.logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            var recoveryCodes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            this.TempData[ManageController.RecoveryCodesKey] = recoveryCodes.ToArray();

            return this.RedirectToAction(nameof(this.ShowRecoveryCodes));
        }

        [HttpGet]
        public IActionResult ShowRecoveryCodes()
        {
            var recoveryCodes = (String[])this.TempData[ManageController.RecoveryCodesKey];
            if (recoveryCodes == null)
            {
                return this.RedirectToAction(nameof(this.TwoFactorAuthentication));
            }

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
            return this.View(model);
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            return this.View(nameof(this.ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.userManager.SetTwoFactorEnabledAsync(user, false);
            await this.userManager.ResetAuthenticatorKeyAsync(user);
            this.logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return this.RedirectToAction(nameof(this.EnableAuthenticator));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodesWarning()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
            }

            return this.View(nameof(this.GenerateRecoveryCodes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            this.logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            return this.View(nameof(this.ShowRecoveryCodes), model);
        }

        [Authorize(Roles = ApplicationRoles.Combined.Staff)]
        public async Task<IActionResult> Appearance()
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                return this.NotFound();
            }

            var model = new AppearanceViewModel
            {
                AvailableThemes = this.themesService.GetAvailableThemes(),
                Theme = employee.AppearanceTheme,
                Background = employee.AppearanceBackgroundId,
                Opacity = employee.AppearanceOpacity ?? 1,
                StatusMessage = this.StatusMessage,
            };

            return this.View(model);
        }

        [Authorize(Roles = ApplicationRoles.Combined.Staff)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Appearance(AppearanceViewModel model)
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var updatedEmployee = await this.repository.Query<Employee>().SingleAsync(x => x.Id == employee.Id);
                updatedEmployee.AppearanceTheme = model.Theme;
                updatedEmployee.AppearanceBackgroundId = model.Background;
                updatedEmployee.AppearanceOpacity = model.Opacity;

                await this.repository.UpdateAsync(updatedEmployee);
                await this.repository.SaveChangesAsync();

                this.StatusMessage = "Appearance changed";
                return this.RedirectToAction("Appearance");
            }

            model.StatusMessage = this.StatusMessage;
            model.AvailableThemes = this.themesService.GetAvailableThemes();
            return this.View(model);
        }

        [Authorize(Roles = ApplicationRoles.Client)]
        public async Task<IActionResult> Notifications()
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Client client))
            {
                return this.NotFound();
            }

            var model = new NotificationsViewModel
            {
                Client = client,
                AllowManualEmails = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledManualEmails),
                AllowHandpieceNotifications = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledHandpieceNotifications),
                AllowFeedbackRequests = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledFeedbackRequests),
                AllowMaintenanceReminders = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledMaintenanceReminders),
            };

            return this.View(model);
        }

        [Authorize(Roles = ApplicationRoles.Client)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Notifications(NotificationsViewModel model)
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Client client))
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var editableClient = await this.repository.Query<Client>().SingleAsync(x => x.Id == client.Id);

                editableClient.NotificationsOptions = (editableClient.NotificationsOptions | ClientNotificationsOptions.DisabledManualEmails)
                                                      ^ (model.AllowManualEmails ? ClientNotificationsOptions.DisabledManualEmails : ClientNotificationsOptions.None);

                editableClient.NotificationsOptions = (editableClient.NotificationsOptions | ClientNotificationsOptions.DisabledHandpieceNotifications)
                                                      ^ (model.AllowHandpieceNotifications ? ClientNotificationsOptions.DisabledHandpieceNotifications : ClientNotificationsOptions.None);

                editableClient.NotificationsOptions = (editableClient.NotificationsOptions | ClientNotificationsOptions.DisabledFeedbackRequests)
                                                      ^ (model.AllowFeedbackRequests ? ClientNotificationsOptions.DisabledFeedbackRequests : ClientNotificationsOptions.None);

                editableClient.NotificationsOptions = (editableClient.NotificationsOptions | ClientNotificationsOptions.DisabledMaintenanceReminders)
                                                      ^ (model.AllowMaintenanceReminders ? ClientNotificationsOptions.DisabledMaintenanceReminders : ClientNotificationsOptions.None);

                await this.repository.UpdateAsync(editableClient);
                await this.repository.SaveChangesAsync();

                return this.RedirectToAction("NotificationsUpdated");
            }

            model.Client = client;
            return this.View(model);
        }

        [Authorize(Roles = ApplicationRoles.Client)]
        public async Task<IActionResult> NotificationsUpdated()
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Client client))
            {
                return this.NotFound();
            }

            return this.View();
        }

        #region Helpers

        private String FormatKey(String unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private String GenerateQrCodeUri(String email, String unformattedKey)
        {
            return string.Format(
                ManageController.AuthenticatorUriFormat,
                this.urlEncoder.Encode("DentalDrill.CRM"),
                this.urlEncoder.Encode(email),
                unformattedKey);
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
        {
            var unformattedKey = await this.userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await this.userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await this.userManager.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = this.FormatKey(unformattedKey);
            model.AuthenticatorUri = this.GenerateQrCodeUri(user.Email, unformattedKey);
        }

        #endregion
    }
}
