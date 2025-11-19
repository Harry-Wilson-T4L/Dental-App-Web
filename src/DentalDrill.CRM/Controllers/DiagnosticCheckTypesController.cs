using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/DiagnosticCheckType")]
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class DiagnosticCheckTypesController : BaseTelerikFullInlineCrudController<Guid, DiagnosticCheckType, EmptyEmployeeIndexViewModel, DiagnosticCheckTypeViewModel>
    {
        public DiagnosticCheckTypesController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.CreateHandler.Overrides.BeforeEntityCreated = this.BeforeEntityCreated;

            var permissionsValidator = new DefaultEntityPermissionsValidator<DiagnosticCheckType>(this.PermissionsHub, "/Domain/DiagnosticCheckType", null, null);
            this.MoveUpHandler = new BasicCrudCustomOperationActionHandler<Guid, DiagnosticCheckType, Object>(this, this.ControllerServices, permissionsValidator);
            this.MoveDownHandler = new BasicCrudCustomOperationActionHandler<Guid, DiagnosticCheckType, Object>(this, this.ControllerServices, permissionsValidator);
            this.SortItemsHandler = new BasicCrudDetailsActionHandler<Guid, DiagnosticCheckType, DiagnosticCheckTypeViewModel>(this, this.ControllerServices, permissionsValidator);
            this.SortItemsReadHandler = new TelerikCrudAjaxDependentReadActionHandler<(Guid TypeId, Guid ItemId), DiagnosticCheckItemType, Guid, DiagnosticCheckType, DiagnosticCheckItemSortViewModel>(this, this.ControllerServices, new DefaultDependentEntityPermissionsValidator<DiagnosticCheckItemType, DiagnosticCheckType>(controllerServices.PermissionsHub, null, null, null, null));
            this.ItemMoveUpHandler = new BasicCrudCustomOperationActionHandler<(Guid TypeId, Guid ItemId), DiagnosticCheckItemType, Object>(this, this.ControllerServices, new DefaultEntityPermissionsValidator<DiagnosticCheckItemType>(controllerServices.PermissionsHub, null, null, null));
            this.ItemMoveDownHandler = new BasicCrudCustomOperationActionHandler<(Guid TypeId, Guid ItemId), DiagnosticCheckItemType, Object>(this, this.ControllerServices, new DefaultEntityPermissionsValidator<DiagnosticCheckItemType>(controllerServices.PermissionsHub, null, null, null));

            this.MoveUpHandler.Overrides.ExecuteOperation = this.ExecuteMoveUpOperation;
            this.MoveUpHandler.Overrides.GetOperationSuccessResult = this.GetMoveUpOperationSuccessResult;
            this.MoveDownHandler.Overrides.ExecuteOperation = this.ExecuteMoveDownOperation;
            this.MoveDownHandler.Overrides.GetOperationSuccessResult = this.GetMoveDownOperationSuccessResult;

            this.SortItemsReadHandler.Overrides.PrepareReadQuery = this.SortItemsReadPrepareReadQuery;
            this.SortItemsReadHandler.Overrides.ConvertEntityToViewModel = this.SortItemsReadConvertToViewModel;

            this.ItemMoveUpHandler.Overrides.QuerySingleEntity = this.QuerySingleSortItemEntity;
            this.ItemMoveUpHandler.Overrides.ExecuteOperation = this.ExecuteItemMoveUpOperation;
            this.ItemMoveUpHandler.Overrides.GetOperationSuccessResult = (id, entity, model) => Task.FromResult<IActionResult>(this.Json(new Object()));

            this.ItemMoveDownHandler.Overrides.QuerySingleEntity = this.QuerySingleSortItemEntity;
            this.ItemMoveDownHandler.Overrides.ExecuteOperation = this.ExecuteItemMoveDownOperation;
            this.ItemMoveDownHandler.Overrides.GetOperationSuccessResult = (id, entity, model) => Task.FromResult<IActionResult>(this.Json(new Object()));
        }

        protected BasicCrudCustomOperationActionHandler<Guid, DiagnosticCheckType, Object> MoveUpHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, DiagnosticCheckType, Object> MoveDownHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, DiagnosticCheckType, DiagnosticCheckTypeViewModel> SortItemsHandler { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<(Guid TypeId, Guid ItemId), DiagnosticCheckItemType, Guid, DiagnosticCheckType, DiagnosticCheckItemSortViewModel> SortItemsReadHandler { get; }

        protected BasicCrudCustomOperationActionHandler<(Guid TypeId, Guid ItemId), DiagnosticCheckItemType, Object> ItemMoveUpHandler { get; }

        protected BasicCrudCustomOperationActionHandler<(Guid TypeId, Guid ItemId), DiagnosticCheckItemType, Object> ItemMoveDownHandler { get; }

        [AjaxPost]
        public Task<IActionResult> MoveUp(Guid id) => this.MoveUpHandler.Execute(id, new Object());

        [AjaxPost]
        public Task<IActionResult> MoveDown(Guid id) => this.MoveDownHandler.Execute(id, new Object());

        public Task<IActionResult> SortItems(Guid id) => this.SortItemsHandler.Details(id);

        [AjaxPost]
        public Task<IActionResult> SortItemsRead(Guid typeId, [DataSourceRequest] DataSourceRequest request) => this.SortItemsReadHandler.Read(typeId, request);

        [AjaxPost]
        public Task<IActionResult> ItemMoveUp(Guid typeId, Guid itemId) => this.ItemMoveUpHandler.Execute((typeId, itemId), new Object());

        [AjaxPost]
        public Task<IActionResult> ItemMoveDown(Guid typeId, Guid itemId) => this.ItemMoveDownHandler.Execute((typeId, itemId), new Object());

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<IQueryable<DiagnosticCheckType>> PrepareReadQuery()
        {
            var query = this.Repository.QueryWithoutTracking<DiagnosticCheckType>().OrderBy(x => x.OrderNo);
            return Task.FromResult<IQueryable<DiagnosticCheckType>>(query);
        }

        private async Task BeforeEntityCreated(DiagnosticCheckType entity, DiagnosticCheckTypeViewModel model, Dictionary<String, Object> additionalData)
        {
            var latest = await this.Repository.Query<DiagnosticCheckType>()
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            entity.OrderNo = latest.Count == 0 ? 1 : latest[0].OrderNo + 1;
        }

        private async Task ExecuteMoveUpOperation(Guid id, DiagnosticCheckType entity, Object model)
        {
            var previous = await this.Repository.Query<DiagnosticCheckType>()
                .Where(x => x.OrderNo < entity.OrderNo)
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            if (previous.Count > 0)
            {
                var context = this.ControllerServices.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var query = @"update [DiagnosticCheckTypes]
  set [OrderNo] = case [Id]
    when @previousId then @nextOrder
    when @nextId then @previousOrder
  end
  where [Id] in (@previousId, @nextId)";

                await context.Database.ExecuteSqlRawAsync(query, new Object[]
                {
                    new SqlParameter("previousId", SqlDbType.UniqueIdentifier) { Value = previous[0].Id },
                    new SqlParameter("nextId", SqlDbType.UniqueIdentifier) { Value = entity.Id },
                    new SqlParameter("previousOrder", SqlDbType.Int) { Value = previous[0].OrderNo },
                    new SqlParameter("nextOrder", SqlDbType.Int) { Value = entity.OrderNo },
                });
            }
        }

        private async Task ExecuteMoveDownOperation(Guid id, DiagnosticCheckType entity, Object model)
        {
            var next = await this.Repository.Query<DiagnosticCheckType>()
                .Where(x => x.OrderNo > entity.OrderNo)
                .OrderBy(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            if (next.Count > 0)
            {
                var context = this.ControllerServices.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var query = @"update [DiagnosticCheckTypes]
  set [OrderNo] = case [Id]
    when @previousId then @nextOrder
    when @nextId then @previousOrder
  end
  where [Id] in (@previousId, @nextId)";

                await context.Database.ExecuteSqlRawAsync(query, new Object[]
                {
                    new SqlParameter("previousId", SqlDbType.UniqueIdentifier) { Value = entity.Id },
                    new SqlParameter("nextId", SqlDbType.UniqueIdentifier) { Value = next[0].Id },
                    new SqlParameter("previousOrder", SqlDbType.Int) { Value = entity.OrderNo },
                    new SqlParameter("nextOrder", SqlDbType.Int) { Value = next[0].OrderNo },
                });
            }
        }

        private Task<IActionResult> GetMoveUpOperationSuccessResult(Guid id, DiagnosticCheckType entity, Object model)
        {
            return Task.FromResult<IActionResult>(this.Json(new Object()));
        }

        private Task<IActionResult> GetMoveDownOperationSuccessResult(Guid id, DiagnosticCheckType entity, Object model)
        {
            return Task.FromResult<IActionResult>(this.Json(new Object()));
        }

        private Task<IQueryable<DiagnosticCheckItemType>> SortItemsReadPrepareReadQuery(DiagnosticCheckType parent)
        {
            var query = this.Repository.Query<DiagnosticCheckItemType>()
                .Include(x => x.Item)
                .Where(x => x.TypeId == parent.Id)
                .OrderBy(x => x.OrderNo);

            return Task.FromResult<IQueryable<DiagnosticCheckItemType>>(query);
        }

        private DiagnosticCheckItemSortViewModel SortItemsReadConvertToViewModel(DiagnosticCheckType parent, DiagnosticCheckItemType entity, String[] allowedProperties)
        {
            return new DiagnosticCheckItemSortViewModel
            {
                TypeId = entity.TypeId,
                ItemId = entity.ItemId,
                Name = entity.Item.Name,
                OrderNo = entity.OrderNo,
            };
        }

        private Task<DiagnosticCheckItemType> QuerySingleSortItemEntity((Guid TypeId, Guid ItemId) id)
        {
            return this.Repository.Query<DiagnosticCheckItemType>().SingleOrDefaultAsync(x => x.TypeId == id.TypeId && x.ItemId == id.ItemId);
        }

        private async Task ExecuteItemMoveUpOperation((Guid TypeId, Guid ItemId) id, DiagnosticCheckItemType entity, Object model)
        {
            var previous = await this.Repository.Query<DiagnosticCheckItemType>()
                .Where(x => x.TypeId == entity.TypeId)
                .Where(x => x.OrderNo < entity.OrderNo)
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            if (previous.Count > 0)
            {
                var context = this.ControllerServices.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var query = @"update [DiagnosticCheckItemTypes]
  set [OrderNo] = case [ItemId]
    when @previousId then @nextOrder
    when @nextId then @previousOrder
  end
  where [TypeId] = @typeId and [ItemId] in (@previousId, @nextId)";

                await context.Database.ExecuteSqlRawAsync(query, new Object[]
                {
                    new SqlParameter("typeId", SqlDbType.UniqueIdentifier) { Value = entity.TypeId },
                    new SqlParameter("previousId", SqlDbType.UniqueIdentifier) { Value = previous[0].ItemId },
                    new SqlParameter("nextId", SqlDbType.UniqueIdentifier) { Value = entity.ItemId },
                    new SqlParameter("previousOrder", SqlDbType.Int) { Value = previous[0].OrderNo },
                    new SqlParameter("nextOrder", SqlDbType.Int) { Value = entity.OrderNo },
                });
            }
        }

        private async Task ExecuteItemMoveDownOperation((Guid TypeId, Guid ItemId) id, DiagnosticCheckItemType entity, Object model)
        {
            var next = await this.Repository.Query<DiagnosticCheckItemType>()
                .Where(x => x.TypeId == entity.TypeId)
                .Where(x => x.OrderNo > entity.OrderNo)
                .OrderBy(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            if (next.Count > 0)
            {
                var context = this.ControllerServices.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var query = @"update [DiagnosticCheckItemTypes]
  set [OrderNo] = case [ItemId]
    when @previousId then @nextOrder
    when @nextId then @previousOrder
  end
  where [TypeId] = @typeId and [ItemId] in (@previousId, @nextId)";

                await context.Database.ExecuteSqlRawAsync(query, new Object[]
                {
                    new SqlParameter("typeId", SqlDbType.UniqueIdentifier) { Value = entity.TypeId },
                    new SqlParameter("previousId", SqlDbType.UniqueIdentifier) { Value = entity.ItemId },
                    new SqlParameter("nextId", SqlDbType.UniqueIdentifier) { Value = next[0].ItemId },
                    new SqlParameter("previousOrder", SqlDbType.Int) { Value = entity.OrderNo },
                    new SqlParameter("nextOrder", SqlDbType.Int) { Value = next[0].OrderNo },
                });
            }
        }
    }
}
