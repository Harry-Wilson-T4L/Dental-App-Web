using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobsIndexViewModel
    {
        public JobsIndexFilterModel Filters { get; set; }

        public List<Client> Clients { get; set; }

        public List<Workshop> Workshops { get; set; }

        public List<JobType> JobTypes { get; set; }

        public List<HandpieceModelInfo> Models { get; set; }
    }
}
