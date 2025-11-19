using System;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Permissions;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM.Services.Permissions
{
    public class EmployeeRolePermissionsManager
    {
        public static PermissionsManagerConstructor RequireGlobalPermission(GlobalComponent globalComponent)
        {
            return (hub, parent, permissionsNamespace, provider) =>
            {
                if (Object.ReferenceEquals(permissionsNamespace, ApplicationPermissions.EntityType))
                {
                    return new GlobalComponentEntityTypePermissionsManager(
                        hub,
                        parent,
                        ApplicationPermissions.EntityType,
                        provider,
                        new GlobalComponentPermissionsManagerConfig(globalComponent));
                }
                else
                {
                    throw new InvalidOperationException($"Permission namespace {permissionsNamespace.Name} is not supported");
                }
            };
        }

        public static PermissionsManagerConstructor RequireGlobalPermissions<T>(GlobalComponent globalComponent)
        {
            return (hub, parent, permissionsNamespace, provider) =>
            {
                if (Object.ReferenceEquals(permissionsNamespace, ApplicationPermissions.EntityType))
                {
                    return new GlobalComponentEntityTypePermissionsManager(
                        hub,
                        parent,
                        ApplicationPermissions.EntityType,
                        provider,
                        new GlobalComponentPermissionsManagerConfig(globalComponent));
                }
                else if (Object.ReferenceEquals(permissionsNamespace, ApplicationPermissions.Entity))
                {
                    return new GlobalComponentEntityPermissionsManager<T>(
                        hub,
                        parent,
                        ApplicationPermissions.Entity,
                        provider,
                        new GlobalComponentPermissionsManagerConfig(globalComponent));
                }
                else
                {
                    throw new InvalidOperationException($"Permission namespace {permissionsNamespace.Name} is not supported");
                }
            };
        }

        public static PermissionsManagerConstructor RequireClientsPermissions(ClientEntityComponent component)
        {
            return (hub, parent, permissionsNamespace, provider) =>
            {
                if (Object.ReferenceEquals(permissionsNamespace, ApplicationPermissions.EntityType))
                {
                    return new ClientComponentEntityTypePermissionsManager(
                        hub,
                        parent,
                        ApplicationPermissions.EntityType,
                        provider,
                        new ClientComponentPermissionsManagerConfig(component));
                }
                else
                {
                    throw new InvalidOperationException($"Permission namespace {permissionsNamespace.Name} is not supported");
                }
            };
        }

        public static PermissionsManagerConstructor RequireClientsPermissions<T>(ClientEntityComponent component)
        {
            return (hub, parent, permissionsNamespace, provider) =>
            {
                if (Object.ReferenceEquals(permissionsNamespace, ApplicationPermissions.EntityType))
                {
                    return new ClientComponentEntityTypePermissionsManager(
                        hub,
                        parent,
                        ApplicationPermissions.EntityType,
                        provider,
                        new ClientComponentPermissionsManagerConfig(component));
                }
                else if (Object.ReferenceEquals(permissionsNamespace, ApplicationPermissions.Entity))
                {
                    return new ClientComponentEntityPermissionsManager<T>(
                        hub,
                        parent,
                        ApplicationPermissions.Entity,
                        provider,
                        new ClientComponentPermissionsManagerConfig(component));
                }
                else
                {
                    throw new InvalidOperationException($"Permission namespace {permissionsNamespace.Name} is not supported");
                }
            };
        }
    }
}
