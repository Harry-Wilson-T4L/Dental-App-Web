using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceImageEditVideoViewModel
    {
        public HandpieceImage Original { get; set; }

        [Display(Name = "Video")]
        public Guid? VideoId { get; set; }

        public Guid ParentId { get; set; }

        [Display(Name = "Display")]
        public Boolean Display { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }
    }
}
