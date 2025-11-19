using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceBrandEditModel : IEditModelOriginalEntity<HandpieceBrand>
    {
        public HandpieceBrand Original { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Image")]
        public Guid? ImageId { get; set; }
    }
}
