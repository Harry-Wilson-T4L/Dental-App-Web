using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelSchematicEditImageModel : HandpieceModelSchematicEditBaseModel
    {
        public HandpieceModelSchematicEditImageModel()
        {
            this.Type = HandpieceModelSchematicType.Image;
        }

        [Required]
        public Guid? ImageId { get; set; }
    }
}
