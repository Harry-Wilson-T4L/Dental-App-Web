using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models
{
    public class WorkshopRoleJobTypeStatus
    {
        public Guid Id { get; set; }

        public Guid WorkshopRoleJobTypeId { get; set; }

        public WorkshopRoleJobType WorkshopRoleJobType { get; set; }

        public JobStatus JobStatus { get; set; }

        public JobStatusFlags JobTransitions { get; set; }

        public HandpieceStatusFlags HandpieceTransitionsFrom { get; set; }

        public HandpieceStatusFlags HandpieceTransitionsTo { get; set; }
    }
}
