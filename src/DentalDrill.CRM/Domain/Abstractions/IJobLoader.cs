using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IJobLoader
    {
        Task<IJob> LoadOneAsync(
            Job entity,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null);

        Task<IReadOnlyList<IJob>> LoadListAsync(
            IReadOnlyList<Job> entities,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null);
    }
}
