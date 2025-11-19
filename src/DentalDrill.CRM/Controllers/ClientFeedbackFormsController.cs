using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.ClientFeedbackForms;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientFeedbackFormsController : BaseTelerikIndexlessDependentBasicCrudController<Guid, FeedbackForm, Guid, Client, ClientFeedbackFormReadModel, ClientFeedbackFormDetailsModel, Object, Object, Object>
    {
        public ClientFeedbackFormsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.SendNewFormHandler = new BasicCrudCustomOperationActionHandler<Guid, Client, ClientFeedbackFormSendNewModel>(
                this,
                this.ControllerServices,
                new DefaultEntityPermissionsValidator<Client>(this.ControllerServices.PermissionsHub, null, null, null));

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.SendNewFormHandler.Overrides.InitializeOperationModel = this.InitializeSendNewFormModel;
            this.SendNewFormHandler.Overrides.ExecuteOperation = this.ExecuteSendNewFormOperation;
            this.SendNewFormHandler.Overrides.GetOperationSuccessResult = this.GetSendNewFormSuccessResult;
        }

        protected BasicCrudCustomOperationActionHandler<Guid, Client, ClientFeedbackFormSendNewModel> SendNewFormHandler { get; }

        public Task<IActionResult> SendNewForm(Guid clientId) => this.SendNewFormHandler.Execute(clientId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SendNewForm(Guid clientId, ClientFeedbackFormSendNewModel model) => this.SendNewFormHandler.Execute(clientId, model);

        public override Task<IActionResult> Create(Guid parentId) => throw new NotSupportedException();

        public override Task<IActionResult> Create(Guid parentId, Object model) => throw new NotSupportedException();

        public override Task<IActionResult> Edit(Guid id) => throw new NotSupportedException();

        public override Task<IActionResult> Edit(Guid id, Object model) => throw new NotSupportedException();

        public override Task<IActionResult> Delete(Guid id) => throw new NotSupportedException();

        public override Task<IActionResult> DeleteConfirmed(Guid id) => throw new NotSupportedException();

        private Task<IQueryable<FeedbackForm>> PrepareReadQuery(Client client)
        {
            IQueryable<FeedbackForm> query = this.Repository.QueryWithoutTracking<FeedbackForm>()
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .Where(x => x.ClientId == client.Id)
                .OrderByDescending(x => x.SentOn);

            return Task.FromResult(query);
        }

        private ClientFeedbackFormReadModel ConvertEntityToViewModel(Client client, FeedbackForm entity, String[] allowedProperties)
        {
            return new ClientFeedbackFormReadModel
            {
                Id = entity.Id,
                CreatedOn = entity.CreatedOn,
                SentOn = entity.SentOn,
                TotalRating = entity.TotalRating,
                Status = entity.Status,
                Answers = entity.Answers.ToDictionary(
                    x => x.QuestionId,
                    x => new ClientFeedbackFormReadAnswerModel
                    {
                        IntegerValue = x.IntegerAnswer,
                        StringValue = x.StringAnswer,
                    }),
            };
        }

        private Task<FeedbackForm> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<FeedbackForm>(
                    "Client",
                    "Answers",
                    "Answers.Question")
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<ClientFeedbackFormDetailsModel> ConvertToDetailsModel(FeedbackForm form)
        {
            var model = new ClientFeedbackFormDetailsModel
            {
                Client = form.Client,
                Form = form,
            };

            return Task.FromResult(model);
        }

        private async Task InitializeSendNewFormModel(Guid id, Client client, ClientFeedbackFormSendNewModel model, Boolean initial)
        {
            model.Client = client;
            model.Questions = await this.Repository.QueryWithoutTracking<FeedbackFormQuestion>().OrderBy(x => x.OrderNo).ToListAsync();

            if (initial)
            {
                model.SendEmail = true;
                model.RecipientAddress = client.Email;
                model.SelectedQuestions = model.Questions.Where(x => x.IsEnabled).Select(x => x.Id).ToList();
            }
        }

        private async Task ExecuteSendNewFormOperation(Guid id, Client client, ClientFeedbackFormSendNewModel model)
        {
            var clientDomain = await ClientDomainModel.GetByIdAsync(this.ControllerServices.ServiceProvider, client.Id);
            var feedbackForm = await clientDomain.Feedback.CreateFormAsync(model.SelectedQuestions);
            if (model.SendEmail)
            {
                await feedbackForm.EmailFormAsync(model.RecipientAddress);
            }
        }

        private Task<IActionResult> GetSendNewFormSuccessResult(Guid id, Client client, ClientFeedbackFormSendNewModel model)
        {
            return this.HybridFormResultAsync("ClientFeedbackFormsSendNewForm", this.RedirectToAction("Details", "Clients", new { id = client.Id, Tab = "Feedback" }));
        }
    }
}
