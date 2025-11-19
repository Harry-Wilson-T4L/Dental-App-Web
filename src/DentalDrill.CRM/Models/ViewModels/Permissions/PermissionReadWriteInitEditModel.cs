using System;

namespace DentalDrill.CRM.Models.ViewModels.Permissions
{
    public class PermissionReadWriteInitEditModel<TObject>
        where TObject : struct, Enum
    {
        public PermissionReadWriteInitEditModel()
        {
            this.Object = default;
            this.Read = false;
            this.Write = false;
            this.Init = false;
        }

        public PermissionReadWriteInitEditModel(TObject obj)
        {
            this.Object = obj;
            this.Read = false;
            this.Write = false;
            this.Init = false;
        }

        public PermissionReadWriteInitEditModel(TObject obj, Boolean read, Boolean write, Boolean init)
        {
            this.Object = obj;
            this.Read = read;
            this.Write = write;
            this.Init = init;
        }

        public TObject Object { get; set; }

        public Boolean Read { get; set; }

        public Boolean Write { get; set; }

        public Boolean Init { get; set; }
    }
}
