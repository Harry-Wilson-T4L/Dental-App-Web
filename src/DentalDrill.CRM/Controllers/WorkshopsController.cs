using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Workshops;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.Ordering.SqlServer.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/Workshop")]
    [PermissionsManager("Entity", "/Domain/Workshop/Entities/{entity}")]
    public class WorkshopsController : BaseTelerikFullInlineCrudController<Guid, Workshop, EmptyEmployeeIndexViewModel, WorkshopViewModel>
    {
        public WorkshopsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.MoveUpHandler = new BasicOrderDecreaseActionHandler<Guid, Workshop>(this, this.ControllerServices, this.PermissionsValidator);
            this.MoveDownHandler = new BasicOrderIncreaseActionHandler<Guid, Workshop>(this, this.ControllerServices, this.PermissionsValidator);

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;

            this.CreateHandler.Overrides.BeforeEntityCreated = this.BeforeEntityCreated;

            this.DestroyHandler.Overrides.DeleteEntity = this.DeleteEntity;
        }

        protected BasicOrderDecreaseActionHandler<Guid, Workshop> MoveUpHandler { get; }

        protected BasicOrderIncreaseActionHandler<Guid, Workshop> MoveDownHandler { get; }

        [AjaxPost]
        public Task<IActionResult> MoveUp(Guid id) => this.MoveUpHandler.DecreaseOrder(id);

        [AjaxPost]
        public Task<IActionResult> MoveDown(Guid id) => this.MoveDownHandler.IncreaseOrder(id);

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<IQueryable<Workshop>> PrepareReadQuery()
        {
            IQueryable<Workshop> query = this.Store.Query()
                .Where(x => x.DeletionStatus == DeletionStatus.Normal)
                .OrderBy(x => x.OrderNo);

            return Task.FromResult(query);
        }

        private async Task BeforeEntityCreated(Workshop entity, WorkshopViewModel model, Dictionary<String, Object> additionalData)
        {
            var latest = await this.Repository.Query<Workshop>()
                .OrderByDescending(x => x.OrderNo)
                .Take(1)
                .ToListAsync();

            entity.OrderNo = latest.Count == 0 ? 1 : latest[0].OrderNo + 1;
        }

        private async Task DeleteEntity(Workshop entity, WorkshopViewModel model, Dictionary<String, Object> additionalData)
        {
            entity.DeletionStatus = DeletionStatus.Deleted;
            entity.DeletionDate = DateTime.UtcNow;
            await this.Store.UpdateAsync(entity);
            await this.Repository.SaveChangesAsync();
        }
    }
}
