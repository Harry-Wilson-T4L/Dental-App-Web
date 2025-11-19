using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Emails.Helpers;
using DentalDrill.CRM.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class MaintenanceRequiredEmail : BaseEmail
    {
        public Client Client { get; set; }

        public IReadOnlyList<IClientRepairedItem> Items { get; set; }

        protected override Boolean IsTransactionalEmail => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Client.PrincipalDentist, this.Client.Email));
        }

        protected override String GetSubject()
        {
            return "Handpiece is due for service - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"If you notice the red bell icon next to any of your handpieces on the Handpiece Hub, this indicates that we have not seen that particular handpiece within at least a 12 month period. Therefore, you should withdraw it from circulation and have it checked over for a general service.");
            writer.WriteLine();
            writer.WriteLine($"The following handpieces are due for a service:");

            foreach (var item in this.Items)
            {
                writer.WriteLine($"- {item.Brand} {item.Model} S/N {item.Serial}");
            }

            writer.WriteLine();
            writer.WriteLine($"Please call us on 1300 337 300 to organise a Free pick-up.");
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph(
                $"If you notice the red bell icon ",
                EmailHtml.Image($"cid:{resources["red-bell.png"].ContentId}", "icon", height: 16, style: "height: 1em;"),
                " next to any of your handpieces on the Handpiece Hub, this indicates that we have not seen that particular handpiece within at least a 12 month period. Therefore, you should withdraw it from circulation and have it checked over for a general service."));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph($"The following handpieces are due for a service:"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.UnorderedList(this.Items.Select(item => $"{item.Brand} {item.Model} S/N {item.Serial}").ToArray()));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph($"Please call us on 1300 337 300 to organise a Free pick-up."));
            return wrapper;
        }

        protected override IDictionary<String, MimeEntity> AddBodyLinkedResources(AttachmentCollection resources)
        {
            var loader = new ResourceLoader(resources, base.AddBodyLinkedResources(resources));
            loader.LoadImageFromResources("red-bell.png");
            return loader.GetResult();
        }
    }
}
