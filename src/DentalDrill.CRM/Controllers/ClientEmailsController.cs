using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.ClientEmails;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Mail.Models;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientEmailsController : BaseTelerikIndexlessDependentBasicCrudController<Int32, ClientEmailMessage, Guid, Client, ClientEmailReadModel, ClientEmailMessage, Object, Object, Object>
    {
        public ClientEmailsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.ReadHandler.Overrides.PreprocessRequest = this.PreprocessRequest;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;

            this.DetailsTabSourceHandler = new BasicCrudDetailsActionHandler<Int32, ClientEmailMessage, ClientEmailMessage>(this, this.ControllerServices, this.GetEntityPermissionsValidator());
            this.DetailsTabSourceHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;

            this.SendCustomHandler = new SendCustomEmailActionHandler(this, this.ControllerServices);
            this.SendNewClientHandler = new SendNewClientEmailActionHandler(this, this.ControllerServices);
            this.SendNewQueenslandClientHandler = new SendNewQueenslandClientEmailActionHandler(this, this.ControllerServices);
            this.SendNewInterstateClientHandler = new SendNewInterstateClientEmailActionHandler(this, this.ControllerServices);
            this.SendMaintenanceInstructionsHandler = new SendMaintenanceInstructionsEmailActionHandler(this, this.ControllerServices);
            this.SendScalerInformationHandler = new SendScalerInformationEmailActionHandler(this, this.ControllerServices);
            this.SendChairTechnicianDetailsHandler = new SendChairTechnicianDetailsEmailActionHandler(this, this.ControllerServices);
            this.SendWelcomeHandler = new SendWelcomeEmailActionHandler(this, this.ControllerServices);
            this.SendHandpieceTradersHandler = new SendHandpieceTradersEmailActionHandler(this, this.ControllerServices);
            this.SendMaintenanceRequiredHandler = new SendMaintenanceRequiredEmailActionHandler(this, this.ControllerServices);
        }

        protected BasicCrudDetailsActionHandler<Int32, ClientEmailMessage, ClientEmailMessage> DetailsTabSourceHandler { get; }

        protected SendCustomEmailActionHandler SendCustomHandler { get; }

        protected SendNewClientEmailActionHandler SendNewClientHandler { get; }

        protected SendNewQueenslandClientEmailActionHandler SendNewQueenslandClientHandler { get; }

        protected SendNewInterstateClientEmailActionHandler SendNewInterstateClientHandler { get; }

        protected SendMaintenanceInstructionsEmailActionHandler SendMaintenanceInstructionsHandler { get; }

        protected SendScalerInformationEmailActionHandler SendScalerInformationHandler { get; }

        protected SendChairTechnicianDetailsEmailActionHandler SendChairTechnicianDetailsHandler { get; }

        protected SendWelcomeEmailActionHandler SendWelcomeHandler { get; }

        protected SendHandpieceTradersEmailActionHandler SendHandpieceTradersHandler { get; }

        protected SendMaintenanceRequiredEmailActionHandler SendMaintenanceRequiredHandler { get; }

        public Task<IActionResult> DetailsTabSource(Int32 id) => this.DetailsTabSourceHandler.Details(id);

        public Task<IActionResult> SendCustom(Guid id) => this.SendCustomHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendCustom(Guid id, ClientEmailSendCustomModel model) => this.SendCustomHandler.Send(id, model);

        public Task<IActionResult> SendNewClient(Guid id) => this.SendNewClientHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendNewClient(Guid id, ClientEmailSendNewClientModel model) => this.SendNewClientHandler.Send(id, model);

        public Task<IActionResult> SendNewQueenslandClient(Guid id) => this.SendNewQueenslandClientHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendNewQueenslandClient(Guid id, ClientEmailSendNewQueenslandClientModel model) => this.SendNewQueenslandClientHandler.Send(id, model);

        public Task<IActionResult> SendNewInterstateClient(Guid id) => this.SendNewInterstateClientHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendNewInterstateClient(Guid id, ClientEmailSendNewInterstateClientModel model) => this.SendNewInterstateClientHandler.Send(id, model);

        public Task<IActionResult> SendMaintenanceInstructions(Guid id) => this.SendMaintenanceInstructionsHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendMaintenanceInstructions(Guid id, ClientEmailSendMaintenanceInstructionsModel model) => this.SendMaintenanceInstructionsHandler.Send(id, model);

        public Task<IActionResult> SendScalerInformation(Guid id) => this.SendScalerInformationHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendScalerInformation(Guid id, ClientEmailSendScalerInformationModel model) => this.SendScalerInformationHandler.Send(id, model);

        public Task<IActionResult> SendChairTechnicianDetails(Guid id) => this.SendChairTechnicianDetailsHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendChairTechnicianDetails(Guid id, ClientEmailSendChairTechnicianDetailsModel model) => this.SendChairTechnicianDetailsHandler.Send(id, model);

        public Task<IActionResult> SendWelcome(Guid id) => this.SendWelcomeHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendWelcome(Guid id, ClientEmailSendWelcomeModel model) => this.SendWelcomeHandler.Send(id, model);

        public Task<IActionResult> SendHandpieceTraders(Guid id) => this.SendHandpieceTradersHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendHandpieceTraders(Guid id, ClientEmailSendHandpieceTradersModel model) => this.SendHandpieceTradersHandler.Send(id, model);

        public Task<IActionResult> SendMaintenanceRequired(Guid id) => this.SendMaintenanceRequiredHandler.Send(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendMaintenanceRequired(Guid id, ClientEmailSendMaintenanceRequiredModel model) => this.SendMaintenanceRequiredHandler.Send(id, model);

        private Task PreprocessRequest(DataSourceRequest request)
        {
            request.RenameFields(new Dictionary<String, String>
            {
                ["Created"] = "EmailMessage.Created",
                ["To"] = "EmailMessage.To",
                ["Subject"] = "EmailMessage.Subject",
                ["Status"] = "EmailMessage.Status",
            });

            return Task.CompletedTask;
        }

        private Task<IQueryable<ClientEmailMessage>> PrepareReadQuery(Client parent)
        {
            var query = this.ControllerServices.Repository.QueryWithoutTracking<ClientEmailMessage>("EmailMessage")
                .Where(x => x.ClientId == parent.Id);

            return Task.FromResult(query);
        }

        private ClientEmailReadModel ConvertEntityToViewModel(Client parent, ClientEmailMessage entity, String[] allowedProperties)
        {
            return new ClientEmailReadModel
            {
                Id = entity.EmailMessage.Id,
                Created = entity.EmailMessage.Created,
                To = entity.EmailMessage.To,
                Subject = entity.EmailMessage.Subject,
                Status = entity.EmailMessage.Status,
            };
        }

        private Task<ClientEmailMessage> QuerySingleEntity(Int32 id)
        {
            return this.ControllerServices.Repository.Query<ClientEmailMessage>("EmailMessage").SingleOrDefaultAsync(x => x.EmailMessageId == id);
        }

        protected class SendCustomEmailActionHandler : BaseSendClientEmailActionHandler<CustomEmail, ClientEmailSendCustomModel>
        {
            public SendCustomEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendCustomModel model, Boolean initial)
            {
                model.Client = client;
                return Task.CompletedTask;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendCustomModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<CustomEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendCustomModel model)
            {
                var email = new CustomEmail
                {
                    Employee = employee,
                    Client = client,
                    To = model.To,
                    Subject = model.Subject,
                    Text = model.Text,
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId() => "ClientEmailsSendCustom";

            protected override Int32 GetCreatedEmailSize(CustomEmail email)
            {
                return email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendNewClientEmailActionHandler : BaseSendClientEmailActionHandler<NewClientEmail, ClientEmailSendNewClientModel>
        {
            public SendNewClientEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override async Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendNewClientModel model, Boolean initial)
            {
                if (initial)
                {
                    var stored = await this.Repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewClient);
                    var template = stored?.DeserializeContent<ClientEmailSendNewClientTemplateModel>() ?? EmailTemplates.Defaults.NewClient;

                    model.Greeting = client.PrincipalDentist;
                    model.NewClientOfferText = template.NewClientOfferText;
                    model.MonthlySpecialImage = template.MonthlySpecialImage;
                    model.PriceGuideImage = template.PriceGuideImage;
                    model.Attachments = template.Attachments?.ToList();
                }

                model.Client = client;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendNewClientModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<NewClientEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendNewClientModel model)
            {
                var email = new NewClientEmail
                {
                    Employee = employee,
                    Client = client,
                    To = model.To,
                    Greeting = model.Greeting,
                    DiscountComment = model.DiscountComment,
                    NewClientOfferText = model.NewClientOfferText,
                    MonthlySpecialImage = await this.LoadImageAsync(model.MonthlySpecialImage),
                    PriceGuideImage = await this.LoadImageAsync(model.PriceGuideImage),
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId()
            {
                return "ClientEmailsSendNewClient";
            }

            protected override Int32 GetCreatedEmailSize(NewClientEmail email)
            {
                return email.MonthlySpecialImage.ImageBytes?.Length ?? 0
                    + email.PriceGuideImage.ImageBytes?.Length ?? 0
                    + email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendNewQueenslandClientEmailActionHandler : BaseSendClientEmailActionHandler<NewQueenslandClientEmail, ClientEmailSendNewQueenslandClientModel>
        {
            public SendNewQueenslandClientEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override async Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendNewQueenslandClientModel model, Boolean initial)
            {
                if (initial)
                {
                    var stored = await this.Repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewQueenslandClient);
                    var template = stored?.DeserializeContent<ClientEmailSendNewQueenslandClientTemplateModel>() ?? EmailTemplates.Defaults.NewQueenslandClient;

                    model.Greeting = client.PrincipalDentist;
                    model.LocationImage = template.LocationImage;
                    model.LocationText = template.LocationText;
                    model.NewClientOfferText = template.NewClientOfferText;
                    model.MonthlySpecialImage = template.MonthlySpecialImage;
                    model.PriceGuideImage = template.PriceGuideImage;
                    model.Attachments = template.Attachments?.ToList();
                }

                model.Client = client;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendNewQueenslandClientModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<NewQueenslandClientEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendNewQueenslandClientModel model)
            {
                var email = new NewQueenslandClientEmail
                {
                    Employee = employee,
                    Client = client,
                    To = model.To,
                    Greeting = model.Greeting,
                    DiscountComment = model.DiscountComment,
                    LocationImage = await this.LoadImageAsync(model.LocationImage),
                    LocationText = model.LocationText,
                    NewClientOfferText = model.NewClientOfferText,
                    MonthlySpecialImage = await this.LoadImageAsync(model.MonthlySpecialImage),
                    PriceGuideImage = await this.LoadImageAsync(model.PriceGuideImage),
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId()
            {
                return "ClientEmailsSendNewQueenslandClient";
            }

            protected override Int32 GetCreatedEmailSize(NewQueenslandClientEmail email)
            {
                return email.LocationImage.ImageBytes?.Length ?? 0
                    + email.MonthlySpecialImage.ImageBytes?.Length ?? 0
                    + email.PriceGuideImage.ImageBytes?.Length ?? 0
                    + email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendNewInterstateClientEmailActionHandler : BaseSendClientEmailActionHandler<NewInterstateClientEmail, ClientEmailSendNewInterstateClientModel>
        {
            public SendNewInterstateClientEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override async Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendNewInterstateClientModel model, Boolean initial)
            {
                if (initial)
                {
                    var stored = await this.Repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.NewInterstateClient);
                    var template = stored?.DeserializeContent<ClientEmailSendNewInterstateClientTemplateModel>() ?? EmailTemplates.Defaults.NewInterstateClient;

                    model.Greeting = client.PrincipalDentist;
                    model.MonthlySpecialImage = template.MonthlySpecialImage;
                    model.PriceGuideImage = template.PriceGuideImage;
                    model.Attachments = template.Attachments?.ToList();
                }

                model.Client = client;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendNewInterstateClientModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<NewInterstateClientEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendNewInterstateClientModel model)
            {
                var email = new NewInterstateClientEmail
                {
                    Employee = employee,
                    Client = client,
                    To = model.To,
                    Greeting = model.Greeting,
                    DiscountComment = model.DiscountComment,
                    MonthlySpecialImage = await this.LoadImageAsync(model.MonthlySpecialImage),
                    PriceGuideImage = await this.LoadImageAsync(model.PriceGuideImage),
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId() => "ClientEmailsSendNewInterstateClient";

            protected override Int32 GetCreatedEmailSize(NewInterstateClientEmail email)
            {
                return email.MonthlySpecialImage.ImageBytes?.Length ?? 0
                    + email.PriceGuideImage.ImageBytes?.Length ?? 0
                    + email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendMaintenanceInstructionsEmailActionHandler : BaseSendClientEmailActionHandler<MaintenanceInstructionsEmail, ClientEmailSendMaintenanceInstructionsModel>
        {
            public SendMaintenanceInstructionsEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override async Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendMaintenanceInstructionsModel model, Boolean initial)
            {
                if (initial)
                {
                    var stored = await this.Repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.MaintenanceInstructions);
                    var template = stored?.DeserializeContent<ClientEmailSendMaintenanceInstructionsTemplateModel>() ?? EmailTemplates.Defaults.MaintenanceInstructions;

                    model.Greeting = client.PrincipalDentist;
                    model.PriceGuideImage = template.PriceGuideImage;
                    model.Attachments = template.Attachments?.ToList();
                }

                model.Client = client;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendMaintenanceInstructionsModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<MaintenanceInstructionsEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendMaintenanceInstructionsModel model)
            {
                var email = new MaintenanceInstructionsEmail
                {
                    Employee = employee,
                    Client = client,
                    To = model.To,
                    Greeting = model.Greeting,
                    PriceGuideImage = await this.LoadImageAsync(model.PriceGuideImage),
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId() => "ClientEmailsSendMaintenanceInstructions";

            protected override Int32 GetCreatedEmailSize(MaintenanceInstructionsEmail email)
            {
                return email.PriceGuideImage.ImageBytes?.Length ?? 0
                    + email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendScalerInformationEmailActionHandler : BaseSendClientEmailActionHandler<ScalerInformationEmail, ClientEmailSendScalerInformationModel>
        {
            public SendScalerInformationEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override async Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendScalerInformationModel model, Boolean initial)
            {
                if (initial)
                {
                    var stored = await this.Repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.ScalerInformation);
                    var template = stored?.DeserializeContent<ClientEmailSendScalerInformationTemplateModel>() ?? EmailTemplates.Defaults.ScalerInformation;

                    model.Greeting = client.PrincipalDentist;
                    model.ScalersPricesImage = template.ScalersPricesImage;
                    model.Attachments = template.Attachments?.ToList();
                }

                model.Client = client;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendScalerInformationModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<ScalerInformationEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendScalerInformationModel model)
            {
                var email = new ScalerInformationEmail
                {
                    Employee = employee,
                    Client = client,
                    To = model.To,
                    Greeting = model.Greeting,
                    ScalersPricesImage = await this.LoadImageAsync(model.ScalersPricesImage),
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId() => "ClientEmailsSendScalerInformation";

            protected override Int32 GetCreatedEmailSize(ScalerInformationEmail email)
            {
                return email.ScalersPricesImage.ImageBytes?.Length ?? 0
                    + email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendChairTechnicianDetailsEmailActionHandler : BaseSendClientEmailActionHandler<ChairTechnicianDetailsEmail, ClientEmailSendChairTechnicianDetailsModel>
        {
            public SendChairTechnicianDetailsEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override async Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendChairTechnicianDetailsModel model, Boolean initial)
            {
                if (initial)
                {
                    var stored = await this.Repository.Query<EmailTemplate>().SingleOrDefaultAsync(x => x.Id == EmailTemplates.ChairTechnicianDetails);
                    var template = stored?.DeserializeContent<ClientEmailSendChairTechnicianDetailsTemplateModel>() ?? EmailTemplates.Defaults.ChairTechnicianDetails;

                    model.Greeting = client.PrincipalDentist;
                    model.Following = template.Following;
                    model.Attachments = template.Attachments?.ToList();
                }

                model.Client = client;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendChairTechnicianDetailsModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<ChairTechnicianDetailsEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendChairTechnicianDetailsModel model)
            {
                var email = new ChairTechnicianDetailsEmail
                {
                    Employee = employee,
                    Client = client,
                    To = model.To,
                    Greeting = model.Greeting,
                    Following = model.Following,
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId()
            {
                return "ClientEmailsSendChairTechnicianDetails";
            }

            protected override Int32 GetCreatedEmailSize(ChairTechnicianDetailsEmail email)
            {
                return email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendWelcomeEmailActionHandler : BaseSendClientEmailActionHandler<WelcomeEmail, ClientEmailSendWelcomeModel>
        {
            public SendWelcomeEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendWelcomeModel model, Boolean initial)
            {
                if (initial)
                {
                    model.Password = "Hub2153";
                    model.Attachments = new List<Guid>();
                }

                model.Client = client;
                model.Username = clientUser.UserName;
                return Task.CompletedTask;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendWelcomeModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<WelcomeEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendWelcomeModel model)
            {
                var email = new WelcomeEmail
                {
                    Employee = employee,
                    RecipientEmail = model.To,
                    RecipientName = null,
                    UserName = clientUser.UserName,
                    Password = model.Password,
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId() => "ClientEmailsSendWelcome";

            protected override Int32 GetCreatedEmailSize(WelcomeEmail email)
            {
                return email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendHandpieceTradersEmailActionHandler : BaseSendClientEmailActionHandler<HandpieceTradersEmail, ClientEmailSendHandpieceTradersModel>
        {
            public SendHandpieceTradersEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
            }

            protected override Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendHandpieceTradersModel model, Boolean initial)
            {
                model.Client = client;
                if (initial)
                {
                    model.Greeting = client.PrincipalDentist;
                }

                return Task.CompletedTask;
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendHandpieceTradersModel model)
            {
                return this.ValidateGenericEmailSendModel(client, model.To);
            }

            protected override async Task<HandpieceTradersEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendHandpieceTradersModel model)
            {
                var email = new HandpieceTradersEmail
                {
                    Client = client,
                    To = model.To,
                    Greeting = model.Greeting,
                    Attachments = await this.LoadAttachmentsAsync(model.Attachments),
                };

                return email;
            }

            protected override String GetHybridFormId() => "ClientEmailsSendHandpieceTraders";

            protected override Int32 GetCreatedEmailSize(HandpieceTradersEmail email)
            {
                return email.Attachments?.Sum(x => x.FileBytes?.Length ?? 0) ?? 0;
            }
        }

        protected class SendMaintenanceRequiredEmailActionHandler : BaseSendClientEmailActionHandler<MaintenanceRequiredEmail, ClientEmailSendMaintenanceRequiredModel>
        {
            private readonly IClientManager clientManager;

            public SendMaintenanceRequiredEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller, controllerServices)
            {
                this.clientManager = controllerServices.ServiceProvider.GetRequiredService<IClientManager>();
            }

            protected override ClientNotificationsType NotificationType => ClientNotificationsType.MaintenanceReminder;

            protected override async Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, ClientEmailSendMaintenanceRequiredModel model, Boolean initial)
            {
                model.Client = client;

                var domainClient = await this.clientManager.GetByIdAsync(client.Id);
                var items = await domainClient.RepairedHistory.GetByStatusAsync(ClientRepairedItemStatus.RequiresMaintenance);

                model.Items = items.Select(x => new ClientRepairedItemViewModel
                {
                    Id = x.Id,
                    ClientId = client.Id,
                    ClientName = client.FullName,
                    ClientEmail = client.Email,
                    Brand = x.Brand,
                    Model = x.Model,
                    Serial = x.Serial,
                    LastRepair = x.LastRepair.Number,
                    LastRepairUrl = String.Empty,
                    LastRepairDate = x.LastRepair.CompletedOn ?? x.LastRepair.RepairedOn ?? x.LastRepair.ApprovedOn ?? x.LastRepair.EstimatedOn ?? x.LastRepair.ReceivedOn,
                    LastRepairStatus = x.LastRepair.Status,
                    RemindersLastDateTime = x.RemindersLastDateTime,
                    RemindersCount = x.RemindersCount,
                    TotalRemindersCount = x.TotalRemindersCount,
                    Status = x.Status,
                }).ToList();
            }

            protected override Task<Boolean> ValidateViewModelAsync(Client client, ClientEmailSendMaintenanceRequiredModel model)
            {
                return Task.FromResult(true);
            }

            protected override async Task<MaintenanceRequiredEmail> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, ClientEmailSendMaintenanceRequiredModel model)
            {
                var domainClient = await this.clientManager.GetByIdAsync(client.Id);
                var items = await domainClient.RepairedHistory.GetByStatusAsync(ClientRepairedItemStatus.RequiresMaintenance);
                if (items.Count == 0)
                {
                    throw new InvalidOperationException("No items are due to service");
                }

                var email = new MaintenanceRequiredEmail
                {
                    Client = client,
                    Items = items,
                };

                return email;
            }

            protected override String GetHybridFormId() => "ClientEmailsSendMaintenanceRequired";

            protected override Int32 GetCreatedEmailSize(MaintenanceRequiredEmail email) => 0;
        }

        protected abstract class BaseSendClientEmailActionHandler<TEmailModel, TViewModel> : ActionHandler
            where TEmailModel : IEmail
            where TViewModel : class, new()
        {
            protected const Int32 MaxSize = 7 * 1024 * 1024;

            private readonly IRepository repository;
            private readonly UserEntityResolver userEntityResolver;
            private readonly ClientEmailsService clientEmailsService;

            protected BaseSendClientEmailActionHandler(Controller controller, IEntityControllerServices controllerServices)
                : base(controller)
            {
                this.repository = controllerServices.Repository;
                this.userEntityResolver = controllerServices.ServiceProvider.GetRequiredService<UserEntityResolver>();
                this.clientEmailsService = controllerServices.ServiceProvider.GetRequiredService<ClientEmailsService>();
            }

            protected IRepository Repository => this.repository;

            protected virtual ClientNotificationsType NotificationType => ClientNotificationsType.ManualEmail;

            public async Task<IActionResult> Send(Guid id)
            {
                var client = await this.repository.QueryWithoutTracking<Client>().SingleOrDefaultAsync(x => x.Id == id);
                if (client == null)
                {
                    return this.NotFound();
                }

                var clientUser = await this.repository.QueryWithoutTracking<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == client.UserId);

                var model = new TViewModel();
                await this.InitializeViewModelAsync(client, clientUser, model, initial: true);
                if (!client.NotificationsOptions.HasFlag(ClientNotificationsOptions.Enabled))
                {
                    this.ModelState.AddModelError(String.Empty, "Emails are not enabled for this surgery");
                }
                else if (this.clientEmailsService.IsNotificationsTypeDisabled(client, this.NotificationType))
                {
                    this.ModelState.AddModelError(String.Empty, "Surgery disabled emails of this type");
                }

                return this.View(model);
            }

            public async Task<IActionResult> Send(Guid id, TViewModel model)
            {
                var client = await this.repository.QueryWithoutTracking<Client>().SingleOrDefaultAsync(x => x.Id == id);
                if (client == null)
                {
                    return this.NotFound();
                }

                var clientUser = await this.repository.QueryWithoutTracking<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == client.UserId);

                if (!client.NotificationsOptions.HasFlag(ClientNotificationsOptions.Enabled))
                {
                    this.ModelState.AddModelError(String.Empty, "Emails are not enabled for this surgery");
                }
                else if (this.clientEmailsService.IsNotificationsTypeDisabled(client, this.NotificationType))
                {
                    this.ModelState.AddModelError(String.Empty, "Surgery disabled emails of this type");
                }

                if (this.ModelState.IsValid && await this.ValidateViewModelAsync(client, model))
                {
                    try
                    {
                        var user = await this.userEntityResolver.ResolveCurrentUserEntity();
                        var employee = user as Employee;

                        var email = await this.CreateEmailAsync(client, employee, clientUser, model);
                        if (await this.ValidateCreatedEmailAsync(client, email))
                        {
                            await this.clientEmailsService.SendClientEmail(client, email, this.NotificationType);
                            return await this.Controller.HybridFormResultAsync(this.GetHybridFormId(), this.RedirectToAction("Details", "Clients", new { id = client.Id, Tab = "Emails" }));
                        }
                    }
                    catch (ValidationException validationException)
                    {
                        var member = validationException.ValidationResult.MemberNames?.FirstOrDefault();
                        this.ModelState.AddModelError(member ?? String.Empty, validationException.ValidationResult.ErrorMessage ?? String.Empty);
                    }
                }

                await this.InitializeViewModelAsync(client, clientUser, model, initial: true);
                return this.View(model);
            }

            protected abstract Task InitializeViewModelAsync(Client client, ApplicationUser clientUser, TViewModel model, Boolean initial);

            protected abstract Task<Boolean> ValidateViewModelAsync(Client client, TViewModel model);

            protected abstract Task<TEmailModel> CreateEmailAsync(Client client, Employee employee, ApplicationUser clientUser, TViewModel model);

            protected abstract String GetHybridFormId();

            protected Task<(UploadedImage ImageInfo, Byte[] ImageBytes)> LoadImageAsync(Guid? imageId) => this.clientEmailsService.LoadImageAsync(imageId);

            protected Task<List<(UploadedFile FileInfo, Byte[] FileBytes)>> LoadAttachmentsAsync(IEnumerable<Guid> filesIds) => this.clientEmailsService.LoadAttachmentsAsync(filesIds);

            protected async Task<Boolean> ValidateGenericEmailSendModel(Client client, String to)
            {
                var validEmails = new List<String> { client.Email };
                validEmails.AddRange(client.ParseSecondaryEmails());

                if (!validEmails.Contains(to))
                {
                    this.ModelState.AddModelError("To", "Invalid email");
                    return false;
                }

                var user = await this.userEntityResolver.ResolveCurrentUserEntity();
                if (user == null)
                {
                    this.ModelState.AddModelError(String.Empty, "Invalid current user");
                    return false;
                }

                var employee = user as Employee;
                if (employee == null)
                {
                    this.ModelState.AddModelError(String.Empty, "Invalid current user");
                    return false;
                }

                return true;
            }

            protected Task<Boolean> ValidateCreatedEmailAsync(Client client, TEmailModel email)
            {
                if (this.GetCreatedEmailSize(email) > BaseSendClientEmailActionHandler<TEmailModel, TViewModel>.MaxSize)
                {
                    this.ModelState.AddModelError(String.Empty, "Email content is too large. Check that total size of all images and attachments does not exceed 7MB");
                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            protected abstract Int32 GetCreatedEmailSize(TEmailModel email);
        }
    }
}
