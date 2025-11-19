using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientRepairedHistory
    {
        Task<IReadOnlyList<IClientRepairedItem>> GetAllAsync();

        Task<IReadOnlyList<IClientRepairedItem>> GetByStatusAsync(ClientRepairedItemStatus status);

        Task<IReadOnlyList<IClientRepairedItem>> GetByStatusAsync(params ClientRepairedItemStatus[] status);

        Task<IClientRepairedItem> GetAsync(Guid id);
    }
}
