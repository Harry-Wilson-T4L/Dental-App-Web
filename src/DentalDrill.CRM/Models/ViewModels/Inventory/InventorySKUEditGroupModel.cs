using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUEditGroupModel : IEditModelOriginalEntity<InventorySKU>
    {
        [BindNever]
        public InventorySKU Original { get; set; }

        [BindNever]
        public IReadOnlyList<InventorySKU> Children { get; set; }

        [Display(Name = "Name")]
        [Required]
        public String Name { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Default Child")]
        public Guid? DefaultChild { get; set; }

        [Display(Name = "Hide from statistic")]
        public Boolean HideFromStatistic { get; set; }

        [Display(Name = "Workshop Options")]
        public List<InventorySKUWorkshopOptionsEditModel> WorkshopOptions { get; set; } = new List<InventorySKUWorkshopOptionsEditModel>();
    }
}
