using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels.ClientFeedbackForms
{
    public class ClientFeedbackFormIndexModel
    {
        public Client Client { get; set; }

        public List<FeedbackFormQuestion> Questions { get; set; }

        public IEmployeeAccessClient Access { get; set; }
    }
}
