using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    [PermissionsManager("Type", "/Domain/HandpieceDirectory/ModelSchematic")]
    [PermissionsManager("Entity", "/Domain/HandpieceDirectory/ModelSchematic/{entity}")]
    public class HandpieceModelSchematicsController : HandpieceModelSchematicsControllerBase
    {
        public HandpieceModelSchematicsController(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, IStorageHub storageHub)
            : base(controllerServices, imageUploadService, storageHub)
        {
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateTextHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateTextHandler.Overrides.BeforeEntityCreated = this.BeforeEntityCreated;
            this.CreateTextHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.CreateAttachmentHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateAttachmentHandler.Overrides.BeforeEntityCreated = this.BeforeEntityCreated;
            this.CreateAttachmentHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.CreateImageHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateImageHandler.Overrides.BeforeEntityCreated = this.BeforeEntityCreated;
            this.CreateImageHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.EditTextHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditTextHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditTextHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.EditAttachmentHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditAttachmentHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditAttachmentHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.EditImageHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditImageHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditImageHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;

            this.MoveUpHandler.Overrides.ExecuteOperation = this.ExecuteMoveUpOperation;
            this.MoveUpHandler.Overrides.GetOperationSuccessResult = this.GetMoveOperationSuccessResult;

            this.MoveDownHandler.Overrides.ExecuteOperation = this.ExecuteMoveDownOperation;
            this.MoveDownHandler.Overrides.GetOperationSuccessResult = this.GetMoveOperationSuccessResult;
        }

        public async Task<IActionResult> DownloadAttachment(Guid id)
        {
            var entity = await this.Repository.Query<HandpieceModelSchematic>("Attachment").SingleOrDefaultAsync(x => x.Id == id && x.Type == HandpieceModelSchematicType.Attachment);
            if (entity == null)
            {
                return this.NotFound();
            }

            var container = this.StorageHub.GetContainer(entity.Attachment.Container);
            var path = entity.Attachment.GetContainerPath();

            return this.File(await container.GetFileContentAsync(path), "application/pdf", $"{entity.Attachment.OriginalName}.{entity.Attachment.Extension}");
        }

        private Task<IQueryable<HandpieceModelSchematic>> PrepareReadQuery(HandpieceModel parent)
        {
            IQueryable<HandpieceModelSchematic> query = this.Repository.Query<HandpieceModelSchematic>("Image", "Attachment")
                .Where(x => x.ModelId == parent.Id)
                .OrderBy(x => x.OrderNo);
            return Task.FromResult(query);
        }

        private Task<HandpieceModelSchematic> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<HandpieceModelSchematic>("Model", "Model.Brand", "Image", "Attachment")
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<HandpieceModelSchematicDetailsModel> ConvertToDetailsModel(HandpieceModelSchematic entity)
        {
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            return new HandpieceModelSchematicDetailsModel
            {
                Entity = entity,
                Access = employeeAccess,
            };
        }

        private HandpieceModelSchematicReadModel ConvertEntityToViewModel(HandpieceModel parent, HandpieceModelSchematic entity, String[] allowedProperties)
        {
            var result = new HandpieceModelSchematicReadModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Display = entity.Display,
                Type = entity.Type,
                ThumbnailUrl = "/img/hub-logo-40.png",
            };

            if (entity.Type == HandpieceModelSchematicType.Image && entity.ImageId.HasValue)
            {
                result.ThumbnailUrl = this.ImageUploadService.GetImageUrl(entity.Image, "40");
            }

            return result;
        }

        private Task InitializeCreateModel(HandpieceModel parent, HandpieceModelSchematicEditBaseModel model, Boolean initial)
        {
            model.Parent = parent;
            return Task.CompletedTask;
        }

        private async Task BeforeEntityCreated(HandpieceModel parent, HandpieceModelSchematic entity, HandpieceModelSchematicEditBaseModel model, Dictionary<String, Object> additionalData)
        {
            var latest = await this.Repository.Query<HandpieceModelSchematic>()
                .Where(x => x.ModelId == parent.Id)
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            entity.OrderNo = latest.Count == 0 ? 1 : latest[0].OrderNo + 1;
        }

        private Task<IActionResult> GetCreateSuccessResult(HandpieceModel parent, HandpieceModelSchematic entity, HandpieceModelSchematicEditBaseModel model, Dictionary<String, Object> additionalData)
        {
            var redirect = this.RedirectToAction("Edit", "HandpieceModels", new { id = parent.Id });
            switch (model)
            {
                case HandpieceModelSchematicEditTextModel _: return this.HybridFormResultAsync("HandpieceModelSchematicCreateText", redirect);
                case HandpieceModelSchematicEditAttachmentModel _: return this.HybridFormResultAsync("HandpieceModelSchematicCreateAttachment", redirect);
                case HandpieceModelSchematicEditImageModel _: return this.HybridFormResultAsync("HandpieceModelSchematicCreateImage", redirect);
                default: return Task.FromResult<IActionResult>(redirect);
            }
        }

        private Task InitializeEditModel(HandpieceModelSchematic entity, HandpieceModelSchematicEditBaseModel model, Boolean initial)
        {
            model.Original = entity;
            model.Parent = entity.Model;
            return Task.CompletedTask;
        }

        private Task<IActionResult> GetEditSuccessResult(HandpieceModelSchematic entity, HandpieceModelSchematicEditBaseModel model, Dictionary<String, Object> additionalData)
        {
            var redirect = this.RedirectToAction("Edit", "HandpieceModels", new { id = entity.ModelId });
            switch (model)
            {
                case HandpieceModelSchematicEditTextModel _: return this.HybridFormResultAsync("HandpieceModelSchematicEditText", redirect);
                case HandpieceModelSchematicEditAttachmentModel _: return this.HybridFormResultAsync("HandpieceModelSchematicEditAttachment", redirect);
                case HandpieceModelSchematicEditImageModel _: return this.HybridFormResultAsync("HandpieceModelSchematicEditImage", redirect);
                default: return Task.FromResult<IActionResult>(redirect);
            }
        }

        private Task<IActionResult> GetDeleteSuccessResult(HandpieceModelSchematic entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceModelSchematicDelete", this.RedirectToAction("Edit", "HandpieceModels", new { id = entity.ModelId }));
        }

        private async Task ExecuteMoveUpOperation(Guid id, HandpieceModelSchematic entity, Object model)
        {
            var previous = await this.Repository.Query<HandpieceModelSchematic>()
                .Where(x => x.ModelId == entity.ModelId && x.OrderNo < entity.OrderNo)
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            if (previous.Count > 0)
            {
                await this.SwapOrderNo(entity, previous[0]);
            }
        }

        private async Task ExecuteMoveDownOperation(Guid id, HandpieceModelSchematic entity, Object model)
        {
            var next = await this.Repository.Query<HandpieceModelSchematic>()
                .Where(x => x.ModelId == entity.ModelId && x.OrderNo > entity.OrderNo)
                .OrderBy(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            if (next.Count > 0)
            {
                await this.SwapOrderNo(entity, next[0]);
            }
        }

        private Task<IActionResult> GetMoveOperationSuccessResult(Guid id, HandpieceModelSchematic entity, Object model)
        {
            return Task.FromResult<IActionResult>(this.Json(new Object()));
        }
    }
}
