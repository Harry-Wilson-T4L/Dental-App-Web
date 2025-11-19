using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Services.Permissions
{
    public class ClientComponentPermissionsManagerConfig
    {
        public ClientComponentPermissionsManagerConfig(
            ClientEntityComponent component)
        {
            this.Component = component;
        }

        public ClientEntityComponent Component { get; }
    }
}
