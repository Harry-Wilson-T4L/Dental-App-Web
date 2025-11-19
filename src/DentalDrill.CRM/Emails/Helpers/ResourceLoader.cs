using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Utils;

namespace DentalDrill.CRM.Emails.Helpers
{
    public class ResourceLoader
    {
        private readonly AttachmentCollection attachments;
        private readonly IDictionary<String, MimeEntity> entitiesMap;

        public ResourceLoader(AttachmentCollection attachments)
        {
            this.attachments = attachments;
            this.entitiesMap = new Dictionary<String, MimeEntity>();
        }

        public ResourceLoader(AttachmentCollection attachments, IDictionary<String, MimeEntity> entitiesMap)
        {
            this.attachments = attachments;
            this.entitiesMap = entitiesMap ?? new Dictionary<String, MimeEntity>();
        }

        public void LoadImageFromResources(String name)
        {
            this.LoadImage(name, this.ReadResource(name));
        }

        public void LoadImageFromResources(String name, String resourceName)
        {
            this.LoadImage(name, this.ReadResource(resourceName));
        }

        public void LoadImage(String name, Byte[] bytes)
        {
            var contentType = ContentType.Parse(MimeTypes.GetMimeType(name));
            var resource = this.attachments.Add(name, bytes, contentType);
            resource.ContentId = MimeUtils.GenerateMessageId();
            this.entitiesMap[name] = resource;
        }

        public void LoadImage(String key, String name, Byte[] bytes)
        {
            var contentType = ContentType.Parse(MimeTypes.GetMimeType(name));
            var resource = this.attachments.Add(name, bytes, contentType);
            resource.ContentId = MimeUtils.GenerateMessageId();
            this.entitiesMap[key] = resource;
        }

        public IDictionary<String, MimeEntity> GetResult()
        {
            return this.entitiesMap;
        }

        private Byte[] ReadResource(String name)
        {
            var assembly = typeof(BaseEmail).Assembly;
            using (var ms = new MemoryStream())
            {
                using (var stream = assembly.GetManifestResourceStream($"DentalDrill.CRM.Emails.Resources.{name}"))
                {
                    stream.CopyTo(ms);
                }

                return ms.ToArray();
            }
        }
    }
}
