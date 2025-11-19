using System;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM.Services.Permissions
{
    public abstract class EntityPermissionsValidatorSimpleBase<TEntity> : EntityPermissionsValidatorBase<TEntity>
    {
        protected EntityPermissionsValidatorSimpleBase(IPermissionsHub permissionsHub)
            : base(permissionsHub)
        {
        }

        public abstract Task<Boolean> CanReadAsync();

        public abstract Task<Boolean> CanWriteAsync();

        public override Task<Boolean> CanIndexAsync() => this.CanReadAsync();

        public override Task<Boolean> CanCreateAsync() => this.CanWriteAsync();

        public override Task<Boolean> CanDetailsAsync(TEntity entity) => this.CanReadAsync();

        public override Task<Boolean> CanEditAsync(TEntity entity) => this.CanWriteAsync();

        public override Task<Boolean> CanDeleteAsync(TEntity entity) => this.CanWriteAsync();

        public override async Task<IQueryable<TEntity>> RequireReadAccessAsync(IQueryable<TEntity> query)
        {
            if (await this.CanReadAsync())
            {
                return query;
            }

            return query.Where(x => false);
        }
    }
}
