using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IJobManager
    {
        Task<IReadOnlyList<IJob>> ListAsync();

        Task<IReadOnlyDictionary<Guid, IJob>> LoadResolverAsync();

        Task<IJob> GetByIdAsync(Guid id);
    }
}
