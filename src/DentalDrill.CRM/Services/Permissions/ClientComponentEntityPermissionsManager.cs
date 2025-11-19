using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Permissions;
using DevGuild.AspNetCore.Services.Permissions.Base;
using DevGuild.AspNetCore.Services.Permissions.Entity;
using DevGuild.AspNetCore.Services.Permissions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Services.Permissions
{
    public class ClientComponentEntityPermissionsManager<T> : BasePermissionsManager<ClientComponentPermissionsManagerConfig, T>, IQueryFilteringPermissionsManager<T>
    {
        public ClientComponentEntityPermissionsManager(IPermissionsHub hub, ICorePermissionsManager parentManager, PermissionsNamespace permissionsNamespace, IServiceProvider serviceProvider, ClientComponentPermissionsManagerConfig configuration)
            : base(hub, parentManager, permissionsNamespace, serviceProvider, configuration)
        {
        }

        public new EntityPermissionsNamespace PermissionsNamespace => (EntityPermissionsNamespace)base.PermissionsNamespace;

        public async Task<IQueryable<T>> ApplyQueryFilterAsync(IQueryable<T> query, Permission permission)
        {
            var hasAccess = await this.CheckExplicitPermissionAsync(default, permission);
            return hasAccess == PermissionsResult.Allow
                ? query
                : query.Where(x => false);
        }

        public async Task<IQueryable<T>> ApplyQueryFilterAsync(IQueryable<T> query, IEnumerable<Permission> permissions)
        {
            var hasAccess = new List<PermissionsResult>();
            foreach (var permission in permissions)
            {
                hasAccess.Add(await this.CheckExplicitPermissionAsync(default, permission));
            }

            return hasAccess.All(x => x == PermissionsResult.Allow)
                ? query
                : query.Where(x => false);
        }

        protected override async Task<PermissionsResult> CheckExplicitPermissionAsync(T securedObject, Permission permission)
        {
            var employeeAccess = await this.ServiceProvider.GetRequiredService<UserEntityResolver>().GetEmployeeAccessAsync();
            var hasAccess = permission switch
            {
                not null when permission.Id == this.PermissionsNamespace.Read.Id => employeeAccess.Clients.CanReadComponent(this.Configuration.Component),

                not null when permission.Id == this.PermissionsNamespace.Update.Id => employeeAccess.Clients.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.Delete.Id => employeeAccess.Clients.CanWriteComponent(this.Configuration.Component),

                _ => false,
            };

            return hasAccess ? PermissionsResult.Allow : PermissionsResult.Undefined;
        }
    }
}
