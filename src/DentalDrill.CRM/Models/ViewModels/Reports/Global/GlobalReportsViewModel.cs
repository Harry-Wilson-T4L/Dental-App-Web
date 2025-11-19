using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Global
{
    public class GlobalReportsViewModel
    {
        public List<List<ReportDateRange>> DateRanges { get; set; }

        public ReportDateRange DefaultDateRange { get; set; }
    }
}
