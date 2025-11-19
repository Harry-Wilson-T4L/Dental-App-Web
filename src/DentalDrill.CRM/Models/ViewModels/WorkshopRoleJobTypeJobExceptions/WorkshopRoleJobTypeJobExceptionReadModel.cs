using System;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypeJobExceptions
{
    public class WorkshopRoleJobTypeJobExceptionReadModel
    {
        public Guid Id { get; set; }

        public String JobTypeId { get; set; }

        public String JobTypeName { get; set; }

        public String WhenJobStatus { get; set; }

        public String ReadOnlyFields { get; set; }

        public String HiddenFields { get; set; }
    }
}
