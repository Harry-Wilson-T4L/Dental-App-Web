using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using DevGuild.AspNetCore.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientAttachmentsController : BaseTelerikIndexlessDependentBasicCrudController<Guid, ClientAttachment, Guid, Client, ClientAttachmentReadModel, ClientAttachment, ClientAttachmentCreateModel, ClientAttachmentEditModel, ClientAttachment>
    {
        private readonly IStorageHub storageHub;
        private readonly UserEntityResolver userEntityResolver;

        public ClientAttachmentsController(IEntityControllerServices controllerServices, IStorageHub storageHub, UserEntityResolver userEntityResolver)
            : base(controllerServices)
        {
            this.storageHub = storageHub;
            this.userEntityResolver = userEntityResolver;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        [NonAction]
        public override Task<IActionResult> Details(Guid id)
        {
            throw new NotSupportedException();
        }

        public async Task<IActionResult> Download(Guid id)
        {
            var entity = await this.Repository.Query<ClientAttachment>("File").SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            var container = this.storageHub.GetContainer(entity.File.Container);
            var path = entity.File.GetContainerPath();

            return this.File(await container.GetFileContentAsync(path), "application/octet-stream", $"{entity.File.OriginalName}.{entity.File.Extension}");
        }

        private Task<IQueryable<ClientAttachment>> PrepareReadQuery(Client parent)
        {
            var query = this.Repository.Query<ClientAttachment>("File", "Employee").Where(x => x.ClientId == parent.Id);
            return Task.FromResult(query);
        }

        private ClientAttachmentReadModel ConvertEntityToViewModel(Client parent, ClientAttachment entity, String[] allowedProperties)
        {
            return new ClientAttachmentReadModel
            {
                Id = entity.Id,
                FileName = entity.File.OriginalName + "." + entity.File.Extension,
                UploadedOn = entity.UploadedOn,
                Comment = entity.Comment,
                EmployeeId = entity.EmployeeId,
                EmployeeName = entity.Employee.FirstName + " " + entity.Employee.LastName,
            };
        }

        private Task<ClientAttachment> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<ClientAttachment>("File", "Employee").SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task InitializeCreateModel(Client parent, ClientAttachmentCreateModel model, Boolean initial)
        {
            model.Parent = parent;
            return Task.CompletedTask;
        }

        private async Task InitializeNewEntity(Client parent, ClientAttachment entity, ClientAttachmentCreateModel model)
        {
            var user = await this.userEntityResolver.ResolveCurrentUserEntity();
            switch (user)
            {
                case Employee employee:
                    entity.EmployeeId = employee.Id;
                    break;
                default:
                    throw new InvalidOperationException("Invalid current user");
            }

            entity.FileId = model.FileId ?? throw new InvalidOperationException();
            entity.Comment = model.Comment;
            entity.UploadedOn = DateTime.UtcNow;
        }

        private Task<IActionResult> GetCreateSuccessResult(Client parent, ClientAttachment entity, ClientAttachmentCreateModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("ClientAttachmentsCreate", this.RedirectToAction("Details", "Clients", new { id = entity.ClientId, Tab = "Attachments" }));
        }

        private Task<IActionResult> GetEditSuccessResult(ClientAttachment entity, ClientAttachmentEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("ClientAttachmentsEdit", this.RedirectToAction("Details", "Clients", new { id = entity.ClientId, Tab = "Attachments" }));
        }

        private Task<IActionResult> GetDeleteSuccessResult(ClientAttachment entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("ClientAttachmentsDelete", this.RedirectToAction("Details", "Clients", new { id = entity.ClientId, Tab = "Attachments" }));
        }
    }
}
