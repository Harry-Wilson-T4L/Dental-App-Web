using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class InventorySKUManager : IInventorySKUManager
    {
        private readonly IRepository repository;
        private readonly IInventorySKUFactory factory;
        private readonly IInventorySKUTypeFactory typeFactory;
        private readonly DomainObjectCache<Guid, IInventorySKU> cache = new DomainObjectCache<Guid, IInventorySKU>();

        public InventorySKUManager(IRepository repository, IInventorySKUFactory factory, IInventorySKUTypeFactory typeFactory)
        {
            this.repository = repository;
            this.factory = factory;
            this.typeFactory = typeFactory;
        }

        public Task<IInventorySKU> GetByIdAsync(Guid id)
        {
            return this.cache.TryGetCachedValue(id, async key =>
            {
                var entity = await this.repository.QueryWithoutTracking<InventorySKU>()
                    .Include(x => x.Type)
                    .SingleOrDefaultAsync(x => x.Id == key);
                if (entity == null)
                {
                    return null;
                }

                var type = this.typeFactory.Create(entity.Type);
                return this.factory.Create(entity, type);
            });
        }
    }
}
