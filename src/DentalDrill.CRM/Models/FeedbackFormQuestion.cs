using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum FeedbackFormQuestionType
    {
        Rating,
        MultilineText,
    }

    public class FeedbackFormQuestion
    {
        public Guid Id { get; set; }

        public Int32 OrderNo { get; set; }

        public FeedbackFormQuestionType Type { get; set; }

        public Boolean IsEnabled { get; set; }

        public Boolean IsDisplayed { get; set; }

        [Required]
        [MaxLength(200)]
        public String ShortName { get; set; }

        [Required]
        public String Name { get; set; }
    }
}
