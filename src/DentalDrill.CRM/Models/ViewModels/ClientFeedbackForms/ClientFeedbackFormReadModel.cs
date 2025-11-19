using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.ClientFeedbackForms
{
    public class ClientFeedbackFormReadModel
    {
        public Guid Id { get; set; }

        public FeedbackFormStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? SentOn { get; set; }

        public Int32 TotalRating { get; set; }

        public Dictionary<Guid, ClientFeedbackFormReadAnswerModel> Answers { get; set; }
    }
}
