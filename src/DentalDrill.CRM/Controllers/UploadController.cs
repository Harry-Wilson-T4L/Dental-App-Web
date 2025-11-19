using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Files;
using DevGuild.AspNetCore.Services.Uploads.Files.Controllers;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Services.Uploads.Images.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class UploadController : Controller
    {
        public UploadController(IImageUploadService imageUploadService, IFileUploadService fileUploadService, IRepository repository, IStorageHub storageHub, IPermissionsHub permissionsHub)
        {
            this.UploadImageHandler = new UploadImageActionHandler(this, imageUploadService, repository, storageHub, permissionsHub);
            this.UploadFileHandler = new UploadFileActionHandler(this, fileUploadService, repository, permissionsHub);
        }

        protected UploadImageActionHandler UploadImageHandler { get; }

        protected UploadFileActionHandler UploadFileHandler { get; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Image(String configuration, String previewVariation, IFormFile file) => this.UploadImageHandler.Image(configuration, previewVariation, file);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> File(String configuration, IFormFile file) => this.UploadFileHandler.File(configuration, file);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RemoveFile()
        {
            return Task.FromResult<IActionResult>(this.Content(String.Empty, "text/plain"));
        }
    }
}
