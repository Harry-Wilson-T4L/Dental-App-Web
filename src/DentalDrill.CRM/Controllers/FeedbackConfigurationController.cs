using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class FeedbackConfigurationController : Controller
    {
        private readonly IRepository repository;
        private readonly IEntityPermissionsValidator<FeedbackConfiguration> permissionsValidator;

        public FeedbackConfigurationController(
            IRepository repository,
            IPermissionsHub permissionsHub)
        {
            this.repository = repository;
            this.permissionsValidator = new DefaultEntityPermissionsValidator<FeedbackConfiguration>(
                permissionsHub,
                "/Domain/FeedbackConfiguration",
                "/Domain/FeedbackConfiguration/Entities/{entity}",
                null);
        }

        public async Task<IActionResult> Index()
        {
            var configuration = await this.repository.Query<FeedbackConfiguration>()
                .SingleOrDefaultAsync(x => x.Id == FeedbackConfiguration.Default);
            if (configuration == null)
            {
                return this.NotFound();
            }

            await this.permissionsValidator.DemandCanDetailsAsync(configuration);
            var model = new FeedbackConfigurationIndexModel
            {
                Entity = configuration,
            };

            return this.View(model);
        }

        public async Task<IActionResult> Edit()
        {
            var configuration = await this.repository.Query<FeedbackConfiguration>()
                .SingleOrDefaultAsync(x => x.Id == FeedbackConfiguration.Default);
            if (configuration == null)
            {
                return this.NotFound();
            }

            await this.permissionsValidator.DemandCanEditAsync(configuration);
            var model = new FeedbackConfigurationEditModel
            {
                Entity = configuration,
                AutoSendFormEnabled = configuration.AutoSendFormEnabled,
                AutoSendFormDelayDays = configuration.AutoSendFormDelayDays,
                AutoSendSkipJobs = configuration.AutoSendSkipJobs,
                PeriodLimitingEnabled = configuration.PeriodLimitingEnabled,
                PeriodLimitingType = configuration.PeriodLimitingType,
                PeriodLimitingLength = configuration.PeriodLimitingLength,
                PeriodLimitingQuantity = configuration.PeriodLimitingQuantity,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FeedbackConfigurationEditModel model)
        {
            var configuration = await this.repository.Query<FeedbackConfiguration>()
                .SingleOrDefaultAsync(x => x.Id == FeedbackConfiguration.Default);
            if (configuration == null)
            {
                return this.NotFound();
            }

            await this.permissionsValidator.DemandCanEditAsync(configuration);
            model.Entity = configuration;
            if (this.ModelState.IsValid)
            {
                configuration.AutoSendFormEnabled = model.AutoSendFormEnabled;
                configuration.AutoSendFormDelayDays = model.AutoSendFormDelayDays;
                configuration.AutoSendSkipJobs = model.AutoSendSkipJobs;
                configuration.PeriodLimitingEnabled = model.PeriodLimitingEnabled;
                configuration.PeriodLimitingType = model.PeriodLimitingType;
                configuration.PeriodLimitingLength = model.PeriodLimitingLength;
                configuration.PeriodLimitingQuantity = model.PeriodLimitingQuantity;

                await this.repository.UpdateAsync(configuration);
                await this.repository.SaveChangesAsync();

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }
    }
}
