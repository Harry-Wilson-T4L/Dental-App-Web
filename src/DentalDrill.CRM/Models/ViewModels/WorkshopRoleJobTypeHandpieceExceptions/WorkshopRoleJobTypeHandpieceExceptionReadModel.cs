using System;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeHandpieceExceptions
{
    public class WorkshopRoleJobTypeHandpieceExceptionReadModel
    {
        public Guid Id { get; set; }

        public String JobTypeId { get; set; }

        public String JobTypeName { get; set; }

        public String WhenJobStatus { get; set; }

        public String WhenHandpieceStatus { get; set; }

        public String ReadOnlyFields { get; set; }

        public String HiddenFields { get; set; }
    }
}
