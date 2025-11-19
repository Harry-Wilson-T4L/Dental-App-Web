using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientRepairedHistoryReminder
    {
        IClient Client { get; }

        IReadOnlyList<IClientRepairedItem> ItemsUpForService { get; }

        Task SendEmailAsync();
    }
}
