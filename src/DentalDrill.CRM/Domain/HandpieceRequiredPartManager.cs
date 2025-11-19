using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class HandpieceRequiredPartManager : IHandpieceRequiredPartManager
    {
        private readonly IRepository repository;
        private readonly IHandpieceManager handpieceManager;
        private readonly IInventorySKUManager inventorySKUManager;
        private readonly IHandpieceRequiredPartFactory factory;

        private readonly DomainObjectCache<Guid, IHandpieceRequiredPart> cache = new DomainObjectCache<Guid, IHandpieceRequiredPart>();

        public HandpieceRequiredPartManager(
            IRepository repository,
            IHandpieceManager handpieceManager,
            IInventorySKUManager inventorySkuManager,
            IHandpieceRequiredPartFactory factory)
        {
            this.repository = repository;
            this.handpieceManager = handpieceManager;
            this.inventorySKUManager = inventorySkuManager;
            this.factory = factory;
        }

        public async Task<IReadOnlyList<IHandpieceRequiredPart>> ListAsync([CanBeNull] Expression<Func<HandpieceRequiredPart, Boolean>> predicate, [CanBeNull] IHandpiece handpiece, [CanBeNull] IInventorySKU sku)
        {
            var query = this.repository.QueryWithoutTracking<HandpieceRequiredPart>();
            if (handpiece != null)
            {
                query = query.Where(x => x.HandpieceId == handpiece.Id);
            }

            if (sku != null)
            {
                query = query.Where(x => x.SKUId == sku.Id);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var entities = await query.ToListAsync();
            var result = new List<IHandpieceRequiredPart>();
            foreach (var entity in entities)
            {
                result.Add(await this.GetFromEntityAsync(entity, handpiece, sku));
            }

            return result;
        }

        public Task<IHandpieceRequiredPart> GetByIdAsync(Guid id)
        {
            return this.cache.TryGetCachedValue(id, async key =>
            {
                var part = await this.repository.QueryWithoutTracking<HandpieceRequiredPart>()
                    .SingleOrDefaultAsync(x => x.Id == key);
                if (part == null)
                {
                    return null;
                }

                var handpiece = await this.handpieceManager.GetByIdAsync(part.HandpieceId) ?? throw new InvalidOperationException();
                var sku = await this.inventorySKUManager.GetByIdAsync(part.SKUId) ?? throw new InvalidOperationException();
                return this.factory.Create(part, handpiece, sku);
            });
        }

        public Task<IHandpieceRequiredPart> GetFromEntityAsync(HandpieceRequiredPart part, [CanBeNull] IHandpiece handpiece, [CanBeNull] IInventorySKU sku)
        {
            if (part == null)
            {
                return null;
            }

            if (handpiece != null && handpiece.Id != part.HandpieceId)
            {
                throw new ArgumentException("Handpiece Id mismatch", nameof(handpiece));
            }

            if (sku != null && sku.Id != part.SKUId)
            {
                throw new ArgumentException("SKU Id mismatch", nameof(handpiece));
            }

            return this.cache.TryGetCachedValue(part.Id, async key =>
            {
                handpiece ??= await this.handpieceManager.GetByIdAsync(part.HandpieceId) ?? throw new InvalidOperationException();
                sku ??= await this.inventorySKUManager.GetByIdAsync(part.SKUId) ?? throw new InvalidOperationException();
                return this.factory.Create(part, handpiece, sku);
            });
        }

        public async Task<IHandpieceRequiredPart> FindAsync(Expression<Func<HandpieceRequiredPart, Boolean>> predicate, [CanBeNull] IHandpiece handpiece, [CanBeNull] IInventorySKU sku)
        {
            var query = this.repository.QueryWithoutTracking<HandpieceRequiredPart>();
            if (handpiece != null)
            {
                query = query.Where(x => x.HandpieceId == handpiece.Id);
            }

            if (sku != null)
            {
                query = query.Where(x => x.SKUId == sku.Id);
            }

            var part = await query.SingleOrDefaultAsync(predicate);
            if (part == null)
            {
                return null;
            }

            return await this.GetFromEntityAsync(part, handpiece, sku);
        }

        public async Task<IHandpieceRequiredPart> FindByHandpieceAndSKUAsync(Guid handpieceId, [CanBeNull] IHandpiece handpiece, Guid skuId, [CanBeNull] IInventorySKU sku)
        {
            if (handpiece != null && handpiece.Id != handpieceId)
            {
                throw new ArgumentException("Handpiece Id mismatch", nameof(handpiece));
            }

            if (sku != null && sku.Id != skuId)
            {
                throw new ArgumentException("SKU Id mismatch", nameof(handpiece));
            }

            var part = await this.repository.QueryWithoutTracking<HandpieceRequiredPart>().SingleOrDefaultAsync(x => x.HandpieceId == handpieceId && x.SKUId == skuId);
            if (part == null)
            {
                return null;
            }

            return await this.GetFromEntityAsync(part, handpiece, sku);
        }
    }
}
