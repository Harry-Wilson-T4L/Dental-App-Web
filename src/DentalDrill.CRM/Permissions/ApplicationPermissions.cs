using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Permissions.Entity;

namespace DentalDrill.CRM.Permissions
{
    /// <summary>
    /// Defines application permissions.
    /// </summary>
    public static class ApplicationPermissions
    {
        /// <summary>
        /// Gets the application root permissions.
        /// </summary>
        /// <value>
        /// The application root permissions.
        /// </value>
        public static RootPermissionsNamespace Root { get; } = new RootPermissionsNamespace();

        /// <summary>
        /// Gets the single entity permissions.
        /// </summary>
        /// <value>
        /// The single entity permissions.
        /// </value>
        public static EntityPermissionsNamespace Entity => EntityPermissions.Entity;

        /// <summary>
        /// Gets the entity type permissions.
        /// </summary>
        /// <value>
        /// The entity type permissions.
        /// </value>
        public static EntityTypePermissionsNamespace EntityType => EntityPermissions.EntityType;

        /// <summary>
        /// Gets the entity property permissions.
        /// </summary>
        /// <value>
        /// The entity property permissions.
        /// </value>
        public static EntityPropertyPermissionsNamespace EntityProperty => EntityPermissions.EntityProperty;

        /// <summary>
        /// Gets the single hierarchical entity permissions.
        /// </summary>
        /// <value>
        /// The single hierarchical entity permissions.
        /// </value>
        public static HierarchicalEntityPermissionsNamespace HierarchicalEntity => EntityPermissions.HierarchicalEntity;

        /// <summary>
        /// Gets the hierarchical entity type permissions.
        /// </summary>
        /// <value>
        /// The hierarchical entity type permissions.
        /// </value>
        public static HierarchicalEntityTypePermissionsNamespace HierarchicalEntityType => EntityPermissions.HierarchicalEntityType;

        /// <summary>
        /// Gets the single parent entity permissions.
        /// </summary>
        /// <value>
        /// The single parent entity permissions.
        /// </value>
        public static ParentEntityPermissionsNamespace ParentEntity => EntityPermissions.ParentEntity;

        /// <summary>
        /// Gets the parent entity type permissions.
        /// </summary>
        /// <value>
        /// The parent entity type permissions.
        /// </value>
        public static ParentEntityTypePermissionsNamespace ParentEntityType => EntityPermissions.ParentEntityType;
    }
}
