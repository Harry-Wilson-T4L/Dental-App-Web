using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccessWorkshopJobTypeCollection
    {
        IEmployeeAccessWorkshopJobType this[String jobTypeId] { get; }

        IReadOnlyList<IEmployeeAccessWorkshopJobType> GetAll();

        IReadOnlyList<String> GetAvailable();
    }
}
