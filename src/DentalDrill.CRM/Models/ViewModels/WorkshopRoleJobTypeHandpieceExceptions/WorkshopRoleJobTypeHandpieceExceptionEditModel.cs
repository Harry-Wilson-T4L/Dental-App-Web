using System.Collections.Generic;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.Permissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeHandpieceExceptions
{
    public class WorkshopRoleJobTypeHandpieceExceptionEditModel
    {
        [BindNever]
        public WorkshopRoleJobTypeHandpieceException Original { get; set; }

        [BindNever]
        public WorkshopRoleJobType Parent { get; set; }

        public List<PermissionEditModel<JobStatusFlags>> WhenJobStatus { get; set; } = new();

        public List<PermissionEditModel<HandpieceStatusFlags>> WhenHandpieceStatus { get; set; } = new();

        public List<PermissionReadWriteEditModel<HandpieceEntityField>> Fields { get; set; } = new();
    }
}
