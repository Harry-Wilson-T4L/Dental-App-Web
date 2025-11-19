using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.Inventory;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Data;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.Ordering.SqlServer.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class InventoryController : BaseTelerikIndexlessHierarchicalDependentBasicCrudController<
        Guid, // TIdentifier
        InventorySKU, // TEntity
        Guid, // TParentIdentifier
        InventorySKUType, // TParentEntity
        InventorySKUReadModel, // TReadModel
        InventorySKU, // TDetailsModel
        InventorySKUCreateModel, // TCreateModel
        InventorySKUEditModel, // TEditModel
        InventorySKU> // TDeleteModel
    {
        private readonly INameNormalizationService nameNormalizationService;
        private readonly Lazy<IDateTimeService> dateTimeService;
        private readonly Lazy<IDataTransactionService> dataTransactionService;
        private readonly Lazy<IWorkshopManager> workshopManager;
        private readonly Lazy<UserEntityResolver> userEntityResolver;

        public InventoryController(IEntityControllerServices controllerServices, INameNormalizationService nameNormalizationService)
            : base(controllerServices)
        {
            this.nameNormalizationService = nameNormalizationService;
            this.dateTimeService = controllerServices.ServiceProvider.GetLazyService<IDateTimeService>();
            this.dataTransactionService = controllerServices.ServiceProvider.GetLazyService<IDataTransactionService>();
            this.workshopManager = controllerServices.ServiceProvider.GetLazyService<IWorkshopManager>();
            this.userEntityResolver = controllerServices.ServiceProvider.GetLazyService<UserEntityResolver>();

            this.ReadExHandler = new TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, InventorySKU, Guid, InventorySKUType, InventorySKUIntermediateReadModel, InventorySKUReadModel>(this, this.ControllerServices, this.DependentPermissionsValidator);
            this.EditGroupHandler = new BasicCrudEditActionHandler<Guid, InventorySKU, InventorySKU, InventorySKUEditGroupModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ConvertToLeafHandler = new BasicCrudCustomOperationActionHandler<Guid, InventorySKU, InventorySKUConvertToLeafModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.ConvertToGroupHandler = new BasicCrudCustomOperationActionHandler<Guid, InventorySKU, InventorySKUConvertToGroupModel>(this, this.ControllerServices, this.PermissionsValidator);
            this.MoveUpHandler = new BasicOrderDecreaseActionHandler<Guid, InventorySKU>(this, controllerServices, this.PermissionsValidator);
            this.MoveDownHandler = new BasicOrderIncreaseActionHandler<Guid, InventorySKU>(this, controllerServices, this.PermissionsValidator);

            this.ReadExHandler.Overrides.FinalizeQueryPreparation = this.PrepareReadQuery;
            this.ReadExHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.ValidateCreateModel = this.ValidateCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.AfterEntityCreated = this.AfterEntityCreated;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.ValidateEditModel = this.ValidateEditModel;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.EditGroupHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.EditGroupHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditGroupModelWithEntity;
            this.EditGroupHandler.Overrides.InitializeEditModel = this.InitializeEditGroupModel;
            this.EditGroupHandler.Overrides.ValidateEditModel = this.ValidateEditGroupModel;
            this.EditGroupHandler.Overrides.UpdateExistingEntity = this.UpdateExistingGroupEntity;
            this.EditGroupHandler.Overrides.GetEditSuccessResult = this.GetEditGroupSuccessResult;

            this.ConvertToLeafHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ConvertToLeafHandler.Overrides.InitializeOperationModel = this.InitializeConvertToLeafOperation;
            this.ConvertToLeafHandler.Overrides.ValidateOperationModel = this.ValidateConvertToLeafOperation;
            this.ConvertToLeafHandler.Overrides.ExecuteOperation = this.ExecuteConvertToLeafOperation;
            this.ConvertToLeafHandler.Overrides.GetOperationSuccessResult = this.GetConvertToLeafSuccessResult;

            this.ConvertToGroupHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.ConvertToGroupHandler.Overrides.InitializeOperationModel = this.InitializeConvertToGroupOperation;
            this.ConvertToGroupHandler.Overrides.ValidateOperationModel = this.ValidateConvertToGroupOperation;
            this.ConvertToGroupHandler.Overrides.ExecuteOperation = this.ExecuteConvertToGroupOperation;
            this.ConvertToGroupHandler.Overrides.GetOperationSuccessResult = this.GetConvertToGroupSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDeleteModel;
            this.DeleteHandler.Overrides.BeforeEntityDeleted = this.BeforeEntityDeleted;
            this.DeleteHandler.Overrides.DeleteEntity = this.DeleteEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;

            this.MoveUpHandler.Overrides.FindPrevious = this.FindPrevious;
            this.MoveDownHandler.Overrides.FindNext = this.FindNext;
        }

        protected IDateTimeService DateTimeService => this.dateTimeService.Value;

        protected IDataTransactionService DataTransactionService => this.dataTransactionService.Value;

        protected UserEntityResolver UserEntityResolver => this.userEntityResolver.Value;

        protected TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, InventorySKU, Guid, InventorySKUType, InventorySKUIntermediateReadModel, InventorySKUReadModel> ReadExHandler { get; }

        protected BasicCrudEditActionHandler<Guid, InventorySKU, InventorySKU, InventorySKUEditGroupModel> EditGroupHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, InventorySKU, InventorySKUConvertToLeafModel> ConvertToLeafHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, InventorySKU, InventorySKUConvertToGroupModel> ConvertToGroupHandler { get; }

        protected BasicOrderDecreaseActionHandler<Guid, InventorySKU> MoveUpHandler { get; }

        protected BasicOrderIncreaseActionHandler<Guid, InventorySKU> MoveDownHandler { get; }

        public async Task<IActionResult> Index(Guid? workshop)
        {
            var userAccess = await this.UserEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetInventoryAvailable();

            var model = new InventorySKUIndexModel();
            model.Workshops = await this.Repository.QueryWithoutTracking<Workshop>()
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .Where(x => x.DeletionStatus == DeletionStatus.Normal || x.Id == workshop)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
            if (workshop.HasValue)
            {
                model.SelectedWorkshop = await this.Repository.QueryWithoutTracking<Workshop>()
                    .Where(x => accessibleWorkshops.Contains(x.Id))
                    .SingleOrDefaultAsync(x => x.Id == workshop);
                if (model.SelectedWorkshop == null)
                {
                    return this.NotFound();
                }
            }

            return this.View(model);
        }

        [AjaxPost]
        public override Task<IActionResult> Read(Guid parentId, DataSourceRequest request) => this.ReadExHandler.Read(parentId, request);

        public Task<IActionResult> EditGroup(Guid id) => this.EditGroupHandler.Edit(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditGroup(Guid id, InventorySKUEditGroupModel model) => this.EditGroupHandler.Edit(id, model);

        public Task<IActionResult> ConvertToLeaf(Guid id) => this.ConvertToLeafHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ConvertToLeaf(Guid id, InventorySKUConvertToLeafModel model) => this.ConvertToLeafHandler.Execute(id, model);

        public Task<IActionResult> ConvertToGroup(Guid id) => this.ConvertToGroupHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ConvertToGroup(Guid id, InventorySKUConvertToGroupModel model) => this.ConvertToGroupHandler.Execute(id, model);

        [AjaxPost]
        public Task<IActionResult> MoveUp(Guid id) => this.MoveUpHandler.DecreaseOrder(id);

        [AjaxPost]
        public Task<IActionResult> MoveDown(Guid id) => this.MoveDownHandler.IncreaseOrder(id);

        #region Read

        private Task<IQueryable<InventorySKUIntermediateReadModel>> PrepareReadQuery(InventorySKUType skuType, IQueryable<InventorySKU> query)
        {
            var warningOnly = this.Request.Form.TryGetValue("warningOnly", out var warningOnlyValues) && warningOnlyValues.Any(x => x == "true");
            var workshop = this.Request.Form.TryGetValue("workshop", out var workshopRaw) && Guid.TryParse(workshopRaw, out var workshopGuid) ? workshopGuid : (Guid?)null;

            var dbContext = this.ControllerServices.ServiceProvider.GetRequiredService<DbContext>();
            var sqlText = @"select
  sku.[Id],
  sku.[TypeId],
  sku.[ParentId],
  sku.[OrderNo],
  parent.[DefaultChildId] as [ParentDefaultChildId],
  sku.[Name],
  sku.[Description],
  sku.[NodeType],
  sku.[AveragePrice],
  qty.[TotalQuantity],
  qty.[ShelfQuantity],
  qty.[TrayQuantity],
  qty.[OrderedQuantity],
  qty.[RequestedQuantity],
  warn.[HasWarning],
  wdes.[HasDescendantsWithWarning]
from [InventorySKUs] sku
left join [InventorySKUs] parent on sku.[ParentId] = parent.[Id]";

            if (workshop == null)
            {
                sqlText += Environment.NewLine + "left join [InventorySKUsQuantity] qty on sku.[Id] = qty.[Id]";
                sqlText += Environment.NewLine + "left join [InventorySKUsWarnings] warn on sku.[Id] = warn.[Id]";
                sqlText += Environment.NewLine + "left join [InventorySKUsDescendantsWarnings] wdes on sku.[Id] = wdes.[Id]";
            }
            else
            {
                sqlText += Environment.NewLine + "left join [InventorySKUsWorkshopQuantity] qty on sku.[Id] = qty.[Id] and qty.[WorkshopId] = @WorkshopId";
                sqlText += Environment.NewLine + "left join [InventorySKUsWorkshopWarnings] warn on sku.[Id] = warn.[Id] and warn.[WorkshopId] = @WorkshopId";
                sqlText += Environment.NewLine + "left join [InventorySKUsWorkshopDescendantsWarnings] wdes on sku.[Id] = wdes.[Id] and wdes.[WorkshopId] = @WorkshopId";
            }

            sqlText += Environment.NewLine + "where sku.[TypeId] = @TypeId and sku.[DeletionStatus] = 0";

            if (warningOnly)
            {
                sqlText += Environment.NewLine + "  and ((warn.[HasWarning] is not null and warn.[HasWarning] > 0) or (wdes.[HasDescendantsWithWarning] is not null and wdes.[HasDescendantsWithWarning] > 0))";
            }

            var parameters = new List<Object>();
            parameters.Add(new SqlParameter("TypeId", SqlDbType.UniqueIdentifier) { Value = skuType.Id });
            if (workshop != null)
            {
                parameters.Add(new SqlParameter("WorkshopId", SqlDbType.UniqueIdentifier) { Value = workshop });
            }

            var result = dbContext.Set<InventorySKUIntermediateReadModel>().FromSqlRaw(sqlText, parameters.ToArray());
            return Task.FromResult(result);
        }

        private InventorySKUReadModel ConvertEntityToViewModel(InventorySKUType skuType, InventorySKUIntermediateReadModel sku, String[] allowedProperties)
        {
            var readModel = new InventorySKUReadModel
            {
                Id = sku.Id,
                Name = sku.Name,
                TypeId = skuType.Id,
                ParentId = sku.ParentId,
                OrderNo = sku.OrderNo,
                AveragePrice = sku.AveragePrice,
                Description = sku.Description,
                TotalQuantity = sku.TotalQuantity ?? 0m,
                ShelfQuantity = sku.ShelfQuantity ?? 0m,
                TrayQuantity = sku.TrayQuantity ?? 0m,
                OrderedQuantity = sku.OrderedQuantity ?? 0m,
                RequestedQuantity = sku.RequestedQuantity ?? 0m,
                TotalPrice = sku.AveragePrice.HasValue ? (sku.AveragePrice.Value * (sku.TotalQuantity ?? 0m)) : null,
                NodeType = sku.NodeType,
                IsDefaultChild = sku.ParentDefaultChildId == sku.Id,
                HasWarning = sku.HasWarning > 0,
                HasDescendantsWithWarning = sku.HasDescendantsWithWarning > 0,
            };

            return readModel;
        }

        #endregion // Read

        #region Create

        private async Task InitializeCreateModel(InventorySKUType skuType, InventorySKU parentSKU, InventorySKUCreateModel model, Boolean initial)
        {
            var workshops = await this.Repository.QueryWithoutTracking<Workshop>().OrderBy(x => x.OrderNo).ToListAsync();
            var workshopsMap = workshops.ToDictionary(x => x.Id);
            if (initial)
            {
                model.WorkshopOptions = workshops
                    .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                    .Select(x => new InventorySKUWorkshopOptionsEditModel { WorkshopId = x.Id, WarningQuantity = null, })
                    .ToList();
            }

            model.Type = skuType;
            model.Parent = parentSKU;
            model.WorkshopOptions.ForEach(x =>
            {
                x.Workshop = workshopsMap[x.WorkshopId];
            });
        }

        private Task<Boolean> ValidateCreateModel(InventorySKUType skuType, InventorySKU parentSKU, InventorySKUCreateModel model)
        {
            if (skuType.DeletionStatus != DeletionStatus.Normal)
            {
                this.ModelState.AddModelError(String.Empty, "SKU Type has been deleted.");
                return Task.FromResult(false);
            }

            if (parentSKU != null && parentSKU.NodeType == InventorySKUNodeType.Leaf)
            {
                this.ModelState.AddModelError(String.Empty, "Child SKUs can only be added to group SKU.");
                return Task.FromResult(false);
            }

            if (parentSKU != null && parentSKU.DeletionStatus != DeletionStatus.Normal)
            {
                this.ModelState.AddModelError(String.Empty, "Parent SKU has been deleted.");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private async Task InitializeNewEntity(InventorySKUType skuType, InventorySKU parentSKU, InventorySKU sku, InventorySKUCreateModel model)
        {
            sku.Name = model.Name;
            sku.NormalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            sku.Description = model.Description;
            if (model.IsGroupNode)
            {
                sku.NodeType = InventorySKUNodeType.Group;
                sku.AveragePrice = null;
            }
            else
            {
                sku.NodeType = InventorySKUNodeType.Leaf;
                sku.AveragePrice = model.AveragePrice;
            }

            sku.ApplyWorkshopOptions(model.WorkshopOptions);

            var typeId = skuType.Id;
            var parentId = parentSKU?.Id;

            var lastOrderItem = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.TypeId == typeId && x.ParentId == parentId)
                .OrderByDescending(x => x.OrderNo)
                .FirstOrDefaultAsync();

            sku.OrderNo = (lastOrderItem?.OrderNo ?? 0) + 1;

            var lastAlphaOrderItem = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.TypeId == typeId && x.ParentId == parentId)
                .Where(x => sku.Name.CompareTo(x.Name) > 0)
                .OrderByDescending(x => x.Name)
                .FirstOrDefaultAsync();

            List<InventorySKU> itemsAfterNew;
            if (lastAlphaOrderItem == null)
            {
                itemsAfterNew = await this.Repository.Query<InventorySKU>()
                    .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                    .Where(x => x.TypeId == typeId && x.ParentId == parentId)
                    .OrderBy(x => x.OrderNo)
                    .ToListAsync();
            }
            else
            {
                itemsAfterNew = await this.Repository.Query<InventorySKU>()
                    .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                    .Where(x => x.TypeId == typeId && x.ParentId == parentId)
                    .Where(x => x.OrderNo > lastAlphaOrderItem.OrderNo)
                    .OrderBy(x => x.OrderNo)
                    .ToListAsync();
            }

            var nextOrder = sku.OrderNo + 1;
            foreach (var item in itemsAfterNew)
            {
                item.OrderNo = nextOrder++;
                await this.Repository.UpdateAsync(item);
            }
        }

        private async Task AfterEntityCreated(InventorySKUType type, InventorySKU parent, InventorySKU entity, InventorySKUCreateModel model, Dictionary<String, Object> additionalData)
        {
            var manager = this.ControllerServices.ServiceProvider.GetRequiredService<IInventorySKUManager>();
            var workshopManager = this.ControllerServices.ServiceProvider.GetRequiredService<IWorkshopManager>();
            var workshopSydney = await workshopManager.GetSydneyAsync();
            var sku = await manager.GetByIdAsync(entity.Id) ?? throw new InvalidOperationException("Unable to load domain object after entity was created");

            if (model.InitialShelfQuantity.HasValue && model.InitialShelfQuantity.Value > 0)
            {
                await sku.Movements.CreateAsync<IInventoryInitialMovement, InventoryMovementBuilder.Initial>(
                    workshopSydney,
                    model.InitialShelfQuantity.Value);
            }

            if (model.InitialOrderedQuantity.HasValue && model.InitialOrderedQuantity.Value > 0)
            {
                await sku.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                    workshopSydney,
                    model.InitialOrderedQuantity.Value,
                    builder => builder.WithStatus(InventoryMovementStatus.Ordered));
            }

            if (model.InitialRequestedQuantity.HasValue && model.InitialRequestedQuantity.Value > 0)
            {
                await sku.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                    workshopSydney,
                    model.InitialRequestedQuantity.Value,
                    builder => builder.WithStatus(InventoryMovementStatus.Requested));
            }

            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetCreateSuccessResult(InventorySKUType skuType, InventorySKU parentSKU, InventorySKU sku, InventorySKUCreateModel model, Dictionary<String, Object> additionalData)
        {
            return Task.FromResult(this.HybridFormResult("InventoryCreate", this.RedirectToAction("Index", "Inventory")));
        }

        #endregion // Create

        #region Edit

        private async Task InitializeEditModelWithEntity(InventorySKU entity, InventorySKUEditModel model)
        {
            if (entity.NodeType != InventorySKUNodeType.Leaf)
            {
                throw new InvalidOperationException("This mode of editing only supports Leaf");
            }

            var workshops = await this.Repository.QueryWithoutTracking<Workshop>().OrderBy(x => x.OrderNo).ToListAsync();

            model.Name = entity.Name;
            model.AveragePrice = entity.AveragePrice;
            model.Description = entity.Description;
            model.HideFromStatistic = entity.HideFromStatistic;
            model.WorkshopOptions = workshops
                .Where(x => x.DeletionStatus == DeletionStatus.Normal || entity.WorkshopOptions.Any(y => y.WorkshopId == x.Id))
                .Select(x => new InventorySKUWorkshopOptionsEditModel
                {
                    WorkshopId = x.Id,
                    Workshop = x,
                    WarningQuantity = entity.WorkshopOptions.SingleOrDefault(y => y.WorkshopId == x.Id)?.WarningQuantity,
                })
                .ToList();
        }

        private Task<Boolean> ValidateEditModel(InventorySKU entity, InventorySKUEditModel model)
        {
            if (entity.NodeType != InventorySKUNodeType.Leaf)
            {
                this.ModelState.AddModelError(String.Empty, "This mode of editing only supports Leaf SKU");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private Task UpdateExistingEntity(InventorySKU entity, InventorySKUEditModel model)
        {
            if (entity.NodeType != InventorySKUNodeType.Leaf)
            {
                throw new InvalidOperationException("This mode of editing only supports Leaf");
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AveragePrice = model.AveragePrice;
            entity.HideFromStatistic = model.HideFromStatistic;
            entity.ApplyWorkshopOptions(model.WorkshopOptions);

            return Task.CompletedTask;
        }

        private Task<IActionResult> GetEditSuccessResult(InventorySKU entity, InventorySKUEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("InventoryEdit", this.RedirectToAction("Index", "Inventory"));
        }

        #endregion // Edit

        #region EditGroup

        private async Task InitializeEditGroupModelWithEntity(InventorySKU entity, InventorySKUEditGroupModel model)
        {
            if (entity.NodeType != InventorySKUNodeType.Group)
            {
                throw new InvalidOperationException("This mode of editing only supports Group");
            }

            var workshops = await this.Repository.QueryWithoutTracking<Workshop>().OrderBy(x => x.OrderNo).ToListAsync();

            model.Name = entity.Name;
            model.Description = entity.Description;
            model.DefaultChild = entity.DefaultChildId;
            model.HideFromStatistic = entity.HideFromStatistic;
            model.WorkshopOptions = workshops
                .Where(x => x.DeletionStatus == DeletionStatus.Normal || entity.WorkshopOptions.Any(y => y.WorkshopId == x.Id))
                .Select(x => new InventorySKUWorkshopOptionsEditModel
                {
                    WorkshopId = x.Id,
                    Workshop = x,
                    WarningQuantity = entity.WorkshopOptions.SingleOrDefault(y => y.WorkshopId == x.Id)?.WarningQuantity,
                })
                .ToList();
        }

        private async Task InitializeEditGroupModel(InventorySKU entity, InventorySKUEditGroupModel model, Boolean initial)
        {
            model.Original = entity;
            model.Children = await this.Repository.Query<InventorySKU>().Where(x => x.ParentId == entity.Id && x.DeletionStatus == DeletionStatus.Normal).OrderBy(x => x.Name).ToListAsync();
        }

        private async Task<Boolean> ValidateEditGroupModel(InventorySKU entity, InventorySKUEditGroupModel model)
        {
            if (entity.NodeType != InventorySKUNodeType.Group)
            {
                this.ModelState.AddModelError(String.Empty, "This mode of editing only supports Group SKU");
                return false;
            }

            if (model.DefaultChild.HasValue)
            {
                var foundChild = await this.Repository.QueryWithoutTracking<InventorySKU>().SingleOrDefaultAsync(x => x.Id == model.DefaultChild);
                if (foundChild == null)
                {
                    this.ModelState.AddModelError(nameof(model.DefaultChild), "SKU not found");
                    return false;
                }
                else if (foundChild.ParentId != entity.Id)
                {
                    this.ModelState.AddModelError(nameof(model.DefaultChild), "SKU belong to different group");
                    return false;
                }
            }

            return true;
        }

        private Task UpdateExistingGroupEntity(InventorySKU entity, InventorySKUEditGroupModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.DefaultChildId = model.DefaultChild;
            entity.HideFromStatistic = model.HideFromStatistic;
            entity.ApplyWorkshopOptions(model.WorkshopOptions);

            return Task.CompletedTask;
        }

        private Task<IActionResult> GetEditGroupSuccessResult(InventorySKU entity, InventorySKUEditGroupModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("InventoryEditGroup", this.RedirectToAction("Index", "Inventory"));
        }

        #endregion // EditGroup

        #region ConvertToLeaf

        private async Task InitializeConvertToLeafOperation(Guid id, InventorySKU entity, InventorySKUConvertToLeafModel model, Boolean initial)
        {
            model.Entity = entity;
            model.HasChildren = await this.Repository.Query<InventorySKU>().AnyAsync(x => x.ParentId == entity.Id);
        }

        private async Task<Boolean> ValidateConvertToLeafOperation(Guid id, InventorySKU entity, InventorySKUConvertToLeafModel model)
        {
            if (await this.Repository.Query<InventorySKU>().AnyAsync(x => x.ParentId == entity.Id))
            {
                this.ModelState.AddModelError(String.Empty, "Can't convert group with children to leaf");
                return false;
            }

            return true;
        }

        private async Task ExecuteConvertToLeafOperation(Guid id, InventorySKU entity, InventorySKUConvertToLeafModel model)
        {
            entity.NodeType = InventorySKUNodeType.Leaf;
            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetConvertToLeafSuccessResult(Guid id, InventorySKU entity, InventorySKUConvertToLeafModel model)
        {
            return this.HybridFormResultAsync("InventoryConvertToLeaf", this.RedirectToAction("Index", "Inventory"));
        }

        #endregion // ConvertToLeaf

        #region ConvertToGroup

        private async Task InitializeConvertToGroupOperation(Guid id, InventorySKU entity, InventorySKUConvertToGroupModel model, Boolean initial)
        {
            model.Entity = entity;
            model.HasMovements = await this.Repository.QueryWithoutTracking<InventoryMovement>().AnyAsync(x => x.SKUId == entity.Id);
            if (model.HasMovements)
            {
                model.CreateNewGroup = true;
            }

            if (initial)
            {
                model.GroupName = entity.Name;
                model.LeafName = entity.Name;
            }
        }

        private async Task<Boolean> ValidateConvertToGroupOperation(Guid id, InventorySKU entity, InventorySKUConvertToGroupModel model)
        {
            var hasErrors = false;
            var hasMovements = await this.Repository.QueryWithoutTracking<InventoryMovement>().AnyAsync(x => x.SKUId == entity.Id);
            if (hasMovements && !model.CreateNewGroup)
            {
                this.ModelState.AddModelError(String.Empty, "Can't convert leaf to group without creating new group if it already has movements");
                hasErrors = true;
            }

            if (model.CreateNewGroup && String.IsNullOrEmpty(model.GroupName))
            {
                this.ModelState.AddModelError(nameof(model.GroupName), "Required field");
                hasErrors = true;
            }

            if (model.CreateNewGroup && String.IsNullOrEmpty(model.LeafName))
            {
                this.ModelState.AddModelError(nameof(model.LeafName), "Required field");
                hasErrors = true;
            }

            return !hasErrors;
        }

        private async Task ExecuteConvertToGroupOperation(Guid id, InventorySKU entity, InventorySKUConvertToGroupModel model)
        {
            if (model.CreateNewGroup)
            {
                await using var transaction = await this.DataTransactionService.BeginTransactionAsync();
                var entityOrderNo = entity.OrderNo;
                var lastSKU = await this.Repository.QueryWithoutTracking<InventorySKU>()
                    .Where(x => x.DeletionStatus == DeletionStatus.Normal && x.TypeId == entity.TypeId && x.ParentId == entity.ParentId)
                    .OrderByDescending(x => x.OrderNo)
                    .FirstOrDefaultAsync();
                entity.OrderNo = lastSKU.OrderNo + 1;

                await this.Repository.UpdateAsync(entity);
                await this.Repository.SaveChangesAsync();

                var group = new InventorySKU
                {
                    Id = Guid.NewGuid(),
                    Name = model.GroupName,
                    NormalizedName = this.nameNormalizationService.NormalizeName(model.GroupName),
                    NodeType = InventorySKUNodeType.Group,
                    TypeId = entity.TypeId,
                    ParentId = entity.ParentId,
                    OrderNo = entityOrderNo,
                };

                await this.Repository.InsertAsync(@group);
                await this.Repository.SaveChangesAsync();

                entity.Name = model.LeafName;
                entity.NormalizedName = this.nameNormalizationService.NormalizeName(model.LeafName);
                entity.ParentId = @group.Id;
                entity.OrderNo = 1;

                await this.Repository.UpdateAsync(entity);
                await this.Repository.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            else
            {
                entity.NodeType = InventorySKUNodeType.Group;
                await this.Repository.UpdateAsync(entity);
                await this.Repository.SaveChangesAsync();
            }
        }

        private Task<IActionResult> GetConvertToGroupSuccessResult(Guid id, InventorySKU entity, InventorySKUConvertToGroupModel model)
        {
            return this.HybridFormResultAsync("InventoryConvertToGroup", this.RedirectToAction("Index", "Inventory"));
        }

        #endregion // ConvertToGroup

        #region Delete

        private async Task<InventorySKU> ConvertToDeleteModel(InventorySKU entity)
        {
            var childrenSKUCount = await this.Repository.QueryWithoutTracking<InventorySKU>().CountAsync(x => x.TypeId == entity.TypeId && x.ParentId == entity.Id && x.DeletionStatus == DeletionStatus.Normal);
            var activeMovements = await this.Repository.QueryWithoutTracking<InventoryMovement>()
                .Where(x => x.SKUId == entity.Id)
                .Where(x => x.Type == InventoryMovementType.Repair || x.Type == InventoryMovementType.Order)
                .Where(x => x.Status != InventoryMovementStatus.Completed && x.Status != InventoryMovementStatus.Cancelled)
                .CountAsync();
            var allMovements = await this.Repository.QueryWithoutTracking<InventoryMovement>()
                .Where(x => x.SKUId == entity.Id)
                .Where(x => x.Type != InventoryMovementType.Initial)
                .CountAsync();

            this.ViewBag.ChildrenSKUCount = childrenSKUCount;
            this.ViewBag.ActiveMovementsCount = activeMovements;
            this.ViewBag.AllMovementsCount = allMovements;
            return entity;
        }

        private async Task BeforeEntityDeleted(InventorySKU entity, Dictionary<String, Object> additionalData)
        {
            var childrenSKUCount = await this.ControllerServices.Repository.Query<InventorySKU>().CountAsync(x => x.TypeId == entity.TypeId && x.ParentId == entity.Id && x.DeletionStatus == DeletionStatus.Normal);
            if (childrenSKUCount > 0)
            {
                throw new InvalidOperationException("Unable to delete SKU with existing children");
            }

            var activeMovements = await this.Repository.QueryWithoutTracking<InventoryMovement>()
                .Where(x => x.SKUId == entity.Id)
                .Where(x => x.Type == InventoryMovementType.Repair || x.Type == InventoryMovementType.Order)
                .Where(x => x.Status != InventoryMovementStatus.Completed && x.Status != InventoryMovementStatus.Cancelled)
                .CountAsync();
            if (activeMovements > 0)
            {
                throw new InvalidOperationException("Unable to delete SKU with active movements");
            }
        }

        private async Task DeleteEntity(InventorySKU entity, Dictionary<String, Object> additionalData)
        {
            entity.DeletionStatus = DeletionStatus.Deleted;
            entity.DeletionDate = this.DateTimeService.CurrentUtcTime;
            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetDeleteSuccessResult(InventorySKU entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("InventoryDelete", this.RedirectToAction("Index", "Inventory"));
        }

        #endregion // Delete

        #region Statistics

        [AjaxPost]
        public async Task<IActionResult> Statistics(Guid type, Guid? workshop, [DataSourceRequest] DataSourceRequest request)
        {
            var requestedWorkshop = workshop == null ? null : await this.workshopManager.Value.GetByIdAsync(workshop.Value);
            if (requestedWorkshop == null && workshop.HasValue)
            {
                return this.NotFound();
            }

            var skuType = await this.ControllerServices.ServiceProvider.GetRequiredService<IInventorySKUTypeManager>().GetByIdAsync(type);
            if (skuType == null || skuType.StatisticsMode == InventorySKUTypeStatisticsMode.Hidden)
            {
                return this.NotFound();
            }

            var sql = @"select
  sku.[Id],
  sku.[TypeId],
  tp.[Name] as [TypeName],
  h.[HierarchyOrderNo] as [OrderNo],
  sku.[Name],
  coalesce(qty.[TotalQuantity], 0) as [TotalQuantity],
  coalesce(qty.[ShelfQuantity], 0) as [ShelfQuantity],
  coalesce(qty.[TrayQuantity], 0) as [TrayQuantity],
  coalesce(qty.[OrderedQuantity], 0) as [OrderedQuantity],
  coalesce(qty.[RequestedQuantity], 0) as [RequestedQuantity]
from [InventorySKUs] sku
inner join [InventorySKUTypes] tp on tp.[Id] = sku.[TypeId]
inner join [InventorySKUsHierarchy] h on sku.[Id] = h.[Id]";

            if (requestedWorkshop == null)
            {
                sql += Environment.NewLine + "left join [InventorySKUsQuantity] qty on sku.[Id] = qty.[Id]";
            }
            else
            {
                sql += Environment.NewLine + "left join [InventorySKUsWorkshopQuantity] qty on sku.[Id] = qty.[Id] and qty.[WorkshopId] = @WorkshopId";
            }

            sql += Environment.NewLine + "where sku.[DeletionStatus] = 0 and sku.[HideFromStatistic] = 0";

            switch (skuType.StatisticsMode)
            {
                case InventorySKUTypeStatisticsMode.ShowTopLevel:
                    sql += " and h.[Level] = 0";
                    break;
                case InventorySKUTypeStatisticsMode.ShowSpecificSKUs:
                    sql += " and sku.[NodeType] = 0";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var parameters = new List<SqlParameter>();
            if (requestedWorkshop != null)
            {
                parameters.Add(new SqlParameter("WorkshopId", SqlDbType.UniqueIdentifier) { Value = requestedWorkshop.Id });
            }

            var query = this.ControllerServices.ServiceProvider.GetRequiredService<DbContext>().Set<InventorySKUAvailableStatsModel>().FromSqlRaw(sql, parameters.Cast<Object>().ToArray());

            query = query.Where(x => x.TypeId == type);
            query = query.Where(x => x.TotalQuantity > 0 || x.ShelfQuantity > 0 || x.TrayQuantity > 0 || x.OrderedQuantity > 0 || x.RequestedQuantity > 0);

            var result = await query.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        #endregion

        #region Ordering

        private async Task<InventorySKU> FindPrevious(Guid id, InventorySKU sku)
        {
            var list = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.TypeId == sku.TypeId && x.ParentId == sku.ParentId)
                .Where(x => x.OrderNo < sku.OrderNo)
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            return list.FirstOrDefault();
        }

        private async Task<InventorySKU> FindNext(Guid id, InventorySKU sku)
        {
            var list = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.TypeId == sku.TypeId && x.ParentId == sku.ParentId)
                .Where(x => x.OrderNo > sku.OrderNo)
                .OrderBy(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            return list.FirstOrDefault();
        }

        #endregion

        public async Task<IActionResult> FullCheck()
        {
            return this.View();
        }

        [HttpPost]
        [ActionName("FullCheck")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FullCheckConfirm()
        {
            var allSKUs = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Where(x => x.NodeType == InventorySKUNodeType.Leaf && x.DeletionStatus == DeletionStatus.Normal && x.Type.DeletionStatus == DeletionStatus.Normal)
                .ToListAsync();

            foreach (var skuData in allSKUs)
            {
                var sku = await this.ControllerServices.ServiceProvider.GetRequiredService<IInventorySKUManager>().GetByIdAsync(skuData.Id);
                await using var transaction = await this.ControllerServices.ServiceProvider.GetRequiredService<IDataTransactionService>().BeginTransactionAsync();
                await sku.TryProcessMovementsChangesForAllAsync();
                await transaction.CommitAsync();
            }

            var handpieces = await this.Repository.QueryWithoutTracking<Handpiece>()
                .Where(x => x.HandpieceStatus != HandpieceStatus.SentComplete && x.HandpieceStatus != HandpieceStatus.Cancelled && x.PartsVersion == HandpiecePartsVersion.InventorySKUv1)
                .ToListAsync();

            foreach (var handpieceData in handpieces)
            {
                var handpiece = await this.ControllerServices.ServiceProvider.GetRequiredService<IHandpieceManager>().GetByIdAsync(handpieceData.Id);
                await using var transaction = await this.ControllerServices.ServiceProvider.GetRequiredService<IDataTransactionService>().BeginTransactionAsync();
                await handpiece.Parts.UpdateStockStatusAsync(trackChange: true);
                await transaction.CommitAsync();
            }

            return this.RedirectToAction("Index");
        }

        private Task<InventorySKU> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<InventorySKU>()
                .Include(x => x.Type)
                .Include(x => x.WorkshopOptions)
                .ThenInclude(x => x.Workshop)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
