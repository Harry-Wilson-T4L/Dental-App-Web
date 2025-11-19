using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails.Models;
using DevGuild.AspNetCore.Services.Mail.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class ForgotPasswordEmail : BaseEmail
    {
        public String Recipient { get; set; }

        public String Link { get; set; }

        protected override Boolean IsTransactionalEmail => true;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress((String)null, this.Recipient));
        }

        protected override String GetSubject()
        {
            return "Reset Password";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine("Please reset your password by navigating this link: {0}", this.Link);
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");
            wrapper.InnerHtml.Append("Please reset your password by navigating this ");

            var link = new TagBuilder("a");
            link.Attributes["href"] = this.Link;
            link.InnerHtml.Append("link");

            wrapper.InnerHtml.AppendHtml(link);
            wrapper.InnerHtml.Append(".");
            return wrapper;
        }
    }
}
