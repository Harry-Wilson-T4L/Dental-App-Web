using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccess
    {
        IEmployeeAccessGlobal Global { get; }

        IEmployeeAccessClient Clients { get; }

        IEmployeeAccessWorkshopCollection Workshops { get; }

        IEmployeeAccessInventory Inventory { get; }

        Boolean CanAccessClients();

        Boolean CanAccessWorkshops();

        Boolean CanAccessInventory();
    }
}
