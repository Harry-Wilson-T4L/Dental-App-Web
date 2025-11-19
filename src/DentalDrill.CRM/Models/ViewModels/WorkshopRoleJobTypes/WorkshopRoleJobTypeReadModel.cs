using System;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypes
{
    public class WorkshopRoleJobTypeReadModel
    {
        public Guid Id { get; set; }

        public Guid WorkshopRoleId { get; set; }

        public String WorkshopRoleName { get; set; }

        public String JobTypeId { get; set; }

        public String JobTypeName { get; set; }

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
    }
}
