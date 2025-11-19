using System;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models
{
    public class TutorialVideo : IOrderableEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public String PageId { get; set; }

        public TutorialPage Page { get; set; }

        public Int32 OrderNo { get; set; }

        [Required]
        [MaxLength(200)]
        public String Title { get; set; }

        [Required]
        [MaxLength(500)]
        public String VideoUrl { get; set; }

        public TutorialVideoAvailability Availability { get; set; }
    }
}
