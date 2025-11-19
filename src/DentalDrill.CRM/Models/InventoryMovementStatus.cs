using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum InventoryMovementStatus
    {
        Cancelled = 0,

        Requested = 10,
        Approved = 20,
        Ordered = 30,
        Waiting = 40,

        Allocated = 90,
        Completed = 100,
    }
}
