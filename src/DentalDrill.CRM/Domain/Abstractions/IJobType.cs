using System;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IJobType
    {
        String Id { get; }

        String Name { get; }

        String ShortName { get; }
    }
}
