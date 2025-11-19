using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.ClientFeedbackForms
{
    public class ClientFeedbackFormSendNewModel
    {
        [BindNever]
        public Client Client { get; set; }

        [BindNever]
        public List<FeedbackFormQuestion> Questions { get; set; }

        [Display(Name = "Send email")]
        public Boolean SendEmail { get; set; }

        [Display(Name = "Recipient address")]
        public String RecipientAddress { get; set; }

        [Display(Name = "Questions")]
        public List<Guid> SelectedQuestions { get; set; } = new List<Guid>();
    }
}
