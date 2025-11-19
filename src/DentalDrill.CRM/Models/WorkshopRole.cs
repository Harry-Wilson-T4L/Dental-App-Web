using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models
{
    public class WorkshopRole
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public WorkshopPermissions WorkshopAccess { get; set; }

        public InventoryMovementPermissions InventoryAccess { get; set; }

        public ICollection<WorkshopRoleJobType> JobTypePermissions { get; set; }
    }
}
