using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class ClientHandpieceLoader : DomainLoaderBase, IClientHandpieceLoader
    {
        private readonly IClientHandpieceFactory factory;
        private readonly IClientManager clientManager;
        private readonly IClientFactory clientFactory;

        public ClientHandpieceLoader(
            IClientHandpieceFactory factory,
            IClientManager clientManager,
            IClientFactory clientFactory)
        {
            this.factory = factory;
            this.clientManager = clientManager;
            this.clientFactory = clientFactory;
        }

        public async Task<IClientHandpiece> LoadOneAsync(
            ClientHandpiece entity,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null)
        {
            var client = await DomainLoaderBase.ResolveRequiredDependency(
                entity.ClientId,
                entity.Client,
                clientsResolver,
                key => this.clientManager.GetByIdAsync(key),
                e => this.clientFactory.Create(e));

            return this.factory.Create(entity, client);
        }

        public async Task<IReadOnlyList<IClientHandpiece>> LoadListAsync(
            IReadOnlyList<ClientHandpiece> entities,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null)
        {
            clientsResolver = await DomainLoaderBase.PrepareResolver(
                entities.Select(x => new KeyValuePair<Guid, Client>(x.ClientId, x.Client)).ToList(),
                clientsResolver,
                () => this.clientManager.LoadResolverAsync(),
                e => this.clientFactory.Create(e));

            return entities.Select(x => this.factory.Create(x, clientsResolver[x.ClientId])).ToList();
        }
    }
}
