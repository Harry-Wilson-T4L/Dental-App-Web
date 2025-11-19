using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class ForgotPasswordAmbiguousEmail : BaseEmail
    {
        public String Recipient { get; set; }

        public List<(String UserName, String Link)> Links { get; set; } = new List<(String UserName, String Link)>();

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
            if (this.Links.Count == 0)
            {
                throw new InvalidOperationException("No links set");
            }

            writer.WriteLine("Please reset your password by navigating one of the following links that match the account you want to reset password for:");
            foreach (var link in this.Links)
            {
                writer.WriteLine();
                writer.WriteLine($"- User name: {link.UserName}");
                writer.WriteLine($"  Reset link: {link.Link}");
            }
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            if (this.Links.Count == 0)
            {
                throw new InvalidOperationException("No links set");
            }

            var wrapper = new TagBuilder("div");
            {
                var para = new TagBuilder("p");
                para.InnerHtml.Append("Please reset your password by navigating one of the following links that match the account you want to reset password for ");
                wrapper.InnerHtml.AppendHtml(para);
            }

            {
                var list = new TagBuilder("ul");

                foreach (var link in this.Links)
                {
                    var listItem = new TagBuilder("li");

                    var listItemLink = new TagBuilder("a");
                    listItemLink.Attributes["href"] = link.Link;
                    listItemLink.InnerHtml.Append(link.UserName);

                    listItem.InnerHtml.AppendHtml(listItemLink);
                    list.InnerHtml.AppendHtml(listItem);
                }

                wrapper.InnerHtml.AppendHtml(list);
            }

            return wrapper;
        }
    }
}
