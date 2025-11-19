using System;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IWorkshop
    {
        Guid Id { get; }

        String Name { get; }

        String Description { get; }
    }
}
