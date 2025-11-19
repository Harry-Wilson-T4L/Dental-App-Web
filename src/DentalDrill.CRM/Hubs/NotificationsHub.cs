using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using Microsoft.AspNetCore.SignalR;

namespace DentalDrill.CRM.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly NotificationsHubConnectionsTracker connectionsTracker;
        private readonly NotificationsService notificationsService;
        private readonly UserEntityResolver userResolver;

        public NotificationsHub(
            NotificationsHubConnectionsTracker connectionsTracker,
            NotificationsService notificationsService,
            UserEntityResolver userResolver)
        {
            this.connectionsTracker = connectionsTracker;
            this.notificationsService = notificationsService;
            this.userResolver = userResolver;
        }

        public override async Task OnConnectedAsync()
        {
            var options = await this.notificationsService.GetVisibilityOptions();
            if (options.Employee is not null)
            {
                this.connectionsTracker.RegisterConnection(this.Context.ConnectionId, options);
                return;
            }

            this.Context.Abort();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            this.connectionsTracker.UnregisterConnection(this.Context.ConnectionId);
            return Task.CompletedTask;
        }
    }
}
