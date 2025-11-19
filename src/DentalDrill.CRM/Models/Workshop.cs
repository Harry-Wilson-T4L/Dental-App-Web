using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models
{
    public class Workshop : ISoftDelete, IOrderableEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public String Description { get; set; }

        public Int32 OrderNo { get; set; }

        public DeletionStatus DeletionStatus { get; set; }

        public DateTime? DeletionDate { get; set; }

        public ICollection<Job> Jobs { get; set; }

        public ICollection<InventoryMovement> InventoryMovements { get; set; }
    }
}
