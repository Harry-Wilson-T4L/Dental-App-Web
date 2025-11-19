using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientRepairedHistoryManager
    {
        Task<IReadOnlyList<IClientRepairedHistoryReminder>> GetPendingRemindersAsync();

        Task<IReadOnlyList<IClientRepairedHistoryReminder>> GetAllHistoryAsync();
    }
}
