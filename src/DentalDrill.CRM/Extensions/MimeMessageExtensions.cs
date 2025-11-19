using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Mail.Models;
using DevGuild.AspNetCore.Services.Storage;
using MimeKit;

namespace DentalDrill.CRM.Extensions
{
    public static class MimeMessageExtensions
    {
        public static async Task<MimeMessage> LoadMessageFromStorageAsync(this EmailMessage message, IStorageHub hub)
        {
            try
            {
                var container = hub.GetContainer("Emails");
                if (container == null)
                {
                    return null;
                }

                await using var fileStream = await container.GetFileContentAsync($"{message.MessageType}/Message-{message.Id:00000000}.eml");
                if (fileStream == null)
                {
                    return null;
                }

                return await MimeMessage.LoadAsync(fileStream);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<MimeMessage> LoadMessageAsync(this EmailMessage message)
        {
            if (String.IsNullOrEmpty(message.Content))
            {
                return null;
            }

            var bytes = Encoding.UTF8.GetBytes(message.Content);
            using (var stream = new MemoryStream(bytes))
            {
                return await MimeMessage.LoadAsync(stream);
            }
        }

        public static String PrepareHtml(this MimeMessage message)
        {
            if (message.HtmlBody == null)
            {
                return null;
            }

            var entities = message.BodyParts.ToList();

            return Regex.Replace(message.HtmlBody, @"src=""cid:(?<cid>[^\""]+)""", match =>
            {
                var cid = match.Groups["cid"].Value;
                var matchingEntity = entities.FirstOrDefault(x => x.ContentId == cid) as MimePart;
                if (matchingEntity != null)
                {
                    using (var contentStream = new MemoryStream())
                    {
                        matchingEntity.Content.DecodeTo(contentStream);
                        var base64 = Convert.ToBase64String(contentStream.ToArray(), Base64FormattingOptions.None);
                        return $@"src=""data:{matchingEntity.ContentType.MediaType}/{matchingEntity.ContentType.MediaSubtype};base64,{base64}""";
                    }
                }

                return match.Value;
            });
        }
    }
}
