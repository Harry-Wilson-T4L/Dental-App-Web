using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Permissions.Models;

namespace DentalDrill.CRM.Permissions
{
    /// <summary>
    /// Defines permissions that could be applied at application level.
    /// </summary>
    /// <seealso cref="PermissionsNamespace" />
    public class RootPermissionsNamespace : PermissionsNamespace
    {
        /// <summary>
        /// Gets the permission that allows to administer the application.
        /// </summary>
        /// <value>
        /// The permission that allows to administer the application.
        /// </value>
        public Permission Administer { get; } = new Permission("{C796E4E9-F866-4C55-801E-E35D938CCE83}", "Administer", 1 << 0);
    }
}
