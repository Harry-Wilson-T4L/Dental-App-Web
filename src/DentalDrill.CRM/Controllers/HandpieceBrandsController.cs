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
using DevGuild.AspNetCore.Services.Uploads.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    [PermissionsManager("Type", "/Domain/HandpieceDirectory/Brand")]
    [PermissionsManager("Entity", "/Domain/HandpieceDirectory/Brand/{entity}")]
    public class HandpieceBrandsController : BaseTelerikIndexBasicCrudController<Guid, HandpieceBrand, EmptyEmployeeIndexViewModel, HandpieceBrandReadModel, HandpieceBrand, HandpieceBrandEditModel, HandpieceBrandEditModel, HandpieceBrand>
    {
        private readonly IImageUploadService imageUploadService;
        private readonly INameNormalizationService nameNormalizationService;

        public HandpieceBrandsController(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, INameNormalizationService nameNormalizationService)
            : base(controllerServices)
        {
            this.imageUploadService = imageUploadService;
            this.nameNormalizationService = nameNormalizationService;

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        [Authorize(Roles = ApplicationRoles.Combined.Administrator)]
        public async Task<IActionResult> Import()
        {
            var existingBrands = await this.Repository.Query<HandpieceBrand>().ToListAsync();
            var existingBrandsMap = existingBrands.ToDictionary(x => x.NormalizedName);

            var existingModels = await this.Repository.Query<HandpieceModel>("Brand").ToListAsync();
            var existingModelsMap = existingModels.ToDictionary(x => (x.BrandId, x.NormalizedName));

            var brands = await this.GetImportableBrands();
            foreach (var importedBrand in brands)
            {
                if (existingBrandsMap.TryGetValue(importedBrand.NormalizedName, out var existingBrand))
                {
                    importedBrand.IsImported = true;
                    importedBrand.ExistingBrand = existingBrand;
                    foreach (var importedModel in importedBrand.Models)
                    {
                        if (existingModelsMap.TryGetValue((existingBrand.Id, importedModel.NormalizedName), out var existingModel))
                        {
                            importedModel.IsImported = true;
                            importedModel.ExistingModel = existingModel;
                        }
                    }
                }
            }

            var model = new HandpieceDirectoryImportModel
            {
                Brands = brands,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Combined.Administrator)]
        [RequestFormLimits(ValueCountLimit = 10 * 1024 * 1024)]
        public async Task<IActionResult> Import(HandpieceDirectoryImportModel model)
        {
            var existingBrands = await this.Repository.Query<HandpieceBrand>().ToListAsync();
            var existingBrandsMap = existingBrands.ToDictionary(x => x.NormalizedName);

            var existingModels = await this.Repository.Query<HandpieceModel>("Brand").ToListAsync();
            var existingModelsMap = existingModels.ToDictionary(x => (x.BrandId, x.NormalizedName));

            var brands = await this.GetImportableBrands();
            var reloadedBrandsMap = brands.ToDictionary(x => x.NormalizedName);
            var reloadedModelsMap = brands
                .SelectMany(x => x.Models, (b, m) => new { Brand = b, Model = m })
                .ToDictionary(x => (x.Brand.NormalizedName, x.Model.NormalizedName), x => x.Model);

            foreach (var importedBrand in model.Brands)
            {
                if (existingBrandsMap.TryGetValue(importedBrand.NormalizedName, out var existingBrand))
                {
                    importedBrand.IsImported = true;
                    importedBrand.ExistingBrand = existingBrand;
                    foreach (var importedModel in importedBrand.Models)
                    {
                        if (existingModelsMap.TryGetValue((existingBrand.Id, importedModel.NormalizedName), out var existingModel))
                        {
                            importedModel.IsImported = true;
                            importedModel.ExistingModel = existingModel;
                        }
                    }
                }

                if (reloadedBrandsMap.TryGetValue(importedBrand.NormalizedName, out var reloadedBrand))
                {
                    importedBrand.AlternateNames = reloadedBrand.AlternateNames;
                    foreach (var importedModel in importedBrand.Models)
                    {
                        if (reloadedModelsMap.TryGetValue((importedBrand.NormalizedName, importedModel.NormalizedName), out var reloadedModel))
                        {
                            importedModel.AlternateNames = reloadedModel.AlternateNames;
                        }
                        else
                        {
                            importedModel.AlternateNames = new List<String> { importedModel.CanonicalName };
                        }
                    }
                }
                else
                {
                    importedBrand.AlternateNames = new List<String> { importedBrand.CanonicalName };
                    foreach (var importedModel in importedBrand.Models)
                    {
                        importedModel.AlternateNames = new List<String> { importedModel.CanonicalName };
                    }
                }
            }

            if (this.ModelState.IsValid)
            {
                foreach (var importedBrand in model.Brands)
                {
                    if (importedBrand.IsImported && importedBrand.ExistingBrand == null)
                    {
                        var handpieceBrand = new HandpieceBrand
                        {
                            Id = Guid.NewGuid(),
                            Name = importedBrand.CanonicalName,
                            NormalizedName = this.nameNormalizationService.NormalizeName(importedBrand.CanonicalName),
                        };

                        await this.Repository.InsertAsync(handpieceBrand);
                        importedBrand.ExistingBrand = handpieceBrand;
                    }

                    if (importedBrand.IsImported)
                    {
                        foreach (var importedModel in importedBrand.Models)
                        {
                            if (importedModel.IsImported && importedModel.ExistingModel == null)
                            {
                                var handpieceModel = new HandpieceModel
                                {
                                    BrandId = importedBrand.ExistingBrand.Id,
                                    Name = importedModel.CanonicalName,
                                    NormalizedName = this.nameNormalizationService.NormalizeName(importedModel.CanonicalName),
                                    Type = HandpieceSpeed.Other,
                                };

                                await this.Repository.InsertAsync(handpieceModel);
                                importedModel.ExistingModel = handpieceModel;
                            }
                        }
                    }
                }

                await this.Repository.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        [NonAction]
        public override Task<IActionResult> Details(Guid id)
        {
            // TODO: Maybe reenable later to enable more scoped view of models
            throw new NotSupportedException();
        }

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private async Task<List<HandpieceDirectoryImportBrandModel>> GetImportableBrands()
        {
            var allHandpieces = await this.Repository.Query<Handpiece>()
                .Select(x => new { x.Brand, Model = x.MakeAndModel })
                .OrderBy(x => x.Brand)
                .ThenBy(x => x.Model)
                .ToListAsync();

            var normalizedHandpieces = allHandpieces.Select(x => new
            {
                x.Brand,
                x.Model,
                NormalizedBrand = this.nameNormalizationService.NormalizeName(x.Brand),
                NormalizedModel = this.nameNormalizationService.NormalizeName(x.Model),
            }).ToList();

            var brands = normalizedHandpieces.GroupBy(x => x.NormalizedBrand)
                .Select(x => new HandpieceDirectoryImportBrandModel
                {
                    IsImported = false,
                    NormalizedName = x.Key,
                    CanonicalName = x.Select(y => y.Brand).First(),
                    AlternateNames = x.Select(y => y.Brand).Distinct().ToList(),
                    Models = x.GroupBy(y => y.NormalizedModel).Select(y => new HandpieceDirectoryImportModelModel
                    {
                        IsImported = false,
                        NormalizedName = y.Key,
                        CanonicalName = y.Select(z => z.Model).First(),
                        AlternateNames = y.Select(z => z.Model).Distinct().ToList(),
                    }).OrderBy(y => y.NormalizedName).ToList(),
                })
                .OrderBy(x => x.NormalizedName)
                .ToList();

            return brands;
        }

        private Task<IQueryable<HandpieceBrand>> PrepareReadQuery()
        {
            IQueryable<HandpieceBrand> query = this.Repository.Query<HandpieceBrand>("Image").OrderBy(x => x.Name);
            return Task.FromResult(query);
        }

        private Task<HandpieceBrand> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<HandpieceBrand>("Image").SingleOrDefaultAsync(x => x.Id == id);
        }

        private HandpieceBrandReadModel ConvertEntityToViewModel(HandpieceBrand entity, String[] allowedProperties)
        {
            return new HandpieceBrandReadModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ImageId = entity.ImageId,
                ImageUrl = entity.ImageId.HasValue ? this.imageUploadService.GetImageUrl(entity.Image, "40") : "/img/hub-logo-40.png",
            };
        }

        private Task InitializeNewEntity(HandpieceBrand entity, HandpieceBrandEditModel model)
        {
            entity.Name = model.Name;
            entity.NormalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            entity.ImageId = model.ImageId;
            return Task.CompletedTask;
        }

        private Task<IActionResult> GetCreateSuccessResult(HandpieceBrand entity, HandpieceBrandEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceBrandCreate", this.RedirectToAction("Index"));
        }

        private Task UpdateExistingEntity(HandpieceBrand entity, HandpieceBrandEditModel model)
        {
            entity.Name = model.Name;
            entity.NormalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            entity.ImageId = model.ImageId;
            return Task.CompletedTask;
        }

        private Task<IActionResult> GetEditSuccessResult(HandpieceBrand entity, HandpieceBrandEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceBrandEdit", this.RedirectToAction("Index"));
        }

        private Task<IActionResult> GetDeleteSuccessResult(HandpieceBrand entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpieceBrandDelete", this.RedirectToAction("Index"));
        }
    }
}
