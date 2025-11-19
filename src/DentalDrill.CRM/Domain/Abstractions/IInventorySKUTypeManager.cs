using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventorySKUTypeManager
    {
        Task<IReadOnlyList<IInventorySKUType>> GetAllAsync();

        Task<IReadOnlyList<IInventorySKUType>> GetSomeAsync(Expression<Func<InventorySKUType, Boolean>> filter);

        Task<IInventorySKUType> GetByIdAsync(Guid id);

        Task<IInventorySKUType> GetFromEntityAsync(InventorySKUType skuType);
    }
}
