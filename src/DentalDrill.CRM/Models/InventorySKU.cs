using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models
{
    public class InventorySKU : IHierarchicalEntity<Guid, InventorySKU>, ISoftDelete, IOrderableEntity
    {
        public const Decimal QuantityPrecision = 0.0001m;

        public Guid Id { get; set; }

        public Guid TypeId { get; set; }

        public InventorySKUType Type { get; set; }

        public Guid? ParentId { get; set; }

        public InventorySKU Parent { get; set; }

        public Int32 OrderNo { get; set; }

        public ICollection<InventorySKU> Children { get; set; }

        public Guid? DefaultChildId { get; set; }

        public InventorySKU DefaultChild { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        [Required]
        [MaxLength(200)]
        public String NormalizedName { get; set; }

        public Decimal? AveragePrice { get; set; }

        public Decimal? WarningQuantity { get; set; }

        public String Description { get; set; }

        public InventorySKUNodeType NodeType { get; set; }

        public DeletionStatus DeletionStatus { get; set; }

        public DateTime? DeletionDate { get; set; }

        public Boolean HideFromStatistic { get; set; }

        public ICollection<InventorySKUWorkshopOptions> WorkshopOptions { get; set; }

        public ICollection<InventoryMovement> Movements { get; set; }

        public ICollection<InventoryMovementChange> MovementChanges { get; set; }
    }
}
