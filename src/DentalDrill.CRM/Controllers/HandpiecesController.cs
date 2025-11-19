using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.ObjectModel;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class HandpiecesController : BaseTelerikIndexlessDependentBasicCrudController<Guid, Handpiece, Guid, Job, JobHandpieceReadModel, HandpieceDetailsModel, JobHandpieceEditModel, JobHandpieceEditModel, HandpieceDetailsModel>
    {
        private readonly RepairWorkflowService repairWorkflowService;
        private readonly UserEntityResolver userEntityResolver;
        private readonly IChangeTrackingService<Handpiece, HandpieceChange> changeTrackingService;

        public HandpiecesController(
            IEntityControllerServices controllerServices,
            RepairWorkflowService repairWorkflowService,
            UserEntityResolver userEntityResolver,
            IChangeTrackingService<Handpiece, HandpieceChange> changeTrackingService)
            : base(controllerServices)
        {
            this.repairWorkflowService = repairWorkflowService;
            this.userEntityResolver = userEntityResolver;
            this.changeTrackingService = changeTrackingService;

            this.SetStatusHandler = new BasicCrudEditActionHandler<Guid, Handpiece, Handpiece, JobHandpieceChangeStatusModel>(this, controllerServices, this.GetEntityPermissionsValidator());
            this.SetDiagnosticsHandler = new BasicCrudCustomOperationActionHandler<Guid, Handpiece, HandpieceSetDiagnosticsModel>(this, this.ControllerServices, this.GetEntityPermissionsValidator());

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.DetailsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DetailsHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.CreateHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;
            this.CreateHandler.Overrides.BeforeEntityCreated = this.BeforeEntityCreated;
            this.CreateHandler.Overrides.AfterEntityCreated = this.AfterEntityCreated;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.InitializeEditModel = this.InitializeEditModel;
            this.EditHandler.Overrides.ValidateEditModel = this.ValidateEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.BeforeEntityUpdated = this.BeforeEntityUpdated;
            this.EditHandler.Overrides.AfterEntityUpdated = this.AfterEntityUpdated;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;
            this.EditHandler.Overrides.ConvertToDetailsModel = this.ConvertToDetailsModel;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDetailsModel;

            this.SetStatusHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.SetStatusHandler.Overrides.BeforeEntityUpdated = this.BeforeStatusUpdated;
            this.SetStatusHandler.Overrides.AfterEntityUpdated = this.AfterStatusUpdated;
            this.SetStatusHandler.Overrides.GetEditSuccessResult = this.GetSetStatusSuccessResult;

            this.SetDiagnosticsHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.SetDiagnosticsHandler.Overrides.InitializeOperationModel = this.InitializeSetDiagnosticsModel;
        }

        protected BasicCrudEditActionHandler<Guid, Handpiece, Handpiece, JobHandpieceChangeStatusModel> SetStatusHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, Handpiece, HandpieceSetDiagnosticsModel> SetDiagnosticsHandler { get; }

        public Task<IActionResult> SetStatus(Guid id) => this.SetStatusHandler.Edit(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SetStatus(Guid id, JobHandpieceChangeStatusModel model) => this.SetStatusHandler.Edit(id, model);

        [AjaxGet]
        public Task<IActionResult> SetDiagnostics(Guid id) => this.SetDiagnosticsHandler.Execute(id);

        private Task<IQueryable<Handpiece>> PrepareReadQuery(Job parent)
        {
            var query = this.Repository.Query<Handpiece>("Components", "ServiceLevel", "Creator", "EstimatedBy", "RepairedBy")
                .Where(x => x.JobId == parent.Id);

            return Task.FromResult(query);
        }

        private JobHandpieceReadModel ConvertEntityToViewModel(Job parent, Handpiece entity, String[] allowedProperties)
        {
            return new JobHandpieceReadModel
            {
                Id = entity.Id,
                JobId = entity.JobId,
                JobPosition = entity.JobPosition,
                Brand = entity.Brand,
                MakeAndModel = entity.MakeAndModel,
                Serial = entity.Serial,
                Components = entity.Components.Select(x => new JobHandpieceComponentEditModel
                {
                    Brand = x.Brand,
                    MakeAndModel = x.Model,
                    Serial = x.Serial,
                }).ToList(),
                DiagnosticReport = entity.DiagnosticReport,
                ServiceLevel = entity.ServiceLevelId != null ? entity.ServiceLevel.Name : String.Empty,
                StatusId = entity.HandpieceStatus,
                Status = entity.HandpieceStatus.ToDisplayString(),
                Rating = entity.Rating,
                EstimatedBy = entity.EstimatedBy == null ? null : entity.EstimatedBy.FirstName + " " + entity.EstimatedBy.LastName[0] + ".",
                RepairedBy = entity.RepairedBy == null ? null : entity.RepairedBy.FirstName + " " + entity.RepairedBy.LastName[0] + ".",
                Parts = entity.Parts,
                PartsOutOfStock = entity.PartsOutOfStock,
                CostOfRepair = entity.CostOfRepair ?? 0m,
                InternalComment = entity.InternalComment,
            };
        }

        private Task<Job> QuerySingleParentEntity(Guid id)
        {
            return this.Repository.Query<Job>()
                .Include(x => x.JobType)
                .Include(x => x.Client)
                .Include(x => x.Handpieces)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<Handpiece> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<Handpiece>()
                .Include(x => x.Job).ThenInclude(x => x.Client).ThenInclude(x => x.Corporate)
                .Include(x => x.Job).ThenInclude(x => x.JobType)
                .Include(x => x.Components)
                .Include(x => x.ServiceLevel)
                .Include(x => x.SelectedDiagnosticCheckItems)
                .Include(x => x.Creator)
                .Include(x => x.EstimatedBy)
                .Include(x => x.RepairedBy)
                .Include(x => x.PartsRequired)
                .ThenInclude(x => x.SKU)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<HandpieceDetailsModel> ConvertToDetailsModel(Handpiece entity)
        {
            var job = await this.Repository.QueryWithoutTracking<Job>()
                .Include(x => x.JobType)
                .Include(x => x.Handpieces)
                .SingleOrDefaultAsync(x => x.Id == entity.JobId);
            return new HandpieceDetailsModel
            {
                Id = entity.Id,
                Entity = entity,
                Job = job,
                Previous = job.Handpieces.Where(x => x.JobPosition < entity.JobPosition).OrderByDescending(x => x.JobPosition).FirstOrDefault(),
                Next = job.Handpieces.Where(x => x.JobPosition > entity.JobPosition).OrderBy(x => x.JobPosition).FirstOrDefault(),
                PropertiesAccessControl = await this.repairWorkflowService.GetPropertiesAccessControlAsync(entity),
            };
        }

        private async Task InitializeCreateModel(Job parent, JobHandpieceEditModel model, Boolean initial)
        {
            model.Parent = parent;
            model.ReturnEstimates = await this.Repository.Query<ReturnEstimate>().OrderBy(x => x.Name).ToListAsync();
            model.Access = await this.userEntityResolver.GetEmployeeAccessAsync();
            model.PropertiesAccessControl = await this.repairWorkflowService.GetPropertiesAccessControlForNewEntityAsync(parent);
            model.RequiredParts = new List<HandpieceRequiredPartReadModel>();

            var clientPricing = await this.Repository.Query<CorporatePricingServiceLevel>().Where(x => x.CategoryId == parent.Client.PricingCategoryId).ToListAsync();
            var serviceLevels = await this.Repository.Query<ServiceLevel>().OrderBy(x => x.Name).ToListAsync();
            model.ServiceLevels = serviceLevels.Select(x => new JobHandpieceServiceLevel
            {
                Id = x.Id,
                Name = x.Name,
                CostOfRepair = clientPricing.SingleOrDefault(y => y.ServiceLevelId == x.Id)?.CostOfRepair,
            }).ToList();

            model.AllDiagnosticCheckItems = (await this.Repository.Query<DiagnosticCheckItem>().ToListAsync()).ToDictionary(x => x.Id);
        }

        private async Task BeforeEntityCreated(Job parent, Handpiece entity, JobHandpieceEditModel model, Dictionary<String, Object> additionalData)
        {
            var user = await this.userEntityResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                throw new InvalidOperationException("User is not an Employee");
            }

            entity.HandpieceStatus = await this.repairWorkflowService.GetNewHandpieceStatusAsync(parent);
            entity.CreatorId = employee.Id;
            entity.JobPosition = parent.Handpieces.Any() ? parent.Handpieces.Max(x => x.JobPosition) + 1 : 1;
            entity.PartsVersion = HandpiecePartsVersion.InventorySKUv1;

            if (model.Components != null && model.Components.Count > 0)
            {
                entity.Components = model.Components
                    .Select((x, i) => new HandpieceComponent
                    {
                        HandpieceId = entity.Id,
                        OrderNo = (i + 1),
                        Brand = x.Brand,
                        Model = x.MakeAndModel,
                        Serial = x.Serial,
                    })
                    .ToList();

                entity.ComponentsText = String.Join("; ", model.Components.Select(x => $"{x.Brand} {x.MakeAndModel} S/N {x.Serial}"));
            }
            else
            {
                entity.Components = new List<HandpieceComponent>();
                entity.ComponentsText = String.Empty;
            }

            var clientHandpieces = await this.Repository.Query<ClientHandpiece>()
                .Include(x => x.Components)
                .Where(x => x.ClientId == parent.ClientId)
                .ToListAsync();

            if (model.ClientHandpieceId == null || model.ClientHandpieceId.Value == Guid.Empty)
            {
                entity.ClientHandpiece = new ClientHandpiece
                {
                    ClientId = parent.ClientId,
                    Components = new List<ClientHandpieceComponent>(),
                };

                entity.ClientHandpiece.UpdateFromHandpiece(entity);
            }
            else
            {
                entity.ClientHandpiece = clientHandpieces.Single(x => x.Id == model.ClientHandpieceId);
                entity.ClientHandpiece.UpdateFromHandpiece(entity);
            }
        }

        private async Task AfterEntityCreated(Job parent, Handpiece entity, JobHandpieceEditModel model, Dictionary<String, Object> additionalData)
        {
            await this.changeTrackingService.TrackCreatedEntityAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetCreateSuccessResult(Job parent, Handpiece entity, JobHandpieceEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpiecesCreate", this.RedirectToAction("Edit", "Jobs", new { id = entity.JobId }));
        }

        private async Task InitializeEditModelWithEntity(Handpiece entity, JobHandpieceEditModel model)
        {
            var update = await this.repairWorkflowService.HandleHandpieceOpenAsync(entity.Id);
            if (update.HandpieceStatus != entity.HandpieceStatus)
            {
                entity.HandpieceStatus = update.HandpieceStatus;
            }

            model.Brand = entity.Brand;
            model.MakeAndModel = entity.MakeAndModel;
            model.Serial = entity.Serial;
            model.Components = entity.Components.OrderBy(x => x.OrderNo).Select(x => new JobHandpieceComponentEditModel { Brand = x.Brand, MakeAndModel = x.Model, Serial = x.Serial }).ToList();
            model.ServiceLevelId = entity.ServiceLevelId;
            model.Rating = entity.Rating;
            model.ProblemDescription = entity.ProblemDescription;
            model.Parts = entity.Parts;
            model.PartsOutOfStock = entity.PartsOutOfStock;
            model.PartsComment = entity.PartsComment;
            model.PartsOrdered = entity.PartsOrdered;
            model.PartsRestocked = entity.PartsRestocked;
            model.ReturnById = entity.ReturnById;
            model.CostOfRepair = entity.CostOfRepair;
            model.SpeedType = entity.SpeedType;
            model.Speed = entity.Speed;
            model.InternalComment = entity.InternalComment;
            model.PublicComment = entity.PublicComment;
            model.DiagnosticReportChecked = entity.SelectedDiagnosticCheckItems.OrderBy(x => x.OrderNo).Select(x => x.ItemId).ToList();
            model.DiagnosticReportOther = entity.DiagnosticOther;
            model.ChangeStatus = entity.HandpieceStatus;
        }

        private async Task InitializeEditModel(Handpiece entity, JobHandpieceEditModel model, Boolean initial)
        {
            model.Original = await this.ConvertToDetailsModel(entity);
            model.Parent = model.Original.Entity.Job;
            model.ReturnEstimates = await this.Repository.Query<ReturnEstimate>().OrderBy(x => x.Name).ToListAsync();
            model.Access = await this.userEntityResolver.GetEmployeeAccessAsync();
            model.PropertiesAccessControl = model.Original.PropertiesAccessControl;
            model.PossibleStatuses = await this.repairWorkflowService.GetAvailableStatusChangesAsync(entity);
            var handpiece = await this.ControllerServices.ServiceProvider.GetRequiredService<IHandpieceManager>().GetByIdAsync(entity.Id) ?? throw new InvalidOperationException();
            model.RequiredParts = await handpiece.Parts.ToReadModelAsync();

            var clientPricing = await this.Repository.Query<CorporatePricingServiceLevel>().Where(x => x.CategoryId == entity.Job.Client.PricingCategoryId).ToListAsync();
            var serviceLevels = await this.Repository.Query<ServiceLevel>().OrderBy(x => x.Name).ToListAsync();
            model.ServiceLevels = serviceLevels.Select(x => new JobHandpieceServiceLevel
            {
                Id = x.Id,
                Name = x.Name,
                CostOfRepair = clientPricing.SingleOrDefault(y => y.ServiceLevelId == x.Id)?.CostOfRepair,
            }).ToList();

            model.AllDiagnosticCheckItems = (await this.Repository.Query<DiagnosticCheckItem>().ToListAsync()).ToDictionary(x => x.Id);
        }

        private async Task<Boolean> ValidateEditModel(Handpiece entity, JobHandpieceEditModel model)
        {
            var isValid = true;
            var validators = await this.repairWorkflowService.GetUpdateValidators(entity, model);
            foreach (var validator in validators.Validators)
            {
                var result = await validator.ValidateAsync(model);
                if (!result.IsValid)
                {
                    isValid = false;
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(error.Field, error.Error);
                    }
                }
            }

            return isValid;
        }

        private async Task UpdateExistingEntity(Handpiece entity, JobHandpieceEditModel model)
        {
            var acl = await this.repairWorkflowService.GetPropertiesAccessControlAsync(entity);

            acl.For(x => x.Brand).TryUpdate(entity, model.Brand);
            acl.For(x => x.MakeAndModel).TryUpdate(entity, model.MakeAndModel);
            acl.For(x => x.Serial).TryUpdate(entity, model.Serial);
            acl.For(x => x.ServiceLevelId).TryUpdate(entity, model.ServiceLevelId);
            acl.For(x => x.Rating).TryUpdate(entity, model.Rating);
            acl.For(x => x.ProblemDescription).TryUpdate(entity, model.ProblemDescription);
            acl.For(x => x.Parts).TryUpdate(entity, model.Parts);
            acl.For(x => x.PartsComment).TryUpdate(entity, model.PartsComment);
            acl.For(x => x.ReturnById).TryUpdate(entity, model.ReturnById);
            acl.For(x => x.CostOfRepair).TryUpdate(entity, model.CostOfRepair);
            acl.For(x => x.SpeedType).TryUpdate(entity, model.SpeedType);
            acl.For(x => x.Speed).TryUpdate(entity, model.Speed);
            acl.For(x => x.InternalComment).TryUpdate(entity, model.InternalComment);
            acl.For(x => x.PublicComment).TryUpdate(entity, model.PublicComment);

            if (entity.PartsVersion == HandpiecePartsVersion.Manual)
            {
                acl.For(x => x.PartsOutOfStock).TryUpdate(entity, model.PartsOutOfStock);
                acl.For(x => x.PartsOrdered).TryUpdate(entity, model.PartsOrdered);
                if (acl.For(x => x.PartsRestocked).TryUpdate(entity, model.PartsRestocked))
                {
                    var entry = await this.Repository.Query<StockControlEntry>().SingleOrDefaultAsync(x => x.HandpieceId == entity.Id);
                    if (entry != null)
                    {
                        if (entry.Status == StockControlEntryStatus.Ordered && !entity.PartsRestocked)
                        {
                            entry.Status = StockControlEntryStatus.Ignored;
                        }
                        else if (entry.Status != StockControlEntryStatus.Ordered && entity.PartsRestocked)
                        {
                            entry.Status = StockControlEntryStatus.Ordered;
                        }
                    }
                }
            }
            else if (entity.PartsVersion == HandpiecePartsVersion.InventorySKUv1)
            {
                if (entity.HandpieceStatus.IsNotOneOf(HandpieceStatus.Cancelled, HandpieceStatus.SentComplete))
                {
                    var handpiece = await this.ControllerServices.ServiceProvider.GetRequiredService<IHandpieceManager>().GetByIdAsync(entity.Id);
                    entity.PartsOutOfStock = await handpiece.Parts.GetStockStatusAsync();
                }
            }

            if (acl.For(x => x.DiagnosticReport).CanUpdate)
            {
                entity.DiagnosticOther = model.DiagnosticReportOther;

                var existingItems = await this.Repository.Query<HandpieceDiagnostic>().Where(x => x.HandpieceId == entity.Id).ToListAsync();
                var nextOrderNo = existingItems.Count > 0 ? existingItems.Max(x => x.OrderNo) + 1 : 1;
                foreach (var selectedId in model.DiagnosticReportChecked)
                {
                    var matchingItem = existingItems.SingleOrDefault(x => x.ItemId == selectedId);
                    if (matchingItem != null)
                    {
                        matchingItem.OrderNo = nextOrderNo++;
                        await this.Repository.UpdateAsync(matchingItem);
                        existingItems.Remove(matchingItem);
                    }
                    else
                    {
                        await this.Repository.InsertAsync(new HandpieceDiagnostic { HandpieceId = entity.Id, ItemId = selectedId, OrderNo = nextOrderNo++ });
                    }
                }

                foreach (var remaining in existingItems)
                {
                    await this.Repository.DeleteAsync(remaining);
                }
            }

            if (acl.For(x => x.Serial).CanUpdate)
            {
                var handpieceComponents = entity.Components.OrderBy(x => x.OrderNo).ToList();
                var modelComponents = model.Components ?? new List<JobHandpieceComponentEditModel>();
                var minLength = Math.Min(handpieceComponents.Count, modelComponents.Count);
                var maxOrder = 0;
                for (var i = 0; i < minLength; i++)
                {
                    handpieceComponents[i].Brand = modelComponents[i].Brand;
                    handpieceComponents[i].Model = modelComponents[i].MakeAndModel;
                    handpieceComponents[i].Serial = modelComponents[i].Serial;
                    maxOrder = Math.Max(maxOrder, handpieceComponents[i].OrderNo);
                }

                for (var i = minLength; i < handpieceComponents.Count; i++)
                {
                    entity.Components.Remove(handpieceComponents[i]);
                }

                for (var i = minLength; i < modelComponents.Count; i++)
                {
                    entity.Components.Add(new HandpieceComponent
                    {
                        Brand = modelComponents[i].Brand,
                        Model = modelComponents[i].MakeAndModel,
                        Serial = modelComponents[i].Serial,
                        OrderNo = ++maxOrder,
                    });
                }

                entity.ComponentsText = String.Join("; ", model.Components.Select(x => $"{x.Brand} {x.MakeAndModel} S/N {x.Serial}"));

                var clientHandpiece = await this.Repository.Query<ClientHandpiece>()
                    .Include(x => x.Components)
                    .Include(x => x.Handpieces).ThenInclude(x => x.Job)
                    .SingleAsync(x => x.Id == entity.ClientHandpieceId);
                var lastHandpiece = clientHandpiece.Handpieces.OrderByDescending(x => x.Job.Received).First();
                if (lastHandpiece.Id == entity.Id)
                {
                    // Updating client handpiece entry if last handpiece is changed
                    clientHandpiece.UpdateFromHandpiece(entity);
                }
            }
        }

        private async Task BeforeEntityUpdated(Handpiece entity, JobHandpieceEditModel model, Dictionary<String, Object> additionalData)
        {
            var change = await this.changeTrackingService.CaptureEntityForUpdate(entity);
            additionalData["ChangeTrackingService.Change"] = change;
        }

        private async Task AfterEntityUpdated(Handpiece entity, JobHandpieceEditModel model, Dictionary<String, Object> additionalData)
        {
            var resultingItems = await this.Repository.Query<HandpieceDiagnostic>("Item")
                .Where(x => x.HandpieceId == entity.Id)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            entity.DiagnosticReport = String.Join(", ", resultingItems.Select(x => x.Item.Name));
            if (!String.IsNullOrEmpty(entity.DiagnosticOther))
            {
                if (!String.IsNullOrEmpty(entity.DiagnosticReport))
                {
                    entity.DiagnosticReport += ", " + entity.DiagnosticOther;
                }
                else
                {
                    entity.DiagnosticReport = entity.DiagnosticOther;
                }
            }

            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();

            if (model.ChangeStatus != null && model.ChangeStatus != entity.HandpieceStatus)
            {
                var possibleStatuses = await this.repairWorkflowService.GetAvailableStatusChangesAsync(entity);
                if (!(possibleStatuses == null || !possibleStatuses.Contains(model.ChangeStatus.Value)))
                {
                    if (await this.repairWorkflowService.CanChangeHandpieceStatusAsync(entity, model.ChangeStatus.Value))
                    {
                        await this.repairWorkflowService.ChangeHandpieceStatusAsync(entity, model.ChangeStatus.Value);
                    }
                }
            }

            var change = (HandpieceChange)additionalData["ChangeTrackingService.Change"];
            await this.changeTrackingService.TrackModifyEntityAsync(entity, change);
            await this.Repository.SaveChangesAsync();
        }

        private async Task<IActionResult> GetEditSuccessResult(Handpiece entity, JobHandpieceEditModel model, Dictionary<String, Object> additionalData)
        {
            if (this.Request.Form["Command"].Equals("SaveAndNext"))
            {
                var details = await this.ConvertToDetailsModel(entity);
                if (details.Next != null)
                {
                    return await this.HybridFormResultAsync("HandpiecesEdit", this.RedirectToAction("Edit", new { id = details.Next.Id }));
                }
                else
                {
                    return await this.HybridFormResultAsync("HandpiecesEdit", this.RedirectToAction("Edit", "Jobs", new { id = entity.JobId }));
                }
            }

            return await this.HybridFormResultAsync("HandpiecesEdit", this.RedirectToAction("Edit", new { id = entity.Id }));
        }

        private Task<IActionResult> GetDeleteSuccessResult(Handpiece entity, Dictionary<String, Object> arg2)
        {
            return this.HybridFormResultAsync("HandpiecesDelete", this.RedirectToAction("Edit", "Jobs", new { id = entity.JobId }));
        }

        private async Task BeforeStatusUpdated(Handpiece entity, JobHandpieceChangeStatusModel model, Dictionary<String, Object> additionalData)
        {
            var change = await this.changeTrackingService.CaptureEntityForUpdate(entity);
            additionalData["ChangeTrackingService.Change"] = change;
        }

        private async Task AfterStatusUpdated(Handpiece entity, JobHandpieceChangeStatusModel model, Dictionary<String, Object> additionalData)
        {
            var change = (HandpieceChange)additionalData["ChangeTrackingService.Change"];
            await this.changeTrackingService.TrackModifyEntityAsync(entity, change);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetSetStatusSuccessResult(Handpiece entity, JobHandpieceChangeStatusModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("HandpiecesSetStatus", new
            {
                status = entity.HandpieceStatus,
                visualisation = entity.HandpieceStatus.ToInternalVisualisationNumber(),
                description = entity.HandpieceStatus.ToInternalStatusDescription(),
            }, this.RedirectToAction("Edit", new { id = entity.JobId }));
        }

        private async Task InitializeSetDiagnosticsModel(Guid id, Handpiece entity, HandpieceSetDiagnosticsModel model, Boolean initial)
        {
            var types = await this.Repository.QueryWithoutTracking<DiagnosticCheckType>("Items", "Items.Item")
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            model.Handpiece = entity;
            model.Details = await this.ConvertToDetailsModel(entity);

            model.OtherSelected = !String.IsNullOrEmpty(entity.DiagnosticOther);
            model.OtherText = entity.DiagnosticOther;

            var existingItems = await this.Repository.Query<HandpieceDiagnostic>()
                .Where(x => x.HandpieceId == entity.Id)
                .ToListAsync();
            var existingItemsHashSet = new HashSet<Guid>(existingItems.Select(x => x.ItemId));

            model.DiagnosticTypes = types.Select(x => new HandpieceSetDiagnosticsModelType
            {
                Id = x.Id,
                Name = x.Name,
                Items = x.Items.OrderBy(y => y.OrderNo).Select(y => new SelectableEntity<NamedElement<Guid>>
                {
                    IsSelected = existingItemsHashSet.Contains(y.ItemId),
                    Item = new NamedElement<Guid>
                    {
                        Id = y.ItemId,
                        Name = y.Item.Name,
                    },
                }).ToList(),
            }).ToList();
        }
    }
}
