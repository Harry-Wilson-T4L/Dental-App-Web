using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CorporateSurgeryReportsViewModel
    {
        public Corporate Corporate { get; set; }

        public CorporateAppearance Appearance { get; set; }

        public List<List<ReportDateRange>> DateRanges { get; set; }

        public ReportDateRange DefaultDateRange { get; set; }

        public List<Client> Clients { get; set; }

        public List<State> States { get; set; }
    }
}
