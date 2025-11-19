using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models
{
    public class WorkshopRoleJobType
    {
        public Guid Id { get; set; }

        public Guid WorkshopRoleId { get; set; }

        public WorkshopRole WorkshopRole { get; set; }

        public String JobTypeId { get; set; }

        public JobType JobType { get; set; }

        public JobEntityComponent JobComponentRead { get; set; }

        public JobEntityComponent JobComponentWrite { get; set; }

        public JobEntityField JobFieldRead { get; set; }

        public JobEntityField JobFieldWrite { get; set; }

        public JobEntityField JobFieldInit { get; set; }

        public HandpieceEntityComponent HandpieceComponentRead { get; set; }

        public HandpieceEntityComponent HandpieceComponentWrite { get; set; }

        public HandpieceEntityField HandpieceFieldRead { get; set; }

        public HandpieceEntityField HandpieceFieldWrite { get; set; }

        public HandpieceEntityField HandpieceFieldInit { get; set; }

        public ICollection<WorkshopRoleJobTypeStatus> StatusPermissions { get; set; }

        public ICollection<WorkshopRoleJobTypeJobException> JobExceptions { get; set; }

        public ICollection<WorkshopRoleJobTypeHandpieceException> HandpieceExceptions { get; set; }
    }
}
