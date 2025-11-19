using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;

namespace DentalDrill.CRM.Domain
{
    public class ClientRepairedHistoryReminder : IClientRepairedHistoryReminder
    {
        private readonly ClientEmailsService clientEmailsService;

        public ClientRepairedHistoryReminder(IClient client, IReadOnlyList<IClientRepairedItem> itemsUpForService, ClientEmailsService clientEmailsService)
        {
            this.clientEmailsService = clientEmailsService;
            this.Client = client;
            this.ItemsUpForService = itemsUpForService;
        }

        public IClient Client { get; }

        public IReadOnlyList<IClientRepairedItem> ItemsUpForService { get; }

        public async Task SendEmailAsync()
        {
            var email = new MaintenanceRequiredEmail
            {
                Client = this.Client.Entity,
                Items = this.ItemsUpForService,
            };

            await this.clientEmailsService.SendClientEmail(this.Client.Entity, email, ClientNotificationsType.MaintenanceReminder);

            foreach (var item in this.ItemsUpForService)
            {
                await item.IncreaseSentCounterAsync();
            }
        }
    }
}
