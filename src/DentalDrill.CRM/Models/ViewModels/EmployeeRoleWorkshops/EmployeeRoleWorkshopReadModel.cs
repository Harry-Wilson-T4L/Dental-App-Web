using System;

namespace DentalDrill.CRM.Models.ViewModels.EmployeeRoleWorkshops
{
    public class EmployeeRoleWorkshopReadModel
    {
        public Guid Id { get; set; }

        public Guid WorkshopId { get; set; }

        public String WorkshopName { get; set; }

        public Guid WorkshopRoleId { get; set; }

        public String WorkshopRoleName { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public String EmployeeTypeName { get; set; }
    }
}
