using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IJobTypeManager
    {
        Task<IReadOnlyList<IJobType>> ListAsync();

        Task<IReadOnlyDictionary<String, IJobType>> LoadResolverAsync();

        Task<IJobType> GetByIdAsync(String id);
    }
}
