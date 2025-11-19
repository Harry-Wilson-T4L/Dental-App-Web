using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
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
    [PermissionsManager("Type", "/Domain/HandpieceDirectory/ModelListing")]
    [PermissionsManager("Entity", "/Domain/HandpieceDirectory/ModelListing/{entity}")]
    public class HandpieceModelListingsController : BaseTelerikIndexlessDependentBasicCrudController<
        Guid,
        HandpieceStoreListing,
        Guid,
        HandpieceModel,
        HandpieceModelListingReadModel,
        HandpieceStoreListing,
        HandpieceModelListingEditModel,
        HandpieceModelListingEditModel,
        HandpieceStoreListing>
    {
        public HandpieceModelListingsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            // Read
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            // Create
            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            // Edit
            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.ValidateEditModel = this.ValidateEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            // Delete
            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.DeleteEntity = this.DeleteEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        #region Services

        #endregion // Services

        #region Queries

        private Task<HandpieceStoreListing> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<HandpieceStoreListing>()
                .Include(x => x.Model)
                .Include(x => x.MainImage)
                .Include(x => x.Images)
                .ThenInclude(x => x.Image)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        #endregion // Queries

        #region Read

        private HandpieceModelListingReadModel ConvertEntityToViewModel(HandpieceModel handpieceModel, HandpieceStoreListing storeListing, String[] allowedProperties)
        {
            return new HandpieceModelListingReadModel
            {
                Id = storeListing.Id,
                CreatedOn = storeListing.CreatedOn,
                Status = storeListing.Status,
                SerialNumber = storeListing.SerialNumber,
                Price = storeListing.Price,
                Warranty = storeListing.Warranty,
                Notes = storeListing.Notes,
                Coupling = storeListing.Coupling,
                CosmeticCondition = storeListing.CosmeticCondition,
                FiberOpticBrightness = storeListing.FiberOpticBrightness,
            };
        }

        #endregion // Read

        #region Create

        private Task InitializeCreateModel(HandpieceModel handpieceModel, HandpieceModelListingEditModel model, Boolean initial)
        {
            if (initial)
            {
                model.Price = handpieceModel.PriceNew ?? 0m;
            }

            model.Entity = null;
            model.Parent = handpieceModel;
            return Task.CompletedTask;
        }

        private Task InitializeNewEntity(HandpieceModel handpieceModel, HandpieceStoreListing storeListing, HandpieceModelListingEditModel model)
        {
            storeListing.ModelId = handpieceModel.Id;
            storeListing.Status = HandpieceStoreListingStatus.Listed;
            storeListing.CreatedOn = DateTime.UtcNow;
            storeListing.RequestedOn = null;
            storeListing.SoldOn = null;
            storeListing.SerialNumber = model.SerialNumber;
            storeListing.Price = model.Price;
            storeListing.Warranty = model.Warranty;
            storeListing.Notes = model.Notes;
            storeListing.Coupling = model.Coupling;
            storeListing.CosmeticCondition = model.CosmeticCondition;
            storeListing.FiberOpticBrightness = model.FiberOpticBrightness;

            storeListing.Images = new Collection<HandpieceStoreListingImage>();
            if (model.Images != null && model.Images.Count > 0)
            {
                storeListing.MainImageId = model.Images[0];
                for (var i = 0; i < model.Images.Count; i++)
                {
                    storeListing.Images.Add(new HandpieceStoreListingImage { OrderNo = i + 1, ImageId = model.Images[i] });
                }
            }

            return Task.CompletedTask;
        }

        private Task<IActionResult> GetCreateSuccessResult(HandpieceModel handpieceModel, HandpieceStoreListing storeListing, HandpieceModelListingEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceModelListingCreate", this.RedirectToAction("Edit", "HandpieceModels", new { id = handpieceModel.Id, Tab = "Listings" }));
        }

        #endregion // Create

        #region Edit

        private Task InitializeEditModelWithEntity(HandpieceStoreListing entity, HandpieceModelListingEditModel model)
        {
            model.SerialNumber = entity.SerialNumber;
            model.Price = entity.Price;
            model.Warranty = entity.Warranty;
            model.Notes = entity.Notes;
            model.Coupling = entity.Coupling;
            model.CosmeticCondition = entity.CosmeticCondition;
            model.FiberOpticBrightness = entity.FiberOpticBrightness;
            model.Images = entity.Images.OrderBy(x => x.OrderNo).Select(x => x.ImageId).ToList();
            return Task.CompletedTask;
        }

        private Task InitializeEditModel(HandpieceStoreListing entity, HandpieceModelListingEditModel model, Boolean initial)
        {
            model.Entity = entity;
            model.Parent = entity.Model;
            return Task.CompletedTask;
        }

        private Task<Boolean> ValidateEditModel(HandpieceStoreListing storeListing, HandpieceModelListingEditModel model)
        {
            if (storeListing.Status == HandpieceStoreListingStatus.Deleted)
            {
                this.ModelState.AddModelError(String.Empty, "Cannot change deleted listing");
            }

            if (storeListing.Status == HandpieceStoreListingStatus.Requested)
            {
                this.ModelState.AddModelError(String.Empty, "Cannot change requested listing");
            }

            if (storeListing.Status == HandpieceStoreListingStatus.Sold)
            {
                this.ModelState.AddModelError(String.Empty, "Cannot change sold listing");
            }

            return Task.FromResult(this.ModelState.IsValid);
        }

        private async Task UpdateExistingEntity(HandpieceStoreListing storeListing, HandpieceModelListingEditModel model)
        {
            storeListing.SerialNumber = model.SerialNumber;
            storeListing.Price = model.Price;
            storeListing.Warranty = model.Warranty;
            storeListing.Notes = model.Notes;
            storeListing.Coupling = model.Coupling;
            storeListing.CosmeticCondition = model.CosmeticCondition;
            storeListing.FiberOpticBrightness = model.FiberOpticBrightness;

            if (model.Status.HasValue)
            {
                storeListing.Status = model.Status.Value;
            }

            foreach (var image in storeListing.Images)
            {
                await this.Repository.DeleteAsync(image);
            }

            storeListing.Images.Clear();
            if (model.Images != null && model.Images.Count > 0)
            {
                storeListing.MainImageId = model.Images[0];
                for (var i = 0; i < model.Images.Count; i++)
                {
                    storeListing.Images.Add(new HandpieceStoreListingImage { OrderNo = i + 1, ImageId = model.Images[i] });
                }
            }
            else
            {
                storeListing.MainImageId = null;
            }
        }

        private Task<IActionResult> GetEditSuccessResult(HandpieceStoreListing storeListing, HandpieceModelListingEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceModelListingEdit", this.RedirectToAction("Edit", "HandpieceModels", new { id = storeListing.ModelId, Tab = "Listings" }));
        }

        #endregion // Edit

        #region Delete

        private async Task DeleteEntity(HandpieceStoreListing storeListing, Dictionary<String, Object> additionalData)
        {
            if (storeListing.Status == HandpieceStoreListingStatus.Deleted)
            {
                throw new InvalidOperationException("Cannot delete deleted listing");
            }

            if (storeListing.Status == HandpieceStoreListingStatus.Requested)
            {
                throw new InvalidOperationException("Cannot delete requested listing");
            }

            if (storeListing.Status == HandpieceStoreListingStatus.Sold)
            {
                throw new InvalidOperationException("Cannot delete sold listing");
            }

            storeListing.Status = HandpieceStoreListingStatus.Deleted;

            await this.Repository.UpdateAsync(storeListing);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetDeleteSuccessResult(HandpieceStoreListing storeListing, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceModelListingDelete", this.RedirectToAction("Edit", "HandpieceModels", new { id = storeListing.ModelId, Tab = "Listings" }));
        }

        #endregion // Delete
    }
}
