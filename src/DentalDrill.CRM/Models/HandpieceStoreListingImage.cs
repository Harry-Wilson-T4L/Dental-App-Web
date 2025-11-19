using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Models
{
    public class HandpieceStoreListingImage
    {
        public Guid Id { get; set; }

        public Guid ListingId { get; set; }

        public HandpieceStoreListing Listing { get; set; }

        public Int32 OrderNo { get; set; }

        public Guid ImageId { get; set; }

        public UploadedImage Image { get; set; }
    }
}
