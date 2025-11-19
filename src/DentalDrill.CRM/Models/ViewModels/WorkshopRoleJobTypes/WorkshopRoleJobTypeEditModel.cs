using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypes
{
    public class WorkshopRoleJobTypeEditModel
    {
        [BindNever]
        public WorkshopRoleJobType Original { get; set; }

        [BindNever]
        public WorkshopRole Parent { get; set; }

        public List<PermissionReadWriteEditModel<JobEntityComponent>> JobComponents { get; set; } = new();

        public List<PermissionReadWriteInitEditModel<JobEntityField>> JobFields { get; set; } = new();

        public List<PermissionReadWriteEditModel<HandpieceEntityComponent>> HandpieceComponents { get; set; } = new();

        public List<PermissionReadWriteInitEditModel<HandpieceEntityField>> HandpieceFields { get; set; } = new();
    }
}
