using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails.Helpers;
using DentalDrill.CRM.Emails.Models;
using DevGuild.AspNetCore.Services.Mail.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MimeKit.Utils;

namespace DentalDrill.CRM.Emails
{
    public abstract class BaseEmail : IEmail, IManageNotificationsLinkEmail
    {
        Boolean IManageNotificationsLinkEmail.ManageNotificationsEnabled { get; set; }

        String IManageNotificationsLinkEmail.ManageNotificationsLink { get; set; }

        protected abstract Boolean IsTransactionalEmail { get; }

        public MimeMessage CreateMessage()
        {
            var message = new MimeMessage();
            this.FillRecipients(message.To, message.Cc, message.Bcc);
            message.Subject = this.GetSubject();

            var bodyBuilder = new BodyBuilder();

            using (var textWriter = new StringWriter())
            {
                this.RenderTextBody(textWriter);
                textWriter.Flush();
                bodyBuilder.TextBody = textWriter.ToString();
            }

            var resources = this.AddLinkedResources(bodyBuilder.LinkedResources);

            using (var htmlWriter = new StringWriter())
            {
                var html = this.RenderHtmlBody(resources);
                html.WriteTo(htmlWriter, HtmlEncoder.Default);
                bodyBuilder.HtmlBody = htmlWriter.ToString();
            }

            this.AddAttachments(bodyBuilder.Attachments);

            message.Body = bodyBuilder.ToMessageBody();

            this.ExtraMailMessageProcessing(message);
            return message;
        }

        protected virtual Boolean IsCentered()
        {
            return true;
        }

        protected virtual Boolean IncludeFacebookLike()
        {
            return true;
        }

        protected abstract void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc);

        protected abstract String GetSubject();

        protected virtual void RenderTextBody(TextWriter writer)
        {
            this.RenderTextHeader(writer);
            this.RenderTextContent(writer);
            this.RenderTextFooter(writer);
        }

        protected virtual void RenderTextHeader(TextWriter writer)
        {
        }

        protected abstract void RenderTextContent(TextWriter writer);

        protected virtual void RenderTextFooter(TextWriter writer)
        {
            var contactDetails = this.GetContactDetails();

            writer.WriteLine();
            writer.WriteLine("Kind regards");

            if (!String.IsNullOrEmpty(contactDetails.SenderName))
            {
                writer.WriteLine();
                writer.WriteLine(contactDetails.SenderName);
            }

            writer.WriteLine("Dental Drill Solutions");
            writer.WriteLine();
            writer.WriteLine("P: {0}", contactDetails.Phone);
            writer.WriteLine("A: {0}", contactDetails.Address);
            writer.WriteLine("E: {0}", contactDetails.Email);
            writer.WriteLine("W: {0}", contactDetails.WebSite);
            writer.WriteLine("ABN: {0}", contactDetails.BusinessNumber);

            var manageNotifications = this as IManageNotificationsLinkEmail;
            if (manageNotifications.ManageNotificationsEnabled)
            {
                writer.WriteLine();
                if (this.IsTransactionalEmail)
                {
                    writer.WriteLine($"To change notification settings click this link: {manageNotifications.ManageNotificationsLink}");
                }
                else
                {
                    writer.WriteLine($"To unsubscribe or change notification settings click this link: {manageNotifications.ManageNotificationsLink}");
                }
            }
        }

        protected virtual IHtmlContent RenderHtmlBody(IDictionary<String, MimeEntity> resources)
        {
            var html = new TagBuilder("html");
            var body = new TagBuilder("body");

            var outerTable = new TagBuilder("table");
            outerTable.Attributes.Add("width", "100%");
            outerTable.Attributes.Add("align", "center");
            outerTable.Attributes.Add("border", "0");
            outerTable.Attributes.Add("cellpadding", "0");
            outerTable.Attributes.Add("cellspacing", "0");

            var outerRow = new TagBuilder("tr");
            var outerCell = new TagBuilder("td");
            outerCell.Attributes.Add("align", "center");

            var innerTable = new TagBuilder("table");
            innerTable.Attributes.Add("width", "600");
            innerTable.Attributes.Add("border", "0");
            innerTable.Attributes.Add("cellpadding", "0");
            innerTable.Attributes.Add("cellspacing", "0");

            var header = this.RenderHtmlHeader(resources);
            if (header != null)
            {
                var innerRow = new TagBuilder("tr");
                var innerCell = new TagBuilder("td");
                innerCell.InnerHtml.AppendHtml(header);
                innerRow.InnerHtml.AppendHtml(innerCell);

                this.FormatHeaderCell(innerRow, innerCell);
                innerTable.InnerHtml.AppendHtml(innerRow);
            }

            var content = this.RenderHtmlContent(resources);
            if (content != null)
            {
                var innerRow = new TagBuilder("tr");
                var innerCell = new TagBuilder("td");
                innerCell.InnerHtml.AppendHtml(content);
                innerRow.InnerHtml.AppendHtml(innerCell);

                this.FormatContentCell(innerRow, innerCell);
                innerTable.InnerHtml.AppendHtml(innerRow);
            }

            var footer = this.RenderHtmlFooter(resources);
            if (footer != null)
            {
                var innerRow = new TagBuilder("tr");
                var innerCell = new TagBuilder("td");
                innerCell.InnerHtml.AppendHtml(footer);
                innerRow.InnerHtml.AppendHtml(innerCell);

                this.FormatFooterCell(innerRow, innerCell);
                innerTable.InnerHtml.AppendHtml(innerRow);
            }

            if (this.IsCentered())
            {
                outerCell.InnerHtml.AppendHtml(innerTable);
                outerRow.InnerHtml.AppendHtml(outerCell);
                outerTable.InnerHtml.AppendHtml(outerRow);
                body.InnerHtml.AppendHtml(outerTable);
                html.InnerHtml.AppendHtml(body);
                return html;
            }
            else
            {
                body.InnerHtml.AppendHtml(innerTable);
                html.InnerHtml.AppendHtml(body);
                return html;
            }
        }

        protected virtual IHtmlContent RenderHtmlHeader(IDictionary<String, MimeEntity> resources)
        {
            return null;
        }

        protected abstract IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources);

        protected virtual IHtmlContent RenderHtmlFooter(IDictionary<String, MimeEntity> resources)
        {
            IHtmlContent CreateLineOfText(String text)
            {
                var tag = new TagBuilder("div");
                tag.InnerHtml.Append(text);
                return tag;
            }

            IHtmlContent CreateEmptyLine()
            {
                var tag = new TagBuilder("div");
                tag.InnerHtml.AppendHtml("&nbsp;");
                return tag;
            }

            IHtmlContent CreateLink(String text, String url)
            {
                var tag = new TagBuilder("a");
                tag.Attributes["href"] = url;
                tag.InnerHtml.Append(text);

                return tag;
            }

            IHtmlContent CreateLineOfTextWithLink(String prefix, String suffix, String linkText, String linkUrl)
            {
                var tag = new TagBuilder("div");
                if (!String.IsNullOrEmpty(prefix))
                {
                    tag.InnerHtml.Append(prefix);
                }

                tag.InnerHtml.AppendHtml(CreateLink(linkText, linkUrl));

                if (!String.IsNullOrEmpty(suffix))
                {
                    tag.InnerHtml.Append(suffix);
                }

                return tag;
            }

            IHtmlContent CreateImageFromResources(String resourceName, String alt)
            {
                var tag = new TagBuilder("div");

                var img = new TagBuilder("img");
                img.Attributes["src"] = $"cid:{resources[resourceName].ContentId}";
                img.Attributes["alt"] = alt;

                tag.InnerHtml.AppendHtml(img);
                return tag;
            }

            IHtmlContent CreateLinkedImageFromResources(String resourceName, String url, String alt)
            {
                var tag = new TagBuilder("div");
                var a = new TagBuilder("a");
                a.Attributes["href"] = url;

                var img = new TagBuilder("img");
                img.Attributes["src"] = $"cid:{resources[resourceName].ContentId}";
                img.Attributes["alt"] = alt;

                a.InnerHtml.AppendHtml(img);
                tag.InnerHtml.AppendHtml(a);
                return tag;
            }

            var wrapper = new TagBuilder("div");
            var contactDetails = this.GetContactDetails();

            wrapper.InnerHtml.AppendHtml(CreateEmptyLine());
            wrapper.InnerHtml.AppendHtml(CreateLineOfText("Kind regards"));

            if (!String.IsNullOrEmpty(contactDetails.SenderName))
            {
                wrapper.InnerHtml.AppendHtml(CreateEmptyLine());
                wrapper.InnerHtml.AppendHtml(CreateLineOfText(contactDetails.SenderName));
            }

            wrapper.InnerHtml.AppendHtml(CreateLineOfText("Dental Drill Solutions"));
            wrapper.InnerHtml.AppendHtml(CreateEmptyLine());
            wrapper.InnerHtml.AppendHtml(CreateLinkedImageFromResources("dds-logo.jpg", contactDetails.WebSiteUrl, "Dental Drill Solutions"));
            wrapper.InnerHtml.AppendHtml(CreateLineOfText($"P: {contactDetails.Phone}"));
            wrapper.InnerHtml.AppendHtml(CreateLineOfText($"A: {contactDetails.Address}"));
            wrapper.InnerHtml.AppendHtml(CreateLineOfTextWithLink("E: ", null, contactDetails.Email, $"mailto:{contactDetails.Email}"));
            wrapper.InnerHtml.AppendHtml(CreateLineOfTextWithLink("W: ", null, contactDetails.WebSite, contactDetails.WebSiteUrl));
            wrapper.InnerHtml.AppendHtml(CreateLineOfText($"ABN: {contactDetails.BusinessNumber}"));
            if (this.IncludeFacebookLike())
            {
                wrapper.InnerHtml.AppendHtml(CreateEmptyLine());
                wrapper.InnerHtml.AppendHtml(CreateLinkedImageFromResources("fb-like.jpg", "https://www.facebook.com/dentaldrillsolutions", "Like us on Facebook"));
            }

            var manageNotifications = this as IManageNotificationsLinkEmail;
            if (manageNotifications.ManageNotificationsEnabled)
            {
                wrapper.InnerHtml.AppendHtml(CreateEmptyLine());
                if (this.IsTransactionalEmail)
                {
                    wrapper.InnerHtml.AppendHtml(CreateLineOfTextWithLink("", "", "Change notifications settings", manageNotifications.ManageNotificationsLink));
                }
                else
                {
                    wrapper.InnerHtml.AppendHtml(CreateLineOfTextWithLink("", " to unsubscribe or change notifications settings", "Click here", manageNotifications.ManageNotificationsLink));
                }
            }

            return wrapper;
        }

        protected virtual void AddAttachments(AttachmentCollection attachments)
        {
        }

        protected virtual IDictionary<String, MimeEntity> AddLinkedResources(AttachmentCollection resources)
        {
            void MergeDictionaries(IDictionary<String, MimeEntity> destination, IDictionary<String, MimeEntity> source)
            {
                if (source == null)
                {
                    return;
                }

                foreach (var kv in source)
                {
                    destination[kv.Key] = kv.Value;
                }
            }

            var linkedResources = new Dictionary<String, MimeEntity>();
            MergeDictionaries(linkedResources, this.AddHeaderLinkedResources(resources));
            MergeDictionaries(linkedResources, this.AddBodyLinkedResources(resources));
            MergeDictionaries(linkedResources, this.AddFooterLinkedResources(resources));
            return linkedResources;
        }

        protected virtual IDictionary<String, MimeEntity> AddHeaderLinkedResources(AttachmentCollection resources)
        {
            return null;
        }

        protected virtual IDictionary<String, MimeEntity> AddBodyLinkedResources(AttachmentCollection resources)
        {
            return null;
        }

        protected virtual IDictionary<String, MimeEntity> AddFooterLinkedResources(AttachmentCollection resources)
        {
            var loader = new ResourceLoader(resources);
            loader.LoadImageFromResources("dds-logo.jpg");
            if (this.IncludeFacebookLike())
            {
                loader.LoadImageFromResources("fb-like.jpg", "dds-fb-like.jpg");
            }

            return loader.GetResult();
        }

        protected void LoadImage(IDictionary<String, MimeEntity> resourcesMap, AttachmentCollection resourcesCollection, String name, Byte[] bytes)
        {
            var contentType = ContentType.Parse(MimeTypes.GetMimeType(name));
            var resource = resourcesCollection.Add(name, bytes, contentType);
            resource.ContentId = MimeUtils.GenerateMessageId();
            resourcesMap[name] = resource;
        }

        protected Byte[] ReadResource(String name)
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

        protected virtual EmailContactDetails GetContactDetails()
        {
            var details = new EmailContactDetails
            {
                Phone = "1300 337 300",
                Address = "2/11 Turbo Road, Kings Park NSW 2148 Australia",
                Email = "info@dds11.au",
                WebSite = "www.dds11.au",
                WebSiteUrl = "https://www.dds11.au/",
                HubUrl = "https://hub.dds11.au/",
                BusinessNumber = "25 150 633 515",
            };

            this.OverrideContactDetails(details);
            return details;
        }

        protected virtual void OverrideContactDetails(EmailContactDetails details)
        {
        }

        protected virtual void ExtraMailMessageProcessing(MimeMessage message)
        {
        }

        protected virtual void FormatHeaderCell(TagBuilder row, TagBuilder cell)
        {
        }

        protected virtual void FormatContentCell(TagBuilder row, TagBuilder cell)
        {
        }

        protected virtual void FormatFooterCell(TagBuilder row, TagBuilder cell)
        {
        }
    }
}
