using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.FeedbackForms
{
    public class FeedbackFormDetailsModel
    {
        public Client Client { get; set; }

        public FeedbackForm Form { get; set; }
    }
}
