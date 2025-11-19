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
    public class CorporateDependent2PermissionsValidator<TChild, TParent> : IDependentEntityPermissionsValidator<TChild, TParent>
    {
        private readonly IPermissionsHub permissionsHub;
        private readonly UserEntityResolver userEntityResolver;
        private readonly Func<TParent, Guid> parentCorporateIdAccessor;
        private readonly Func<TChild, Guid> childCorporateIdAccessor;

        public CorporateDependent2PermissionsValidator(IPermissionsHub permissionsHub, UserEntityResolver userEntityResolver, Func<TParent, Guid> parentCorporateIdAccessor, Func<TChild, Guid> childCorporateIdAccessor)
        {
            this.permissionsHub = permissionsHub;
            this.userEntityResolver = userEntityResolver;
            this.parentCorporateIdAccessor = parentCorporateIdAccessor;
            this.childCorporateIdAccessor = childCorporateIdAccessor;
        }

        public async Task<Boolean> CanIndexAsync(TParent parentEntity)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate);
                case Corporate corporate:
                    return this.parentCorporateIdAccessor(parentEntity) == corporate.Id;
                default:
                    return false;
            }
        }

        public Task<Boolean> CanCreateAsync(TParent parentEntity)
        {
            return Task.FromResult(false);
        }

        public async Task<Boolean> CanDetailsAsync(TChild entity)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate);
                case Corporate corporate:
                    return this.childCorporateIdAccessor(entity) == corporate.Id;
                default:
                    return false;
            }
        }

        public Task<Boolean> CanEditAsync(TChild entity)
        {
            return Task.FromResult(false);
        }

        public Task<Boolean> CanDeleteAsync(TChild entity)
        {
            return Task.FromResult(false);
        }

        public async Task<IQueryable<TChild>> RequireReadAccessAsync(TParent parent, IQueryable<TChild> query)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return query;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate) ? query : query.Where(x => false);
                case Corporate corporate:
                    if (this.parentCorporateIdAccessor(parent) == corporate.Id)
                    {
                        return query;
                    }

                    return query.Where(x => false);
                default:
                    return query.Where(x => false);
            }
        }

        public async Task DemandCanIndexAsync(TParent parentEntity)
        {
            if (!await this.CanIndexAsync(parentEntity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanCreateAsync(TParent parentEntity)
        {
            if (!await this.CanCreateAsync(parentEntity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDetailsAsync(TChild entity)
        {
            if (!await this.CanDetailsAsync(entity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanEditAsync(TChild entity)
        {
            if (!await this.CanEditAsync(entity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDeleteAsync(TChild entity)
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
