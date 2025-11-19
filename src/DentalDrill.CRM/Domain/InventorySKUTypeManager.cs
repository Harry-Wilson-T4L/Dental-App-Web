using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class InventorySKUTypeManager : IInventorySKUTypeManager
    {
        private readonly IRepository repository;
        private readonly IInventorySKUTypeFactory factory;

        public InventorySKUTypeManager(IRepository repository, IInventorySKUTypeFactory factory)
        {
            this.repository = repository;
            this.factory = factory;
        }

        public async Task<IReadOnlyList<IInventorySKUType>> GetAllAsync()
        {
            var entities = await this.repository.QueryWithoutTracking<InventorySKUType>()
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            return entities.Select(x => this.factory.Create(x)).ToList();
        }

        public async Task<IReadOnlyList<IInventorySKUType>> GetSomeAsync(Expression<Func<InventorySKUType, Boolean>> filter)
        {
            var entities = await this.repository.QueryWithoutTracking<InventorySKUType>()
                .Where(filter)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            return entities.Select(x => this.factory.Create(x)).ToList();
        }

        public async Task<IInventorySKUType> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.QueryWithoutTracking<InventorySKUType>()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            return this.factory.Create(entity);
        }

        public Task<IInventorySKUType> GetFromEntityAsync(InventorySKUType skuType)
        {
            return Task.FromResult(this.factory.Create(skuType));
        }
    }
}
