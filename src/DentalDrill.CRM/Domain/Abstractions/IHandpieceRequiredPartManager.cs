using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using JetBrains.Annotations;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpieceRequiredPartManager
    {
        Task<IReadOnlyList<IHandpieceRequiredPart>> ListAsync([CanBeNull] Expression<Func<HandpieceRequiredPart, Boolean>> predicate, [CanBeNull] IHandpiece handpiece, [CanBeNull] IInventorySKU sku);

        Task<IHandpieceRequiredPart> GetByIdAsync(Guid id);

        Task<IHandpieceRequiredPart> GetFromEntityAsync(HandpieceRequiredPart part, [CanBeNull] IHandpiece handpiece, [CanBeNull] IInventorySKU sku);

        Task<IHandpieceRequiredPart> FindAsync(Expression<Func<HandpieceRequiredPart, Boolean>> predicate, [CanBeNull] IHandpiece handpiece, [CanBeNull] IInventorySKU sku);

        Task<IHandpieceRequiredPart> FindByHandpieceAndSKUAsync(Guid handpieceId, [CanBeNull] IHandpiece handpiece, Guid skuId, [CanBeNull] IInventorySKU sku);
    }
}
