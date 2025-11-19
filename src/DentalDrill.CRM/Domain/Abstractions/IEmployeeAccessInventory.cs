using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccessInventory
    {
        Boolean HasPermission(InventoryPermissions inventoryPermissions);
    }
}
