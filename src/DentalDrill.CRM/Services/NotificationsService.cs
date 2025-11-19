using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Hubs;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Notifications;
using DentalDrill.CRM.Models.Permissions;
using DevGuild.AspNetCore.Services.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Services
{
    public class NotificationsService
    {
        private readonly IRepository repository;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly UserEntityResolver userResolver;
        private readonly IHubContext<NotificationsHub> notificationsHub;
        private readonly NotificationsHubConnectionsTracker connectionsTracker;

        public NotificationsService(
            IRepository repository,
            IRepositoryFactory repositoryFactory,
            UserEntityResolver userResolver,
            IHubContext<NotificationsHub> notificationsHub,
            NotificationsHubConnectionsTracker connectionsTracker)
        {
            this.repository = repository;
            this.repositoryFactory = repositoryFactory;
            this.userResolver = userResolver;
            this.notificationsHub = notificationsHub;
            this.connectionsTracker = connectionsTracker;
        }

        public async Task<List<Notification>> LoadNotifications()
        {
            using var independentRepository = this.repositoryFactory.CreateRepository();

            var query = await this.PrepareQuery(independentRepository);
            var notifications = await query.OrderByDescending(x => x.CreatedOn).ToListAsync();

            var now = DateTime.UtcNow;
            var hasChanges = false;
            foreach (var notification in notifications.Where(x => x.Status == NotificationStatus.Unread))
            {
                notification.ReadOn = now;
                notification.Status = NotificationStatus.Read;
                await independentRepository.UpdateAsync(notification);
                hasChanges = true;
            }

            if (hasChanges)
            {
                await independentRepository.SaveChangesAsync();
            }

            return notifications;
        }

        public async Task ReadJobNotifications(Job job, params NotificationType[] types)
        {
            using var independentRepository = this.repositoryFactory.CreateRepository();

            var query = await this.PrepareQuery(independentRepository);
            var notificationsQuery = query
                .Where(x => x.Status == NotificationStatus.Unread)
                .Where(x => x.RelatedEntities.Any(y => y.EntityType == NotificationRelatedEntityType.Job && y.EntityId == job.Id));

            if (types != null && types.Length > 0)
            {
                notificationsQuery = notificationsQuery.Where(x => types.Contains(x.Type));
            }

            var notifications = await notificationsQuery.ToListAsync();
            if (notifications.Count > 0)
            {
                foreach (var notification in notifications)
                {
                    notification.Status = NotificationStatus.Read;
                    await independentRepository.UpdateAsync(notification);
                }

                await independentRepository.SaveChangesAsync();

                foreach (var notification in notifications)
                {
                    await this.SendNotificationToClients(notification, NotificationPayload.LoadFrom(notification.Type, notification.Payload));
                }
            }
        }

        public async Task ReadHandpieceStoreOrderNotifications(HandpieceStoreOrder order, params NotificationType[] types)
        {
            using var independentRepository = this.repositoryFactory.CreateRepository();

            var query = await this.PrepareQuery(independentRepository);
            var notificationsQuery = query
                .Where(x => x.Status == NotificationStatus.Unread)
                .Where(x => x.RelatedEntities.Any(y => y.EntityType == NotificationRelatedEntityType.HandpieceStoreOrder && y.EntityId == order.Id));

            if (types != null && types.Length > 0)
            {
                notificationsQuery = notificationsQuery.Where(x => types.Contains(x.Type));
            }

            var notifications = await notificationsQuery.ToListAsync();
            if (notifications.Count > 0)
            {
                foreach (var notification in notifications)
                {
                    notification.Status = NotificationStatus.Read;
                    await independentRepository.UpdateAsync(notification);
                }

                await independentRepository.SaveChangesAsync();

                foreach (var notification in notifications)
                {
                    await this.SendNotificationToClients(notification, NotificationPayload.LoadFrom(notification.Type, notification.Payload));
                }
            }
        }

        public async Task RemoveJobNotifications(Guid jobId)
        {
            using var independentRepository = this.repositoryFactory.CreateRepository();

            var notificationsQuery = independentRepository
                .Query<Notification>()
                .Where(x => x.Status == NotificationStatus.Unread)
                .Where(x => x.RelatedEntities.Any(y => y.EntityType == NotificationRelatedEntityType.Job && y.EntityId == jobId));

            var notifications = await notificationsQuery.ToListAsync();
            if (notifications.Count > 0)
            {
                foreach (var notification in notifications)
                {
                    notification.Status = NotificationStatus.Read;
                    await independentRepository.UpdateAsync(notification);
                }

                await independentRepository.SaveChangesAsync();

                foreach (var notification in notifications)
                {
                    await this.SendNotificationToClients(notification, NotificationPayload.LoadFrom(notification.Type, notification.Payload));
                }
            }
        }

        public Task<Notification> CreateJobNotification(NotificationPayload payload, NotificationScope scope, Job job)
        {
            return this.CreateCustomNotification(payload, scope, job.WorkshopId, null, job);
        }

        public Task<Notification> CreateJobNotification(NotificationPayload payload, NotificationScope scope, Employee recipient, Job job)
        {
            return this.CreateCustomNotification(payload, scope, job.WorkshopId, recipient, job);
        }

        public async Task<Notification> CreateCustomNotification(NotificationPayload payload, NotificationScope scope, Guid? workshopId, Employee recipient, params Object[] related)
        {
            var notification = new Notification
            {
                CreatedOn = DateTime.UtcNow,
                ReadOn = null,
                ResolvedOn = null,
                Scope = scope,
                WorkshopId = workshopId,
                RecipientId = recipient?.Id,
                Type = payload.Type,
                Payload = payload.Save(),
                Status = NotificationStatus.Unread,
                RelatedEntities = new List<NotificationRelatedEntity>(),
            };

            foreach (var relatedEntity in related)
            {
                switch (relatedEntity)
                {
                    case Client client:
                        notification.RelatedEntities.Add(new NotificationRelatedEntity
                        {
                            EntityType = NotificationRelatedEntityType.Client,
                            EntityId = client.Id,
                        });
                        break;
                    case Job job:
                        notification.RelatedEntities.Add(new NotificationRelatedEntity
                        {
                            EntityType = NotificationRelatedEntityType.Job,
                            EntityId = job.Id,
                        });
                        break;
                    case Handpiece handpiece:
                        notification.RelatedEntities.Add(new NotificationRelatedEntity
                        {
                            EntityType = NotificationRelatedEntityType.Handpiece,
                            EntityId = handpiece.Id,
                        });
                        break;
                    case HandpieceStoreOrder handpieceStoreOrder:
                        notification.RelatedEntities.Add(new NotificationRelatedEntity
                        {
                            EntityType = NotificationRelatedEntityType.HandpieceStoreOrder,
                            EntityId = handpieceStoreOrder.Id,
                        });
                        break;
                    default:
                        throw new InvalidOperationException($"Related entity of type {relatedEntity?.GetType()} is not supported");
                }
            }

            using (var repository = this.repositoryFactory.CreateRepository())
            {
                await repository.InsertAsync(notification);
                await repository.SaveChangesAsync();
            }

            await this.SendNotificationToClients(notification, payload);
            return notification;
        }

        public async Task<NotificationVisibilityOptions> GetVisibilityOptions()
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            var access = await this.userResolver.GetEmployeeAccessAsync();

            var options = new NotificationVisibilityOptions();
            options.Employee = user as Employee;
            options.GlobalScope = NotificationScope.None;
            if (access.Global.CanReadComponent(GlobalComponent.HandpiecesOrder))
            {
                options.GlobalScope |= NotificationScope.HandpieceStoreOrder;
            }

            if (access.Clients.CanReadComponent(ClientEntityComponent.Callback))
            {
                options.GlobalScope |= NotificationScope.ClientCallback;
            }

            options.WorkshopScopes = new List<NotificationWorkshopVisibilityOptions>();
            var workshops = await this.repository.Query<Workshop>().ToListAsync();
            foreach (var workshop in workshops)
            {
                var workshopScope = access.Workshops[workshop.Id].GetNotificationScope();
                if (workshopScope != NotificationScope.None)
                {
                    options.WorkshopScopes.Add(new NotificationWorkshopVisibilityOptions
                    {
                        Workshop = workshop,
                        Scope = workshopScope,
                    });
                }
            }

            return options;
        }

        public Task<IQueryable<Notification>> PrepareQuery()
        {
            return this.PrepareQuery(null);
        }

        private async Task<IQueryable<Notification>> PrepareQuery([CanBeNull] IRepository repositoryOverride)
        {
            var queryRepository = repositoryOverride ?? this.repository;
            var query = queryRepository.Query<Notification>();

            var options = await this.GetVisibilityOptions();

            var conditions = new List<Expression<Func<Notification, Boolean>>>();
            if (options.GlobalScope != NotificationScope.None)
            {
                conditions.Add(x => x.WorkshopId == null && (x.Scope & options.GlobalScope) != NotificationScope.None);
            }

            foreach (var workshopOptions in options.WorkshopScopes.Where(x => x.Scope != NotificationScope.None))
            {
                conditions.Add(x => x.WorkshopId == workshopOptions.Workshop.Id && (x.Scope & workshopOptions.Scope) != NotificationScope.None);
            }

            query = query.WhereOneOf(conditions);
            query = query.Where(x => x.RecipientId == null || x.RecipientId == options.Employee.Id);

            return query;
        }

        private async Task SendNotificationToClients(Notification notification, NotificationPayload payload)
        {
            var connectionIds = this.connectionsTracker.GetMatchingConnections(notification.Scope, notification.WorkshopId, notification.RecipientId);
            await this.notificationsHub.Clients.Clients(connectionIds).SendAsync("ReceiveUpdate");
        }
    }
}
