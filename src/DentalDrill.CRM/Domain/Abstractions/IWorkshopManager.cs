using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IWorkshopManager
    {
        Task<IReadOnlyList<IWorkshop>> ListAllAsync();

        Task<IReadOnlyList<IWorkshop>> ListAllAsync(IEmployeeAccess access, Func<IEmployeeAccessWorkshop, Boolean> predicate);

        Task<IReadOnlyList<IWorkshop>> ListActiveAsync([CanBeNull] IReadOnlyList<Guid> extra = null);

        Task<IReadOnlyList<IWorkshop>> ListActiveAsync(IEmployeeAccess access, Func<IEmployeeAccessWorkshop, Boolean> predicate, [CanBeNull] IReadOnlyList<Guid> extra = null);

        Task<IReadOnlyDictionary<Guid, IWorkshop>> LoadResolverAsync();

        Task<IWorkshop> GetSydneyAsync();

        Task<IWorkshop> GetByIdAsync(Guid id);

        Task<IWorkshop> GetActiveByIdAsync(Guid id);
    }
}
