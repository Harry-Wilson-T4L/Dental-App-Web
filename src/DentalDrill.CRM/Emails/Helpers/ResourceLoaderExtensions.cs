using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;

namespace DentalDrill.CRM.Emails.Helpers
{
    public static class ResourceLoaderExtensions
    {
        public static void LoadUploadedImage(this ResourceLoader loader, String name, UploadedImage image, Byte[] bytes)
        {
            if (image != null && bytes != null)
            {
                var variation = image.Variations.SingleOrDefault(x => x.Id == "Original");
                if (variation != null)
                {
                    loader.LoadImage(name, $"{name}.{variation.Extension}", bytes);
                }
            }
        }
    }
}
