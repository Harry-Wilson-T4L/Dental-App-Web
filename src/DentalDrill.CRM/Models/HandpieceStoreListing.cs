using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Models
{
    public enum HandpieceStoreListingStatus
    {
        Listed = 0,
        Unlisted = 1,
        Deleted = 2,
        Requested = 3,
        Sold = 4,
    }

    public class HandpieceStoreListing
    {
        public Guid Id { get; set; }

        public Guid ModelId { get; set; }

        public HandpieceModel Model { get; set; }

        public HandpieceStoreListingStatus Status { get; set; }

        [MaxLength(100)]
        [Display(Name = "S/N")]
        public String SerialNumber { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Requested On")]
        public DateTime? RequestedOn { get; set; }

        [Display(Name = "Sold On")]
        public DateTime? SoldOn { get; set; }

        [Display(Name = "Price")]
        public Decimal Price { get; set; }

        [MaxLength(200)]
        [Display(Name = "Warranty")]
        public String Warranty { get; set; }

        [Display(Name = "Notes")]
        public String Notes { get; set; }

        [MaxLength(200)]
        [Display(Name = "Coupling/Fitting")]
        public String Coupling { get; set; }

        [MaxLength(200)]
        [Display(Name = "Cosmetic Condition")]
        public String CosmeticCondition { get; set; }

        [MaxLength(200)]
        [Display(Name = "Fibre Optic Brightness")]
        public String FiberOpticBrightness { get; set; }

        [Display(Name = "Main Image")]
        public Guid? MainImageId { get; set; }

        [Display(Name = "Main Image")]
        public UploadedImage MainImage { get; set; }

        public ICollection<HandpieceStoreListingImage> Images { get; set; }
    }
}
