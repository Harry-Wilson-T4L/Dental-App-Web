using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceChangeItemViewModel
    {
        public Guid Id { get; set; }

        public DateTime ChangedOn { get; set; }

        public String ChangedByName { get; set; }

        public HandpieceStatus? OldStatus { get; set; }

        public String OldStatusName { get; set; }

        public HandpieceStatus? NewStatus { get; set; }

        public String NewStatusName { get; set; }

        public String OldContent { get; set; }

        public String NewContent { get; set; }

        public String OldFields { get; set; }

        public String NewFields { get; set; }
    }
}
