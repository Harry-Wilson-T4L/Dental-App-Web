using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientHandpieceManager
    {
        Task<IReadOnlyList<IClientHandpiece>> ListAsync();

        Task<IReadOnlyDictionary<Guid, IClientHandpiece>> LoadResolverAsync();

        Task<IClientHandpiece> GetByIdAsync(Guid id);
    }
}
