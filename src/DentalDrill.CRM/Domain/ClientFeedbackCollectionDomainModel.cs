using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class ClientFeedbackCollectionDomainModel : IClientFeedbackCollection
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IRepository repository;

        private readonly IClient client;

        public ClientFeedbackCollectionDomainModel(IServiceProvider serviceProvider, IClient client)
        {
            this.serviceProvider = serviceProvider;
            this.repository = serviceProvider.GetService<IRepository>();

            this.client = client;
        }

        public async Task<IFeedbackForm> CreateFormAsync(IReadOnlyList<Guid> selectedQuestions = null)
        {
            List<FeedbackFormQuestion> questions;
            if (selectedQuestions == null)
            {
                questions = await this.repository.Query<FeedbackFormQuestion>()
                    .Where(x => x.IsEnabled)
                    .OrderBy(x => x.OrderNo)
                    .ToListAsync();
            }
            else
            {
                questions = await this.repository.Query<FeedbackFormQuestion>()
                    .Where(x => selectedQuestions.Contains(x.Id))
                    .OrderBy(x => x.OrderNo)
                    .ToListAsync();
            }

            var feedbackForm = new FeedbackForm
            {
                ClientId = this.client.Id,
                Status = FeedbackFormStatus.New,
                CreatedOn = DateTime.UtcNow,
                ScheduledOn = null,
                SentOn = null,
                ExpiresOn = null,
                OpenedOn = null,
                CompletedOn = null,
                TotalRating = 0,
                Answers = new List<FeedbackFormAnswer>(),
            };

            foreach (var question in questions)
            {
                feedbackForm.Answers.Add(new FeedbackFormAnswer
                {
                    QuestionId = question.Id,
                    IntegerAnswer = null,
                    StringAnswer = null,
                });
            }

            await this.repository.InsertAsync(feedbackForm);
            await this.repository.SaveChangesAsync();

            return await FeedbackFormDomainModel.GetByIdAsync(this.serviceProvider, feedbackForm.Id);
        }

        public async Task ProcessJobCompletionAsync()
        {
            var feedbackConfiguration = await this.repository.QueryWithoutTracking<FeedbackConfiguration>().SingleOrDefaultAsync(x => x.Id == FeedbackConfiguration.Default);
            if (feedbackConfiguration == null)
            {
                return;
            }

            if (feedbackConfiguration.AutoSendFormEnabled == false)
            {
                return;
            }

            if (feedbackConfiguration.PeriodLimitingEnabled)
            {
                var (from, to) = this.GetPeriodForLimit(feedbackConfiguration);
                var count = await this.repository.QueryWithoutTracking<FeedbackForm>()
                    .Where(x => x.ClientId == this.client.Id)
                    .Where(x => x.CreatedOn >= from && x.CreatedOn < to)
                    .CountAsync();

                if (count >= feedbackConfiguration.PeriodLimitingQuantity)
                {
                    return;
                }
            }

            var clientOptions = await this.GetClientOptionsAsync();
            if (clientOptions.SkippedJobs >= feedbackConfiguration.AutoSendSkipJobs)
            {
                clientOptions.SkippedJobs = 0;
                await this.AutoSendAsync(feedbackConfiguration);
            }
            else
            {
                clientOptions.SkippedJobs++;
            }
        }

        private (DateTime From, DateTime To) GetPeriodForLimit(FeedbackConfiguration configuration)
        {
            switch (configuration.PeriodLimitingType)
            {
                case FeedbackConfigurationPeriodType.Day:
                {
                    var today = DateTime.UtcNow.Date;
                    var to = today.AddDays(1);
                    var from = to.AddDays(-configuration.PeriodLimitingLength);

                    return (from, to);
                }

                case FeedbackConfigurationPeriodType.Week:
                {
                    var today = DateTime.UtcNow.Date;
                    var monday = today.AddDays(1);
                    while (monday.DayOfWeek != DayOfWeek.Monday)
                    {
                        monday = monday.AddDays(1);
                    }

                    var from = monday.AddDays(-7 * configuration.PeriodLimitingLength);
                    return (from, monday);
                }

                case FeedbackConfigurationPeriodType.Month:
                {
                    var today = DateTime.UtcNow.Date;
                    var nextMonth = today.AddMonths(1);
                    var to = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                    var from = to.AddMonths(-configuration.PeriodLimitingLength);

                    return (from, to);
                }

                case FeedbackConfigurationPeriodType.Quarter:
                {
                    var today = DateTime.UtcNow.Date;
                    var nextMonth = today.AddMonths(1);
                    while ((nextMonth.Month % 3) != 1)
                    {
                        nextMonth = today.AddMonths(1);
                    }

                    var to = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                    var from = to.AddMonths(-3 * configuration.PeriodLimitingLength);

                    return (from, to);
                }

                case FeedbackConfigurationPeriodType.Year:
                {
                    var today = DateTime.UtcNow.Date;
                    var nextYear = new DateTime(today.Year + 1, 1, 1);
                    var from = nextYear.AddYears(-configuration.PeriodLimitingLength);

                    return (from, nextYear);
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<ClientFeedbackOptions> GetClientOptionsAsync()
        {
            var options = await this.repository.Query<ClientFeedbackOptions>().SingleOrDefaultAsync(x => x.ClientId == this.client.Id);
            if (options == null)
            {
                options = new ClientFeedbackOptions
                {
                    ClientId = this.client.Id,
                    SkippedJobs = 0,
                };

                await this.repository.InsertAsync(options);
            }
            else
            {
                await this.repository.UpdateAsync(options);
            }

            return options;
        }

        private async Task<FeedbackForm> AutoSendAsync(FeedbackConfiguration configuration)
        {
            var questions = await this.repository.Query<FeedbackFormQuestion>()
                .Where(x => x.IsEnabled)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            var createdOn = DateTime.UtcNow;
            var scheduledOn = createdOn.AddDays(configuration.AutoSendFormDelayDays);

            var feedbackForm = new FeedbackForm
            {
                ClientId = this.client.Id,
                Status = FeedbackFormStatus.New,
                CreatedOn = createdOn,
                ScheduledOn = scheduledOn,
                SentOn = null,
                ExpiresOn = null,
                OpenedOn = null,
                CompletedOn = null,
                TotalRating = 0,
                Answers = new List<FeedbackFormAnswer>(),
            };

            foreach (var question in questions)
            {
                feedbackForm.Answers.Add(new FeedbackFormAnswer
                {
                    QuestionId = question.Id,
                    IntegerAnswer = null,
                    StringAnswer = null,
                });
            }

            await this.repository.InsertAsync(feedbackForm);

            return feedbackForm;
        }
    }
}
