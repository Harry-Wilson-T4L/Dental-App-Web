using System;
using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class ClientRepairedHistoryReminderFactory : IClientRepairedHistoryReminderFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ClientRepairedHistoryReminderFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IClientRepairedHistoryReminder Create(IClient client, IReadOnlyList<IClientRepairedItem> itemsUpForService)
        {
            return new ClientRepairedHistoryReminder(
                client,
                itemsUpForService,
                this.serviceProvider.GetRequiredService<ClientEmailsService>());
        }
    }
}
