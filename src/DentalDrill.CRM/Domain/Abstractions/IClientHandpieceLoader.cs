using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientHandpieceLoader
    {
        Task<IClientHandpiece> LoadOneAsync(
            ClientHandpiece entity,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null);

        Task<IReadOnlyList<IClientHandpiece>> LoadListAsync(
            IReadOnlyList<ClientHandpiece> entities,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null);
    }
}
