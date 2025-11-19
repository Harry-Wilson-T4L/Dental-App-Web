using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUReadModel
    {
        public Guid Id { get; set; }

        public Guid TypeId { get; set; }

        public Guid? ParentId { get; set; }

        public Int32 OrderNo { get; set; }

        public String Name { get; set; }

        public Decimal TotalQuantity { get; set; }

        public Decimal ShelfQuantity { get; set; }

        public Decimal TrayQuantity { get; set; }

        public Decimal OrderedQuantity { get; set; }

        public Decimal RequestedQuantity { get; set; }

        public Decimal? AveragePrice { get; set; }

        public Decimal? TotalPrice { get; set; }

        public String Description { get; set; }

        public InventorySKUNodeType NodeType { get; set; }

        public Boolean IsDefaultChild { get; set; }

        public Boolean HasWarning { get; set; }

        public Boolean HasDescendantsWithWarning { get; set; }
    }
}
