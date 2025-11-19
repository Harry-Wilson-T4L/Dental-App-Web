using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelSchematicEditAttachmentModel : HandpieceModelSchematicEditBaseModel
    {
        public HandpieceModelSchematicEditAttachmentModel()
        {
            this.Type = HandpieceModelSchematicType.Attachment;
        }

        [Required]
        public Guid? AttachmentId { get; set; }
    }
}
