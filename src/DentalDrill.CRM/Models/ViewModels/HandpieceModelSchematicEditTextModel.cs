using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelSchematicEditTextModel : HandpieceModelSchematicEditBaseModel
    {
        public HandpieceModelSchematicEditTextModel()
        {
            this.Type = HandpieceModelSchematicType.Text;
        }

        [Required]
        public String Text { get; set; }
    }
}
