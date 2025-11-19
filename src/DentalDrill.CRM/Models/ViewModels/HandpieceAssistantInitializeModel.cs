using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceAssistantInitializeModel
    {
        [BindNever]
        public Handpiece Handpiece { get; set; }

        [BindNever]
        public HandpieceBrand Brand { get; set; }

        [BindNever]
        public HandpieceModel Model { get; set; }

        [Display(Name = "Type")]
        public HandpieceSpeed Type { get; set; }

        [Display(Name = "Image")]
        public Guid? ImageId { get; set; }

        public String Description { get; set; }
    }
}
