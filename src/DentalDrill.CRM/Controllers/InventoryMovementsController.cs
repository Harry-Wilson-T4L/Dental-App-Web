using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.InventoryMovements;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Data;
using DentalDrill.CRM.Services.GenericFlags;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public partial class InventoryMovementsController : Controller
    {
        private readonly Lazy<IDataTransactionService> dataTransactionService;
        private readonly Lazy<IWorkshopManager> workshopManager;
        private readonly Lazy<IInventorySKUTypeManager> inventorySKUTypeManager;
        private readonly Lazy<IInventorySKUManager> inventorySKUManager;
        private readonly Lazy<IInventoryMovementManager> inventoryMovementManager;
        private readonly Lazy<GenericFlagsService> genericFlagsService;
        private readonly Lazy<UserEntityResolver> userEntityResolver;

        public InventoryMovementsController(IEntityControllerServices controllerServices)
        {
            this.ControllerServices = controllerServices;
            this.PermissionsValidator = new DefaultEntityPermissionsValidator<InventorySKU>(controllerServices.PermissionsHub, null, null, null);

            this.dataTransactionService = controllerServices.ServiceProvider.GetLazyService<IDataTransactionService>();
            this.workshopManager = controllerServices.ServiceProvider.GetLazyService<IWorkshopManager>();
            this.inventorySKUTypeManager = controllerServices.ServiceProvider.GetLazyService<IInventorySKUTypeManager>();
            this.inventorySKUManager = controllerServices.ServiceProvider.GetLazyService<IInventorySKUManager>();
            this.inventoryMovementManager = controllerServices.ServiceProvider.GetLazyService<IInventoryMovementManager>();
            this.genericFlagsService = controllerServices.ServiceProvider.GetLazyService<GenericFlagsService>();
            this.userEntityResolver = controllerServices.ServiceProvider.GetLazyService<UserEntityResolver>();

            this.ReadSKUsHandler = new TelerikCrudAjaxReadActionHandler<Guid, InventorySKU, InventoryMovementLeafSKUModel>(this, controllerServices, this.PermissionsValidator);

            this.ReadSKUsHandler.Overrides.PrepareReadQuery = this.PrepareReadAvailableQuery;
            this.ReadSKUsHandler.Overrides.ConvertEntityToViewModel = this.ConvertAvailableEntityToViewModel;
        }

        protected IEntityControllerServices ControllerServices { get; }

        protected IEntityPermissionsValidator<InventorySKU> PermissionsValidator { get; }

        protected IRepository Repository => this.ControllerServices.Repository;

        protected IDataTransactionService DataTransactionService => this.dataTransactionService.Value;

        protected IWorkshopManager WorkshopManager => this.workshopManager.Value;

        protected IInventorySKUTypeManager InventorySKUTypeManager => this.inventorySKUTypeManager.Value;

        protected IInventorySKUManager InventorySKUManager => this.inventorySKUManager.Value;

        protected IInventoryMovementManager InventoryMovementManager => this.inventoryMovementManager.Value;

        protected GenericFlagsService GenericFlagsService => this.genericFlagsService.Value;

        protected UserEntityResolver UserEntityResolver => this.userEntityResolver.Value;

        protected TelerikCrudAjaxReadActionHandler<Guid, InventorySKU, InventoryMovementLeafSKUModel> ReadSKUsHandler { get; }

        public async Task<IActionResult> Index(Guid? workshop, Guid? sku, Boolean? group, String tab)
        {
            var userAccess = await this.UserEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetAvailable(x => x.HasInventoryPermission(InventoryMovementPermissions.MovementRead));
            if (accessibleWorkshops.Count == 0)
            {
                return this.NotFound();
            }

            var availableWorkshops = await this.Repository.QueryWithoutTracking<Workshop>()
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .Where(x => x.DeletionStatus == DeletionStatus.Normal || x.Id == workshop)
                .OrderBy(x => x.OrderNo)
                .ToListAsync();
            Workshop selectedWorkshop = null;
            if (workshop.HasValue)
            {
                selectedWorkshop = await this.Repository.QueryWithoutTracking<Workshop>()
                    .Where(x => accessibleWorkshops.Contains(x.Id))
                    .SingleOrDefaultAsync(x => x.Id == workshop);
                if (selectedWorkshop == null)
                {
                    return this.NotFound();
                }
            }

            IInventorySKU requestedSKU = null;
            IReadOnlyList<IInventorySKU> filteredSKU = null;
            if (sku.HasValue)
            {
                requestedSKU = await this.InventorySKUManager.GetByIdAsync(sku.Value);
                if (requestedSKU == null)
                {
                    return this.NotFound();
                }

                filteredSKU = await requestedSKU.GetDescendantsAndSelfAsync();
            }

            var model = new InventoryMovementIndexModel
            {
                RequestedSKU = requestedSKU,
                FilteredSKU = filteredSKU,
                StatsTypes = await this.InventorySKUTypeManager.GetSomeAsync(x => x.StatisticsMode != InventorySKUTypeStatisticsMode.Hidden),
                GroupMovements = group != false,
                Tab = tab,
                Workshops = availableWorkshops,
                SelectedWorkshop = selectedWorkshop,
            };

            return this.View(model);
        }

        public async Task<IActionResult> Preview(Guid sku, Guid? workshop, String tab)
        {
            var requestedSKU = await this.InventorySKUManager.GetByIdAsync(sku);
            if (requestedSKU == null)
            {
                return this.NotFound();
            }

            var model = new InventoryMovementPreviewModel
            {
                RequestedSKU = requestedSKU,
                Workshop = workshop == null
                    ? null
                    : await this.Repository.QueryWithoutTracking<Workshop>().SingleOrDefaultAsync(x => x.Id == workshop),
                Tab = tab,
            };

            return this.View(model);
        }

        public Task<IActionResult> ReadSKUs([DataSourceRequest] DataSourceRequest request) => this.ReadSKUsHandler.Read(request);

        public async Task<IActionResult> Create(Guid? workshop)
        {
            var model = new InventoryMovementCreateModel
            {
                Quantity = 1,
                Type = InventoryMovementCreateType.Order,
                PreselectedWorkshop = workshop == null ? null : await this.WorkshopManager.GetActiveByIdAsync(workshop.Value),
                AvailableWorkshops = await this.WorkshopManager.ListActiveAsync(await this.UserEntityResolver.GetEmployeeAccessAsync(), x =>
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateOrder) ||
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateFound) ||
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateLost) ||
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove)),
            };

            if (workshop != null && model.PreselectedWorkshop == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? workshop, InventoryMovementCreateModel model)
        {
            var employeeAccess = await this.UserEntityResolver.GetEmployeeAccessAsync();
            model.PreselectedWorkshop = workshop == null ? null : await this.WorkshopManager.GetActiveByIdAsync(workshop.Value);
            model.AvailableWorkshops = await this.WorkshopManager.ListActiveAsync(employeeAccess, x =>
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateOrder) ||
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateFound) ||
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateLost) ||
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove));
            if (workshop != null && model.PreselectedWorkshop == null)
            {
                return this.NotFound();
            }

            if (!model.Validate(this.ModelState) || !this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var domainSKU = await this.InventorySKUManager.GetByIdAsync(model.SKU.Value);
            switch (model.Type)
            {
                case InventoryMovementCreateType.Found:
                    if (!employeeAccess.Workshops[model.ToWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateFound))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var toWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.ToWorkshop!.Value);
                        if (toWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryFoundMovement, InventoryMovementBuilder.Found>(
                            toWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.TryProcessMovementsChangesAsync(toWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                case InventoryMovementCreateType.Lost:
                    if (!employeeAccess.Workshops[model.FromWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateLost))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var fromWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.FromWorkshop!.Value);
                        if (fromWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryLostMovement, InventoryMovementBuilder.Lost>(
                            fromWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.TryProcessMovementsChangesAsync(fromWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                case InventoryMovementCreateType.Order:
                    if (!employeeAccess.Workshops[model.ToWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateOrder))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var toWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.ToWorkshop!.Value);
                        if (toWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                            toWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment).WithStatus(model.Status));
                        await domainSKU.TryProcessMovementsChangesAsync(toWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                case InventoryMovementCreateType.MoveBetweenWorkshops:
                case InventoryMovementCreateType.MoveFromAnotherWorkshop:
                case InventoryMovementCreateType.MoveToAnotherWorkshop:
                    if (!employeeAccess.Workshops[model.ToWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove) ||
                        !employeeAccess.Workshops[model.FromWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var fromWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.FromWorkshop!.Value);
                        var toWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.ToWorkshop!.Value);
                        if (fromWorkshop == null || toWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryMoveToAnotherWorkshopMovement, InventoryMovementBuilder.MoveToAnotherWorkshop>(
                            fromWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.Movements.CreateAsync<IInventoryMoveFromAnotherWorkshopMovement, InventoryMovementBuilder.MoveFromAnotherWorkshop>(
                            toWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.TryProcessMovementsChangesAsync(fromWorkshop);
                        await domainSKU.TryProcessMovementsChangesAsync(toWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return await this.HybridFormResultAsync("InventoryMovementsCreate", this.RedirectToAction("Index", "InventoryMovements"));
        }

        public async Task<IActionResult> CreateForSKU(Guid sku, Guid? workshop)
        {
            var model = new InventoryMovementCreateForSKUModel
            {
                SKU = await this.Repository.QueryWithoutTracking<InventorySKU>()
                    .Include(x => x.Type)
                    .SingleOrDefaultAsync(x => x.Id == sku),
                Quantity = 1,
                Type = InventoryMovementCreateType.Order,
                PreselectedWorkshop = workshop == null ? null : await this.WorkshopManager.GetActiveByIdAsync(workshop.Value),
                AvailableWorkshops = await this.WorkshopManager.ListActiveAsync(await this.UserEntityResolver.GetEmployeeAccessAsync(), x =>
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateOrder) ||
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateFound) ||
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateLost) ||
                    x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove)),
            };

            if (model.SKU == null || (workshop != null && model.PreselectedWorkshop == null))
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForSKU(Guid sku, Guid? workshop, InventoryMovementCreateForSKUModel model)
        {
            var employeeAccess = await this.UserEntityResolver.GetEmployeeAccessAsync();
            model.SKU = await this.Repository.QueryWithoutTracking<InventorySKU>()
                .Include(x => x.Type)
                .SingleOrDefaultAsync(x => x.Id == sku);
            model.PreselectedWorkshop = workshop == null ? null : await this.WorkshopManager.GetActiveByIdAsync(workshop.Value);
            model.AvailableWorkshops = await this.WorkshopManager.ListActiveAsync(employeeAccess, x =>
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateOrder) ||
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateFound) ||
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateLost) ||
                x.HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove));

            if (model.SKU == null || (workshop != null && model.PreselectedWorkshop == null))
            {
                return this.NotFound();
            }

            if (!model.Validate(this.ModelState) || !this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var domainSKU = await this.InventorySKUManager.GetByIdAsync(sku);
            switch (model.Type)
            {
                case InventoryMovementCreateType.Found:
                    if (!employeeAccess.Workshops[model.ToWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateFound))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var toWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.ToWorkshop!.Value);
                        if (toWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryFoundMovement, InventoryMovementBuilder.Found>(
                            toWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.TryProcessMovementsChangesAsync(toWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                case InventoryMovementCreateType.Lost:
                    if (!employeeAccess.Workshops[model.FromWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateLost))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var fromWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.FromWorkshop!.Value);
                        if (fromWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryLostMovement, InventoryMovementBuilder.Lost>(
                            fromWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.TryProcessMovementsChangesAsync(fromWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                case InventoryMovementCreateType.Order:
                    if (!employeeAccess.Workshops[model.ToWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateOrder))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var toWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.ToWorkshop!.Value);
                        if (toWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryOrderMovement, InventoryMovementBuilder.Order>(
                            toWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment).WithStatus(model.Status));
                        await domainSKU.TryProcessMovementsChangesAsync(toWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                case InventoryMovementCreateType.MoveBetweenWorkshops:
                case InventoryMovementCreateType.MoveFromAnotherWorkshop:
                case InventoryMovementCreateType.MoveToAnotherWorkshop:
                    if (!employeeAccess.Workshops[model.ToWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove) ||
                        !employeeAccess.Workshops[model.FromWorkshop!.Value].HasInventoryPermission(InventoryMovementPermissions.MovementCreateMove))
                    {
                        this.ModelState.AddModelError("Type", "Does not have necessary permissions to create movements of this type in this workshop");
                        return this.View(model);
                    }

                    await using (var transaction = await this.DataTransactionService.BeginTransactionAsync())
                    {
                        var fromWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.FromWorkshop!.Value);
                        var toWorkshop = await this.WorkshopManager.GetActiveByIdAsync(model.ToWorkshop!.Value);
                        if (fromWorkshop == null || toWorkshop == null)
                        {
                            this.ModelState.AddModelError(String.Empty, "Unable to link workshop");
                            return this.View(model);
                        }

                        await domainSKU.Movements.CreateAsync<IInventoryMoveToAnotherWorkshopMovement, InventoryMovementBuilder.MoveToAnotherWorkshop>(
                            fromWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.Movements.CreateAsync<IInventoryMoveFromAnotherWorkshopMovement, InventoryMovementBuilder.MoveFromAnotherWorkshop>(
                            toWorkshop,
                            model.Quantity.Value,
                            builder => builder.WithComment(model.Comment));
                        await domainSKU.TryProcessMovementsChangesAsync(fromWorkshop);
                        await domainSKU.TryProcessMovementsChangesAsync(toWorkshop);
                        await transaction.CommitAsync();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return await this.HybridFormResultAsync("InventoryMovementsCreateForSKU", this.RedirectToAction("Index", "Inventory"));
        }

        private async Task<IQueryable<InventorySKU>> PrepareReadAvailableQuery()
        {
            var userAccess = await this.UserEntityResolver.GetEmployeeAccessAsync();
            var accessibleWorkshops = userAccess.Workshops.GetAvailable(x => x.HasInventoryPermission(InventoryMovementPermissions.MovementRead));

            var query = this.Repository.QueryWithoutTracking<InventorySKU>()
                .Include(x => x.Type)
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .Where(x => x.NodeType == InventorySKUNodeType.Leaf);

            if (accessibleWorkshops.Count == 0)
            {
                query = query.Where(x => false);
            }

            return query;
        }

        private InventoryMovementLeafSKUModel ConvertAvailableEntityToViewModel(InventorySKU sku, String[] allowedProperties)
        {
            return new InventoryMovementLeafSKUModel
            {
                Id = sku.Id,
                Name = sku.Name,
            };
        }
    }
}
