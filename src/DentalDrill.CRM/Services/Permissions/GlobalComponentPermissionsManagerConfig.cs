using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Services.Permissions
{
    public class GlobalComponentPermissionsManagerConfig
    {
        public GlobalComponentPermissionsManagerConfig(GlobalComponent component)
        {
            this.Component = component;
        }

        public GlobalComponent Component { get; }
    }
}
