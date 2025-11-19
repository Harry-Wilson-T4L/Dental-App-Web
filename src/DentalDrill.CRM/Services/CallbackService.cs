using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Hubs;
using DentalDrill.CRM.Models;
using Microsoft.AspNetCore.SignalR;

namespace DentalDrill.CRM.Services
{
    public class CallbackService
    {
        private readonly IHubContext<NotificationsHub> notificationsHub;
        private readonly NotificationsHubConnectionsTracker connectionsTracker;

        public CallbackService(IHubContext<NotificationsHub> notificationsHub, NotificationsHubConnectionsTracker connectionsTracker)
        {
            this.notificationsHub = notificationsHub;
            this.connectionsTracker = connectionsTracker;
        }

        public async Task NotifyAssignedEmployee(params Guid[] employees)
        {
            foreach (var employee in employees)
            {
                var connectionIds = this.connectionsTracker.GetMatchingConnections(NotificationScope.ClientCallback, null, employee);
                await this.notificationsHub.Clients.Clients(connectionIds).SendAsync("CallbackUpdated");
            }
        }
    }
}
