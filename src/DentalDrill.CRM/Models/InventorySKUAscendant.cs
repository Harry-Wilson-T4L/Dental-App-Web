using System;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUAscendant
    {
        public Guid DescendantId { get; set; }

        public InventorySKU Descendant { get; set; }

        public Guid AscendantId { get; set; }

        public InventorySKU Ascendant { get; set; }
    }
}
