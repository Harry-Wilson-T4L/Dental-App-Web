using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoles
{
    public class WorkshopRoleEditModel
    {
        [BindNever]
        public WorkshopRole Original { get; set; }

        public String Name { get; set; }

        public List<PermissionEditModel<WorkshopPermissions>> WorkshopAccess { get; set; } = new();

        public List<PermissionEditModel<InventoryMovementPermissions>> InventoryAccess { get; set; } = new();
    }
}
