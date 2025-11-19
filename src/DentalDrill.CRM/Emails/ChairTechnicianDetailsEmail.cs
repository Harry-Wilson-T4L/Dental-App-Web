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
    public class ChairTechnicianDetailsEmail : BaseEmployeeEmail
    {
        public Client Client { get; set; }

        public String To { get; set; }

        public String Greeting { get; set; }

        public String Following { get; set; }

        public List<(UploadedFile FileInfo, Byte[] FileBytes)> Attachments { get; set; }

        protected override Boolean IsTransactionalEmail => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Greeting, this.To));
        }

        protected override String GetSubject()
        {
            return "Chair Technician’s Details - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"Hi {this.Greeting},");
            writer.WriteLine($"Following {this.Following}, we have a chair technician that we recommend to our clients - he might be able to help you "
                             + $"with your chair servicing/upgrades.");
            writer.WriteLine();
            writer.WriteLine("His details are as follows:");
            writer.WriteLine();
            writer.WriteLine("Jamie Warner");
            writer.WriteLine("URDENT");
            writer.WriteLine("PO Box 1044");
            writer.WriteLine("Dundas NSW 2117");
            writer.WriteLine("jamie@urdentptyltd.com");
            writer.WriteLine("0411 217 278");
            writer.WriteLine();
            writer.WriteLine("Do mention that we (DDS) referred you to him (he's covers the Sydney Metro area and some country towns too, "
                             + "so I'm sure he would be able to help or give advice on the phone if worst case scenario). Hope this info helps.");
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                $"Hi {this.Greeting},",
                EmailHtml.LineBreak(),
                $"Following {this.Following}, we have a chair technician that we recommend to our clients - he might be able to help you"
                + $" with your chair servicing/upgrades."));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("His details are as follows:"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                "Jamie Warner", EmailHtml.LineBreak(),
                "URDENT", EmailHtml.LineBreak(),
                "PO Box 1044", EmailHtml.LineBreak(),
                "Dundas NSW 2117", EmailHtml.LineBreak(),
                EmailHtml.Anchor("jamie@urdentptyltd.com", "mailto:jamie@urdentptyltd.com"), EmailHtml.LineBreak(),
                "0411 217 278"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph("Do mention that we (DDS) referred you to him (he’s covers the Sydney Metro area "
                                                             + "and some country towns too, so I’m sure he would be able to help or give advice "
                                                             + "on the phone if worst case scenario). Hope this info helps."));

            return wrapper;
        }
    }
}
