using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
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
    [PermissionsManager("Type", "/Domain/TutorialVideo")]
    [PermissionsManager("Entity", "/Domain/TutorialVideo/Entities/{entity}")]
    public class TutorialVideosController : BaseTelerikFullInlineIndexlessDependentCrudController<Guid, TutorialVideo, String, TutorialPage, TutorialVideoViewModel>
    {
        public TutorialVideosController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.ConvertEntityToViewModel = (parentId, parent, entity) => Task.FromResult(this.ConvertEntityToViewModel(parent, entity, null));

            this.UpdateHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.UpdateHandler.Overrides.ConvertEntityToViewModel = (entity) => Task.FromResult(this.ConvertEntityToViewModel(null, entity, null));

            this.MoveUpHandler = new BasicOrderDecreaseActionHandler<Guid, TutorialVideo>(this, controllerServices, this.PermissionsValidator);
            this.MoveDownHandler = new BasicOrderIncreaseActionHandler<Guid, TutorialVideo>(this, controllerServices, this.PermissionsValidator);

            this.MoveUpHandler.Overrides.FindPrevious = this.FindPrevious;
            this.MoveDownHandler.Overrides.FindNext = this.FindNext;
        }

        protected BasicOrderDecreaseActionHandler<Guid, TutorialVideo> MoveUpHandler { get; }

        protected BasicOrderIncreaseActionHandler<Guid, TutorialVideo> MoveDownHandler { get; }

        public async Task<IActionResult> Index(String parentId)
        {
            var page = await this.Repository.Query<TutorialPage>().SingleOrDefaultAsync(x => x.Id == parentId);
            if (page == null)
            {
                return this.NotFound();
            }

            await this.PermissionsValidator.DemandCanIndexAsync();
            var employeeAccess = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
            var model = new TutorialPageDetailsModel
            {
                Entity = page,
                Access = employeeAccess,
            };

            return this.View(model);
        }

        [AjaxPost]
        public Task<IActionResult> MoveUp(Guid id) => this.MoveUpHandler.DecreaseOrder(id);

        [AjaxPost]
        public Task<IActionResult> MoveDown(Guid id) => this.MoveDownHandler.IncreaseOrder(id);

        private TutorialVideoViewModel ConvertEntityToViewModel(TutorialPage parent, TutorialVideo entity, String[] allowedProperties)
        {
            return new TutorialVideoViewModel
            {
                Id = entity.Id,
                OrderNo = entity.OrderNo,
                Title = entity.Title,
                VideoUrl = entity.VideoUrl,
                AvailableForClients = entity.Availability.HasFlag(TutorialVideoAvailability.Client),
                AvailableForCorporates = entity.Availability.HasFlag(TutorialVideoAvailability.Corporate),
                AvailableForStaff = entity.Availability.HasFlag(TutorialVideoAvailability.Staff),
            };
        }

        private async Task InitializeNewEntity(String parentId, TutorialPage parent, TutorialVideo entity, TutorialVideoViewModel model)
        {
            var lastVideo = await this.Repository.QueryWithoutTracking<TutorialVideo>()
                .Where(x => x.PageId == parent.Id)
                .OrderByDescending(x => x.OrderNo)
                .FirstOrDefaultAsync();

            entity.OrderNo = (lastVideo?.OrderNo ?? 0) + 1;
            entity.Title = model.Title;
            entity.VideoUrl = model.VideoUrl;
            entity.Availability =
                (model.AvailableForClients ? TutorialVideoAvailability.Client : TutorialVideoAvailability.None) |
                (model.AvailableForCorporates ? TutorialVideoAvailability.Corporate : TutorialVideoAvailability.None) |
                (model.AvailableForStaff ? TutorialVideoAvailability.Staff : TutorialVideoAvailability.None);
        }

        private Task UpdateExistingEntity(TutorialVideo entity, TutorialVideoViewModel model)
        {
            entity.Title = model.Title;
            entity.VideoUrl = model.VideoUrl;
            entity.Availability =
                (model.AvailableForClients ? TutorialVideoAvailability.Client : TutorialVideoAvailability.None) |
                (model.AvailableForCorporates ? TutorialVideoAvailability.Corporate : TutorialVideoAvailability.None) |
                (model.AvailableForStaff ? TutorialVideoAvailability.Staff : TutorialVideoAvailability.None);

            return Task.CompletedTask;
        }

        private Task<TutorialVideo> FindPrevious(Guid id, TutorialVideo entity)
        {
            return this.Repository.Query<TutorialVideo>()
                .Where(x => x.PageId == entity.PageId)
                .Where(x => x.OrderNo < entity.OrderNo)
                .OrderByDescending(x => x.OrderNo)
                .FirstOrDefaultAsync();
        }

        private Task<TutorialVideo> FindNext(Guid id, TutorialVideo entity)
        {
            return this.Repository.Query<TutorialVideo>()
                .Where(x => x.PageId == entity.PageId)
                .Where(x => x.OrderNo > entity.OrderNo)
                .OrderBy(x => x.OrderNo)
                .FirstOrDefaultAsync();
        }
    }
}
