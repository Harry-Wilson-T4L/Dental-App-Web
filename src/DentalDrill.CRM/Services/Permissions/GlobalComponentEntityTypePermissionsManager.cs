using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Permissions;
using DevGuild.AspNetCore.Services.Permissions.Base;
using DevGuild.AspNetCore.Services.Permissions.Entity;
using DevGuild.AspNetCore.Services.Permissions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Services.Permissions
{
    public class GlobalComponentEntityTypePermissionsManager : BasePermissionsManager<GlobalComponentPermissionsManagerConfig>
    {
        public GlobalComponentEntityTypePermissionsManager(
            IPermissionsHub hub,
            ICorePermissionsManager parentManager,
            EntityTypePermissionsNamespace permissionsNamespace,
            IServiceProvider serviceProvider,
            GlobalComponentPermissionsManagerConfig configuration)
            : base(hub, parentManager, permissionsNamespace, serviceProvider, configuration)
        {
        }

        public new EntityTypePermissionsNamespace PermissionsNamespace => (EntityTypePermissionsNamespace)base.PermissionsNamespace;

        protected override async Task<PermissionsResult> CheckExplicitPermissionAsync(Permission permission)
        {
            var employeeAccess = await this.ServiceProvider.GetRequiredService<UserEntityResolver>().GetEmployeeAccessAsync();
            var hasAccess = permission switch
            {
                not null when permission.Id == this.PermissionsNamespace.Access.Id => employeeAccess.Global.CanReadComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.ReadAnyEntity.Id => employeeAccess.Global.CanReadComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.ReadAnyProperty.Id => employeeAccess.Global.CanReadComponent(this.Configuration.Component),

                not null when permission.Id == this.PermissionsNamespace.Create.Id => employeeAccess.Global.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.CreateAnyEntity.Id => employeeAccess.Global.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.InitializeAnyProperty.Id => employeeAccess.Global.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.UpdateAnyEntity.Id => employeeAccess.Global.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.UpdateAnyProperty.Id => employeeAccess.Global.CanWriteComponent(this.Configuration.Component),
                not null when permission.Id == this.PermissionsNamespace.DeleteAnyEntity.Id => employeeAccess.Global.CanWriteComponent(this.Configuration.Component),

                _ => false,
            };

            return hasAccess ? PermissionsResult.Allow : PermissionsResult.Undefined;
        }
    }
}
