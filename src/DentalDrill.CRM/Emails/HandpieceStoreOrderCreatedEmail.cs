using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class HandpieceStoreOrderCreatedEmail : BaseEmail
    {
        public String Recipient { get; set; }

        public String Link { get; set; }

        public HandpieceStoreOrder Order { get; set; }

        public String SubjectPrefix { get; set; }

        protected override Boolean IsTransactionalEmail => true;

        protected override Boolean IsCentered()
        {
            return false;
        }

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.AddRange(InternetAddressList.Parse(this.Recipient));
        }

        protected override String GetSubject()
        {
            return (this.SubjectPrefix ?? String.Empty) + "Handpiece Order - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"Order {this.Order.OrderNumber} was created");
            writer.WriteLine($"Open in browser: {this.Link}");
            writer.WriteLine();
            writer.WriteLine($"Created By: {this.Order.CreatedBy?.UserName}");

            if (this.Order.EmployeeId.HasValue)
            {
                writer.WriteLine($"Employee: {this.Order.Employee.FirstName} {this.Order.Employee.LastName}");
            }

            if (this.Order.ClientId.HasValue)
            {
                writer.WriteLine($"Client: {this.Order.Client.Name}");
            }

            if (this.Order.CorporateId.HasValue)
            {
                writer.WriteLine($"Corporate: {this.Order.Corporate.Name}");
            }

            writer.WriteLine();

            writer.WriteLine($"Surgery Name: {this.Order.SurgeryName}");
            writer.WriteLine($"Contact Name: {this.Order.ContactName}");
            writer.WriteLine($"Contact Email: {this.Order.ContactEmail}");
            writer.WriteLine();

            foreach (var item in this.Order.Items)
            {
                writer.WriteLine($"Handpiece: {item.Listing.Model.Brand.Name} {item.Listing.Model.Name}");
            }
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.Heading2(EmailHtml.Anchor($"Order {this.Order.OrderNumber}", this.Link))));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.EmptyParagraph()));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Created By:"), this.Order.CreatedBy?.UserName }));

            if (this.Order.EmployeeId.HasValue)
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Employee:"), $"{this.Order.Employee.FirstName} {this.Order.Employee.LastName}" }));
            }

            if (this.Order.ClientId.HasValue)
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Client:"), this.Order.Client.Name }));
            }

            if (this.Order.CorporateId.HasValue)
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Corporate:"), this.Order.Corporate.Name }));
            }

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.EmptyParagraph()));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Surgery Name:"), this.Order.SurgeryName }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Contact Name:"), this.Order.ContactName }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Contact Email:"), this.Order.ContactEmail }));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.EmptyParagraph()));

            foreach (var item in this.Order.Items)
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Handpiece:"), $"{item.Listing.Model.Brand.Name} {item.Listing.Model.Name}" }));
            }

            return wrapper;
        }
    }
}
