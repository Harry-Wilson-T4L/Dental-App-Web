using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class TutorialVideoViewModel
    {
        public Guid Id { get; set; }

        public Int32 OrderNo { get; set; }

        [Required]
        [MaxLength(200)]
        public String Title { get; set; }

        [Required]
        [MaxLength(500)]
        public String VideoUrl { get; set; }

        public Boolean AvailableForClients { get; set; }

        public Boolean AvailableForCorporates { get; set; }

        public Boolean AvailableForStaff { get; set; }
    }
}
