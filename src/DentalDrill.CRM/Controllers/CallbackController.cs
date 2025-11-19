using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
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

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class CallbackController : Controller
    {
        private readonly IEntityControllerServices controllerServices;
        private readonly UserEntityResolver userEntityResolver;
        private readonly CallbackService callbackService;

        public CallbackController(IEntityControllerServices controllerServices, UserEntityResolver userEntityResolver, CallbackService callbackService)
        {
            this.controllerServices = controllerServices;
            this.userEntityResolver = userEntityResolver;
            this.callbackService = callbackService;
            var permissionsValidator = new DefaultEntityPermissionsValidator<ClientCallbackNotification>(controllerServices.PermissionsHub, null, null, null);

            this.IndexHandler = new TelerikCrudIndexActionHandler<Guid, ClientCallbackNotification, EmptyIndexViewModel>(this, controllerServices, permissionsValidator);
            this.ReadHandler = new TelerikCrudAjaxReadActionHandler<Guid, ClientCallbackNotification, CallbackReadModel>(this, controllerServices, permissionsValidator);
            this.MarkAsReadHandler = new BasicCrudCustomOperationActionHandler<Guid, ClientCallbackNotification, Object>(this, controllerServices, permissionsValidator);

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.MarkAsReadHandler.Overrides.QuerySingleEntity = this.QuerySingleEntityForMarkAsRead;
            this.MarkAsReadHandler.Overrides.ExecuteOperation = this.ExecuteMarkAsReadOperation;
            this.MarkAsReadHandler.Overrides.GetOperationSuccessResult = this.GetMarkAsReadOperationSuccessResult;
        }

        protected IRepository Repository => this.controllerServices.Repository;

        protected TelerikCrudIndexActionHandler<Guid, ClientCallbackNotification, EmptyIndexViewModel> IndexHandler { get; }

        protected TelerikCrudAjaxReadActionHandler<Guid, ClientCallbackNotification, CallbackReadModel> ReadHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, ClientCallbackNotification, Object> MarkAsReadHandler { get; }

        public Task<IActionResult> Index() => this.IndexHandler.Index();

        [AjaxPost]
        public Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request) => this.ReadHandler.Read(request);

        [HttpPost]
        public Task<IActionResult> MarkAsRead(Guid id) => this.MarkAsReadHandler.Execute(id, new Object());

        [HttpPost]
        public async Task<IActionResult> Total()
        {
            var query = await this.PrepareReadQuery();
            var totals = await query.GroupBy(x => x.Status).Select(x => new { Status = x.Key, Count = x.Count() }).ToListAsync();

            var totalNew = totals.SingleOrDefault(x => x.Status == ClientCallbackNotificationStatus.Active)?.Count ?? 0;
            var totalDone = totals.SingleOrDefault(x => x.Status == ClientCallbackNotificationStatus.Done)?.Count ?? 0;

            return this.Json(new
            {
                @new = totalNew,
                done = totalDone,
                total = totalNew + totalDone,
            });
        }

        private async Task<IQueryable<ClientCallbackNotification>> PrepareReadQuery()
        {
            var employee = (Employee)await this.userEntityResolver.ResolveCurrentUserEntity();
            var minDate = new DateTime(1970, 1, 1);
            var query = this.Repository.Query<ClientCallbackNotification>("Client")
                .Where(x => x.AssignedToId == employee.Id)
                .Where(x => x.Status == ClientCallbackNotificationStatus.Active)
                .OrderBy(x => x.CallDateTime);

            return query;
        }

        private CallbackReadModel ConvertEntityToViewModel(ClientCallbackNotification entity, String[] allowedProperties)
        {
            return new CallbackReadModel
            {
                Id = entity.Id,
                ClientId = entity.Client.Id,
                ClientName = entity.Client.Name,
                CallDateTime = entity.CallDateTime,
                Note = entity.Note,
            };
        }

        private async Task<ClientCallbackNotification> QuerySingleEntityForMarkAsRead(Guid id)
        {
            var employee = (Employee)await this.userEntityResolver.ResolveCurrentUserEntity();
            return await this.Repository.Query<ClientCallbackNotification>("Client", "AssignedTo", "CreatedBy")
                .Where(x => x.AssignedToId == employee.Id && x.Status != ClientCallbackNotificationStatus.Done)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task ExecuteMarkAsReadOperation(Guid id, ClientCallbackNotification entity, Object model)
        {
            entity.Status = ClientCallbackNotificationStatus.Done;
            entity.CompletedOn = DateTime.UtcNow;

            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();

            await this.callbackService.NotifyAssignedEmployee(entity.AssignedToId ?? throw new InvalidOperationException());
        }

        private Task<IActionResult> GetMarkAsReadOperationSuccessResult(Guid id, ClientCallbackNotification entity, Object model)
        {
            return Task.FromResult<IActionResult>(this.Json(new { }));
        }
    }
}
