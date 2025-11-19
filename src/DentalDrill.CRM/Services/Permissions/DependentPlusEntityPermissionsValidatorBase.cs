using System;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM.Services.Permissions
{
    public abstract class DependentPlusEntityPermissionsValidatorBase<TEntity, TParentEntity> : DependentEntityPermissionsValidatorBase<TEntity, TParentEntity>, IEntityPermissionsValidator<TEntity>
    {
        private readonly IPermissionsHub permissionsHub;

        protected DependentPlusEntityPermissionsValidatorBase(IPermissionsHub permissionsHub)
            : base(permissionsHub)
        {
            this.permissionsHub = permissionsHub;
        }

        public abstract Task<Boolean> CanIndexAsync();

        public override Task<Boolean> CanIndexAsync(TParentEntity parentEntity)
        {
            return this.CanIndexAsync();
        }

        public abstract Task<Boolean> CanCreateAsync();

        public override Task<Boolean> CanCreateAsync(TParentEntity parentEntity)
        {
            return this.CanCreateAsync();
        }

        public abstract Task<IQueryable<TEntity>> RequireReadAccessAsync(IQueryable<TEntity> query);

        public override Task<IQueryable<TEntity>> RequireReadAccessAsync(TParentEntity parent, IQueryable<TEntity> query)
        {
            return this.RequireReadAccessAsync(query);
        }

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
    }
}
