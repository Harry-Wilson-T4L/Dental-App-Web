using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Mail.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class ClientRepairedHistoryManager : IClientRepairedHistoryManager
    {
        private readonly IClientManager clientManager;
        private readonly IRepository repository;
        private readonly IClientRepairedHistoryReminderFactory factory;
        private readonly IDateTimeService dateTimeService;

        public ClientRepairedHistoryManager(IClientManager clientManager, IRepository repository, IClientRepairedHistoryReminderFactory factory, IDateTimeService dateTimeService)
        {
            this.clientManager = clientManager;
            this.repository = repository;
            this.factory = factory;
            this.dateTimeService = dateTimeService;
        }

        public async Task<IReadOnlyList<IClientRepairedHistoryReminder>> GetPendingRemindersAsync()
        {
            var today = this.dateTimeService.CurrentUtcTime;
            var beginningOfTheMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var eligibleClients = await this.clientManager
                .ListFilteredAsync(x => (x.NotificationsOptions & ClientNotificationsOptions.Enabled) == ClientNotificationsOptions.Enabled &&
                                        (x.NotificationsOptions & ClientNotificationsOptions.DisabledMaintenanceReminders) != ClientNotificationsOptions.DisabledMaintenanceReminders &&
                                        x.Email != null &&
                                        x.Email != String.Empty &&
                                        x.UserId != null);
            var result = new List<IClientRepairedHistoryReminder>();
            foreach (var client in eligibleClients)
            {
                var lastEmail = await this.repository.Query<ClientEmailMessage>()
                    .Include(x => x.EmailMessage)
                    .Where(x => x.ClientId == client.Id && x.EmailMessage.MessageType == nameof(MaintenanceRequiredEmail))
                    .Where(x => x.EmailMessage.Status == EmailMessageStatus.Sent)
                    .OrderByDescending(x => x.EmailMessage.Created)
                    .FirstOrDefaultAsync();
                if (lastEmail != null && lastEmail.EmailMessage.Created > beginningOfTheMonth)
                {
                    continue;
                }

                var hasAnyEmail = await this.repository.Query<ClientEmailMessage>()
                    .Where(x => x.ClientId == client.Id)
                    .AnyAsync();
                if (!hasAnyEmail)
                {
                    continue;
                }

                var requiresMaintenance = await client.RepairedHistory.GetByStatusAsync(ClientRepairedItemStatus.RequiresMaintenance);
                if (requiresMaintenance.Count < 3)
                {
                    continue;
                }

                var reminder = this.factory.Create(client, requiresMaintenance);
                result.Add(reminder);
            }

            return result;
        }

        public async Task<IReadOnlyList<IClientRepairedHistoryReminder>> GetAllHistoryAsync()
        {
            var eligibleClients = await this.clientManager
                .ListFilteredAsync(x => (x.NotificationsOptions & ClientNotificationsOptions.Enabled) == ClientNotificationsOptions.Enabled &&
                                        (x.NotificationsOptions & ClientNotificationsOptions.DisabledMaintenanceReminders) != ClientNotificationsOptions.DisabledMaintenanceReminders &&
                                        x.Email != null &&
                                        x.Email != String.Empty &&
                                        x.UserId != null);
            var result = new List<IClientRepairedHistoryReminder>();
            foreach (var client in eligibleClients)
            {
                var hasAnyEmail = await this.repository.Query<ClientEmailMessage>()
                    .Where(x => x.ClientId == client.Id)
                    .AnyAsync();
                if (!hasAnyEmail)
                {
                    continue;
                }

                var requiresMaintenance = await client.RepairedHistory.GetByStatusAsync(ClientRepairedItemStatus.RequiresMaintenance, ClientRepairedItemStatus.RemindedRecently, ClientRepairedItemStatus.Disabled);
                var reminder = this.factory.Create(client, requiresMaintenance);
                result.Add(reminder);
            }

            return result;
        }
    }
}
