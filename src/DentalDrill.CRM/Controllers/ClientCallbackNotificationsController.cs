using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientCallbackNotificationsController : BaseTelerikIndexlessDependentBasicCrudController<
        Guid, ClientCallbackNotification,
        Guid, Client,
        ClientCallbackNotificationReadModel, ClientCallbackNotification, ClientCallbackNotificationEditModel, ClientCallbackNotificationEditModel, ClientCallbackNotification>
    {
        private readonly UserEntityResolver userEntityResolver;
        private readonly CallbackService callbackService;
        private readonly CalendarService calendarService;

        private Guid? previousAssignedTo;

        public ClientCallbackNotificationsController(IEntityControllerServices controllerServices, UserEntityResolver userEntityResolver, CallbackService callbackService, CalendarService calendarService)
            : base(controllerServices)
        {
            this.userEntityResolver = userEntityResolver;
            this.callbackService = callbackService;
            this.calendarService = calendarService;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.AfterEntityCreated = this.AfterEntityCreated;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.AfterEntityUpdated = this.AfterEntityUpdated;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
            this.DeleteHandler.Overrides.AfterEntityDeleted = this.AfterEntityDeleted;
        }

        private Task<IQueryable<ClientCallbackNotification>> PrepareReadQuery(Client parent)
        {
            var query = this.Repository.Query<ClientCallbackNotification>("CreatedBy", "AssignedTo")
                .Where(x => x.ClientId == parent.Id)
                .OrderByDescending(x => x.CreatedOn);
            return Task.FromResult<IQueryable<ClientCallbackNotification>>(query);
        }

        private ClientCallbackNotificationReadModel ConvertEntityToViewModel(Client parent, ClientCallbackNotification entity, String[] allowedProperties)
        {
            return new ClientCallbackNotificationReadModel
            {
                Id = entity.Id,
                CreatedOn = DateTime.SpecifyKind(entity.CreatedOn, DateTimeKind.Utc),
                CreatedByName = entity.CreatedBy.GetFullName(),
                AssignedToName = entity.AssignedTo?.GetFullName(),
                CallDateTime = entity.CallDateTime,
                Note = entity.Note,
            };
        }

        private Task<ClientCallbackNotification> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<ClientCallbackNotification>("Client", "CreatedBy", "AssignedTo").SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task InitializeCreateModel(Client parent, ClientCallbackNotificationEditModel model, Boolean initial)
        {
            model.Parent = parent;
            model.Employees = await this.Repository.Query<Employee>()
                .Where(x => (x.Role.ClientComponentRead & ClientEntityComponent.Callback) == ClientEntityComponent.Callback)
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();
        }

        private async Task InitializeEditModel(ClientCallbackNotification entity, ClientCallbackNotificationEditModel model, Boolean initial)
        {
            model.Original = entity;
            model.Parent = entity.Client;
            model.Employees = await this.Repository.Query<Employee>()
                .Where(x => (x.Role.ClientComponentRead & ClientEntityComponent.Callback) == ClientEntityComponent.Callback)
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();
        }

        private async Task InitializeNewEntity(Client parent, ClientCallbackNotification entity, ClientCallbackNotificationEditModel model)
        {
            var employee = (Employee)await this.userEntityResolver.ResolveCurrentUserEntity();
            entity.ClientId = parent.Id;
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedById = employee.Id;

            entity.AssignedToId = model.AssignedToId;
            entity.CallDateTime = model.CallDateTime;
            entity.Note = model.Note;

            entity.Status = ClientCallbackNotificationStatus.New;
            entity.CompletedOn = null;
        }

        private async Task AfterEntityCreated(Client parent, ClientCallbackNotification entity, ClientCallbackNotificationEditModel model, Dictionary<String, Object> additionalData)
        {
            if (entity.AssignedToId.HasValue)
            {
                await this.callbackService.NotifyAssignedEmployee(entity.AssignedToId.Value);
            }
        }

        private Task UpdateExistingEntity(ClientCallbackNotification entity, ClientCallbackNotificationEditModel model)
        {
            this.previousAssignedTo = entity.AssignedToId;

            entity.AssignedToId = model.AssignedToId;
            entity.CallDateTime = model.CallDateTime;
            entity.Note = model.Note;

            var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.calendarService.TimeZone);
            if (entity.Status == ClientCallbackNotificationStatus.Active && (entity.CallDateTime == null || entity.CallDateTime.Value > date))
            {
                entity.Status = ClientCallbackNotificationStatus.New;
            }

            return Task.CompletedTask;
        }

        private async Task AfterEntityUpdated(ClientCallbackNotification entity, ClientCallbackNotificationEditModel model, Dictionary<String, Object> additionalData)
        {
            if (this.previousAssignedTo != entity.AssignedToId)
            {
                if (this.previousAssignedTo.HasValue)
                {
                    await this.callbackService.NotifyAssignedEmployee(this.previousAssignedTo.Value);
                }

                if (entity.AssignedToId.HasValue)
                {
                    await this.callbackService.NotifyAssignedEmployee(entity.AssignedToId.Value);
                }
            }
        }

        private async Task AfterEntityDeleted(ClientCallbackNotification entity, Dictionary<String, Object> additionalData)
        {
            if (entity.AssignedToId.HasValue)
            {
                await this.callbackService.NotifyAssignedEmployee(entity.AssignedToId.Value);
            }
        }

        private Task<IActionResult> GetCreateSuccessResult(Client parent, ClientCallbackNotification entity, ClientCallbackNotificationEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("CreateCallbackNotification", this.RedirectToAction("Details", "Clients", new { id = parent.Id, Tab = "Callback" }));
        }

        private Task<IActionResult> GetEditSuccessResult(ClientCallbackNotification entity, ClientCallbackNotificationEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("EditCallbackNotification", this.RedirectToAction("Details", "Clients", new { id = entity.ClientId, Tab = "Callback" }));
        }

        private Task<IActionResult> GetDeleteSuccessResult(ClientCallbackNotification entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("DeleteCallbackNotification", this.RedirectToAction("Details", "Clients", new { id = entity.ClientId, Tab = "Callback" }));
        }
    }
}
