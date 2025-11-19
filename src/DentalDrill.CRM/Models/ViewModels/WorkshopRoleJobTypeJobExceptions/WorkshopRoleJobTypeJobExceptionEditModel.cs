using System.Collections.Generic;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeJobExceptions
{
    public class WorkshopRoleJobTypeJobExceptionEditModel
    {
        [BindNever]
        public WorkshopRoleJobTypeJobException Original { get; set; }

        [BindNever]
        public WorkshopRoleJobType Parent { get; set; }

        public List<PermissionEditModel<JobStatusFlags>> WhenJobStatus { get; set; } = new();

        public List<PermissionReadWriteEditModel<JobEntityField>> Fields { get; set; } = new();
    }
}
