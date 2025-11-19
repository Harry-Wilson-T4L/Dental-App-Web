using System;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Models
{
    public class HandpieceImage
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public Guid? ImageId { get; set; }

        public UploadedImage Image { get; set; }

        public Guid HandpieceId { get; set; }

        public Handpiece Handpiece { get; set; }

        public Boolean Display { get; set; }

        public String Description { get; set; }

        public Guid? VideoId { get; set; }

        public UploadedFile Video { get; set; }
    }
}
