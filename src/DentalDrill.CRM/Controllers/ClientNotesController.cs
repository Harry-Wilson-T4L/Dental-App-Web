using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    [PermissionsManager("Type", "/Domain/Internal/ClientNote")]
    [PermissionsManager("Entity", "/Domain/Internal/ClientNote/{entity}")]
    public class ClientNotesController : BaseTelerikIndexlessDependentBasicCrudController<Guid, ClientNote, Guid, Client, ClientNoteViewModel, ClientNote, ClientNoteEditViewModel, ClientNoteEditViewModel, ClientNote>
    {
        private readonly UserEntityResolver userEntityResolver;

        public ClientNotesController(IEntityControllerServices controllerServices, UserEntityResolver userEntityResolver)
            : base(controllerServices)
        {
            this.userEntityResolver = userEntityResolver;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        private Task<IQueryable<ClientNote>> PrepareReadQuery(Client parent)
        {
            var query = this.Repository.Query<ClientNote>("Creator").OrderByDescending(x => x.Created).Where(x => x.ClientId == parent.Id);
            return Task.FromResult(query);
        }

        private ClientNoteViewModel ConvertEntityToViewModel(Client parent, ClientNote entity, String[] fields)
        {
            return new ClientNoteViewModel()
            {
                Id = entity.Id,
                Created = entity.Created,
                CreatorName = $"{entity.Creator.FirstName} {entity.Creator.LastName}",
                Text = entity.Text,
            };
        }

        private Task<ClientNote> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<ClientNote>("Client", "Creator").SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task InitializeCreateModel(Client parent, ClientNoteEditViewModel model, Boolean initial)
        {
            model.Parent = parent;
            return Task.CompletedTask;
        }

        private async Task InitializeNewEntity(Client parent, ClientNote entity, ClientNoteEditViewModel viewModel)
        {
            var user = await this.userEntityResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                throw new InvalidOperationException();
            }

            entity.CreatorId = employee.Id;
            entity.Created = DateTime.UtcNow;
            entity.Text = viewModel.Text;
        }

        private Task UpdateExistingEntity(ClientNote entity, ClientNoteEditViewModel model)
        {
            entity.Text = model.Text;
            return Task.CompletedTask;
        }

        private Task<IActionResult> GetCreateSuccessResult(Client parent, ClientNote entity, ClientNoteEditViewModel viewModel, Dictionary<String, Object> data)
        {
            return this.HybridFormResultAsync("CreateNote", this.RedirectToAction("Details", "Clients", new { id = parent.Id }));
        }

        private Task<IActionResult> GetEditSuccessResult(ClientNote entity, ClientNoteEditViewModel viewModel, Dictionary<String, Object> data)
        {
            return this.HybridFormResultAsync("EditNote", this.RedirectToAction("Details", "Clients", new { id = entity.ClientId }));
        }

        private Task<IActionResult> GetDeleteSuccessResult(ClientNote entity, Dictionary<String, Object> data)
        {
            return this.HybridFormResultAsync("DeleteNote", this.RedirectToAction("Details", "Clients", new { id = entity.ClientId }));
        }
    }
}