using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models
{
    public class WorkshopRoleJobTypeJobException
    {
        public Guid Id { get; set; }

        public Guid WorkshopRoleJobTypeId { get; set; }

        public WorkshopRoleJobType WorkshopRoleJobType { get; set; }

        public JobStatusFlags WhenJobStatus { get; set; }

        public JobEntityField ReadOnlyFields { get; set; }

        public JobEntityField HiddenFields { get; set; }
    }
}
