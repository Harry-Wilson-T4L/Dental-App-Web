using System;

namespace DentalDrill.CRM.Models.ViewModels.Permissions
{
    public class PermissionReadWriteEditModel<TObject>
        where TObject : struct, Enum
    {
        public PermissionReadWriteEditModel()
        {
            this.Object = default;
            this.Read = false;
            this.Write = false;
        }

        public PermissionReadWriteEditModel(TObject obj)
        {
            this.Object = obj;
            this.Read = false;
            this.Write = false;
        }

        public PermissionReadWriteEditModel(TObject obj, Boolean read, Boolean write)
        {
            this.Object = obj;
            this.Read = read;
            this.Write = write;
        }

        public TObject Object { get; set; }

        public Boolean Read { get; set; }

        public Boolean Write { get; set; }
    }
}
