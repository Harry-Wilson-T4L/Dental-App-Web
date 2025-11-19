using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Microsoft.AspNetCore.Http;

namespace DentalDrill.CRM.Services.Generation
{
    public sealed class HandpieceImagesLoader : IDisposable
    {
        private readonly IFormFile archiveFile;
        private readonly IImageUploadService imageUploadService;
        private readonly ZipArchive archive;
        private readonly Dictionary<String, Stream> entries;
        private readonly Dictionary<String, UploadedImage> resolvedImages = new Dictionary<String, UploadedImage>();

        public HandpieceImagesLoader(IImageUploadService imageUploadService, IFormFile archiveFile)
        {
            this.imageUploadService = imageUploadService;
            this.archiveFile = archiveFile;
            if (this.archiveFile != null)
            {
                this.archive = new ZipArchive(archiveFile.OpenReadStream(), ZipArchiveMode.Read, true);
                this.entries = this.archive.Entries.ToDictionary(x => x.FullName, this.OpenEntry);
            }
            else
            {
                this.archive = null;
                this.entries = new Dictionary<String, Stream>();
            }
        }

        public (String FullName, Stream Stream)[] SelectImage(String brand, String model)
        {
            var normalizedBrand = this.NormalizeName(brand);
            var normalizedModel = this.NormalizeName(model);

            var brandModelDirectoryRegex = new Regex($@"^{normalizedBrand}/{normalizedModel}/[^/]+\.(jpg|jpeg|png)$");
            var brandModelFileRegex = new Regex($@"^{normalizedBrand}/{normalizedModel}\.(jpg|jpeg|png)$");
            var brandDirectoryRegex = new Regex($@"^{normalizedBrand}/_/[^/]+\.(jpg|jpeg|png)$");
            var brandFileRegex = new Regex($@"^{normalizedBrand}\.(jpg|jpeg|png)$");
            var genericDirectoryRegex = new Regex($@"^_/[^/]+\.(jpg|jpeg|png)$");
            var genericFileRegex = new Regex($@"^_\.(jpg|jpeg|png)$");

            (String FullName, Stream Stream)[] matches;

            matches = this.entries.Where(x => brandModelDirectoryRegex.IsMatch(x.Key)).Select(x => (x.Key, x.Value)).ToArray();
            if (matches.Length > 0)
            {
                return matches;
            }

            matches = this.entries.Where(x => brandModelFileRegex.IsMatch(x.Key)).Select(x => (x.Key, x.Value)).ToArray();
            if (matches.Length > 0)
            {
                return matches;
            }

            matches = this.entries.Where(x => brandDirectoryRegex.IsMatch(x.Key)).Select(x => (x.Key, x.Value)).ToArray();
            if (matches.Length > 0)
            {
                return matches;
            }

            matches = this.entries.Where(x => brandFileRegex.IsMatch(x.Key)).Select(x => (x.Key, x.Value)).ToArray();
            if (matches.Length > 0)
            {
                return matches;
            }

            matches = this.entries.Where(x => genericDirectoryRegex.IsMatch(x.Key)).Select(x => (x.Key, x.Value)).ToArray();
            if (matches.Length > 0)
            {
                return matches;
            }

            matches = this.entries.Where(x => genericFileRegex.IsMatch(x.Key)).Select(x => (x.Key, x.Value)).ToArray();
            if (matches.Length > 0)
            {
                return matches;
            }

            return new (String FullName, Stream Stream)[0];
        }

        public async Task<UploadedImage> ResolveImage(String fullName)
        {
            if (this.resolvedImages.TryGetValue(fullName, out var image))
            {
                return image;
            }

            if (this.entries.TryGetValue(fullName, out var stream))
            {
                var imageResult = await this.imageUploadService.CreateUploadedImageAsync("HandpieceImage", fullName, stream);
                if (imageResult.Succeeded)
                {
                    this.resolvedImages[fullName] = imageResult.Image;
                    return imageResult.Image;
                }
            }

            return null;
        }

        public void Dispose()
        {
            this.archive?.Dispose();
        }

        private Stream OpenEntry(ZipArchiveEntry entry)
        {
            var copyStream = new MemoryStream();
            using (var archiveStream = entry.Open())
            {
                archiveStream.CopyTo(copyStream);
                copyStream.Position = 0;
                return copyStream;
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
