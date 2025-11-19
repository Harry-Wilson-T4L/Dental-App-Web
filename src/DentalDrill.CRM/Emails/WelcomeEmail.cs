using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails.Helpers;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class WelcomeEmail : BaseEmployeeEmail
    {
        public String RecipientName { get; set; }

        public String RecipientEmail { get; set; }

        public String UserName { get; set; }

        public String Password { get; set; }

        public List<(UploadedFile FileInfo, Byte[] FileBytes)> Attachments { get; set; }

        protected override Boolean IsTransactionalEmail => true;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            if (!String.IsNullOrEmpty(this.RecipientName))
            {
                to.Add(new MailboxAddress(Encoding.UTF8, this.RecipientName, this.RecipientEmail));
            }
            else
            {
                to.Add(new MailboxAddress((String)null, this.RecipientEmail));
            }
        }

        protected override String GetSubject()
        {
            return "Welcome to Handpiece Hub - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine("Welcome to Handpiece Hub!");
            writer.WriteLine();
            writer.WriteLine("Handpiece Hub is the home of your handpiece repair history – any time, any place, anywhere.");
            writer.WriteLine("By entering in your personal login and password, not only can you open the history file of all "
                             + "your previously repaired handpieces with DDS, but you can check-in 24/7 from any computer in any location, "
                             + "and monitor the status of your current repairs in the workshop.");
            writer.WriteLine("Our easy to use Hub allows you to access all of the assessment notes made by our technicians in the workshop, "
                             + "backed up by images of each handpiece, their serial numbers, makes and models. You can also run reports and "
                             + "identify the problem handpieces within your inventory.");
            writer.WriteLine("Please click on this link: https://hub.dds11.au (and save it to your browser favourites)");
            writer.WriteLine();
            writer.WriteLine("Login: {0}", this.UserName);
            writer.WriteLine("Temporary password: {0}", this.Password);
            writer.WriteLine("(upon first opening the Hub you will be asked to change this password to something you and your staff members can all remember)");
            writer.WriteLine();
            writer.WriteLine("Book your FREE Pick-Up by calling 1300 337 300");
            writer.WriteLine();
            writer.WriteLine("Don't forget to open up our Facebook page, \"Like\" us to get our regular posts on info, tips and specials "
                             + "to appear on your news feed: https://www.facebook.com/dentaldrillsolutions");
            writer.WriteLine();
            writer.WriteLine("For details about our service, handpiece sales and other information please visit our website https://www.dds11.au/");
        }

        protected override void RenderTextFooter(TextWriter writer)
        {
            var contactDetails = this.GetContactDetails();

            writer.WriteLine();
            writer.WriteLine("Dental Drill Solutions");
            writer.WriteLine();
            writer.WriteLine("P: {0}", contactDetails.Phone);
            writer.WriteLine("A: {0}", contactDetails.Address);
            writer.WriteLine("E: {0}", contactDetails.Email);
            writer.WriteLine("W: {0}", contactDetails.WebSite);
            writer.WriteLine("ABN: {0}", contactDetails.BusinessNumber);
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var contactDetails = this.GetContactDetails();
            var wrapper = new TagBuilder("div");

            wrapper.InnerHtml.AppendHtml(EmailHtml.Centered(
                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["dds-logo.jpg"].ContentId}", "Dental Drill Solutions", width: 300), contactDetails.WebSiteUrl),
                EmailHtml.Heading2("Welcome to Handpiece Hub!"),
                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["dds-handpiece-hub.jpg"].ContentId}", "Handpiece Hub", width: 300), contactDetails.HubUrl)));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("Handpiece Hub is the home of your handpiece repair history – any time, any place, anywhere."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("By entering in your personal login and password, not only can you open the history file of all "
                                                             + "your previously repaired handpieces with DDS, but you can check-in 24/7 from any computer in any location, "
                                                             + "and monitor the status of your current repairs in the workshop."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("Our easy to use Hub allows you to access all of the assessment notes made by our technicians in the workshop, "
                                                             + "backed up by images of each handpiece, their serial numbers, makes and models. You can also run reports and "
                                                             + "identify the problem handpieces within your inventory."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Please click on this link: ",
                EmailHtml.Bold(EmailHtml.Anchor(contactDetails.HubUrl, contactDetails.HubUrl, "color: #000000")),
                " (and save it to your browser favourites)"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.EmptyParagraph());

            wrapper.InnerHtml.AppendHtml(EmailHtml.Centered(
                EmailHtml.Div("Login: ", EmailHtml.Bold(this.UserName, "color: #00b0f0;")),
                EmailHtml.Div("Temporary Password: ", EmailHtml.Bold(this.Password, "color: #00b0f0;")),
                EmailHtml.Div("(upon first opening the Hub you will be asked to change this password", EmailHtml.LineBreak(), " to something you and your staff members can all remember)")));

            wrapper.InnerHtml.AppendHtml(EmailHtml.EmptyParagraph());

            wrapper.InnerHtml.AppendHtml(EmailHtml.CenteredBox("border-color: #e61e1e; border-style: solid; border-width: 3px;", 3, EmailHtml.Div(
                    EmailHtml.Div("Book your FREE Pick-Up by calling", "font-size: 1.5em; font-weight: bold;"),
                    EmailHtml.Div("1300 337 300", "font-size: 1.75em; font-weight: bold; color: #1584d9"))));

            wrapper.InnerHtml.AppendHtml(EmailHtml.EmptyParagraph());

            wrapper.InnerHtml.AppendHtml(EmailHtml.Centered(
                EmailHtml.Div(
                    EmailHtml.Div(
                        "Don't forget to open up our Facebook page, ",
                        EmailHtml.LineBreak(),
                        "\"Like\" us to get our regular posts on info, tips ",
                        EmailHtml.LineBreak(),
                        "and specials to appear on your news feed"),
                    "font-size: 1.5em; font-weight: bold;"),
                EmailHtml.Div("(click on the link below)", "font-weight: bold;"),
                EmailHtml.EmptyParagraph(),
                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["fb-like.jpg"].ContentId}", "Like us on Facebook", width: 300), "https://www.facebook.com/dentaldrillsolutions"),
                EmailHtml.EmptyParagraph(),
                EmailHtml.Paragraph(
                    "For details about our service, handpiece sales and other information please visit our website ",
                    EmailHtml.Bold(EmailHtml.Anchor(contactDetails.WebSite, contactDetails.WebSiteUrl, "color: #000000"))),
                EmailHtml.EmptyParagraph()));

            return wrapper;
        }

        protected override IHtmlContent RenderHtmlFooter(IDictionary<String, MimeEntity> resources)
        {
            var contactDetails = this.GetContactDetails();
            var wrapper = new TagBuilder("div");

            wrapper.InnerHtml.AppendHtml(EmailHtml.Centered(
                EmailHtml.Div(
                    EmailHtml.Div(
                        EmailHtml.Div("Dental Drill Solutions"),
                        EmailHtml.Div($"P: {contactDetails.Phone}"),
                        EmailHtml.Div($"A: {contactDetails.Address}"),
                        EmailHtml.Div($"E: ", EmailHtml.Anchor(contactDetails.Email, $"mailto:{contactDetails.Email}", "color: #000000")),
                        EmailHtml.Div($"W: ", EmailHtml.Anchor(contactDetails.WebSite, contactDetails.WebSiteUrl, "color: #000000")),
                        EmailHtml.Div($"ABN: {contactDetails.BusinessNumber}")),
                    "font-weight: bold")));

            return wrapper;
        }

        protected override IDictionary<String, MimeEntity> AddBodyLinkedResources(AttachmentCollection resources)
        {
            var loader = new ResourceLoader(resources);
            loader.LoadImageFromResources("dds-handpiece-hub.jpg");
            return loader.GetResult();
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
