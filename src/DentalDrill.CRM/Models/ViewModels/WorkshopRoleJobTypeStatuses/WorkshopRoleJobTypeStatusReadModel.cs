using System;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeStatuses
{
    public class WorkshopRoleJobTypeStatusReadModel
    {
        public Guid Id { get; set; }

        public String JobTypeId { get; set; }

        public String JobTypeName { get; set; }

        public String JobStatus { get; set; }

        public String JobTransitions { get; set; }

        public String HandpieceTransitionsFrom { get; set; }

        public String HandpieceTransitionsTo { get; set; }
    }
}
