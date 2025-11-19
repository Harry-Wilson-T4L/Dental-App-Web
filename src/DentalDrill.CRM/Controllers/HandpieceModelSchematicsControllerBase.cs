using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Images;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    public abstract class HandpieceModelSchematicsControllerBase : Controller
    {
        protected HandpieceModelSchematicsControllerBase(IEntityControllerServices controllerServices, IImageUploadService imageUploadService, IStorageHub storageHub)
        {
            this.ControllerServices = controllerServices;
            this.ImageUploadService = imageUploadService;
            this.StorageHub = storageHub;

            var permissionsValidator = new DefaultEntityPermissionsValidator<HandpieceModelSchematic>(
                controllerServices.PermissionsHub,
                PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "Type"),
                PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "Entity"),
                null);
            var dependentPermissionsValidator = new DefaultDependentEntityPermissionsValidator<HandpieceModelSchematic, HandpieceModel>(
                controllerServices.PermissionsHub,
                PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "Type"),
                PermissionsManagerAttribute.GetAnnotatedPermissionsManager(this.GetType(), "Entity"),
                null,
                null);

            this.ReadHandler = new TelerikCrudAjaxDependentReadActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicReadModel>(this, this.ControllerServices, dependentPermissionsValidator);
            this.CreateTextHandler = new BasicCrudDependentCreateActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicEditTextModel>(this, this.ControllerServices, dependentPermissionsValidator);
            this.CreateAttachmentHandler = new BasicCrudDependentCreateActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicEditAttachmentModel>(this, this.ControllerServices, dependentPermissionsValidator);
            this.CreateImageHandler = new BasicCrudDependentCreateActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicEditImageModel>(this, this.ControllerServices, dependentPermissionsValidator);
            this.DetailsHandler = new BasicCrudDetailsActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematicDetailsModel>(this, this.ControllerServices, permissionsValidator);
            this.EditTextHandler = new BasicCrudEditActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic, HandpieceModelSchematicEditTextModel>(this, this.ControllerServices, permissionsValidator);
            this.EditAttachmentHandler = new BasicCrudEditActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic, HandpieceModelSchematicEditAttachmentModel>(this, this.ControllerServices, permissionsValidator);
            this.EditImageHandler = new BasicCrudEditActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic, HandpieceModelSchematicEditImageModel>(this, this.ControllerServices, permissionsValidator);
            this.DeleteHandler = new BasicCrudDeleteActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic>(this, this.ControllerServices, permissionsValidator);
            this.MoveUpHandler = new BasicCrudCustomOperationActionHandler<Guid, HandpieceModelSchematic, Object>(this, this.ControllerServices, permissionsValidator);
            this.MoveDownHandler = new BasicCrudCustomOperationActionHandler<Guid, HandpieceModelSchematic, Object>(this, this.ControllerServices, permissionsValidator);
        }

        protected IEntityControllerServices ControllerServices { get; }

        protected IRepository Repository => this.ControllerServices.Repository;

        protected IImageUploadService ImageUploadService { get; }

        protected IStorageHub StorageHub { get; }

        protected TelerikCrudAjaxDependentReadActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicReadModel> ReadHandler { get; }

        protected BasicCrudDependentCreateActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicEditTextModel> CreateTextHandler { get; }

        protected BasicCrudDependentCreateActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicEditAttachmentModel> CreateAttachmentHandler { get; }

        protected BasicCrudDependentCreateActionHandler<Guid, HandpieceModelSchematic, Guid, HandpieceModel, HandpieceModelSchematicEditImageModel> CreateImageHandler { get; }

        protected BasicCrudDetailsActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematicDetailsModel> DetailsHandler { get; }

        protected BasicCrudEditActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic, HandpieceModelSchematicEditTextModel> EditTextHandler { get; }

        protected BasicCrudEditActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic, HandpieceModelSchematicEditAttachmentModel> EditAttachmentHandler { get; }

        protected BasicCrudEditActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic, HandpieceModelSchematicEditImageModel> EditImageHandler { get; }

        protected BasicCrudDeleteActionHandler<Guid, HandpieceModelSchematic, HandpieceModelSchematic> DeleteHandler { get; }

        protected BasicCrudCustomOperationActionHandler<Guid, HandpieceModelSchematic, Object> MoveUpHandler { get; set; }

        protected BasicCrudCustomOperationActionHandler<Guid, HandpieceModelSchematic, Object> MoveDownHandler { get; set; }

        [AjaxPost]
        public Task<IActionResult> Read(Guid parentId, [DataSourceRequest] DataSourceRequest request) => this.ReadHandler.Read(parentId, request);

        public Task<IActionResult> CreateText(Guid parentId) => this.CreateTextHandler.Create(parentId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateText(Guid parentId, HandpieceModelSchematicEditTextModel model) => this.CreateTextHandler.Create(parentId, model);

        public Task<IActionResult> CreateAttachment(Guid parentId) => this.CreateAttachmentHandler.Create(parentId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateAttachment(Guid parentId, HandpieceModelSchematicEditAttachmentModel model) => this.CreateAttachmentHandler.Create(parentId, model);

        public Task<IActionResult> CreateImage(Guid parentId) => this.CreateImageHandler.Create(parentId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateImage(Guid parentId, HandpieceModelSchematicEditImageModel model) => this.CreateImageHandler.Create(parentId, model);

        public Task<IActionResult> Details(Guid id) => this.DetailsHandler.Details(id);

        public Task<IActionResult> EditText(Guid id) => this.EditTextHandler.Edit(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditText(Guid id, HandpieceModelSchematicEditTextModel model) => this.EditTextHandler.Edit(id, model);

        public Task<IActionResult> EditAttachment(Guid id) => this.EditAttachmentHandler.Edit(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditAttachment(Guid id, HandpieceModelSchematicEditAttachmentModel model) => this.EditAttachmentHandler.Edit(id, model);

        public Task<IActionResult> EditImage(Guid id) => this.EditImageHandler.Edit(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditImage(Guid id, HandpieceModelSchematicEditImageModel model) => this.EditImageHandler.Edit(id, model);

        public Task<IActionResult> Delete(Guid id) => this.DeleteHandler.Delete(id);

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirm(Guid id) => this.DeleteHandler.DeleteConfirmed(id);

        [AjaxPost]
        public Task<IActionResult> MoveUp(Guid id) => this.MoveUpHandler.Execute(id, new Object());

        [AjaxPost]
        public Task<IActionResult> MoveDown(Guid id) => this.MoveDownHandler.Execute(id, new Object());

        protected async Task SwapOrderNo(HandpieceModelSchematic first, HandpieceModelSchematic second)
        {
            var context = this.ControllerServices.ServiceProvider.GetService<ApplicationDbContext>();

            var queryText = @"update [HandpieceModelSchematics]
  set [OrderNo] = case [Id]
    when @previousId then @nextOrder
    when @nextId then @previousOrder
  end
  where [Id] in (@previousId, @nextId)";

            var parameters = new List<Object>();
            parameters.Add(new SqlParameter("previousId", SqlDbType.UniqueIdentifier) { Value = first.Id });
            parameters.Add(new SqlParameter("nextId", SqlDbType.UniqueIdentifier) { Value = second.Id });
            parameters.Add(new SqlParameter("previousOrder", SqlDbType.Int) { Value = first.OrderNo });
            parameters.Add(new SqlParameter("nextOrder", SqlDbType.Int) { Value = second.OrderNo });

            await context.Database.ExecuteSqlRawAsync(queryText, parameters);
        }
    }
}
