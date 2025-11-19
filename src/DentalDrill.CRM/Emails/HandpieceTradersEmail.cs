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
    public class HandpieceTradersEmail : BaseEmail
    {
        public Client Client { get; set; }

        public String To { get; set; }

        public String Greeting { get; set; }

        public List<(UploadedFile FileInfo, Byte[] FileBytes)> Attachments { get; set; }

        protected override Boolean IsTransactionalEmail => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Greeting, this.To));
        }

        protected override String GetSubject()
        {
            return "Handpiece Traders";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine("Dental Drill Solutions are proud to announce our affiliation with Handpiece Traders.");
            writer.WriteLine();
            writer.WriteLine("Dental Drill Solutions (DDS) have formed an alliance with Handpiece Traders enabling DDS to close the loop in the maintenance, repair, and when necessary replacement of dental handpieces.");
            writer.WriteLine("Handpiece Traders are the only reconditioned dental handpiece store in Australia & New Zealand and should be the first stop for all Dental Professionals to visit when looking to purchase more handpieces.");
            writer.WriteLine("This partnership enables DDS to not only supply handpiece maintenance and service requirements as it has done for many years, but we now can offer refurbished, pre-loved, high and low speed handpieces when the cost of repair outweighs the cost to replace or where a surgery, through natural organic growth needs more handpieces. Through Handpiece Traders you can contact and purchase using the online store.");
            writer.WriteLine("Now our clients get the comfort of being able to buy, maintain and repair handpieces – all with the backing of DDS.");
            writer.WriteLine();
            writer.WriteLine("Please visit https://handpiecetraders.com/ to see how much you can save on reconditioned handpieces.");
        }

        protected override void RenderTextFooter(TextWriter writer)
        {
            writer.WriteLine($"");
            writer.WriteLine($"Handpiece Traders");
            writer.WriteLine($"");
            writer.WriteLine($"Need More Genuine Handpieces but are on a Tight Budget?");
            writer.WriteLine($"Check out the Handpiece Traders Online Store for all Genuine Brands of Reconditioned & Ex-Stock/Discontinued Models");
            writer.WriteLine($"");
            writer.WriteLine($"All the Brands you Love and Trust!");
            writer.WriteLine($"");
            writer.WriteLine($"Click HERE to visit our Store: https://www.handpiecetraders.com/shop");
            writer.WriteLine($"");
            writer.WriteLine($"***********************************");
            writer.WriteLine($"If you can’t afford to buy brand new then Handpiece Traders is the next best thing!");
            writer.WriteLine($"What We Offer: We stock all of the trusted brands that you are currently using, so why not add a few more to your collection?");
            writer.WriteLine($"Secure Payment: PayPal is available at checkout for fast and secure online payments.");
            writer.WriteLine($"Warranty: Each handpiece comes with standard 3 months warranty, with extended warranty options available. It gives you peace of mind that you are buying a quality handpiece with guaranteed longevity.");
            writer.WriteLine($"Don’t take the gamble buying a cheap unknown handpiece, but the genuine brand names you trust.");
            writer.WriteLine($"To view over 200 handpieces and accessories in stock click here: https://www.handpiecetraders.com");
            writer.WriteLine($"");
            writer.WriteLine($"FOLLOW US");
            writer.WriteLine($"Facebook: https://www.facebook.com/handpiecetraders");
            writer.WriteLine($"Instagram: https://www.instagram.com/handpiecetraders");
            writer.WriteLine($"LinkedIn: https://www.linkedin.com/company/handpiece-traders/");
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");
            var root = wrapper.InnerHtml;

            if (resources.TryGetValue("handpiece-traders-affiliation.jpg", out var handpieceTradersAffiliation))
            {
                root.AppendHtml(EmailHtml.Center(
                    EmailHtml.Image($"cid:{handpieceTradersAffiliation.ContentId}", "Dental Drill Solutions are proud to announce our affiliation with Handpiece Traders")));
            }

            root.AppendHtml(EmailHtml.ParagraphStyled(
                "font-size: 1.5em;",
                EmailHtml.Bold("Dental Drill Solutions"),
                " are proud to announce our affiliation with ",
                EmailHtml.Bold("Handpiece Traders")));

            root.AppendHtml(EmailHtml.Paragraph(
                "Dental Drill Solutions (DDS) have formed an alliance with Handpiece Traders enabling DDS to close the loop in the maintenance, repair, and when necessary replacement of dental handpieces."));
            root.AppendHtml(EmailHtml.Paragraph(
                "Handpiece Traders are the only reconditioned dental handpiece store in Australia & New Zealand and should be the first stop for all Dental Professionals to visit when looking to purchase more handpieces."));
            root.AppendHtml(EmailHtml.Paragraph(
                "This partnership enables DDS to not only supply handpiece maintenance and service requirements as it has done for many years, but we now can offer refurbished, pre-loved, high and low speed handpieces when the cost of repair outweighs the cost to replace or where a surgery, through natural organic growth needs more handpieces. Through Handpiece Traders you can contact and purchase using the online store."));
            root.AppendHtml(EmailHtml.Paragraph(
                "Now our clients get the comfort of being able to buy, maintain and repair handpieces – all with the backing of DDS."));
            root.AppendHtml(EmailHtml.Paragraph(
                "Please visit ",
                EmailHtml.Bold(EmailHtml.Anchor("HandpieceTraders.com", "https://handpiecetraders.com")),
                " to see how much you can save on reconditioned handpieces."));

            return wrapper;
        }

        protected override void FormatFooterCell(TagBuilder row, TagBuilder cell)
        {
            base.FormatFooterCell(row, cell);
            cell.MergeAttribute("style", "background-color: black; font-family: Verdana, Arial, sans-serif; color: white; padding-left: 60px; padding-right: 60px;");
        }

        protected override IHtmlContent RenderHtmlFooter(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");
            var root = wrapper.InnerHtml;

            root.AppendHtml(EmailHtml.Div(
                text: "Need More Genuine Handpieces",
                style: "text-align: center; color: white; line-height: 1; font-size: 1.75em; margin-top: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "but are on a Tight Budget?",
                style: "text-align: center; color: white; line-height: 1; font-size: 1.75em; margin-bottom: 0.5em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "Check out the Handpiece Traders Online Store for all",
                style: "text-align: center; color: white; line-height: 1.2;"));

            root.AppendHtml(EmailHtml.Div(
                text: "Genuine Brands of Reconditioned & Ex-Stock/Discontinued Models",
                style: "text-align: center; color: white; line-height: 1.2;"));

            if (resources.TryGetValue("handpiece-traders-brands.png", out var handpieceTradersBrands))
            {
                root.AppendHtml(EmailHtml.Div(
                    content: EmailHtml.Image(src: $"cid:{handpieceTradersBrands.ContentId}", alt: "All the Brands you Love and Trust!", width: 560),
                    style: "text-align: center;"));
            }

            if (resources.TryGetValue("handpiece-traders-store-link.png", out var handpieceTradersStoreLink))
            {
                root.AppendHtml(EmailHtml.Div(
                    content: EmailHtml.Anchor(
                        content: EmailHtml.Image(src: $"cid:{handpieceTradersStoreLink.ContentId}", alt: "Click HERE to visit our Store", width: 300),
                        link: "https://www.handpiecetraders.com/shop"),
                    style: "text-align: center;"));
            }

            root.AppendHtml(EmailHtml.Div(
                text: "***********************************",
                style: "text-align: center; color: red; font-size: 1.25em; margin-top: 1.5em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "If you can’t afford to buy brand new then",
                style: "text-align: center; color: white; line-height: 1; font-size: 1.5em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "Handpiece Traders is the next best thing!",
                style: "text-align: center; color: white; line-height: 1; font-size: 1.5em; margin-bottom: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "What We Offer: We stock all of the trusted brands that you are currently using, so why not add a few more to your collection?",
                style: "color: white; line-height: 1.2; margin-bottom: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "Secure Payment: PayPal is available at checkout for fast and secure online payments.",
                style: "color: white; line-height: 1.2; margin-bottom: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "Warranty: Each handpiece comes with standard 3 months warranty, with extended warranty options available. It gives you peace of mind that you are buying a quality handpiece with guaranteed longevity.",
                style: "color: white; line-height: 1.2; margin-bottom: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "Don’t take the gamble buying a cheap unknown handpiece, but the genuine brand names you trust.",
                style: "color: white; line-height: 1.2; margin-bottom: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "To view over 200 handpieces and accessories in stock click on",
                style: "text-align: center; color: white; margin-bottom: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                content: EmailHtml.Anchor(
                    text: "HandpieceTraders.com",
                    link: "https://www.handpiecetraders.com",
                    style: "color: white; text-decoration: underline;"),
                style: "text-align: center; color: white; font-size: 1.75em;  margin-bottom: 1em;"));

            root.AppendHtml(EmailHtml.Div(
                text: "FOLLOW US",
                style: "text-align: center; color: white; margin-bottom: 1em;"));

            if (resources.TryGetValue("handpiece-traders-facebook.png", out var handpieceTradersFacebook) &&
                resources.TryGetValue("handpiece-traders-instagram.png", out var handpieceTradersInstagram) &&
                resources.TryGetValue("handpiece-traders-linkedin.png", out var handpieceTradersLinkedIn))
            {
                root.AppendHtml(EmailHtml.Div(
                    content: new Object[]
                    {
                        EmailHtml.Anchor(
                            content: EmailHtml.Image(src: $"cid:{handpieceTradersFacebook.ContentId}", alt: "Facebook", width: 55),
                            link: "https://www.facebook.com/handpiecetraders"),
                        EmailHtml.Span(new HtmlString(" &nbsp; ")),
                        EmailHtml.Anchor(
                            content: EmailHtml.Image(src: $"cid:{handpieceTradersInstagram.ContentId}", alt: "Instagram", width: 55),
                            link: "https://www.instagram.com/handpiecetraders"),
                        EmailHtml.Span(new HtmlString(" &nbsp; ")),
                        EmailHtml.Anchor(
                            content: EmailHtml.Image(src: $"cid:{handpieceTradersLinkedIn.ContentId}", alt: "LinkedIn", width: 55),
                            link: "https://www.linkedin.com/company/handpiece-traders/"),
                    },
                    style: "text-align: center; margin-bottom: 2em;"));
            }

            return wrapper;
        }

        protected override IDictionary<String, MimeEntity> AddBodyLinkedResources(AttachmentCollection resources)
        {
            var loader = new ResourceLoader(resources, base.AddBodyLinkedResources(resources));
            loader.LoadImageFromResources("handpiece-traders-affiliation.jpg");
            loader.LoadImageFromResources("handpiece-traders-brands.png");
            loader.LoadImageFromResources("handpiece-traders-store-link.png");
            loader.LoadImageFromResources("handpiece-traders-facebook.png");
            loader.LoadImageFromResources("handpiece-traders-instagram.png");
            loader.LoadImageFromResources("handpiece-traders-linkedin.png");

            return loader.GetResult();
        }

        protected override IDictionary<String, MimeEntity> AddFooterLinkedResources(AttachmentCollection resources)
        {
            return null;
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
