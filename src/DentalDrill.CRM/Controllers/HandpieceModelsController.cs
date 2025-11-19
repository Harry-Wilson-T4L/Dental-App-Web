using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using DevGuild.AspNetCore.Services.Uploads.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    [PermissionsManager("Type", "/Domain/HandpieceDirectory/Model")]
    [PermissionsManager("Entity", "/Domain/HandpieceDirectory/Model/{entity}")]
    public class HandpieceModelsController : BaseTelerikIndexlessDependentBasicCrudController<Guid, HandpieceModel, Guid, HandpieceBrand, HandpieceModelReadModel, HandpieceModelEditModel, HandpieceModelCreateModel, HandpieceModelEditModel, HandpieceModel>
    {
        private readonly IImageUploadService imageUploadService;
        private readonly INameNormalizationService nameNormalizationService;

        public HandpieceModelsController(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, INameNormalizationService nameNormalizationService)
            : base(controllerServices)
        {
            this.imageUploadService = imageUploadService;
            this.nameNormalizationService = nameNormalizationService;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.ValidateCreateModel = this.ValidateCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.ValidateEditModel = this.ValidateEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditViewResult = this.GetEditViewResult;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        private Task<IQueryable<HandpieceModel>> PrepareReadQuery(HandpieceBrand parent)
        {
            IQueryable<HandpieceModel> query = this.Repository.Query<HandpieceModel>("Image")
                .Where(x => x.BrandId == parent.Id)
                .OrderBy(x => x.Name);

            return Task.FromResult(query);
        }

        private Task<HandpieceModel> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<HandpieceModel>("Brand", "Image").SingleOrDefaultAsync(x => x.Id == id);
        }

        private HandpieceModelReadModel ConvertEntityToViewModel(HandpieceBrand parent, HandpieceModel entity, String[] allowedProperties)
        {
            return new HandpieceModelReadModel
            {
                Id = entity.Id,
                BrandId = entity.BrandId,
                BrandName = entity.Brand.Name,
                Name = entity.Name,
                Description = entity.Description,
                ImageId = entity.ImageId,
                ImageUrl = entity.ImageId.HasValue ? this.imageUploadService.GetImageUrl(entity.Image, "40") : "/img/hub-logo-40.png",
            };
        }

        private Task InitializeCreateModel(HandpieceBrand parent, HandpieceModelCreateModel model, Boolean initial)
        {
            model.Parent = parent;
            return Task.CompletedTask;
        }

        private async Task<Boolean> ValidateCreateModel(HandpieceBrand handpieceBrand, HandpieceModelCreateModel model)
        {
            var normalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            var exists = await this.Repository.QueryWithoutTracking<HandpieceModel>()
                .AnyAsync(x => x.BrandId == handpieceBrand.Id && x.NormalizedName == normalizedName);

            if (exists)
            {
                this.ModelState.AddModelError("Name", "Model with that name already exists");
                return false;
            }

            return true;
        }

        private Task InitializeNewEntity(HandpieceBrand parent, HandpieceModel entity, HandpieceModelCreateModel model)
        {
            entity.Name = model.Name;
            entity.NormalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            entity.Type = model.Type;
            entity.ImageId = model.ImageId;
            entity.Description = model.Description;
            return Task.CompletedTask;
        }

        private Task<IActionResult> GetCreateSuccessResult(HandpieceBrand parent, HandpieceModel entity, HandpieceModelCreateModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceModelCreate", this.RedirectToAction("Index", "HandpieceBrands"));
        }

        private async Task<HandpieceModelEditModel> ConvertToDetailsModel(HandpieceModel entity)
        {
            return new HandpieceModelEditModel
            {
                Original = entity,
                Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync(),
                Parent = entity.Brand,
                Brands = await this.Repository.Query<HandpieceBrand>().OrderBy(x => x.Name).ToListAsync(),

                BrandId = entity.BrandId,
                Name = entity.Name,
                Type = entity.Type,
                ImageId = entity.ImageId,
                PriceNew = entity.PriceNew,
                Description = entity.Description,
                WorkshopNotes = entity.WorkshopNotes,
            };
        }

        private async Task InitializeEditModel(HandpieceModel entity, HandpieceModelEditModel model, Boolean initial)
        {
            model.Original = entity;
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            model.Parent = entity.Brand;
            model.Brands = await this.Repository.Query<HandpieceBrand>().OrderBy(x => x.Name).ToListAsync();
        }

        private async Task<Boolean> ValidateEditModel(HandpieceModel handpieceModel, HandpieceModelEditModel model)
        {
            var normalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            var exists = await this.Repository.QueryWithoutTracking<HandpieceModel>()
                .AnyAsync(x => x.BrandId == handpieceModel.BrandId && x.Id != handpieceModel.Id && x.NormalizedName == normalizedName);

            if (exists)
            {
                this.ModelState.AddModelError("Name", "Model with that name already exists");
                return false;
            }

            return true;
        }

        private Task UpdateExistingEntity(HandpieceModel entity, HandpieceModelEditModel model)
        {
            entity.BrandId = model.BrandId;
            entity.Name = model.Name;
            entity.NormalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            entity.Type = model.Type;
            entity.ImageId = model.ImageId;
            entity.PriceNew = model.PriceNew;
            entity.Description = model.Description;
            entity.WorkshopNotes = model.WorkshopNotes;
            return Task.CompletedTask;
        }

        private Task<IActionResult> GetEditViewResult(HandpieceModel entity, HandpieceModelEditModel model)
        {
            return Task.FromResult<IActionResult>(this.View("Details", model));
        }

        private Task<IActionResult> GetEditSuccessResult(HandpieceModel entity, HandpieceModelEditModel model, Dictionary<String, Object> additionalData)
        {
            return Task.FromResult<IActionResult>(this.RedirectToAction("Details", new { id = entity.Id }));
        }

        private Task<IActionResult> GetDeleteSuccessResult(HandpieceModel entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceModelDelete", this.RedirectToAction("Index", "HandpieceBrands"));
        }
    }
}
