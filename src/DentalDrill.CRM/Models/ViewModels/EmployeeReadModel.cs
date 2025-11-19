using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class EmployeeReadModel
    {
        public Guid Id { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public Guid RoleId { get; set; }

        public String RoleName { get; set; }

        public EmployeeType Type { get; set; }

        public String TypeName { get; set; }

        public String UserName { get; set; }

        public String DeletionStatus { get; set; }
    }
}
