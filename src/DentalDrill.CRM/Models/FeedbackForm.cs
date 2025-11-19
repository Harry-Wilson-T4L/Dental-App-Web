using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum FeedbackFormStatus
    {
        New,
        Completed,
        Expired,
        Cancelled,
    }

    public class FeedbackForm
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public FeedbackFormStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? SentOn { get; set; }

        public DateTime? ScheduledOn { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public DateTime? OpenedOn { get; set; }

        public DateTime? CompletedOn { get; set; }

        public Int32 TotalRating { get; set; }

        public ICollection<FeedbackFormAnswer> Answers { get; set; }
    }
}
