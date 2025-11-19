using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientManager
    {
        IClientRepairedHistoryManager RepairHistory { get; }

        Task<IReadOnlyList<IClient>> ListFilteredAsync(Expression<Func<Client, Boolean>> filter);

        Task<IReadOnlyDictionary<Guid, IClient>> LoadResolverAsync();

        Task<IClient> GetByIdAsync(Guid id);

        Task<IClient> GetByUrlIdAsync(String urlId);
    }
}
