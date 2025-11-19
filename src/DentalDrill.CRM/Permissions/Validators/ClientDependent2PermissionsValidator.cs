using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM.Permissions.Validators
{
    public class ClientDependent2PermissionsValidator<TChild, TParent> : IDependentEntityPermissionsValidator<TChild, TParent>
    {
        private readonly IPermissionsHub permissionsHub;
        private readonly UserEntityResolver userEntityResolver;
        private readonly Func<TParent, Guid> parentClientIdAccessor;
        private readonly Func<TChild, Guid> childClientIdAccessor;

        public ClientDependent2PermissionsValidator(IPermissionsHub permissionsHub, UserEntityResolver userEntityResolver, Func<TParent, Guid> parentClientIdAccessor, Func<TChild, Guid> childClientIdAccessor)
        {
            this.permissionsHub = permissionsHub;
            this.userEntityResolver = userEntityResolver;
            this.parentClientIdAccessor = parentClientIdAccessor;
            this.childClientIdAccessor = childClientIdAccessor;
        }

        public async Task<Boolean> CanIndexAsync(TParent parentEntity)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients();
                case Client client:
                    return this.parentClientIdAccessor(parentEntity) == client.Id;
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
                    return access.CanAccessClients();
                case Client client:
                    return this.childClientIdAccessor(entity) == client.Id;
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
                    return access.CanAccessClients() ? query : query.Where(x => false);
                case Client client:
                    if (this.parentClientIdAccessor(parent) == client.Id)
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
