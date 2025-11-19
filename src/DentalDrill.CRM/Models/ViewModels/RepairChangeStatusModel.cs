using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class RepairChangeStatusModel : IEditModelOriginalEntity<RepairDetailsViewModel>
    {
        public RepairDetailsViewModel Original { get; set; }

        [Display(Name = "Status")]
        public Int32 StatusNumber { get; set; }

        public List<SelectEnumViewModel> Statuses { get; set; }
    }
}
