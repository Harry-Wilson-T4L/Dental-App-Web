using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.FeedbackForms
{
    public class FeedbackFormReadModel
    {
        public Guid Id { get; set; }

        public FeedbackFormStatus Status { get; set; }

        public Guid ClientId { get; set; }

        public String ClientFullName { get; set; }

        public String ClientName { get; set; }

        public String ClientPrincipalDentist { get; set; }

        public String ClientSuburb {get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? SentOn { get; set; }

        public Int32 TotalRating { get; set; }

        public Dictionary<Guid, FeedbackFormReadAnswerModel> Answers { get; set; }
    }
}
