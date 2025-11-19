using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class ClientRepairedHistory : IClientRepairedHistory
    {
        private readonly IRepository repository;
        private readonly IClientRepairedItemFactory factory;
        private readonly IHandpieceLoader handpieceLoader;

        private readonly IClient client;

        public ClientRepairedHistory(
            IClient client,
            IRepository repository,
            IClientRepairedItemFactory factory,
            IHandpieceLoader handpieceLoader)
        {
            this.client = client;
            this.repository = repository;
            this.factory = factory;
            this.handpieceLoader = handpieceLoader;
        }

        public async Task<IReadOnlyList<IClientRepairedItem>> GetAllAsync()
        {
            var clientHandpieces = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Components)
                .Include(x => x.Handpieces).ThenInclude(x => x.Job)
                .Include(x => x.Handpieces).ThenInclude(x => x.ServiceLevel)
                .Where(x => x.ClientId == this.client.Id)
                .ToListAsync();

            var result = new List<IClientRepairedItem>();
            foreach (var clientHandpiece in clientHandpieces)
            {
                var handpiecesEntities = clientHandpiece.Handpieces
                    .Where(x => x.Job.ClientId == this.client.Id)
                    .ToList();
                var handpieces = await this.handpieceLoader.LoadListAsync(
                    handpiecesEntities,
                    clientsResolver: new Dictionary<Guid, IClient> { [this.client.Id] = this.client });

                var repairedItem = this.factory.Create(this.client, clientHandpiece, handpieces);
                result.Add(repairedItem);
            }

            return result;
        }

        public async Task<IReadOnlyList<IClientRepairedItem>> GetByStatusAsync(ClientRepairedItemStatus status)
        {
            var all = await this.GetAllAsync();
            return all.Where(x => x.Status == status).ToList();
        }

        public async Task<IReadOnlyList<IClientRepairedItem>> GetByStatusAsync(params ClientRepairedItemStatus[] status)
        {
            var all = await this.GetAllAsync();
            return all.Where(x => status.Contains(x.Status)).ToList();
        }

        public async Task<IClientRepairedItem> GetAsync(Guid id)
        {
            var clientHandpiece = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Handpieces).ThenInclude(x => x.Job)
                .Include(x => x.Handpieces).ThenInclude(x => x.ServiceLevel)
                .Where(x => x.ClientId == this.client.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (clientHandpiece == null)
            {
                return null;
            }

            var handpiecesEntities = clientHandpiece.Handpieces
                .Where(x => x.Job.ClientId == this.client.Id)
                .ToList();
            var handpieces = await this.handpieceLoader.LoadListAsync(
                handpiecesEntities,
                clientsResolver: new Dictionary<Guid, IClient> { [this.client.Id] = this.client });

            return this.factory.Create(this.client, clientHandpiece, handpieces);
        }
    }
}
