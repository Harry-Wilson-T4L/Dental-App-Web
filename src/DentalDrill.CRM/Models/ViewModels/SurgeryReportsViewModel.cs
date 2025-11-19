using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class SurgeryReportsViewModel
    {
        public Client Surgery { get; set; }

        public ClientAppearance Appearance { get; set; }

        public List<List<ReportDateRange>> DateRanges { get; set; }

        public ReportDateRange DefaultDateRange { get; set; }
    }
}
