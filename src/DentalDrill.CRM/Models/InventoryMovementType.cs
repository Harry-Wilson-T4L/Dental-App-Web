using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum InventoryMovementType
    {
        /// <summary>Movement that sets the initial quantity as part of the initial stock data import.</summary>
        Initial = 0,

        /// <summary>Movement represents an order.</summary>
        Order = 1,

        /// <summary>Movement that corrects quantity by increasing it.</summary>
        Found = 2,

        /// <summary>Movement that transfer parts from another workshop.</summary>
        MoveFromAnotherWorkshop = 100,

        EphemeralMissingRequiredQuantity = 500,

        /// <summary>Movement that is created as part of repair process and would result in SKU consumption.</summary>
        Repair = 1000,

        /// <summary>Movement that corrects quantity by decreasing it.</summary>
        Lost = 1001,

        /// <summary>Movement that is created as part of repair process in case of partial allocation.</summary>
        RepairFragment = 1002,

        /// <summary>Movement that transfer parts to another workshop.</summary>
        MoveToAnotherWorkshop = 1100,
    }
}
