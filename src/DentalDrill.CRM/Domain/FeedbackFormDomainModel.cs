using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Validation;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class FeedbackFormDomainModel : IFeedbackForm
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IRepository repository;

        private readonly Guid id;

        private FeedbackForm form;

        private FeedbackFormDomainModel(IServiceProvider serviceProvider, FeedbackForm form)
        {
            this.serviceProvider = serviceProvider;
            this.repository = serviceProvider.GetService<IRepository>();

            this.id = form.Id;
            this.form = form;
        }

        public Guid Id => this.id;

        public FeedbackFormStatus Status => this.form.Status;

        public DateTime? SentOn => this.form.SentOn;

        public DateTime? ExpiresOn => this.form.ExpiresOn;

        public DateTime? OpenedOn => this.form.OpenedOn;

        public DateTime? CompletedOn => this.form.CompletedOn;

        public static async Task<FeedbackFormDomainModel> GetByIdAsync(IServiceProvider serviceProvider, Guid id)
        {
            var repository = serviceProvider.GetService<IRepository>();

            var form = await repository.Query<FeedbackForm>()
                .Include(x => x.Client)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (form == null)
            {
                return null;
            }

            return new FeedbackFormDomainModel(serviceProvider, form);
        }

        public async Task RefreshAsync()
        {
            var updatedForm = await this.repository.Query<FeedbackForm>()
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .SingleOrDefaultAsync(x => x.Id == this.id);
            if (updatedForm == null)
            {
                throw new InvalidOperationException("Unable to reload feedback form");
            }

            this.form = updatedForm;
        }

        public IReadOnlyList<FeedbackFormAnswer> GetQuestionsAndAnswers()
        {
            return this.form.Answers
                .OrderBy(x => x.Question.OrderNo)
                .ToList();
        }

        public ModelValidationResult ValidateAnswers(IReadOnlyDictionary<Guid, FeedbackFormFillModelAnswer> answers)
        {
            var questions = this.GetQuestionsAndAnswers();
            var errors = new List<ModelValidationError>();
            foreach (var question in questions)
            {
                switch (question.Question.Type)
                {
                    case FeedbackFormQuestionType.Rating:
                    {
                        if (answers.TryGetValue(question.QuestionId, out var answerModel) && answerModel is FeedbackFormFillModelRatingAnswer ratingAnswer && ratingAnswer.Value.HasValue)
                        {
                            if (ratingAnswer.Value.Value < 1 || ratingAnswer.Value.Value > 5)
                            {
                                errors.Add(new ModelValidationError($"[{question.QuestionId}]", "Must be between 1 and 5"));
                            }
                        }
                        else
                        {
                            errors.Add(new ModelValidationError($"[{question.QuestionId}]", "Required"));
                        }

                        break;
                    }

                    case FeedbackFormQuestionType.MultilineText:
                    {
                        if (answers.TryGetValue(question.QuestionId, out var answerModel) && answerModel is FeedbackFormFillModelMultilineTextAnswer textAnswer && !String.IsNullOrWhiteSpace(textAnswer.Value))
                        {
                            // Valid
                        }
                        else
                        {
                            errors.Add(new ModelValidationError($"[{question.QuestionId}]", "Required"));
                        }

                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return ModelValidationResult.FromErrorsList(errors);
        }

        public async Task OpenAsync()
        {
            if (this.form.Status == FeedbackFormStatus.New && this.form.OpenedOn == null)
            {
                this.form.OpenedOn = DateTime.UtcNow;
                await this.repository.UpdateAsync(this.form);
                await this.repository.SaveChangesAsync();

                await this.RefreshAsync();
            }
        }

        public async Task CompleteFormAsync(IReadOnlyDictionary<Guid, FeedbackFormFillModelAnswer> answers)
        {
            var questions = this.GetQuestionsAndAnswers();
            var totalRating = 0;
            foreach (var question in questions)
            {
                switch (question.Question.Type)
                {
                    case FeedbackFormQuestionType.Rating:
                    {
                        if (answers.TryGetValue(question.QuestionId, out var answerModel) && answerModel is FeedbackFormFillModelRatingAnswer ratingAnswer && ratingAnswer.Value.HasValue)
                        {
                            question.IntegerAnswer = ratingAnswer.Value;
                            question.StringAnswer = null;
                            totalRating += (Int32)(ratingAnswer.Value ?? 0m);
                        }

                        break;
                    }

                    case FeedbackFormQuestionType.MultilineText:
                    {
                        if (answers.TryGetValue(question.QuestionId, out var answerModel) && answerModel is FeedbackFormFillModelMultilineTextAnswer multilineTextAnswer)
                        {
                            question.IntegerAnswer = null;
                            question.StringAnswer = multilineTextAnswer.Value;
                        }

                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                await this.repository.UpdateAsync(question);
            }

            this.form.Status = FeedbackFormStatus.Completed;
            this.form.CompletedOn = DateTime.UtcNow;
            this.form.TotalRating = totalRating;

            await this.repository.UpdateAsync(this.form);
            await this.repository.SaveChangesAsync();

            await this.RefreshAsync();
        }

        public async Task EmailFormAsync(String email)
        {
            this.form.SentOn = DateTime.UtcNow;
            await this.repository.UpdateAsync(this.form);
            await this.repository.SaveChangesAsync();

            var configuration = this.serviceProvider.GetRequiredService<IConfiguration>();
            var baseUrl = configuration.GetValue<String>("Application:BaseUrl");
            var message = new FeedbackRequestEmail(this, email, baseUrl);
            var emailService = this.serviceProvider.GetRequiredService<ClientEmailsService>();
            await emailService.SendClientEmail(this.form.Client, message, ClientNotificationsType.FeedbackRequest);
        }
    }
}
