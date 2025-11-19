using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceImageViewModel
    {
        [PropertyMapping(PropertyMappingMode.ToDetails)]
        public Guid Id { get; set; }

        public Guid HandpieceId { get; set; }

        [Display(Name = "Uploaded Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Thumbnail")]
        public String ImageUrl { get; set; }

        [Display(Name = "Video")]
        public String VideoUrl { get; set; }

        [Display(Name = "Display")]
        public Boolean Display { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }
    }
}
