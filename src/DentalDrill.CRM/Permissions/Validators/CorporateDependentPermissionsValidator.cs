using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM.Permissions.Validators
{
    public class CorporateDependentPermissionsValidator<T> : IDependentEntityPermissionsValidator<T, Corporate>
    {
        private readonly IPermissionsHub permissionsHub;
        private readonly UserEntityResolver userEntityResolver;
        private readonly Func<T, Guid> corporateIdAccessor;

        public CorporateDependentPermissionsValidator(IPermissionsHub permissionsHub, UserEntityResolver userEntityResolver, Func<T, Guid> corporateIdAccessor)
        {
            this.permissionsHub = permissionsHub;
            this.userEntityResolver = userEntityResolver;
            this.corporateIdAccessor = corporateIdAccessor;
        }

        public async Task<Boolean> CanIndexAsync(Corporate parentEntity)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate);
                case Corporate corporate:
                    return parentEntity.Id == corporate.Id;
                default:
                    return false;
            }
        }

        public Task<Boolean> CanCreateAsync(Corporate parentEntity)
        {
            return Task.FromResult(false);
        }

        public async Task<Boolean> CanDetailsAsync(T entity)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate);
                case Corporate corporate:
                    return this.corporateIdAccessor(entity) == corporate.Id;
                default:
                    return false;
            }
        }

        public Task<Boolean> CanEditAsync(T entity)
        {
            return Task.FromResult(false);
        }

        public Task<Boolean> CanDeleteAsync(T entity)
        {
            return Task.FromResult(false);
        }

        public async Task<IQueryable<T>> RequireReadAccessAsync(Corporate parent, IQueryable<T> query)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return query;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate) ? query : query.Where(x => false);
                case Corporate corporate when corporate.Id == parent.Id:
                    return query;
                default:
                    return query.Where(x => false);
            }
        }

        public async Task DemandCanIndexAsync(Corporate parentEntity)
        {
            if (!await this.CanIndexAsync(parentEntity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanCreateAsync(Corporate parentEntity)
        {
            if (!await this.CanCreateAsync(parentEntity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDetailsAsync(T entity)
        {
            if (!await this.CanDetailsAsync(entity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanEditAsync(T entity)
        {
            if (!await this.CanEditAsync(entity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDeleteAsync(T entity)
        {
            if (!await this.CanDeleteAsync(entity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public Task<Boolean> CanReadPropertyAsync(String propertyName)
        {
            return Task.FromResult(true);
        }

        public Task<Boolean> CanInitializePropertyAsync(String propertyName)
        {
            return Task.FromResult(false);
        }

        public Task<Boolean> CanUpdatePropertyAsync(String propertyName)
        {
            return Task.FromResult(false);
        }
    }
}
