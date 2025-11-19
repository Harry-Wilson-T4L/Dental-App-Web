using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models.Workflow;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobDetailsModel
    {
        public Guid Id { get; set; }

        public Job Entity { get; set; }

        public JobActionsList ActionsList { get; set; }

        public Boolean CanAddHandpieces { get; set; }

        public Boolean IsWorkshopTechnician { get; set; }

        public Boolean IsOfficeAdministrator { get; set; }

        public Boolean IsCompanyAdministrator { get; set; }
    }
}
