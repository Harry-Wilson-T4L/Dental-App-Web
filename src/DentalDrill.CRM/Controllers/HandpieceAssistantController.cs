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
using DevGuild.AspNetCore.Services.Uploads.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class HandpieceAssistantController : Controller
    {
        private readonly IEntityControllerServices controllerServices;
        private readonly IImageUploadService imageUploadService;
        private readonly INameNormalizationService nameNormalizationService;

        public HandpieceAssistantController(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, INameNormalizationService nameNormalizationService)
        {
            this.controllerServices = controllerServices;
            this.imageUploadService = imageUploadService;
            this.nameNormalizationService = nameNormalizationService;
        }

        protected IRepository Repository => this.controllerServices.Repository;

        public async Task<IActionResult> Index(Guid handpieceId)
        {
            var handpiece = await this.Repository.Query<Handpiece>().SingleOrDefaultAsync(x => x.Id == handpieceId);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            var normalizedBrand = this.nameNormalizationService.NormalizeName(handpiece.Brand);
            var normalizedModel = this.nameNormalizationService.NormalizeName(handpiece.MakeAndModel);

            var handpieceModel = await this.Repository.Query<HandpieceModel>("Brand", "Schematics", "Schematics.Image", "Schematics.Attachment")
                .SingleOrDefaultAsync(x => x.NormalizedName == normalizedModel && x.Brand.NormalizedName == normalizedBrand);

            if (handpieceModel == null)
            {
                return this.View("IndexNoModel", handpiece);
            }

            var model = new HandpieceAssistantIndexViewModel
            {
                Id = handpieceModel.Id,
                Model = handpieceModel,
                Brand = handpieceModel.Brand,
                Schematics = new List<HandpieceModelSchematicReadModel>(),
            };

            foreach (var schematic in handpieceModel.Schematics.Where(x => x.Display).OrderBy(x => x.OrderNo))
            {
                var schematicReadModel = new HandpieceModelSchematicReadModel
                {
                    Id = schematic.Id,
                    Type = schematic.Type,
                    Title = schematic.Title,
                    Display = schematic.Display,
                };

                switch (schematic.Type)
                {
                    case HandpieceModelSchematicType.Text:
                        schematicReadModel.ThumbnailUrl = "/img/hub-logo.png";
                        break;
                    case HandpieceModelSchematicType.Attachment:
                        schematicReadModel.ThumbnailUrl = "/img/hub-logo.png";
                        break;
                    case HandpieceModelSchematicType.Image:
                        schematicReadModel.ThumbnailUrl = schematic.ImageId.HasValue
                            ? await this.imageUploadService.GetImageUrlAsync(schematic.Image, "160")
                            : "/img/hub-logo.png";
                        break;
                }

                model.Schematics.Add(schematicReadModel);
            }

            return this.View(model);
        }

        public async Task<IActionResult> Preview(Guid id, Guid? schematicId)
        {
            var handpieceModel = await this.Repository.Query<HandpieceModel>("Brand", "Schematics", "Schematics.Image", "Schematics.Attachment")
                .SingleOrDefaultAsync(x => x.Id == id);
            if (handpieceModel == null)
            {
                return this.NotFound();
            }

            this.ViewBag.SchematicId = schematicId;
            return this.View(handpieceModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNotes(Guid id, [FromBody] HandpieceAssistantUpdateNotesModel model)
        {
            var handpieceModel = await this.Repository.Query<HandpieceModel>()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (handpieceModel == null)
            {
                return this.NotFound();
            }

            handpieceModel.WorkshopNotes = model.Notes;
            await this.Repository.UpdateAsync(handpieceModel);
            await this.Repository.SaveChangesAsync();

            return this.NoContent();
        }

        public async Task<IActionResult> Initialize(Guid handpieceId)
        {
            var handpiece = await this.Repository.Query<Handpiece>().SingleOrDefaultAsync(x => x.Id == handpieceId);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            var model = new HandpieceAssistantInitializeModel
            {
                Handpiece = handpiece,
                Type = handpiece.SpeedType,
            };

            var normalizedBrand = this.nameNormalizationService.NormalizeName(handpiece.Brand);
            var normalizedModel = this.nameNormalizationService.NormalizeName(handpiece.MakeAndModel);

            model.Model = await this.Repository.Query<HandpieceModel>("Brand")
                .SingleOrDefaultAsync(x => x.NormalizedName == normalizedModel && x.Brand.NormalizedName == normalizedBrand);

            model.Brand = await this.Repository.Query<HandpieceBrand>()
                .SingleOrDefaultAsync(x => x.NormalizedName == normalizedBrand);

            if (model.Model != null)
            {
                this.ModelState.AddModelError(String.Empty, "Directory entry already exists");
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Initialize(Guid handpieceId, HandpieceAssistantInitializeModel model)
        {
            var handpiece = await this.Repository.Query<Handpiece>().SingleOrDefaultAsync(x => x.Id == handpieceId);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            model.Handpiece = handpiece;

            var normalizedBrand = this.nameNormalizationService.NormalizeName(handpiece.Brand);
            var normalizedModel = this.nameNormalizationService.NormalizeName(handpiece.MakeAndModel);

            model.Model = await this.Repository.Query<HandpieceModel>("Brand")
                .SingleOrDefaultAsync(x => x.NormalizedName == normalizedModel && x.Brand.NormalizedName == normalizedBrand);

            model.Brand = await this.Repository.Query<HandpieceBrand>()
                .SingleOrDefaultAsync(x => x.NormalizedName == normalizedBrand);

            if (model.Model != null)
            {
                this.ModelState.AddModelError(String.Empty, "Directory entry already exists");
            }

            if (this.ModelState.IsValid)
            {
                if (model.Brand == null)
                {
                    var newBrand = new HandpieceBrand
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Handpiece.Brand,
                        NormalizedName = this.nameNormalizationService.NormalizeName(model.Handpiece.Brand),
                    };

                    await this.Repository.InsertAsync(newBrand);
                    model.Brand = newBrand;
                }

                var newModel = new HandpieceModel
                {
                    BrandId = model.Brand.Id,
                    Name = model.Handpiece.MakeAndModel,
                    NormalizedName = this.nameNormalizationService.NormalizeName(model.Handpiece.MakeAndModel),
                    Type = model.Type,
                    Description = model.Description,
                    ImageId = model.ImageId,
                };

                await this.Repository.InsertAsync(newModel);
                await this.Repository.SaveChangesAsync();

                return await this.HybridFormResultAsync("HandpieceAssistantInitialize", this.RedirectToAction("Edit", "Handpieces", new { id = model.Handpiece.Id }));
            }

            return this.View(model);
        }
    }
}
