using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public abstract class HandpieceModelSchematicEditBaseModel
    {
        [BindNever]
        public HandpieceModel Parent { get; set; }

        [BindNever]
        public HandpieceModelSchematic Original { get; set; }

        [BindNever]
        public HandpieceModelSchematicType Type { get; set; }

        [MaxLength(200)]
        public String Title { get; set; }

        public Boolean Display { get; set; }
    }
}
