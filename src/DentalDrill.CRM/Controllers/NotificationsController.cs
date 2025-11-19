using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Notifications;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notification = DentalDrill.CRM.Models.Notification;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class NotificationsController : Controller
    {
        private readonly IEntityControllerServices controllerServices;
        private readonly NotificationsService notificationsService;

        private Boolean showRead;

        public NotificationsController(IEntityControllerServices controllerServices, NotificationsService notificationsService)
        {
            this.controllerServices = controllerServices;
            this.notificationsService = notificationsService;
            var permissionsValidator = new DefaultEntityPermissionsValidator<Notification>(controllerServices.PermissionsHub, null, null, null);

            this.IndexHandler = new TelerikCrudIndexActionHandler<Guid, Notification, EmptyIndexViewModel>(this, controllerServices, permissionsValidator);
            this.ReadHandler = new TelerikCrudAjaxReadActionHandler<Guid, Notification, NotificationReadModel>(this, controllerServices, permissionsValidator);

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;
        }

        protected IRepository Repository => this.controllerServices.Repository;

        protected TelerikCrudIndexActionHandler<Guid, Notification, EmptyIndexViewModel> IndexHandler { get; }

        protected TelerikCrudAjaxReadActionHandler<Guid, Notification, NotificationReadModel> ReadHandler { get; }

        public Task<IActionResult> Index() => this.IndexHandler.Index();

        public Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, Boolean? showRead)
        {
            this.showRead = showRead ?? false;
            return this.ReadHandler.Read(request);
        }

        [AjaxPost]
        public async Task<IActionResult> Total()
        {
            var query = await this.notificationsService.PrepareQuery();
            var totals = await query.GroupBy(x => x.Status).Select(x => new { Status = x.Key, Count = x.Count() }).ToListAsync();

            var totalUnread = totals.SingleOrDefault(x => x.Status == NotificationStatus.Unread)?.Count ?? 0;
            var totalRead = totals.SingleOrDefault(x => x.Status == NotificationStatus.Read)?.Count ?? 0;
            var totalResolved = totals.SingleOrDefault(x => x.Status == NotificationStatus.Resolved)?.Count ?? 0;

            return this.Json(new
            {
                unread = totalUnread,
                read = totalRead,
                resolved = totalResolved,
                all = totalUnread + totalRead + totalResolved,
            });
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> Upgrade()
        {
            var notifications = await this.Repository.Query<Notification>()
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            var jobs = await this.Repository.Query<Job>("Client")
                .ToListAsync();
            var jobsMap = jobs.ToDictionary(x => x.Id);

            foreach (var notification in notifications)
            {
                var payload = JObject.Parse(notification.Payload);
                var payloadChanged = false;
                switch (notification.Type)
                {
                    case NotificationType.JobCreated:
                    case NotificationType.JobEstimated:
                    case NotificationType.JobApprovedForRepair:
                    case NotificationType.JobRepairComplete:
                    {
                        var jobId = Guid.Parse(payload["JobId"].Value<String>());
                        if (jobsMap.TryGetValue(jobId, out var job))
                        {
                            if (!payload.ContainsKey("SurgeryId"))
                            {
                                payload.Add("SurgeryId", job.ClientId.ToString());
                                payloadChanged = true;
                            }

                            if (!payload.ContainsKey("SurgeryName"))
                            {
                                payload.Add("SurgeryName", job.Client.Name);
                                payloadChanged = true;
                            }
                        }

                        break;
                    }
                }

                if (payloadChanged)
                {
                    notification.Payload = payload.ToString(Formatting.None);
                    await this.Repository.UpdateAsync(notification);
                }
            }

            await this.Repository.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        private async Task<IQueryable<Notification>> PrepareReadQuery()
        {
            var query = await this.notificationsService.PrepareQuery();

            if (this.showRead)
            {
                query = query.Where(x => x.Status != NotificationStatus.Resolved);
            }
            else
            {
                query = query.Where(x => x.Status == NotificationStatus.Unread);
            }

            return query;
        }

        private NotificationReadModel ConvertEntityToViewModel(Notification entity, String[] allowedProperties)
        {
            return new NotificationReadModel
            {
                Id = entity.Id,
                CreatedOn = entity.CreatedOn,
                ReadOn = entity.ReadOn,
                ResolvedOn = entity.ResolvedOn,
                Type = entity.Type,
                Payload = NotificationPayload.LoadFrom(entity.Type, entity.Payload),
                Status = entity.Status,
            };
        }
    }
}
