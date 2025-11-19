using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM.Permissions.Validators
{
    public class ClientDependentPermissionsValidator<T> : IDependentEntityPermissionsValidator<T, Client>
    {
        private readonly IPermissionsHub permissionsHub;
        private readonly UserEntityResolver userEntityResolver;
        private readonly Func<T, Guid> clientIdAccessor;

        public ClientDependentPermissionsValidator(IPermissionsHub permissionsHub, UserEntityResolver userEntityResolver, Func<T, Guid> clientIdAccessor)
        {
            this.permissionsHub = permissionsHub;
            this.userEntityResolver = userEntityResolver;
            this.clientIdAccessor = clientIdAccessor;
        }

        public async Task<Boolean> CanIndexAsync(Client parentEntity)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients();
                case Client client:
                    return parentEntity.Id == client.Id;
                default:
                    return false;
            }
        }

        public Task<Boolean> CanCreateAsync(Client parentEntity)
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
                    return access.CanAccessClients();
                case Client client:
                    return this.clientIdAccessor(entity) == client.Id;
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

        public async Task<IQueryable<T>> RequireReadAccessAsync(Client parent, IQueryable<T> query)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return query;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() ? query : query.Where(x => false);
                case Client client when client.Id == parent.Id:
                    return query;
                default:
                    return query.Where(x => false);
            }
        }

        public async Task DemandCanIndexAsync(Client parentEntity)
        {
            if (!await this.CanIndexAsync(parentEntity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanCreateAsync(Client parentEntity)
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
