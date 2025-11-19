using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models
{
    public class EmployeeRole
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public GlobalComponent GlobalComponentRead { get; set; }

        public GlobalComponent GlobalComponentWrite { get; set; }

        public ClientEntityComponent ClientComponentRead { get; set; }

        public ClientEntityComponent ClientComponentWrite { get; set; }

        public ClientEntityField ClientFieldRead { get; set; }

        public ClientEntityField ClientFieldWrite { get; set; }

        public ClientEntityField ClientFieldInit { get; set; }

        public InventoryPermissions InventoryAccess { get; set; }

        public ICollection<EmployeeRoleWorkshop> WorkshopRoles { get; set; }
    }
}
