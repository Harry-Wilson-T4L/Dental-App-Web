using System.Threading.Tasks;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.Ordering.SqlServer.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers.Base
{
    public abstract class BaseTelerikIndexlessOrderableBasicCrudController<TIdentifier, TEntity, TReadModel, TDetailsModel, TCreateModel, TEditModel, TDeleteModel>
        : BaseTelerikIndexlessBasicCrudController<TIdentifier, TEntity, TReadModel, TDetailsModel, TCreateModel, TEditModel, TDeleteModel>
        where TEntity : class, IOrderableEntity, new()
        where TDetailsModel : class, new()
        where TCreateModel : class, new()
        where TEditModel : class, new()
        where TDeleteModel : class, new()
    {
        protected BaseTelerikIndexlessOrderableBasicCrudController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.MoveUpHandler = new BasicOrderDecreaseActionHandler<TIdentifier, TEntity>(this, controllerServices, this.PermissionsValidator);
            this.MoveDownHandler = new BasicOrderIncreaseActionHandler<TIdentifier, TEntity>(this, controllerServices, this.PermissionsValidator);
        }

        protected BasicOrderDecreaseActionHandler<TIdentifier, TEntity> MoveUpHandler { get; }

        protected BasicOrderIncreaseActionHandler<TIdentifier, TEntity> MoveDownHandler { get; }

        [HttpPost]
        public Task<IActionResult> MoveUp(TIdentifier id) => this.MoveUpHandler.DecreaseOrder(id);

        [HttpPost]
        public Task<IActionResult> MoveDown(TIdentifier id) => this.MoveDownHandler.IncreaseOrder(id);
    }
}
