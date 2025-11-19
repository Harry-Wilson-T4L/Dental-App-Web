using System;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class SurgeryHandpieceViewModel
    {
        public Guid Id { get; set; }

        public String JobNumber { get; set; }

        public String Brand { get; set; }

        public String MakeAndModel { get; set; }

        public String Serial { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ExternalHandpieceStatus Status { get; set; }

        public Int32 Rating { get; set; }

        public UploadedImage Image { get; set; }

        public String ImageUrl { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public HandpieceSpeed SpeedType { get; set; }

        public DateTime Received { get; set; }

        public Guid? ClientId { get; set; }
    }
}
