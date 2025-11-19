using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class FeedbackFormFillModel
    {
        [BindNever]
        public FeedbackFormDomainModel Form { get; set; }

        [BindNever]
        public IReadOnlyList<FeedbackFormAnswer> Answers { get; set; }

        public Dictionary<Guid, FeedbackFormFillModelAnswer> Answer { get; set; } = new Dictionary<Guid, FeedbackFormFillModelAnswer>();
    }
}
