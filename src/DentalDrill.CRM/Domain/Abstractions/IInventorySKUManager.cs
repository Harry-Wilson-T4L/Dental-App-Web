using System;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventorySKUManager
    {
        Task<IInventorySKU> GetByIdAsync(Guid id);
    }
}
