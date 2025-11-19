using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.FeedbackForms;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/FeedbackForm")]
    [PermissionsManager("Entity", "/Domain/FeedbackForm/Entities/{entity}")]
    public class FeedbackFormsController : BaseTelerikIndexBasicDetailsController<Guid, FeedbackForm, FeedbackFormIndexModel, FeedbackFormReadModel, FeedbackFormDetailsModel>
    {
        public FeedbackFormsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.AjaxReadHandler.Overrides.PreprocessRequest = this.PreprocessRequest;
            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;
        }

        protected IRepository Repository => this.ControllerServices.Repository;

        private async Task InitializeIndexViewModel(FeedbackFormIndexModel model)
        {
            model.Questions = await this.Repository.Query<FeedbackFormQuestion>()
                .Where(x => x.IsDisplayed)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task PreprocessRequest(DataSourceRequest request)
        {
            request.RenameFields(new Dictionary<String, String>
            {
                ["ClientFullName"] = "Client.FullName",
            });

            return Task.CompletedTask;
        }

        private Task<IQueryable<FeedbackForm>> PrepareReadQuery()
        {
            IQueryable<FeedbackForm> query = this.Repository.QueryWithoutTracking<FeedbackForm>()
                .Include(x => x.Client)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .OrderByDescending(x => x.SentOn);

            return Task.FromResult(query);
        }

        private FeedbackFormReadModel ConvertEntityToViewModel(FeedbackForm entity, String[] allowedProperties)
        {
            return new FeedbackFormReadModel
            {
                Id = entity.Id,
                ClientId = entity.Client.Id,
                ClientFullName = entity.Client.FullName,
                ClientName = entity.Client.Name,
                ClientPrincipalDentist = entity.Client.PrincipalDentist,
                ClientSuburb = entity.Client.Suburb,
                CreatedOn = entity.CreatedOn,
                SentOn = entity.SentOn,
                TotalRating = entity.TotalRating,
                Status = entity.Status,
                Answers = entity.Answers.ToDictionary(
                    x => x.QuestionId,
                    x => new FeedbackFormReadAnswerModel
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

        private Task<FeedbackFormDetailsModel> ConvertToDetailsModel(FeedbackForm form)
        {
            var model = new FeedbackFormDetailsModel
            {
                Client = form.Client,
                Form = form,
            };

            return Task.FromResult(model);
        }
    }
}
