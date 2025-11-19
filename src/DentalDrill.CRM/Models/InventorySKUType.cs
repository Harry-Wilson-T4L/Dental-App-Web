using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUType : IOrderableEntity, ISoftDelete
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        [Required]
        [MaxLength(200)]
        public String NormalizedName { get; set; }

        public Int32 OrderNo { get; set; }

        public HandpieceSpeedCompatibility? HandpieceSpeedCompatibility { get; set; }

        public InventorySKUTypeStatisticsMode StatisticsMode { get; set; }

        public DeletionStatus DeletionStatus { get; set; }

        public DateTime? DeletionDate { get; set; }

        public ICollection<InventorySKU> SKUs { get; set; }
    }
}
