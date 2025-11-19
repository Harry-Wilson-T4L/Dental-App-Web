using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.InventoryTypes
{
    public class InventorySKUTypeReadModel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public Int32 OrderNo { get; set; }

        public HandpieceSpeedCompatibility HandpieceSpeedCompatibility { get; set; }

        public Int32 SKUCount { get; set; }
    }
}
