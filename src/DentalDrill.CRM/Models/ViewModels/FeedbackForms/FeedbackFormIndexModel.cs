using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels.FeedbackForms
{
    public class FeedbackFormIndexModel
    {
        public List<FeedbackFormQuestion> Questions { get; set; }

        public IEmployeeAccess Access { get; set; }
    }
}
