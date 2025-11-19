using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientFeedbackCollection
    {
        Task<IFeedbackForm> CreateFormAsync(IReadOnlyList<Guid> selectedQuestions = null);

        Task ProcessJobCompletionAsync();
    }
}
