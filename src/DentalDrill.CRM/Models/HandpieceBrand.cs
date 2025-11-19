using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Models
{
    public class HandpieceBrand
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        [Required]
        public String Name { get; set; }

        [MaxLength(200)]
        [Required]
        public String NormalizedName { get; set; }

        public Guid? ImageId { get; set; }

        public UploadedImage Image { get; set; }

        public ICollection<HandpieceModel> Models { get; set; }
    }
}
