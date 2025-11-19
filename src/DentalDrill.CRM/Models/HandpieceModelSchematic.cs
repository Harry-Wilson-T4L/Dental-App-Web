using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Models
{
    public enum HandpieceModelSchematicType
    {
        Text,
        Attachment,
        Image,
    }

    public class HandpieceModelSchematic
    {
        public Guid Id { get; set; }

        public Guid ModelId { get; set; }

        public HandpieceModel Model { get; set; }

        public Int32 OrderNo { get; set; }

        [MaxLength(200)]
        public String Title { get; set; }

        public Boolean Display { get; set; }

        public HandpieceModelSchematicType Type { get; set; }

        public String Text { get; set; }

        public Guid? AttachmentId { get; set; }

        public UploadedFile Attachment { get; set; }

        public Guid? ImageId { get; set; }

        public UploadedImage Image { get; set; }
    }
}
