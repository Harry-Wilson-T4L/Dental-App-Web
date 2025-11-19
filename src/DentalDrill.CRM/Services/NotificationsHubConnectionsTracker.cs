using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Notifications;

namespace DentalDrill.CRM.Services
{
    public class NotificationsHubConnectionsTracker
    {
        private readonly Object lockObject = new Object();
        private readonly List<ConnectionInfo> connections = new List<ConnectionInfo>();

        public void RegisterConnection(String connectionId, NotificationVisibilityOptions visibilityOptions)
        {
            lock (this.lockObject)
            {
                var connectionInfo = new ConnectionInfo(connectionId, visibilityOptions);
                this.connections.Add(connectionInfo);
            }
        }

        public List<String> GetMatchingConnections(NotificationScope scope, Guid? workshopId, Guid? recipientId)
        {
            lock (this.lockObject)
            {
                if (workshopId == null)
                {
                    return this.connections
                        .Where(x => (x.VisibilityOptions.GlobalScope & scope) != NotificationScope.None)
                        .Where(x => recipientId == null || recipientId == x.VisibilityOptions.Employee.Id)
                        .Select(x => x.ConnectionId)
                        .ToList();
                }
                else
                {
                    return this.connections
                        .Where(x => x.VisibilityOptions.WorkshopScopes.Any(y => y.Workshop.Id == workshopId && (y.Scope & scope) != NotificationScope.None))
                        .Where(x => recipientId == null || recipientId == x.VisibilityOptions.Employee.Id)
                        .Select(x => x.ConnectionId)
                        .ToList();
                }
            }
        }

        public void UnregisterConnection(String connectionId)
        {
            lock (this.lockObject)
            {
                var connection = this.connections.FirstOrDefault(x => x.ConnectionId == connectionId);
                if (connection != null)
                {
                    this.connections.Remove(connection);
                }
            }
        }

        private class ConnectionInfo
        {
            public ConnectionInfo(String connectionId, NotificationVisibilityOptions visibilityOptions)
            {
                this.ConnectionId = connectionId;
                this.VisibilityOptions = visibilityOptions;
            }

            public String ConnectionId { get; }

            public NotificationVisibilityOptions VisibilityOptions { get; }
        }
    }
}
