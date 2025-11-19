using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Mail.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class EmailConfirmationEmail : BaseEmail
    {
        public String Recipient { get; set; }

        public String Link { get; set; }

        protected override Boolean IsTransactionalEmail => true;

        public MimeMessage CreateMessage1()
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress((String)null, this.Recipient));
            message.Subject = "Confirm your email";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Please confirm your account by navigating this link: {this.Link}";
            bodyBuilder.HtmlBody = $"Please confirm your account by clicking here: <a href=\"{HtmlEncoder.Default.Encode(this.Link)}\">link</a>.";

            message.Body = bodyBuilder.ToMessageBody();
            return message;
        }

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress((String)null, this.Recipient));
        }

        protected override String GetSubject()
        {
            return "Confirm your email";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine("Please confirm your account by navigating this link: {0}", this.Link);
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");
            wrapper.InnerHtml.Append("Please confirm your account by navigating this ");

            var link = new TagBuilder("a");
            link.Attributes["href"] = this.Link;
            link.InnerHtml.Append("link");

            wrapper.InnerHtml.AppendHtml(link);
            wrapper.InnerHtml.Append(".");
            return wrapper;
        }
    }
}
