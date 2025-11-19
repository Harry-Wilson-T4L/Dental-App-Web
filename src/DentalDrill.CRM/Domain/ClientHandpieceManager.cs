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
    public class ClientHandpieceManager : IClientHandpieceManager
    {
        private readonly IRepository repository;
        private readonly IClientHandpieceLoader loader;

        public ClientHandpieceManager(
            IRepository repository,
            IClientHandpieceLoader loader)
        {
            this.repository = repository;
            this.loader = loader;
        }

        public async Task<IReadOnlyList<IClientHandpiece>> ListAsync()
        {
            var entities = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Client)
                .Include(x => x.Components)
                .OrderBy(x => x.ClientId)
                .ThenBy(x => x.Brand)
                .ThenBy(x => x.Model)
                .ThenBy(x => x.Serial)
                .ToListAsync();

            return await this.loader.LoadListAsync(entities);
        }

        public async Task<IReadOnlyDictionary<Guid, IClientHandpiece>> LoadResolverAsync()
        {
            var list = await this.ListAsync();
            return list.ToDictionary(x => x.Id);
        }

        public async Task<IClientHandpiece> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Client)
                .Include(x => x.Components)
                .OrderBy(x => x.ClientId)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            return await this.loader.LoadOneAsync(entity);
        }
    }
}
