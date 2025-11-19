using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Validation;
using DentalDrill.CRM.Models.ViewModels;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IFeedbackForm
    {
        Guid Id { get; }

        FeedbackFormStatus Status { get; }

        DateTime? SentOn { get; }

        DateTime? ExpiresOn { get; }

        DateTime? OpenedOn { get; }

        DateTime? CompletedOn { get; }

        Task RefreshAsync();

        IReadOnlyList<FeedbackFormAnswer> GetQuestionsAndAnswers();

        ModelValidationResult ValidateAnswers(IReadOnlyDictionary<Guid, FeedbackFormFillModelAnswer> answers);

        Task OpenAsync();

        Task CompleteFormAsync(IReadOnlyDictionary<Guid, FeedbackFormFillModelAnswer> answers);

        Task EmailFormAsync(String email);
    }
}
