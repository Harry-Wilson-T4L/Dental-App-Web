using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DentalDrill.CRM.Emails.Helpers;
using DentalDrill.CRM.Emails.Models;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class NewQueenslandClientEmail : BaseEmployeeEmail
    {
        public Client Client { get; set; }

        public String To { get; set; }

        public String Greeting { get; set; }

        public String DiscountComment { get; set; }

        public (UploadedImage Image, Byte[] ImageBytes) LocationImage { get; set; }

        public String LocationText { get; set; }

        public String NewClientOfferText { get; set; }

        public (UploadedImage Image, Byte[] ImageBytes) MonthlySpecialImage { get; set; }

        public (UploadedImage Image, Byte[] ImageBytes) PriceGuideImage { get; set; }

        public List<(UploadedFile FileInfo, Byte[] FileBytes)> Attachments { get; set; }

        protected override Boolean IsTransactionalEmail => false;

        protected override Boolean IncludeFacebookLike() => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Greeting, this.To));
        }

        protected override String GetSubject()
        {
            return "Queensland Clients General Email - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"Hi {this.Greeting},");
            writer.WriteLine($"Following my call today, please find some information below about DDS. We are very local to you and would love to help with all your handpiece repairs and servicing and save you lots of money at the same time!");
            writer.WriteLine();
            writer.WriteLine("*Get your 10% discount today! *");
            if (!String.IsNullOrEmpty(this.DiscountComment))
            {
                writer.WriteLine();
                writer.WriteLine(this.DiscountComment);
            }

            writer.WriteLine();
            writer.WriteLine(this.LocationText);
            writer.WriteLine("Our business is to know you and what your practice requires. We focus on providing you with the personal service you deserve.");
            writer.WriteLine("WE WOULD LIKE TO BE YOUR LOCAL HANDPIECE REPAIR CENTRE!");
            writer.WriteLine("At DDS our aim is to offer a convenient and easy to use service, using only premium parts and bearings to OEM standards for most known and used makes and models of handpieces in Australia. ");
            writer.WriteLine("Our handpieces are repaired in days – not weeks, with FREE comprehensive diagnostic estimates prior to repair.");
            writer.WriteLine("Please find below our current Monthly Special along with our price list. Sometimes our specials are brand specific.");
            writer.WriteLine(this.NewClientOfferText);
            writer.WriteLine("We will apply the best offer depending upon what mix of handpieces you give us, either high/low speed, different brands or what nature of repairs are needed etc. We also repair couplers, scalers and motors too.");
            writer.WriteLine();
            writer.WriteLine("Handpiece Hub is the home of your handpiece repair history – any time, any place, anywhere.");
            writer.WriteLine("Our New Handpiece Hub will help you keep record of all the comings and goings of all your handpiece repairs and servicing to ensure you are on top of maintaining a smooth running practice.");
            writer.WriteLine("By entering in your personal login and password, not only can you open the history file of all your previously repaired handpieces with DDS, but you can check-in 24-7 from any computer in any location, and monitor the status of your current repairs in the workshop.");
            writer.WriteLine("The Hub is easy to use and allows you to access all the assessment notes made by our technicians in the workshop, backed up by images of each handpiece, their serial numbers, makes and models. You can also run comparison reports and identify the problem handpieces within your inventories and establish trends and problem areas within your budget analysis.");
            writer.WriteLine();
            writer.WriteLine("Please visit our website https://www.dds11.au/ or ‘Like Us’ on Facebook https://www.facebook.com/dentaldrillsolutions for more info on handpiece sales and our repair service.");

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
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                $"Hi {this.Greeting},",
                EmailHtml.LineBreak(),
                "Following my call today, please find some information below about DDS. We are very local to you and would love to help with all your handpiece repairs and servicing and save you lots of money at the same time!"));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Bold(EmailHtml.Italic("*Get your 10% discount today! *")), "color: #0099ff;"));

            if (!String.IsNullOrEmpty(this.DiscountComment))
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(this.DiscountComment));
            }

            if (resources.TryGetValue("location", out var locationImage))
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Image($"cid:{locationImage.ContentId}", "QLD Workshop")));
            }

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(this.LocationText));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("Our business is to know you and what your practice requires. We focus on providing you with the personal service you deserve."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("WE WOULD LIKE TO BE YOUR LOCAL HANDPIECE REPAIR CENTRE!"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("At DDS our aim is to offer a convenient and easy to use service, using only premium parts and bearings to OEM standards for most known and used makes and models of handpieces in Australia."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("Our handpieces are repaired in days – not weeks, with FREE comprehensive diagnostic estimates prior to repair."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("Please find below our current Monthly Special along with our price list. Sometimes our specials are brand specific."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(this.NewClientOfferText));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("We will apply the best offer depending upon what mix of handpieces you give us, either high/low speed, different brands or what nature of repairs are needed etc. We also repair couplers, scalers and motors too."));

            if (resources.TryGetValue("monthly-special", out var monthlySpecial))
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Image($"cid:{monthlySpecial.ContentId}", "Monthly Special")));
            }

            if (resources.TryGetValue("price-guide", out var priceGuide))
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Image($"cid:{priceGuide.ContentId}", "Price Guide")));
            }

            wrapper.InnerHtml.AppendHtml(EmailHtml.CenteredBox(style: String.Empty, borderSize: 1, contentAlign: "left",
                EmailHtml.Div(
                    EmailHtml.Center(
                        EmailHtml.Anchor(
                            content: EmailHtml.Image($"cid:{resources["dds-handpiece-hub.jpg"].ContentId}", "Handpiece Hub", width: 300, height: 300),
                            link: "https://hub.dds11.au/"),
                        style: "float: left !important;"),
                    EmailHtml.Div(
                        EmailHtml.Center(
                            EmailHtml.Bold("Handpiece Hub is the home"),
                            EmailHtml.LineBreak(),
                            EmailHtml.Bold("of your handpiece repair history"),
                            EmailHtml.LineBreak(),
                            EmailHtml.Bold("– any time, any place, anywhere.")),
                        EmailHtml.Paragraph(
                            "Our New ",
                            EmailHtml.Bold("Handpiece Hub"),
                            " will help you keep record of all the comings and goings of all your handpiece repairs and servicing to ensure you are on top of maintaining a smooth running practice."),
                        EmailHtml.Paragraph("By entering in your personal login and password, not only can you open the history file of all your previously repaired handpieces with DDS, but you can check-in 24-7 from any computer in any location, and monitor the status of your current repairs in the workshop."),
                        EmailHtml.Paragraph("The Hub is easy to use and allows you to access all the assessment notes made by our technicians in the workshop, backed up by images of each handpiece, their serial numbers, makes and models. You can also run comparison reports and identify the problem handpieces within your inventories and establish trends and problem areas within your budget analysis.")))));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["website.jpg"].ContentId}", "https://www.dds11.au/"), "https://www.dds11.au/")));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Please click on the image link to visit our website ",
                EmailHtml.Anchor("www.dds11.au", "https://www.dds11.au/"),
                " or ‘Like Us’ on Facebook ",
                EmailHtml.Anchor("www.facebook.com/dentaldrillsolutions", "https://www.facebook.com/dentaldrillsolutions"),
                " for more info on handpiece sales and our repair service."));

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
            loader.LoadImageFromResources("website.jpg");
            loader.LoadImageFromResources("dds-handpiece-hub.jpg");
            loader.LoadImageFromResources("dds-fb-like.jpg");
            loader.LoadImageFromResources("dds-instagram.png");

            loader.LoadUploadedImage("location", this.LocationImage.Image, this.LocationImage.ImageBytes);
            loader.LoadUploadedImage("monthly-special", this.MonthlySpecialImage.Image, this.MonthlySpecialImage.ImageBytes);
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
