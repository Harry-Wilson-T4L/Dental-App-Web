using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Services.Generation
{
    public class HandpieceImagesService
    {
        private readonly IRepository repository;
        private readonly IStorageHub storageHub;
        private readonly IImageUploadService imageUploadService;

        public HandpieceImagesService(IRepository repository, IStorageHub storageHub, IImageUploadService imageUploadService)
        {
            this.repository = repository;
            this.storageHub = storageHub;
            this.imageUploadService = imageUploadService;
        }

        public HandpieceImagesLoader CreateLoader(IFormFile file)
        {
            return new HandpieceImagesLoader(this.imageUploadService, file);
        }

        public async Task<Byte[]> CreateArchive()
        {
            var handpieces = await this.repository.Query<Handpiece>("Images", "Images.Image").ToListAsync();
            var entries = new Dictionary<String, UploadedImage>();
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var handpiece in handpieces)
                    {
                        var images = handpiece.Images.Select(x => x.Image).ToList();
                        if (images.Count == 0)
                        {
                            continue;
                        }

                        var brandName = this.NormalizeName(handpiece.Brand);
                        var modelName = this.NormalizeName(handpiece.MakeAndModel);

                        foreach (var image in images)
                        {
                            var variation = image.Variations.Single(x => x.Id == "2560");
                            var fullName = $"{brandName}/{modelName}/{image.Id:N}.{variation.Extension}";
                            if (entries.ContainsKey(fullName))
                            {
                                continue;
                            }

                            entries.Add(fullName, image);

                            var container = this.storageHub.GetContainer(image.Container);
                            using (var imageStream = await container.GetFileContentAsync(image.GetContainerPath(variation.Id)))
                            {
                                var entry = zipArchive.CreateEntry(fullName);
                                using (var entryStream = entry.Open())
                                {
                                    await imageStream.CopyToAsync(entryStream);
                                    await entryStream.FlushAsync();
                                }
                            }
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }

        private String NormalizeName(String name)
        {
            name = name.Trim();
            name = Regex.Replace(name, @"\s", String.Empty);
            name = Regex.Replace(name, @"[^A-Za-z0-9]", "_");

            return name;
        }
    }
}
