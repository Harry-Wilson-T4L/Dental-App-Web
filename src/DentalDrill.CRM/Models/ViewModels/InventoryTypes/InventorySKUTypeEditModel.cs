using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryTypes
{
    public class InventorySKUTypeEditModel : IEditModelOriginalEntity<InventorySKUType>
    {
        [BindNever]
        public InventorySKUType Original { get;set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Compatible Speed")]
        public HandpieceSpeedCompatibility[] HandpieceSpeedCompatibility { get; set; } = new HandpieceSpeedCompatibility[0];

        [Display(Name = "Show in Statistics")]
        public InventorySKUTypeStatisticsMode StatisticsMode { get; set; }
    }
}
