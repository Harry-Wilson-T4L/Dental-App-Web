using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpieceManager
    {
        Task<IHandpiece> GetByIdAsync(Guid id);

        Task<IHandpiece> FindOneAsync(Expression<Func<Handpiece, Boolean>> predicate);

        Task<IReadOnlyList<IHandpiece>> FindManyAsync(Expression<Func<Handpiece, Boolean>> predicate);
    }
}
