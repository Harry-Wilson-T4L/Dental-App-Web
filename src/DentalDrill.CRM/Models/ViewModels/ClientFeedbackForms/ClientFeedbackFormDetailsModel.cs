using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.ClientFeedbackForms
{
    public class ClientFeedbackFormDetailsModel
    {
        public Client Client { get; set; }

        public FeedbackForm Form { get; set; }
    }
}
