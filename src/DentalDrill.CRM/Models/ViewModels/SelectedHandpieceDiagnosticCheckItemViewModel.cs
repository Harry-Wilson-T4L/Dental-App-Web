using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class SelectedHandpieceDiagnosticCheckItemViewModel
    {
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }

        public String ItemName { get; set; }

        public Boolean Checked { get; set; }
    }
}
