using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementHistoryChangeReadModel
    {
        public Guid Id { get; set; }

        public DateTime ChangedOn { get; set; }

        public Guid ChangedBy { get; set; }

        public String ChangedByName { get; set; }

        public ChangeAction Action { get; set; }

        public String ActionName { get; set; }

        public InventoryMovementStatus? OldStatus { get; set; }

        public String OldStatusName { get; set; }

        public InventoryMovementStatus? NewStatus { get; set; }

        public String NewStatusName { get; set; }

        public Decimal? OldQuantity { get; set; }

        public Decimal? NewQuantity { get; set; }

        public Decimal? OldPrice { get; set; }

        public Decimal? NewPrice { get; set; }

        public String OldComment { get; set; }

        public String NewComment { get; set; }
    }
}
