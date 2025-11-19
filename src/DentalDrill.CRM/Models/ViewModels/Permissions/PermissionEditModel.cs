using System;

namespace DentalDrill.CRM.Models.ViewModels.Permissions
{
    public class PermissionEditModel<TObject>
        where TObject : struct, Enum
    {
        public PermissionEditModel()
        {
            this.Object = default;
            this.Enabled = false;
        }

        public PermissionEditModel(TObject obj)
        {
            this.Object = obj;
            this.Enabled = false;
        }

        public PermissionEditModel(TObject obj, Boolean enabled)
        {
            this.Object = obj;
            this.Enabled = enabled;
        }

        public TObject Object { get; set; }

        public Boolean Enabled { get; set; }
    }
}
