using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceImageEditViewModel : IEditModelOriginalEntity<HandpieceImage>
    {
        public HandpieceImage Original { get; set; }

        [Display(Name = "Image")]
        public Guid? ImageId { get; set; }

        public Guid ParentId { get; set; }

        [Display(Name = "Display")]
        public Boolean Display { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }
    }
}
