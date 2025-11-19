using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DentalDrill.CRM.Emails;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Mail;
using DevGuild.AspNetCore.Services.Mail.Models;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Files;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DentalDrill.CRM.Services
{
    public class ClientEmailsService
    {
        private readonly IRepository repository;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IEmailService emailService;
        private readonly IImageUploadService imageUploadService;
        private readonly IFileUploadService fileUploadService;
        private readonly IStorageHub storageHub;
        private readonly IDataProtectionProvider dataProtectionProvider;
        private readonly IConfiguration configuration;

        public ClientEmailsService(
            IRepository repository,
            IRepositoryFactory repositoryFactory,
            IEmailService emailService,
            IImageUploadService imageUploadService,
            IFileUploadService fileUploadService,
            IStorageHub storageHub,
            IDataProtectionProvider dataProtectionProvider,
            IConfiguration configuration)
        {
            this.repository = repository;
            this.repositoryFactory = repositoryFactory;
            this.emailService = emailService;
            this.imageUploadService = imageUploadService;
            this.fileUploadService = fileUploadService;
            this.storageHub = storageHub;
            this.dataProtectionProvider = dataProtectionProvider;
            this.configuration = configuration;
        }

        public async Task SendClientEmail(Client client, IEmail email, ClientNotificationsType notificationType)
        {
            if (client.NotificationsOptions.HasFlag(ClientNotificationsOptions.Enabled) && !this.IsNotificationsTypeDisabled(client, notificationType))
            {
                if (email is IManageNotificationsLinkEmail manageNotifications)
                {
                    var baseUrl = this.configuration.GetValue<String>("Application:BaseUrl").TrimEnd('/');
                    manageNotifications.ManageNotificationsEnabled = true;
                    manageNotifications.ManageNotificationsLink = $"{baseUrl}/Account/Notifications?token={HttpUtility.UrlEncode(await this.GenerateManageToken(client))}";
                }

                var message = await this.emailService.SendAsync(email);
                await this.AssignClientToMessage(client, message);
            }
        }

        public Boolean IsNotificationsTypeDisabled(Client client, ClientNotificationsType notificationType)
        {
            return notificationType switch
            {
                ClientNotificationsType.ManualEmail => client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledManualEmails),
                ClientNotificationsType.HandpieceNotification => client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledHandpieceNotifications),
                ClientNotificationsType.FeedbackRequest => client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledFeedbackRequests),
                ClientNotificationsType.MaintenanceReminder => client.NotificationsOptions.HasFlag(ClientNotificationsOptions.DisabledMaintenanceReminders),
                _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null),
            };
        }

        public async Task<String> GenerateManageToken(Client client)
        {
            await using var ms = new MemoryStream();
            await using (var writer = new BinaryWriter(ms, new UTF8Encoding(false, true), true))
            {
                writer.Write(DateTimeOffset.UtcNow.UtcTicks);
                writer.Write(client.Id.ToByteArray());
                writer.Write("ManageNotifications");

                if (client.UserId.HasValue)
                {
                    var user = await this.repository.Query<ApplicationUser>().SingleAsync(x => x.Id == client.UserId);
                    writer.Write(user.Id.ToByteArray());
                    writer.Write(user.SecurityStamp);
                }
                else
                {
                    writer.Write(Guid.Empty.ToByteArray());
                    writer.Write(String.Empty);
                }
            }

            var protector = this.dataProtectionProvider.CreateProtector("EmailManageNotifications");
            var protectedBytes = protector.Protect(ms.ToArray());
            return Convert.ToBase64String(protectedBytes);
        }

        public async Task<(Boolean Success, Client Client, ApplicationUser User, String ErrorCode)> VerifyManageToken(String token)
        {
            try
            {
                var protector = this.dataProtectionProvider.CreateProtector("EmailManageNotifications");
                var unprotectedData = protector.Unprotect(Convert.FromBase64String(token));
                await using var ms = new MemoryStream(unprotectedData);
                using (var reader = new BinaryReader(ms, new UTF8Encoding(false, true), true))
                {
                    var creationTime = new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
                    var clientId = new Guid(reader.ReadBytes(16));
                    var purpose = reader.ReadString();
                    var userId = new Guid(reader.ReadBytes(16));
                    var securityStamp = reader.ReadString();

                    if (purpose != "ManageNotifications")
                    {
                        return (false, null, null, "InvalidToken");
                    }

                    var client = await this.repository.Query<Client>()
                        .Include(x => x.User)
                        .SingleOrDefaultAsync(x => x.Id == clientId);

                    if (client == null)
                    {
                        return (false, null, null, "InvalidToken");
                    }

                    if (client.User == null)
                    {
                        if (userId != Guid.Empty || securityStamp != String.Empty)
                        {
                            return (false, client, null, "InvalidSecurityState");
                        }

                        var tokenLifetime = TimeSpan.FromDays(30);
                        if (creationTime + tokenLifetime < DateTime.UtcNow)
                        {
                            return (false, client, null, "TokenExpired");
                        }

                        return (true, client, null, null);
                    }
                    else
                    {
                        if (userId != client.User.Id || securityStamp != client.User.SecurityStamp)
                        {
                            return (false, client, client.User, "InvalidSecurityState");
                        }

                        var tokenLifetime = TimeSpan.FromDays(30);
                        if (creationTime + tokenLifetime < DateTime.UtcNow)
                        {
                            return (false, client, client.User, "TokenExpired");
                        }

                        return (true, client, client.User, null);
                    }
                }
            }
            catch
            {
                // Invalid token
                return (false, null, null, "InvalidToken");
            }
        }

        public async Task<List<(UploadedFile FileInfo, Byte[] FileBytes)>> LoadAttachmentsAsync(IEnumerable<Guid> filesIds)
        {
            var result = new List<(UploadedFile FileInfo, Byte[] FileBytes)>();
            if (filesIds == null)
            {
                return result;
            }

            foreach (var fileId in filesIds)
            {
                var file = await this.fileUploadService.GetUploadedFileAsync(fileId);
                var container = this.storageHub.GetContainer(file.Container);

                Byte[] fileBytes;
                using (var fileStream = new MemoryStream())
                using (var dataStream = await container.GetFileContentAsync(file.GetContainerPath()))
                {
                    await dataStream.CopyToAsync(fileStream);
                    fileBytes = fileStream.ToArray();
                }

                result.Add((file, fileBytes));
            }

            return result;
        }

        public async Task<(UploadedImage ImageInfo, Byte[] ImageBytes)> LoadImageAsync(Guid? imageId)
        {
            if (imageId == null)
            {
                return (null, null);
            }

            var image = await this.imageUploadService.GetUploadedImageAsync(imageId.Value);

            var container = this.storageHub.GetContainer(image.Container);
            Byte[] imageBytes;
            using (var imageStream = new MemoryStream())
            using (var dataStream = await container.GetFileContentAsync(image.GetContainerPath("Original")))
            {
                await dataStream.CopyToAsync(imageStream);
                imageBytes = imageStream.ToArray();
            }

            return (image, imageBytes);
        }

        private async Task AssignClientToMessage(Client client, EmailMessage message)
        {
            using (var repository = this.repositoryFactory.CreateRepository())
            {
                var clientEmailMessage = new ClientEmailMessage
                {
                    ClientId = client.Id,
                    EmailMessageId = message.Id,
                };

                await repository.InsertAsync(clientEmailMessage);
                await repository.SaveChangesAsync();
            }
        }
    }
}
