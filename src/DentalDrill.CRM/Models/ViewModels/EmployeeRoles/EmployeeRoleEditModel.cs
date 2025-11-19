using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.EmployeeRoles
{
    public class EmployeeRoleEditModel
    {
        [BindNever]
        public EmployeeRole Original { get; set; }

        public String Name { get; set; }

        public List<PermissionReadWriteEditModel<GlobalComponent>> Global { get; set; } = new();

        public List<PermissionReadWriteEditModel<ClientEntityComponent>> ClientComponents { get; set; } = new();

        public List<PermissionReadWriteInitEditModel<ClientEntityField>> ClientFields { get; set; } = new();

        public List<PermissionEditModel<InventoryPermissions>> Inventory { get; set; } = new();
    }
}
