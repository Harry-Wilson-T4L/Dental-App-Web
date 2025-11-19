using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class FeedbackFormAnswer
    {
        public Guid Id { get; set; }

        public Guid FormId { get; set; }

        public FeedbackForm Form { get; set; }

        public Guid QuestionId { get; set; }

        public FeedbackFormQuestion Question { get; set; }

        public Int64? IntegerAnswer { get; set; }

        public String StringAnswer { get; set; }
    }
}
