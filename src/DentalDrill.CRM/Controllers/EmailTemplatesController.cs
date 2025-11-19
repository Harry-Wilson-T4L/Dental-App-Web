using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.ClientEmails;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class EmailTemplatesController : Controller
    {
        private readonly IRepository repository;
        private readonly IEntityPermissionsValidator<EmailTemplate> permissionsValidator;

        public EmailTemplatesController(
            IRepository repository,
            IPermissionsHub permissionsHub)
        {
            this.repository = repository;
            this.permissionsValidator = new DefaultEntityPermissionsValidator<EmailTemplate>(
                permissionsHub,
                "/Domain/EmailTemplate",
                "/Domain/EmailTemplate/Entities/{entity}",
                null);
        }

        public async Task<IActionResult> Index()
        {
            await this.permissionsValidator.DemandCanIndexAsync();
            return this.View();
        }

        public async Task<IActionResult> NewClient()
        {
            var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewClient);
            await this.permissionsValidator.DemandCanDetailsAsync(stored);
            var model = stored?.DeserializeContent<ClientEmailSendNewClientTemplateModel>() ?? EmailTemplates.Defaults.NewClient;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewClient(ClientEmailSendNewClientTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewClient);
                if (stored == null)
                {
                    await this.permissionsValidator.DemandCanCreateAsync();
                    stored = new EmailTemplate { Id = EmailTemplates.NewClient };
                    stored.SerializeContent(model);

                    await this.repository.InsertAsync(stored);
                    await this.repository.SaveChangesAsync();
                }
                else
                {
                    await this.permissionsValidator.DemandCanEditAsync(stored);
                    stored.SerializeContent(model);

                    await this.repository.UpdateAsync(stored);
                    await this.repository.SaveChangesAsync();
                }

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        public async Task<IActionResult> NewQueenslandClient()
        {
            var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewQueenslandClient);
            await this.permissionsValidator.DemandCanDetailsAsync(stored);
            var model = stored?.DeserializeContent<ClientEmailSendNewQueenslandClientTemplateModel>() ?? EmailTemplates.Defaults.NewQueenslandClient;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewQueenslandClient(ClientEmailSendNewQueenslandClientTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewQueenslandClient);
                if (stored == null)
                {
                    await this.permissionsValidator.DemandCanCreateAsync();
                    stored = new EmailTemplate { Id = EmailTemplates.NewQueenslandClient };
                    stored.SerializeContent(model);

                    await this.repository.InsertAsync(stored);
                    await this.repository.SaveChangesAsync();
                }
                else
                {
                    await this.permissionsValidator.DemandCanEditAsync(stored);
                    stored.SerializeContent(model);

                    await this.repository.UpdateAsync(stored);
                    await this.repository.SaveChangesAsync();
                }

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        public async Task<IActionResult> NewInterstateClient()
        {
            var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewInterstateClient);
            await this.permissionsValidator.DemandCanDetailsAsync(stored);
            var model = stored?.DeserializeContent<ClientEmailSendNewInterstateClientTemplateModel>() ?? EmailTemplates.Defaults.NewInterstateClient;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewInterstateClient(ClientEmailSendNewInterstateClientTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewInterstateClient);
                if (stored == null)
                {
                    await this.permissionsValidator.DemandCanCreateAsync();
                    stored = new EmailTemplate { Id = EmailTemplates.NewInterstateClient };
                    stored.SerializeContent(model);

                    await this.repository.InsertAsync(stored);
                    await this.repository.SaveChangesAsync();
                }
                else
                {
                    await this.permissionsValidator.DemandCanEditAsync(stored);
                    stored.SerializeContent(model);

                    await this.repository.UpdateAsync(stored);
                    await this.repository.SaveChangesAsync();
                }

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        public async Task<IActionResult> MaintenanceInstructions()
        {
            var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.MaintenanceInstructions);
            await this.permissionsValidator.DemandCanDetailsAsync(stored);
            var model = stored?.DeserializeContent<ClientEmailSendMaintenanceInstructionsTemplateModel>() ?? EmailTemplates.Defaults.MaintenanceInstructions;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MaintenanceInstructions(ClientEmailSendMaintenanceInstructionsTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.MaintenanceInstructions);
                if (stored == null)
                {
                    await this.permissionsValidator.DemandCanCreateAsync();
                    stored = new EmailTemplate { Id = EmailTemplates.MaintenanceInstructions };
                    stored.SerializeContent(model);

                    await this.repository.InsertAsync(stored);
                    await this.repository.SaveChangesAsync();
                }
                else
                {
                    await this.permissionsValidator.DemandCanEditAsync(stored);
                    stored.SerializeContent(model);

                    await this.repository.UpdateAsync(stored);
                    await this.repository.SaveChangesAsync();
                }

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        public async Task<IActionResult> ScalerInformation()
        {
            var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.ScalerInformation);
            await this.permissionsValidator.DemandCanDetailsAsync(stored);
            var model = stored?.DeserializeContent<ClientEmailSendScalerInformationTemplateModel>() ?? EmailTemplates.Defaults.ScalerInformation;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScalerInformation(ClientEmailSendScalerInformationTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.ScalerInformation);
                if (stored == null)
                {
                    await this.permissionsValidator.DemandCanCreateAsync();
                    stored = new EmailTemplate { Id = EmailTemplates.ScalerInformation };
                    stored.SerializeContent(model);

                    await this.repository.InsertAsync(stored);
                    await this.repository.SaveChangesAsync();
                }
                else
                {
                    await this.permissionsValidator.DemandCanEditAsync(stored);
                    stored.SerializeContent(model);

                    await this.repository.UpdateAsync(stored);
                    await this.repository.SaveChangesAsync();
                }

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        public async Task<IActionResult> ChairTechnicianDetails()
        {
            var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.ChairTechnicianDetails);
            await this.permissionsValidator.DemandCanDetailsAsync(stored);
            var model = stored?.DeserializeContent<ClientEmailSendChairTechnicianDetailsTemplateModel>() ?? EmailTemplates.Defaults.ChairTechnicianDetails;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChairTechnicianDetails(ClientEmailSendChairTechnicianDetailsTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var stored = await this.repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.ChairTechnicianDetails);
                if (stored == null)
                {
                    await this.permissionsValidator.DemandCanCreateAsync();
                    stored = new EmailTemplate { Id = EmailTemplates.ChairTechnicianDetails };
                    stored.SerializeContent(model);

                    await this.repository.InsertAsync(stored);
                    await this.repository.SaveChangesAsync();
                }
                else
                {
                    await this.permissionsValidator.DemandCanEditAsync(stored);
                    stored.SerializeContent(model);

                    await this.repository.UpdateAsync(stored);
                    await this.repository.SaveChangesAsync();
                }

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }
    }
}
