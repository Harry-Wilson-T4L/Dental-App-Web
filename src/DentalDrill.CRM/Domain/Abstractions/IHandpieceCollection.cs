using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using JetBrains.Annotations;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpieceCollection
    {
        Task<IHandpiece> GetByIdAsync(Guid id);

        Task<IHandpiece> FindOneAsync(Expression<Func<Handpiece, Boolean>> predicate);

        Task<IReadOnlyList<IHandpiece>> FindManyAsync([CanBeNull] Expression<Func<Handpiece, Boolean>> predicate = null);
    }
}
