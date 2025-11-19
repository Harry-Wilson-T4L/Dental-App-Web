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
    public class CorporatePermissionsValidator : IEntityPermissionsValidator<Corporate>
    {
        private readonly IPermissionsHub permissionsHub;
        private readonly UserEntityResolver userEntityResolver;

        public CorporatePermissionsValidator(IPermissionsHub permissionsHub, UserEntityResolver userEntityResolver)
        {
            this.permissionsHub = permissionsHub;
            this.userEntityResolver = userEntityResolver;
        }

        public async Task<Boolean> CanIndexAsync()
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate);
                default:
                    return false;
            }
        }

        public Task<Boolean> CanCreateAsync()
        {
            return Task.FromResult(false);
        }

        public async Task<Boolean> CanDetailsAsync(Corporate entity)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return true;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate);
                case Corporate corporate when corporate.Id == entity.Id:
                    return true;
                default:
                    return false;
            }
        }

        public Task<Boolean> CanEditAsync(Corporate entity)
        {
            return Task.FromResult(false);
        }

        public Task<Boolean> CanDeleteAsync(Corporate entity)
        {
            return Task.FromResult(false);
        }

        public async Task<IQueryable<Corporate>> RequireReadAccessAsync(IQueryable<Corporate> query)
        {
            switch (await this.userEntityResolver.ResolveCurrentUserEntity())
            {
                case Administrator _:
                    return query;
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return access.CanAccessClients() && access.Global.CanReadComponent(GlobalComponent.Corporate) ? query : query.Where(x => false);
                case Corporate corporate:
                    return query.Where(x => x.Id == corporate.Id);
                default:
                    return query.Where(x => false);
            }
        }

        public async Task DemandCanIndexAsync()
        {
            if (!await this.CanIndexAsync())
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanCreateAsync()
        {
            if (!await this.CanCreateAsync())
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDetailsAsync(Corporate entity)
        {
            if (!await this.CanDetailsAsync(entity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanEditAsync(Corporate entity)
        {
            if (!await this.CanEditAsync(entity))
            {
                throw await this.permissionsHub.CreateUnauthorizedExceptionAsync();
            }
        }

        public async Task DemandCanDeleteAsync(Corporate entity)
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
