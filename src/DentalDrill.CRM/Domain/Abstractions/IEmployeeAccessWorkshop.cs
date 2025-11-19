using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccessWorkshop
    {
        Guid WorkshopId { get; }

        IEmployeeAccessWorkshopJobTypeCollection JobTypes { get; }

        EmployeeType LegacyRole { get; }

        Boolean CanAccessWorkshop();

        Boolean HasWorkshopPermission(WorkshopPermissions workshopPermissions);

        Boolean CanAccessInventory();

        Boolean HasInventoryPermission(InventoryMovementPermissions permission);

        NotificationScope GetNotificationScope();
    }
}
