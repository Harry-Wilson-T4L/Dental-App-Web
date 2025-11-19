using System;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Permissions;
using DevGuild.AspNetCore.Services.Permissions.Base;
using DevGuild.AspNetCore.Services.Permissions.Entity;
using DevGuild.AspNetCore.Services.Permissions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Services.Permissions
{
    public class ClientComponentEntityTypePermissionsManager : BasePermissionsManager<ClientComponentPermissionsManagerConfig>
    {
        public ClientComponentEntityTypePermissionsManager(IPermissionsHub hub, ICorePermissionsManager parentManager, PermissionsNamespace permissionsNamespace, IServiceProvider serviceProvider, ClientComponentPermissionsManagerConfig configuration)
            : base(hub, parentManager, permissionsNamespace, serviceProvider, configuration)
        {
        }

        public new EntityTypePermissionsNamespace PermissionsNamespace => (EntityTypePermissionsNamespace)base.PermissionsNamespace;

        protected override async Task<PermissionsResult> CheckExplicitPermissionAsync(Permission permission)
        {
            var employeAccess = await this.ServiceProvider.GetRequiredService<UserEntityResolver>().GetEmployeeAccessAsync();
            var hasAccess = permission switch
            {
                not null when permission.Id == this.PermissionsNamespace.Access.Id => employeAccess.Clients.CanReadComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.ReadAnyEntity.Id => employeAccess.Clients.CanReadComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.ReadAnyProperty.Id => employeAccess.Clients.CanReadComponent(this.Configuration.Component),

                not null when permission.Id == this.PermissionsNamespace.Create.Id => employeAccess.Clients.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.CreateAnyEntity.Id => employeAccess.Clients.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.InitializeAnyProperty.Id => employeAccess.Clients.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.UpdateAnyEntity.Id => employeAccess.Clients.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.UpdateAnyProperty.Id => employeAccess.Clients.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.DeleteAnyEntity.Id => employeAccess.Clients.CanWriteComponent(this.Configuration.Component),

                _ => false,
            };

            return hasAccess ? PermissionsResult.Allow : PermissionsResult.Undefined;
        }
    }
}
