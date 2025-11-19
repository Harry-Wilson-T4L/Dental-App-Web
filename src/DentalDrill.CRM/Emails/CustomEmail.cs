using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class CustomEmail : BaseEmployeeEmail
    {
        public Client Client { get; set; }

        public String To { get; set; }

        public String Subject { get; set; }

        public String Text { get; set; }

        public List<(UploadedFile FileInfo, Byte[] FileBytes)> Attachments { get; set; }

        protected override Boolean IsTransactionalEmail => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Client.PrincipalDentist, this.To));
        }

        protected override String GetSubject()
        {
            return $"{this.Subject} - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            var lines = this.Text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");
            var lines = this.Text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                wrapper.InnerHtml.Append(lines[i]);
                if (i < lines.Length - 1)
                {
                    wrapper.InnerHtml.AppendHtml(new TagBuilder("br") { TagRenderMode = TagRenderMode.SelfClosing });
                }
            }

            return wrapper;
        }

        protected override void AddAttachments(AttachmentCollection attachments)
        {
            base.AddAttachments(attachments);
            foreach (var file in this.Attachments)
            {
                attachments.Add($"{file.FileInfo.OriginalName}.{file.FileInfo.Extension}", file.FileBytes);
            }
        }
    }
}
