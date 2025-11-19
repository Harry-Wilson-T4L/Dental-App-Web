using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails.Helpers;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class MaintenanceInstructionsEmail : BaseEmployeeEmail
    {
        public Client Client { get; set; }

        public String To { get; set; }

        public String Greeting { get; set; }

        public List<(UploadedFile FileInfo, Byte[] FileBytes)> Attachments { get; set; }

        public (UploadedImage Image, Byte[] ImageBytes) PriceGuideImage { get; set; }

        protected override Boolean IsTransactionalEmail => false;

        protected override Boolean IncludeFacebookLike() => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Greeting, this.To));
        }

        protected override String GetSubject()
        {
            return "DDS Maintenance Instructions - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"Hi {this.Greeting},");
            writer.WriteLine("As mentioned on the phone, please find attached our info sheets on the correct lubrication procedures to ensure handpiece "
                             + "health and maintenance, especially for after repairs as the new parts need to be well lubricated to settle in.");
            writer.WriteLine();
            writer.WriteLine("Do not always rely on the correct lubrication being supplied through a machine if you have one, as sometimes the levels "
                             + "are too low or it runs out without telling you. So it is important to make sure your machine is kept serviced also. ");
            writer.WriteLine("We always recommend having a spray lubrication can at hand with the correct nozzle to use for the brand of high speed handpiece in question.");
            writer.WriteLine("The NSK Pana Plus spray cans are $59 each and the different brands of nozzle are $29 each.");
            writer.WriteLine();
            writer.WriteLine("The On-The-Go Sheet is the most beneficial for you to try if a handpiece suddenly stops spinning during use.");
            writer.WriteLine("Try lubricating it to get it going again as per the directions to relieve the problem.");
            writer.WriteLine();
            writer.WriteLine("Following these directions should result in your handpieces lasting longer between repairs and running more efficiently.");
            writer.WriteLine();
            writer.WriteLine("It is a good idea to print them off and pin them to a wall for your staff’s quick reference.");
            writer.WriteLine();
            writer.WriteLine("If you would like to refer to the short step-by-step video clips please click on the links here:");
            writer.WriteLine("- https://video.wixstatic.com/video/13a7ed_ff955aadad6e45a19e18ca72e3303aec/480p/mp4/file.mp4");
            writer.WriteLine("- https://video.wixstatic.com/video/13a7ed_a1e3d1bc9c164058b4b53ede6857f845/360p/mp4/file.mp4");
            writer.WriteLine();
            writer.WriteLine("Don’t forget that if you have any other questions or are sourcing new handpieces then check our sales pages on our website "
                             + "or run it past me first with specific makes and models in case I can save you some $$$’s!");

            writer.WriteLine();
            writer.WriteLine("Check out our DDS Facebook & Instagram pages & LIKE US for regular maintenance tips, specials and updates!");
            writer.WriteLine("https://facebook.com/dentaldrillsolutions");
            writer.WriteLine("https://instagram.com/dentaldrillsolutions");
            writer.WriteLine();
            writer.WriteLine("Or visit our website:");
            writer.WriteLine("https://www.dds11.au");
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph($"Hi {this.Greeting},"));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "As mentioned on the phone, please  find attached our info sheets on the correct lubrication procedures to ensure handpiece "
                + "health and maintenance, especially for after repairs as the new parts need to be well lubricated to settle in."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Do not always rely on the correct lubrication being supplied through a machine if you have one, as sometimes the levels "
                + "are too low or it runs out without telling you. So it is important to make sure your machine is kept serviced also.",
                EmailHtml.LineBreak(),
                "We always recommend having a spray lubrication can at hand with the correct nozzle to use for the brand of high speed handpiece in question.",
                EmailHtml.LineBreak(),
                "The NSK Pana Plus spray cans are $59 each and the different brands of nozzle are $29 each."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "The On-The-Go Sheet is the most beneficial for you to try if a handpiece suddenly stops spinning during use.",
                EmailHtml.LineBreak(),
                "Try lubricating it to get it going again as per the directions to relieve the problem."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Following these directions should result in your handpieces lasting longer between repairs and running more efficiently."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "It is a good idea to print them off and pin them to a wall for your staff’s quick reference."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "If you would like to refer to the short step-by-step video clips please click on the thumbnail links here"));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                EmailHtml.Anchor(
                    EmailHtml.Image($"cid:{resources["maintenance-instructions-1.jpg"].ContentId}", "10 Steps to handpiece Maintenance"),
                    "https://video.wixstatic.com/video/13a7ed_ff955aadad6e45a19e18ca72e3303aec/480p/mp4/file.mp4"),
                EmailHtml.LineBreak(),
                EmailHtml.Anchor("10 Steps to handpiece Maintenance", "https://video.wixstatic.com/video/13a7ed_ff955aadad6e45a19e18ca72e3303aec/480p/mp4/file.mp4")));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                EmailHtml.Anchor(
                    EmailHtml.Image($"cid:{resources["maintenance-instructions-2.jpg"].ContentId}", "Nozzle Selection & Lubrication Guide"),
                    "https://video.wixstatic.com/video/13a7ed_a1e3d1bc9c164058b4b53ede6857f845/360p/mp4/file.mp4"),
                EmailHtml.LineBreak(),
                EmailHtml.Anchor("Nozzle Selection & Lubrication Guide", "https://video.wixstatic.com/video/13a7ed_a1e3d1bc9c164058b4b53ede6857f845/360p/mp4/file.mp4")));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Don’t forget that if you have any other questions or are sourcing new handpieces then check our sales pages on our website "
                + "or run it past me first with specific makes and models in case I can save you some $$$’s!"));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("Please also find below our price list"));

            if (resources.TryGetValue("price-guide", out var priceGuide))
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Image($"cid:{priceGuide.ContentId}", "Price Guide")));
            }

            wrapper.InnerHtml.AppendHtml(EmailHtml.SimpleAlignedTable(
                tableAlign: "center",
                style: "margin-top: 1em;",
                EmailHtml.Anchor(
                    EmailHtml.Image($"cid:{resources["dds-fb-like.jpg"].ContentId}", "Facebook", width: 239, height: 71),
                    "https://facebook.com/dentaldrillsolutions"),
                EmailHtml.Anchor(
                    EmailHtml.Image($"cid:{resources["dds-instagram.png"].ContentId}", "Instagram", width: 71, height: 71),
                    "https://instagram.com/dentaldrillsolutions")));

            wrapper.InnerHtml.AppendHtml(EmailHtml.CenterWithStyle(
                style: "margin-top: 1em;",
                EmailHtml.Bold("Check out our DDS Facebook & Instagram pages & LIKE US"),
                EmailHtml.LineBreak(),
                EmailHtml.Bold("for regular maintenance tips, specials and updates!")));

            wrapper.InnerHtml.AppendHtml(EmailHtml.CenterWithStyle(
                style: "font-size: 0.8em; margin-top: 1em;",
                EmailHtml.Bold("Click on the images above or go to:"),
                EmailHtml.LineBreak(),
                EmailHtml.Bold(EmailHtml.Anchor("www.facebook.com/dentaldrillsolutions", "https://facebook.com/dentaldrillsolutions")),
                EmailHtml.LineBreak(),
                EmailHtml.Bold(EmailHtml.Anchor("www.instagram.com/dentaldrillsolutions", "https://instagram.com/dentaldrillsolutions"))));

            wrapper.InnerHtml.AppendHtml(EmailHtml.CenterWithStyle(
                style: "font-size: 0.8em; margin-top: 1em;",
                EmailHtml.Bold("Or visit our website:"),
                EmailHtml.LineBreak(),
                EmailHtml.Bold(EmailHtml.Anchor("www.dds11.au", "https://www.dds11.au/"))));

            return wrapper;
        }

        protected override IDictionary<String, MimeEntity> AddBodyLinkedResources(AttachmentCollection resources)
        {
            var loader = new ResourceLoader(resources, base.AddBodyLinkedResources(resources));
            loader.LoadImageFromResources("maintenance-instructions-1.jpg");
            loader.LoadImageFromResources("maintenance-instructions-2.jpg");
            loader.LoadImageFromResources("dds-fb-like.jpg");
            loader.LoadImageFromResources("dds-instagram.png");
            loader.LoadUploadedImage("price-guide", this.PriceGuideImage.Image, this.PriceGuideImage.ImageBytes);
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
