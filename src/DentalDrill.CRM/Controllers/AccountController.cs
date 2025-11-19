using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.Identity.Account;
using DentalDrill.CRM.Models.ViewModels.Identity.Manage;
using DentalDrill.CRM.Services;
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
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailService emailService;
        private readonly ILogger logger;
        private readonly ClientEmailsService clientEmailsService;
        private readonly IRepository repository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ILogger<AccountController> logger,
            ClientEmailsService clientEmailsService,
            IRepository repository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.logger = logger;
            this.clientEmailsService = clientEmailsService;
            this.repository = repository;
        }

        [TempData]
        public String ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(String returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, String returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User logged in.");
                    return this.LocalRedirectOrDefault(returnUrl);
                }
                else if (result.RequiresTwoFactor)
                {
                    return this.RedirectToAction(nameof(this.LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                else if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return this.RedirectToAction(nameof(this.Lockout));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return this.View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(Boolean rememberMe, String returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            this.ViewData["ReturnUrl"] = returnUrl;

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, Boolean rememberMe, String returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await this.signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                this.logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return this.LocalRedirectOrDefault(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                this.logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return this.RedirectToAction(nameof(this.Lockout));
            }
            else
            {
                this.logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                this.ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return this.View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(String returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, String returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await this.signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                this.logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return this.LocalRedirectOrDefault(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                this.logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return this.RedirectToAction(nameof(this.Lockout));
            }
            else
            {
                this.logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                this.ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return this.View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(String provider, String returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = this.Url.Action(nameof(this.ExternalLoginCallback), "Account", new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return this.Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(String returnUrl = null, String remoteError = null)
        {
            if (remoteError != null)
            {
                this.ErrorMessage = $"Error from external provider: {remoteError}";
                return this.RedirectToAction(nameof(this.Login));
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return this.RedirectToAction(nameof(this.Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                this.logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return this.LocalRedirectOrDefault(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                return this.RedirectToAction(nameof(this.Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                this.ViewData["ReturnUrl"] = returnUrl;
                this.ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return this.View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, String returnUrl = null)
        {
            if (this.ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await this.signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await this.userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await this.userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        this.logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return this.LocalRedirectOrDefault(returnUrl);
                    }
                }

                result.AddErrorsToModelState(this.ModelState);
            }

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View(nameof(this.ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(String userId, String code)
        {
            if (userId == null || code == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await this.userManager.ConfirmEmailAsync(user, code);
            return this.View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var normalizedEmail = this.userManager.NormalizeEmail(model.Email);
                var matchingUsers = await this.repository.Query<ApplicationUser>().Where(x => x.NormalizedEmail == normalizedEmail).ToListAsync();

                if (matchingUsers.Count == 0)
                {
                    this.logger.LogWarning("Forgot password requested for user that does not exist: {email}", model.Email);
                    return this.RedirectToAction(nameof(this.ForgotPasswordConfirmation));
                }
                else if (matchingUsers.Count == 1)
                {
                    var user = matchingUsers[0];
                    this.logger.LogWarning("Forgot password requested for user with email {email}", model.Email);
                    var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = this.Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, this.Request.Scheme);

                    try
                    {
                        await this.emailService.SendAsync(new ForgotPasswordEmail
                        {
                            Recipient = model.Email,
                            Link = callbackUrl,
                        });
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e, "Failed to send forgot password email to user(s) with email {email}", model.Email);
                    }
                }
                else
                {
                    this.logger.LogWarning("Forgot password requested for multiple users with email {email}: {userNames}", model.Email, String.Join(",", matchingUsers.Select(x => x.UserName)));

                    var email = new ForgotPasswordAmbiguousEmail
                    {
                        Recipient = model.Email,
                    };

                    foreach (var user in matchingUsers)
                    {
                        var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = this.Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, this.Request.Scheme);
                        email.Links.Add((user.UserName, callbackUrl));
                    }

                    try
                    {
                        await this.emailService.SendAsync(email);
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e, "Failed to send forgot password email to user(s) with email {email}", model.Email);
                    }
                }

                return this.RedirectToAction(nameof(this.ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(Guid? userId, String code = null)
        {
            if (userId == null)
            {
                return this.NotFound();
            }

            if (code == null)
            {
                return this.NotFound();
            }

            var model = new ResetPasswordViewModel { UserId = userId.Value, Code = code };
            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(Guid userId, ResetPasswordViewModel model)
        {
            model.UserId = userId;
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return this.RedirectToAction(nameof(this.ResetPasswordConfirmation));
            }

            var result = await this.userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return this.RedirectToAction(nameof(this.ResetPasswordConfirmation));
            }

            result.AddErrorsToModelState(this.ModelState);
            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }

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
                throw new ApplicationException($"Unable to load password of user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var model = new ChangePasswordViewModel();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, Boolean? logOut)
        {
            if (logOut == true)
            {
                await this.signInManager.SignOutAsync();
                this.logger.LogInformation("User logged out.");
                return this.RedirectToAction(nameof(HomeController.Index), "Home");
            }

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

            return this.RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Notifications(String token)
        {
            var tokenResult = await this.clientEmailsService.VerifyManageToken(token);
            if (!tokenResult.Success)
            {
                return this.RedirectToAction("NotificationsInvalidToken");
            }

            var client = tokenResult.Client;
            var model = new AccountNotificationsViewModel
            {
                Client = client,
                Token = token,
                AllowManualEmails = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledManualEmails),
                AllowHandpieceNotifications = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledHandpieceNotifications),
                AllowFeedbackRequests = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledFeedbackRequests),
                AllowMaintenanceReminders = !client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledMaintenanceReminders),
            };

            return this.View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Notifications(AccountNotificationsViewModel model)
        {
            var tokenResult = await this.clientEmailsService.VerifyManageToken(model.Token);
            if (!tokenResult.Success)
            {
                return this.RedirectToAction("NotificationsInvalidToken");
            }

            var client = tokenResult.Client;
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

        [AllowAnonymous]
        public Task<IActionResult> NotificationsInvalidToken()
        {
            return Task.FromResult<IActionResult>(this.View());
        }

        [AllowAnonymous]
        public Task<IActionResult> NotificationsUpdated()
        {
            return Task.FromResult<IActionResult>(this.View());
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            this.HttpContext.Response.StatusCode = 403;
            return this.View();
        }
    }
}
