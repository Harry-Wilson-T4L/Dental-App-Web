using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUHierarchy
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        [MaxLength(320)]
        public Byte[] HierarchyId { get; set; }

        public Int32 Level { get; set; }

        [Required]
        [MaxLength(80)]
        public Byte[] HierarchyOrderNo { get; set; }
    }
}
