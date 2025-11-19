using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Models
{
    public class ClientAppearance : IClientDependent
    {
        public Guid Id { get; set; }

        public Guid? LogoId { get; set; }

        public UploadedImage Logo { get; set; }

        public Guid? BackgroundImageId { get; set; }

        public UploadedImage BackgroundImage { get; set; }

        public String PrimaryColor { get; set; }

        public String SecondaryColor { get; set; }

        public String HeaderTextColor { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public static ClientAppearance Default()
        {
            return new ClientAppearance
            {
                PrimaryColor = "#ffffff",
                SecondaryColor = "#ffffff",
                HeaderTextColor = "#333333",
            };
        }
    }
}
