using System;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM.Services.Permissions
{
    public abstract class EntityPermissionsValidatorBase<TEntity> : IEntityPermissionsValidator<TEntity>
    {
        private readonly IPermissionsHub permissionsHub;

        protected EntityPermissionsValidatorBase(IPermissionsHub permissionsHub)
        {
            this.permissionsHub = permissionsHub;
        }

        public abstract Task<Boolean> CanIndexAsync();

        public abstract Task<Boolean> CanCreateAsync();

        public abstract Task<Boolean> CanDetailsAsync(TEntity entity);

        public abstract Task<Boolean> CanEditAsync(TEntity entity);

        public abstract Task<Boolean> CanDeleteAsync(TEntity entity);

        public abstract Task<IQueryable<TEntity>> RequireReadAccessAsync(IQueryable<TEntity> query);

        public async Task DemandCanIndexAsync()
        {
            var canIndex = await this.CanIndexAsync();
            if (!canIndex)
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanCreateAsync()
        {
            var canCreate = await this.CanCreateAsync();
            if (!canCreate)
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDetailsAsync(TEntity entity)
        {
            var canDetails = await this.CanDetailsAsync(entity);
            if (!canDetails)
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanEditAsync(TEntity entity)
        {
            var canEdit = await this.CanEditAsync(entity);
            if (!canEdit)
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDeleteAsync(TEntity entity)
        {
            var canDelete = await this.CanDeleteAsync(entity);
            if (!canDelete)
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public virtual Task<Boolean> CanReadPropertyAsync(String propertyName)
        {
            return Task.FromResult(true);
        }

        public virtual Task<Boolean> CanInitializePropertyAsync(String propertyName)
        {
            return Task.FromResult(true);
        }

        public virtual Task<Boolean> CanUpdatePropertyAsync(String propertyName)
        {
            return Task.FromResult(true);
        }
    }
}
