using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpieceLoader
    {
        Task<IHandpiece> LoadOneAsync(
            Handpiece entity,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null,
            IReadOnlyDictionary<Guid, IJob> jobResolver = null,
            IReadOnlyDictionary<Guid, IClientHandpiece> clientHandpieceResolver = null);

        Task<IReadOnlyList<IHandpiece>> LoadListAsync(
            IReadOnlyList<Handpiece> entities,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null,
            IReadOnlyDictionary<Guid, IJob> jobResolver = null,
            IReadOnlyDictionary<Guid, IClientHandpiece> clientHandpieceResolver = null);
    }
}
