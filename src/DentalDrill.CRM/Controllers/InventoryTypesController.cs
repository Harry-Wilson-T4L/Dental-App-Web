using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Controllers.Base;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.InventoryTypes;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Permissions;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class InventoryTypesController : BaseTelerikIndexlessOrderableBasicCrudController<
        Guid, // TIdentifier
        InventorySKUType, // TEntity
        InventorySKUTypeReadModel, // TReadModel
        InventorySKUType, // TDetailsModel
        InventorySKUTypeEditModel, // TCreateModel
        InventorySKUTypeEditModel, // TEditModel
        InventorySKUType> // TDeleteModel
    {
        private readonly INameNormalizationService nameNormalizationService;
        private readonly Lazy<IDateTimeService> dateTimeService;

        public InventoryTypesController(IEntityControllerServices controllerServices, INameNormalizationService nameNormalizationService)
            : base(controllerServices)
        {
            this.nameNormalizationService = nameNormalizationService;
            this.dateTimeService = controllerServices.ServiceProvider.GetLazyService<IDateTimeService>();

            this.SortHandler = new BasicCrudCustomOperationActionHandler<Guid, InventorySKUType, InventorySKUTypeSortModel>(this, this.ControllerServices, this.PermissionsValidator);

            this.AjaxReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.AjaxReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.EditHandler.Overrides.InitializeEditModelWithEntity = this.InitializeEditModelWithEntity;
            this.EditHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.EditHandler.Overrides.GetEditSuccessResult = this.GetEditSuccessResult;

            this.DeleteHandler.Overrides.ConvertToDeleteModel = this.ConvertToDeleteModel;
            this.DeleteHandler.Overrides.BeforeEntityDeleted = this.BeforeEntityDeleted;
            this.DeleteHandler.Overrides.DeleteEntity = this.DeleteEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;

            this.MoveUpHandler.Overrides.FindPrevious = this.FindPrevious;
            this.MoveDownHandler.Overrides.FindNext = this.FindNext;

            this.SortHandler.Overrides.InitializeOperationModel = this.InitializeSortModel;
            this.SortHandler.Overrides.ValidateOperationModel = this.ValidateSortModel;
            this.SortHandler.Overrides.ExecuteOperation = this.ExecuteSortOperation;
            this.SortHandler.Overrides.GetOperationSuccessResult = this.GetSortSuccessResult;
        }

        protected IDateTimeService DateTimeService => this.dateTimeService.Value;

        protected BasicCrudCustomOperationActionHandler<Guid, InventorySKUType, InventorySKUTypeSortModel> SortHandler { get; }

        public Task<IActionResult> Sort(Guid id) => this.SortHandler.Execute(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Sort(Guid id, InventorySKUTypeSortModel model) => this.SortHandler.Execute(id, model);

        protected override IEntityPermissionsValidator<InventorySKUType> GetPermissionsValidator()
        {
            return new CustomPermissionsValidator(
                this.ControllerServices.PermissionsHub,
                this.ControllerServices.ServiceProvider.GetRequiredService<UserEntityResolver>());
        }

        private async Task<IQueryable<InventorySKUType>> PrepareReadQuery()
        {
            var warningOnly = this.Request.Form["warningOnly"].AsBooleanOrDefault();
            var skuName = this.Request.Form["skuName"].AsStringOrDefault();

            var query = this.Repository.QueryWithoutTracking<InventorySKUType>();

            var limitTypes = new List<Guid>();
            if (warningOnly)
            {
                limitTypes = await this.Repository.QueryWithoutTracking<InventorySKUWarning>()
                    .Where(warnings => warnings.HasWarning > 0)
                    .GroupJoin(this.Repository.QueryWithoutTracking<InventorySKU>(), warning => warning.Id, sku => sku.Id, (warning, sku) => sku)
                    .SelectMany(x => x)
                    .Select(x => x.TypeId)
                    .Distinct()
                    .ToListAsync();
            }

            if (String.IsNullOrEmpty(skuName))
            {
                if (limitTypes.Count == 0)
                {
                    query = query.Include(x => x.SKUs.Where(y => y.DeletionStatus == DeletionStatus.Normal));
                }
                else
                {
                    var warnings = this.Repository.QueryWithoutTracking<InventorySKUWarning>();
                    query = query.Include(x => x.SKUs.Where(y => y.DeletionStatus == DeletionStatus.Normal && warnings.Any(z => z.Id == y.Id && z.HasWarning > 0)));
                }
            }
            else
            {
                if (limitTypes.Count == 0)
                {
                    query = query.Include(x => x.SKUs.Where(y => y.DeletionStatus == DeletionStatus.Normal && y.Name.Contains(skuName)));
                }
                else
                {
                    var warnings = this.Repository.QueryWithoutTracking<InventorySKUWarning>();
                    query = query.Include(x => x.SKUs.Where(y => y.DeletionStatus == DeletionStatus.Normal && y.Name.Contains(skuName) && warnings.Any(z => z.Id == y.Id && z.HasWarning > 0)));
                }
            }

            query = query.Where(x => x.DeletionStatus == DeletionStatus.Normal);
            return query;
        }

        private InventorySKUTypeReadModel ConvertEntityToViewModel(InventorySKUType skuType, String[] allowedProperties)
        {
            return new InventorySKUTypeReadModel
            {
                Id = skuType.Id,
                Name = skuType.Name,
                HandpieceSpeedCompatibility = skuType.HandpieceSpeedCompatibility ?? HandpieceSpeedCompatibility.All,
                OrderNo = skuType.OrderNo,
                SKUCount = skuType.SKUs.Count,
            };
        }

        private async Task InitializeNewEntity(InventorySKUType entity, InventorySKUTypeEditModel model)
        {
            entity.Name = model.Name;
            entity.NormalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            entity.HandpieceSpeedCompatibility = model.HandpieceSpeedCompatibility.CombineValueOrDefault();
            entity.StatisticsMode = model.StatisticsMode;

            var nextOrderQuery = this.Repository.QueryWithoutTracking<InventorySKUType>().Where(x => x.DeletionStatus == DeletionStatus.Normal);
            entity.OrderNo = await nextOrderQuery.AnyAsync() ? await nextOrderQuery.MaxAsync(x => x.OrderNo) + 1 : 1;
        }

        private Task<IActionResult> GetCreateSuccessResult(InventorySKUType entity, InventorySKUTypeEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("InventoryTypesCreate", this.RedirectToAction("Index", "Inventory"));
        }

        private Task InitializeEditModelWithEntity(InventorySKUType entity, InventorySKUTypeEditModel model)
        {
            model.Name = entity.Name;
            model.HandpieceSpeedCompatibility = (entity.HandpieceSpeedCompatibility ?? HandpieceSpeedCompatibility.None).SplitValue();
            model.StatisticsMode = entity.StatisticsMode;
            return Task.CompletedTask;
        }

        private Task UpdateExistingEntity(InventorySKUType entity, InventorySKUTypeEditModel model)
        {
            entity.Name = model.Name;
            entity.NormalizedName = this.nameNormalizationService.NormalizeName(model.Name);
            entity.HandpieceSpeedCompatibility = model.HandpieceSpeedCompatibility.CombineValueOrDefault();
            entity.StatisticsMode = model.StatisticsMode;

            return Task.CompletedTask;
        }

        private Task<IActionResult> GetEditSuccessResult(InventorySKUType entity, InventorySKUTypeEditModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("InventoryTypesEdit", this.RedirectToAction("Index", "Inventory"));
        }

        private async Task<InventorySKUType> ConvertToDeleteModel(InventorySKUType entity)
        {
            var skuCount = await this.Repository.Query<InventorySKU>().CountAsync(x => x.TypeId == entity.Id && x.DeletionStatus == DeletionStatus.Normal);
            this.ViewBag.SKUCount = skuCount;
            return entity;
        }

        private async Task BeforeEntityDeleted(InventorySKUType entity, Dictionary<String, Object> additionalData)
        {
            var skuCount = await this.Repository.Query<InventorySKU>().CountAsync(x => x.TypeId == entity.Id && x.DeletionStatus == DeletionStatus.Normal);
            if (skuCount > 0)
            {
                throw new InvalidOperationException("Unable to delete SKU type with existing SKUs");
            }
        }

        private async Task DeleteEntity(InventorySKUType entity, Dictionary<String, Object> additionalData)
        {
            entity.DeletionStatus = DeletionStatus.Deleted;
            entity.DeletionDate = this.DateTimeService.CurrentUtcTime;
            await this.Repository.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }

        private Task<IActionResult> GetDeleteSuccessResult(InventorySKUType entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("InventoryTypesDelete", this.RedirectToAction("Index", "Inventory"));
        }

        private async Task<InventorySKUType> FindPrevious(Guid id, InventorySKUType entity)
        {
            var list = await this.Repository.Query<InventorySKUType>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.OrderNo < entity.OrderNo)
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            return list.FirstOrDefault();
        }

        private async Task<InventorySKUType> FindNext(Guid id, InventorySKUType entity)
        {
            var list = await this.Repository.Query<InventorySKUType>()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.OrderNo > entity.OrderNo)
                .OrderBy(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            return list.FirstOrDefault();
        }

        private async Task InitializeSortModel(Guid id, InventorySKUType entity, InventorySKUTypeSortModel model, Boolean initial)
        {
            model.Original = entity;
            model.AvailableGroupSKUs = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Where(x => x.TypeId == entity.Id && x.NodeType == InventorySKUNodeType.Group && x.DeletionStatus == DeletionStatus.Normal)
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (initial)
            {
                model.Scope = InventorySKUTypeSortScope.All;
                model.Method = InventorySKUTypeSortMethod.AlphaAscending;
                model.SpecificSKU = null;
            }
        }

        private Task<Boolean> ValidateSortModel(Guid id, InventorySKUType entity, InventorySKUTypeSortModel model)
        {
            if (model.Scope == InventorySKUTypeSortScope.Specific || model.Scope == InventorySKUTypeSortScope.SpecificRecursive)
            {
                if (model.SpecificSKU == null)
                {
                    this.ModelState.AddModelError(nameof(model.SpecificSKU), "Specific SKU must be selected when choosing Specific scope");
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }

        private async Task ExecuteSortOperation(Guid id, InventorySKUType entity, InventorySKUTypeSortModel model)
        {
            Func<List<InventorySKU>, Task<List<InventorySKU>>> sortMethod = model.Method switch
            {
                InventorySKUTypeSortMethod.AlphaAscending => SortAlphaAscending,
                InventorySKUTypeSortMethod.AlphaDescending => SortAlphaDescending,
                _ => throw new ArgumentOutOfRangeException(),
            };

            switch (model.Scope)
            {
                case InventorySKUTypeSortScope.All:
                    await SortSKUChildren(entity.Id, null, sortMethod, true);
                    break;
                case InventorySKUTypeSortScope.TopLevel:
                    await SortSKUChildren(entity.Id, null, sortMethod, false);
                    break;
                case InventorySKUTypeSortScope.Specific:
                    await SortSKUChildren(entity.Id, model.SpecificSKU, sortMethod, false);
                    break;
                case InventorySKUTypeSortScope.SpecificRecursive:
                    await SortSKUChildren(entity.Id, model.SpecificSKU, sortMethod, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await this.Repository.SaveChangesAsync();

            static Task<List<InventorySKU>> SortAlphaAscending(List<InventorySKU> list)
            {
                return Task.FromResult(list.OrderBy(x => x.Name).ToList());
            }

            static Task<List<InventorySKU>> SortAlphaDescending(List<InventorySKU> list)
            {
                return Task.FromResult(list.OrderByDescending(x => x.Name).ToList());
            }

            async Task SortSKUChildren(Guid typeId, Guid? parentId, Func<List<InventorySKU>, Task<List<InventorySKU>>> sort, Boolean sortRecursive)
            {
                var itemsToSort = await this.Repository.Query<InventorySKU>()
                    .Where(x => x.TypeId == typeId && x.ParentId == parentId)
                    .ToListAsync();

                if (itemsToSort.Count == 0)
                {
                    return;
                }

                var maxOrder = itemsToSort.Max(x => x.OrderNo);
                itemsToSort = await sort(itemsToSort);
                foreach (var item in itemsToSort)
                {
                    item.OrderNo = ++maxOrder;
                    await this.Repository.UpdateAsync(item);
                }

                if (sortRecursive)
                {
                    foreach (var group in itemsToSort.Where(x => x.NodeType == InventorySKUNodeType.Group))
                    {
                        await SortSKUChildren(typeId, group.Id, sort, true);
                    }
                }
            }
        }

        private Task<IActionResult> GetSortSuccessResult(Guid id, InventorySKUType entity, InventorySKUTypeSortModel model)
        {
            return this.HybridFormResultAsync("InventoryTypesSort", this.RedirectToAction("Index", "Inventory"));
        }

        private class CustomPermissionsValidator : EntityPermissionsValidatorSimpleBase<InventorySKUType>
        {
            private readonly UserEntityResolver userEntityResolver;
            private IEmployeeAccess access;

            public CustomPermissionsValidator(IPermissionsHub permissionsHub, UserEntityResolver userEntityResolver)
                : base(permissionsHub)
            {
                this.userEntityResolver = userEntityResolver;
            }

            public override async Task<Boolean> CanReadAsync()
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess.Inventory.HasPermission(InventoryPermissions.SKURead);
            }

            public override async Task<Boolean> CanWriteAsync()
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess.Inventory.HasPermission(InventoryPermissions.SKUTypeWrite);
            }

            private Task<IEmployeeAccess> ResolveAccessAsync()
            {
                if (this.access != null)
                {
                    return Task.FromResult(this.access);
                }

                async Task<IEmployeeAccess> LoadAccessAsync()
                {
                    this.access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return this.access;
                }

                return LoadAccessAsync();
            }
        }
    }
}
