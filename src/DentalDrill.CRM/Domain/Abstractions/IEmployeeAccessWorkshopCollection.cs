using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccessWorkshopCollection
    {
        IEmployeeAccessWorkshop this[Guid workshopId] { get; }

        IReadOnlyList<Guid> GetAvailable();

        IReadOnlyList<Guid> GetAvailable(Func<IEmployeeAccessWorkshop, Boolean> predicate);

        IReadOnlyList<Guid> GetWorkshopAvailable();

        IReadOnlyList<Guid> GetInventoryAvailable();

        IReadOnlyList<Guid> GetWorkshopDeclared();

        IReadOnlyList<IEmployeeAccessWorkshop> GetAll();
    }
}
