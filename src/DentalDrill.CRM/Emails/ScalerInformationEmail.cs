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
    public class ScalerInformationEmail : BaseEmployeeEmail
    {
        public Client Client { get; set; }

        public String To { get; set; }

        public String Greeting { get; set; }

        public (UploadedImage Image, Byte[] ImageBytes) ScalersPricesImage { get; set; }

        public List<(UploadedFile FileInfo, Byte[] FileBytes)> Attachments { get; set; }

        protected override Boolean IsTransactionalEmail => false;

        protected override Boolean IncludeFacebookLike() => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Greeting, this.To));
        }

        protected override String GetSubject()
        {
            return "DDS Scaler Information & Prices - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"Hi {this.Greeting},");
            writer.WriteLine();
            writer.WriteLine("As discussed on the phone, please find the info below on scalers that may help.");
            writer.WriteLine("We sell replacement MK-Dent scalers which are far more economical and ‘disposably priced’ in comparison "
                             + "to buying EMS/Aceton/Satelec/NSK/Kavo branded scalers at $1000+. It may work out cheaper changing "
                             + "over any current scaler chair line fittings if they are not EMS or Aceton /Satelec, so that you can use "
                             + "these cheaper priced scalers in the future and save more money in the long term.");
            writer.WriteLine("But as I mentioned it all depends upon how the tip goes on correctly as to how long they are likely to last too!");
            writer.WriteLine();
            writer.WriteLine("For most scalers on which the thread has gone or has lost vibration etc. it’s not possible to repair as the main "
                             + "body inside is glued together during the manufacturing process so it’s impossible to get inside to replace damaged parts, "
                             + "hence being unrepairable. Some Kavo scalers are repairable but you are looking at in excess of $650+ to repair, "
                             + "so you would need to consider its age also against repairing or replacing it.");
            writer.WriteLine();
            writer.WriteLine("Our MK-dent scalers are priced as below:");
            writer.WriteLine("Do call me if you have any further questions about the scalers available or want to place an order.");
            writer.WriteLine("Payment is required upfront to confirm the order and it usually takes 2 days delivery direct from manufacturer.");
            writer.WriteLine();
            writer.WriteLine("Scaler Troubleshooting:");
            writer.WriteLine();
            writer.WriteLine("With scalers and any other instruments with a thread on it, always do a half turn in the opposite direction to ensure "
                             + "you line up the thread, you may feel a slight click and that usually means it has fallen into its correct groove and "
                             + "should then be easy to screw on. If it’s ever hard to screw on then it’s definitely not sat on square and you are "
                             + "doing more harm by forcing it to realign itself.");
            writer.WriteLine();
            writer.WriteLine("Always ensure that you are using the correct tip for the correct brand too!!");
            writer.WriteLine("The thread pitch is finer on the EMS (4 flat planes) and coarser on the Satelec Scalers (2 flat planes) see photos.");
            writer.WriteLine("If you purchase the wrong tips by mistake, they will only go on about half way. If you use the tip wrench you can force "
                             + "it on, but stripping the threads in the process.");
            writer.WriteLine("If the threads are stripped then the scaler is ready for the bin (as in this case!).");
            writer.WriteLine("If a tip has got stuck or caused damage to the scaler, then the tip should be discarded too.");
            writer.WriteLine();
            writer.WriteLine("In addition to the notes above this explains threads:");
            writer.WriteLine();
            writer.WriteLine("Scaler usually has male thread");
            writer.WriteLine("Scaler tips usually have female thread");
            writer.WriteLine("Both need to line up to make sure that the tip screws on properly.");
            writer.WriteLine("If either of the threads are damaged then you can risk damaging the other in the process of trying to force it on, "
                             + "wearing away the thread that keeps the tip held onto the scaler.");
            writer.WriteLine("(In a similar way that if you try and screw a water bottle to on that’s not lined up properly it will go on at an angle "
                             + "and can eventually break the plastic thread of the bottle, and the lid won’t fit on and water will spill out etc.)");
            writer.WriteLine("The photo below shows the thread that has worn away, you can see a distinct line between the top area where the tip "
                             + "screws on and has worn away.");

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
                "As discussed on the phone, please find the info below on scalers that may help.",
                EmailHtml.LineBreak(),
                "We sell replacement MK-Dent scalers which are far more economical and ‘disposably priced’ in comparison "
                + "to buying EMS/Aceton/Satelec/NSK/Kavo branded scalers at $1000+. It may work out cheaper changing "
                + "over any current scaler chair line fittings if they are not EMS or Aceton /Satelec, so that you can use "
                + "these cheaper priced scalers in the future and save more money in the long term.",
                EmailHtml.LineBreak(),
                "But as I mentioned it all depends upon how the tip goes on correctly as to how long they are likely to last too!"));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "For most scalers on which the thread has gone or has lost vibration etc. it’s not possible to repair as the main "
                + "body inside is glued together during the manufacturing process so it’s impossible to get inside to replace damaged parts, "
                + "hence being unrepairable. Some Kavo scalers are repairable but you are looking at in excess of $650+ to repair, "
                + "so you would need to consider its age also against repairing or replacing it."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Our MK-dent scalers are priced as below: ",
                EmailHtml.LineBreak(),
                "Do call me if you have any further questions about the scalers available or want to place an order. ",
                EmailHtml.LineBreak(),
                "Payment is required upfront to confirm the order and it usually takes 2 days delivery direct from manufacturer."));

            if (resources.TryGetValue("scaler-pricing", out var scalerPricing))
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Image($"cid:{scalerPricing.ContentId}", "Scaler pricing")));
            }

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                EmailHtml.Bold("Scaler Troubleshooting:")));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "With scalers and any other instruments with a thread on it, always do a half turn in the opposite direction to ensure you line up the thread, "
                + "you may feel a slight click and that usually means it has fallen into its correct groove and should then be easy to screw on. If it’s ever hard "
                + "to screw on then it’s definitely not sat on square and you are doing more harm by forcing it to realign itself."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Always ensure that you are using the correct tip for the correct brand too!!",
                EmailHtml.LineBreak(),
                "The thread pitch is finer on the EMS (4 flat planes) and coarser on the Satelec Scalers (2 flat planes) see photos.",
                EmailHtml.LineBreak(),
                "If you purchase the wrong tips by mistake, they will only go on about half way. If you use the tip wrench you can force it on, but stripping the threads in the process.",
                EmailHtml.LineBreak(),
                "If the threads are stripped then the scaler is ready for the bin (as in this case!).",
                EmailHtml.LineBreak(),
                "If a tip has got stuck or caused damage to the scaler, then the tip should be discarded too."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.SimpleTable(
                EmailHtml.Image($"cid:{resources["scaler-troubleshooting-1.jpg"].ContentId}", "Scaler Troubleshooting"),
                EmailHtml.Image($"cid:{resources["scaler-troubleshooting-2.jpg"].ContentId}", "Scaler Troubleshooting")));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "In addition to the notes above this explains threads:"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Scaler usually has male thread ",
                EmailHtml.LineBreak(),
                "Scaler tips usually have female thread ",
                EmailHtml.LineBreak(),
                "Both need to line up to make sure that the tip screws on properly. ",
                EmailHtml.LineBreak(),
                "If either of the threads are damaged then you can risk damaging the other in the process of trying to force it on, "
                + "wearing away the thread that keeps the tip held onto the scaler. ",
                EmailHtml.LineBreak(),
                "(In a similar way that if you try and screw a water bottle to on that’s not lined up properly it will go on "
                + "at an angle and can eventually break the plastic thread of the bottle, and the lid won’t fit on and water will spill out etc.) ",
                EmailHtml.LineBreak(),
                "The photo below shows the thread that has worn away, you can see a distinct line between the top area where the tip screws on and has worn away."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(EmailHtml.Image($"cid:{resources["scaler-troubleshooting-3.jpg"].ContentId}", "Scaler Troubleshooting")));

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
            loader.LoadImageFromResources("scaler-troubleshooting-1.jpg");
            loader.LoadImageFromResources("scaler-troubleshooting-2.jpg");
            loader.LoadImageFromResources("scaler-troubleshooting-3.jpg");
            loader.LoadImageFromResources("dds-fb-like.jpg");
            loader.LoadImageFromResources("dds-instagram.png");
            loader.LoadUploadedImage("scaler-pricing", this.ScalersPricesImage.Image, this.ScalersPricesImage.ImageBytes);
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
