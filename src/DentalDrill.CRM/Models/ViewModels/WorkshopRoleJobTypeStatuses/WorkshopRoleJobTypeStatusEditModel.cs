using System.Collections.Generic;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeStatuses
{
    public class WorkshopRoleJobTypeStatusEditModel
    {
        [BindNever]
        public WorkshopRoleJobTypeStatus Original { get; set; }

        [BindNever]
        public WorkshopRoleJobType Parent { get; set; }

        public List<PermissionEditModel<JobStatusFlags>> JobTransitions { get; set; } = new();

        public List<PermissionEditModel<HandpieceStatusFlags>> HandpieceTransitionsFrom { get; set; } = new();

        public List<PermissionEditModel<HandpieceStatusFlags>> HandpieceTransitionsTo { get; set; } = new();
    }
}
