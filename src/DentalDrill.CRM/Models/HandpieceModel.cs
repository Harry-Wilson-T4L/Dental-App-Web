using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Models
{
    public class HandpieceModel
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public HandpieceBrand Brand { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        [Required]
        [MaxLength(200)]
        public String NormalizedName { get; set; }

        public HandpieceSpeed Type { get; set; }

        public Guid? ImageId { get; set; }

        public UploadedImage Image { get; set; }

        public String Description { get; set; }

        public String WorkshopNotes { get; set; }

        public Decimal? PriceNew { get; set; }

        public ICollection<HandpieceModelSchematic> Schematics { get; set; }

        public ICollection<HandpieceStoreListing> StoreListings { get; set; }
    }
}
