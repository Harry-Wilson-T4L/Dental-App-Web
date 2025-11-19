using System.Collections.Generic;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientRepairedHistoryReminderFactory
    {
        IClientRepairedHistoryReminder Create(IClient client, IReadOnlyList<IClientRepairedItem> itemsUpForService);
    }
}
