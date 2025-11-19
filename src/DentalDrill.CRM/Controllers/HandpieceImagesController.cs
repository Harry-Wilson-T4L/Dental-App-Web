using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class HandpieceImagesController : BaseTelerikFullInlineIndexlessDependentCrudController<Guid, HandpieceImage, Guid, Handpiece, HandpieceImageViewModel>
    {
        private readonly IImageUploadService imageUploadService;
        private readonly IStorageHub storageHub;

        public HandpieceImagesController(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, IStorageHub storageHub)
            : base(controllerServices)
        {
            this.imageUploadService = imageUploadService;
            this.storageHub = storageHub;

            var permissionsValidator = new DefaultDependentEntityPermissionsValidator<HandpieceImage, Handpiece>(
                permissionsHub: this.ControllerServices.PermissionsHub,
                typeManagerPath: PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "Type"),
                entityManagerPath: PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "Entity"),
                propertyManagerPath: PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "Property"),
                parentEntityManagerPath: PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "ParentEntity"));

            this.IndexHandler = new TelerikCrudDependentIndexActionHandler<Guid, HandpieceImage, Guid, Handpiece, RepairDetailsViewModel>(this, controllerServices, permissionsValidator);
            this.PageCreateHandler = new BasicCrudDependentCreateActionHandler<Guid, HandpieceImage, Guid, Handpiece, HandpieceImageEditViewModel>(this, controllerServices, permissionsValidator);
            this.CreateVideoHandler = new BasicCrudDependentCreateActionHandler<Guid, HandpieceImage, Guid, Handpiece, HandpieceImageEditVideoViewModel>(this, controllerServices, permissionsValidator);
            this.BatchCreateHandler = new BasicCrudCustomOperationActionHandler<Guid, Handpiece, HandpieceImageBatchCreateViewModel>(this, this.ControllerServices, new DefaultEntityPermissionsValidator<Handpiece>(this.ControllerServices.PermissionsHub, null, null, null));
            this.PreviewHandler = new BasicCrudDetailsActionHandler<Guid, Handpiece, HandpieceImagePreviewModel>(this, controllerServices, new DefaultEntityPermissionsValidator<Handpiece>(controllerServices.PermissionsHub, null, null, null));

            this.IndexHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.PageCreateHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.PageCreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.PageCreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.PageCreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.CreateVideoHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.CreateVideoHandler.Overrides.InitializeCreateModel = this.InitializeCreateVideoModel;
            this.CreateVideoHandler.Overrides.InitializeNewEntity = this.InitializeNewVideoEntity;
            this.CreateVideoHandler.Overrides.GetCreateSuccessResult = this.GetCreateVideoSuccessResult;

            this.UpdateHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.UpdateHandler.Overrides.ConvertEntityToViewModel = this.ConvertUpdatedEntityToViewModel;

            this.BatchCreateHandler.Overrides.QuerySingleEntity = this.QuerySingleParentEntity;
            this.BatchCreateHandler.Overrides.InitializeOperationModel = this.InitializeBatchCreateModel;
            this.BatchCreateHandler.Overrides.ExecuteOperation = this.ExecuteBatchCreateOperation;
            this.BatchCreateHandler.Overrides.GetOperationSuccessResult = this.GetBatchCreateSuccessResult;

            this.PreviewHandler.Overrides.QuerySingleEntity = this.QuerySingleParentEntityForPreview;
            this.PreviewHandler.Overrides.ConvertToDetailsModel = this.ConvertToPreviewModel;
        }

        protected TelerikCrudDependentIndexActionHandler<Guid, HandpieceImage, Guid, Handpiece, RepairDetailsViewModel> IndexHandler { get; }

        protected BasicCrudDependentCreateActionHandler<Guid, HandpieceImage, Guid, Handpiece, HandpieceImageEditViewModel> PageCreateHandler { get; }

        protected BasicCrudDependentCreateActionHandler<Guid, HandpieceImage, Guid, Handpiece, HandpieceImageEditVideoViewModel> CreateVideoHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Handpiece, HandpieceImageBatchCreateViewModel> BatchCreateHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, Handpiece, HandpieceImagePreviewModel> PreviewHandler { get; }

        public Task<IActionResult> Index(Guid parentId) => this.IndexHandler.Index(parentId);

        public Task<IActionResult> Create(Guid parentId) => this.PageCreateHandler.Create(parentId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Create(Guid parentId, HandpieceImageEditViewModel model) => this.PageCreateHandler.Create(parentId, model);

        [NonAction]
        public override Task<IActionResult> Create(Guid parentId, DataSourceRequest request, HandpieceImageViewModel model)
        {
            throw new NotSupportedException();
        }

        public Task<IActionResult> CreateVideo(Guid parentId) => this.CreateVideoHandler.Create(parentId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateVideo(Guid parentId, HandpieceImageEditVideoViewModel model) => this.CreateVideoHandler.Create(parentId, model);

        public Task<IActionResult> BatchCreate(Guid parentId) => this.BatchCreateHandler.Execute(parentId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> BatchCreate(Guid parentId, HandpieceImageBatchCreateViewModel model) => this.BatchCreateHandler.Execute(parentId, model);

        public Task<IActionResult> Preview(Guid parentId) => this.PreviewHandler.Details(parentId);

        private Task<IQueryable<HandpieceImage>> PrepareReadQuery(Handpiece parent)
        {
            var query = this.Repository.Query<HandpieceImage>("Image", "Video").Where(x => x.HandpieceId == parent.Id);
            return Task.FromResult(query);
        }

        private Task<HandpieceImage> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<HandpieceImage>("Image", "Video").SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<HandpieceImageViewModel> ConvertUpdatedEntityToViewModel(HandpieceImage entity)
        {
            var image = await this.Repository.Query<UploadedImage>().SingleOrDefaultAsync(x => x.Id == entity.ImageId);
            var video = await this.Repository.Query<UploadedFile>().SingleOrDefaultAsync(x => x.Id == entity.VideoId);

            return new HandpieceImageViewModel()
            {
                Id = entity.Id,
                HandpieceId = entity.HandpieceId,
                Date = entity.Date,
                Description = entity.Description,
                Display = entity.Display,
                ImageUrl = video != null ? "/img/video-placeholder.png" : this.imageUploadService.GetImageUrl(image, "300"),
                VideoUrl = video != null ? this.storageHub.GetContainer(video.Container).GetFileUrl(video.GetContainerPath()) : null,
            };
        }

        private Task InitializeIndexViewModel(RepairDetailsViewModel model, Handpiece entity)
        {
            model.Id = entity.Id;

            return Task.CompletedTask;
        }

        private Task UpdateExistingEntity(HandpieceImage entity, HandpieceImageViewModel model)
        {
            entity.Display = model.Display;
            entity.Description = model.Description;

            return Task.CompletedTask;
        }

        private HandpieceImageViewModel ConvertEntityToViewModel(Handpiece parent, HandpieceImage entity, String[] data)
        {
            return new HandpieceImageViewModel
            {
                Id = entity.Id,
                HandpieceId = entity.HandpieceId,
                Date = entity.Date,
                Description = entity.Description,
                Display = entity.Display,
                ImageUrl = entity.Video != null ? "/img/video-placeholder.png" : this.imageUploadService.GetImageUrl(entity.Image, "300"),
                VideoUrl = entity.Video != null ? this.storageHub.GetContainer(entity.Video.Container).GetFileUrl(entity.Video.GetContainerPath()) : null,
            };
        }

        private async Task<Handpiece> QuerySingleParentEntity(Guid id)
        {
            return await this.Repository.Query<Handpiece>().SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task InitializeCreateModel(Handpiece parent, HandpieceImageEditViewModel viewModel, Boolean initial)
        {
            viewModel.ParentId = parent.Id;

            return Task.CompletedTask;
        }

        private Task InitializeNewEntity(Handpiece parent, HandpieceImage entity, HandpieceImageEditViewModel viewModel)
        {
            if (!viewModel.ImageId.HasValue)
            {
                throw new InvalidOperationException("Image is missing");
            }

            entity.HandpieceId = parent.Id;
            entity.ImageId = viewModel.ImageId.Value;
            entity.VideoId = null;
            entity.Date = DateTime.UtcNow;
            entity.Display = viewModel.Display;
            entity.Description = viewModel.Description;

            return Task.CompletedTask;
        }

        private Task<IActionResult> GetCreateSuccessResult(Handpiece parent, HandpieceImage entity, HandpieceImageEditViewModel viewModel, Dictionary<String, Object> data)
        {
            return this.HybridFormResultAsync("CreateImage", this.RedirectToAction("Edit", "Handpieces", new { id = parent.Id }));
        }

        private Task InitializeCreateVideoModel(Handpiece parent, HandpieceImageEditVideoViewModel model, Boolean initial)
        {
            model.ParentId = parent.Id;
            return Task.CompletedTask;
        }

        private Task InitializeNewVideoEntity(Handpiece parent, HandpieceImage entity, HandpieceImageEditVideoViewModel model)
        {
            if (!model.VideoId.HasValue)
            {
                throw new InvalidOperationException("Video is missing");
            }

            entity.HandpieceId = parent.Id;
            entity.ImageId = null;
            entity.VideoId = model.VideoId.Value;
            entity.Date = DateTime.UtcNow;
            entity.Display = model.Display;
            entity.Description = model.Description;

            return Task.CompletedTask;
        }

        private Task<IActionResult> GetCreateVideoSuccessResult(Handpiece parent, HandpieceImage entity, HandpieceImageEditVideoViewModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("CreateVideo", this.RedirectToAction("Edit", "Handpieces", new { id = parent.Id }));
        }

        private Task InitializeBatchCreateModel(Guid parentId, Handpiece parent, HandpieceImageBatchCreateViewModel model, Boolean initial)
        {
            model.Parent = parent;
            return Task.CompletedTask;
        }

        private async Task ExecuteBatchCreateOperation(Guid parentId, Handpiece parent, HandpieceImageBatchCreateViewModel model)
        {
            foreach (var image in model.Images)
            {
                var entity = new HandpieceImage
                {
                    HandpieceId = parent.Id,
                    ImageId = image,
                    Date = DateTime.UtcNow,
                    Display = model.Display,
                    Description = String.Empty,
                };

                await this.Repository.InsertAsync(entity);
            }

            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetBatchCreateSuccessResult(Guid parentId, Handpiece parent, HandpieceImageBatchCreateViewModel model)
        {
            return this.HybridFormResultAsync("HandpieceImageBatchCreate", this.RedirectToAction("Edit", "Handpieces", new { id = parent.Id }));
        }

        private Task<Handpiece> QuerySingleParentEntityForPreview(Guid id)
        {
            return this.Repository.QueryWithoutTracking<Handpiece>()
                .Include(x => x.Images)
                .ThenInclude(x => x.Image)
                .Include(x => x.Images)
                .ThenInclude(x => x.Video)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<HandpieceImagePreviewModel> ConvertToPreviewModel(Handpiece entity)
        {
            var model = new HandpieceImagePreviewModel
            {
                Images = entity.Images
                    .OrderBy(x => x.Date)
                    .Select(x => new HandpieceImageViewModel
                    {
                        Id = x.Id,
                        HandpieceId = x.HandpieceId,
                        Date = x.Date,
                        Description = x.Description,
                        Display = x.Display,
                        ImageUrl = this.imageUploadService.GetImageUrl(x.Image, "2560"),
                        VideoUrl = x.Video != null ? this.storageHub.GetContainer(x.Video.Container).GetFileUrl(x.Video.GetContainerPath()) : null,
                    })
                    .ToList(),
                SelectedImage = this.Request.Query.TryGetValue("selected", out var selectedValue) && selectedValue.Count > 0 && Guid.TryParse(selectedValue[0], out var selected)
                    ? (Guid?)selected
                    : null,
            };

            return Task.FromResult(model);
        }
    }
}