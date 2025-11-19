using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models
{
    public class WorkshopRoleJobTypeHandpieceException
    {
        public Guid Id { get; set; }

        public Guid WorkshopRoleJobTypeId { get; set; }

        public WorkshopRoleJobType WorkshopRoleJobType { get; set; }

        public JobStatusFlags WhenJobStatus { get; set; }

        public HandpieceStatusFlags WhenHandpieceStatus { get; set; }

        public HandpieceEntityField ReadOnlyFields { get; set; }

        public HandpieceEntityField HiddenFields { get; set; }
    }
}
