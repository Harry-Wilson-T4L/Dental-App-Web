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
    public class PickupRequestReceivedEmail : BaseEmail
    {
        public String Recipient { get; set; }

        public String Link { get; set; }

        public PickupRequest Request { get; set; }

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
            return (this.SubjectPrefix ?? String.Empty) + "Pickup Request - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"New pickup request received");
            writer.WriteLine($"Open in browser: {this.Link}");
            writer.WriteLine();
            switch (this.Request.Type)
            {
                case PickupRequestType.GreaterSydney:
                    writer.WriteLine($"Type of practice: Greater Sydney Practices");
                    break;
                case PickupRequestType.Australia:
                    writer.WriteLine($"Type of practice: Australia Wide Practices");
                    break;
                case PickupRequestType.NewZealand:
                    writer.WriteLine($"Type of practice: New Zealand Practices");
                    break;
                case PickupRequestType.Queensland:
                    writer.WriteLine($"Type of practice: Queensland Practices");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            writer.WriteLine($"Created By: {this.Request.CreatedBy?.UserName}");

            if (this.Request.EmployeeId.HasValue)
            {
                writer.WriteLine($"Employee: {this.Request.Employee.FirstName} {this.Request.Employee.LastName}");
            }

            if (this.Request.ClientId.HasValue)
            {
                writer.WriteLine($"Client: {this.Request.Client.Name}");
            }

            if (this.Request.CorporateId.HasValue)
            {
                writer.WriteLine($"Corporate: {this.Request.Corporate.Name}");
            }

            writer.WriteLine();

            writer.WriteLine($"Practice Name: {this.Request.PracticeName}");
            writer.WriteLine($"Number of handpieces: {this.Request.HandpiecesCount}");
            writer.WriteLine($"Contact Person: {this.Request.ContactPerson}");
            writer.WriteLine($"Email: {this.Request.Email}");
            writer.WriteLine($"Phone: {this.Request.Phone}");
            writer.WriteLine($"Address Line 1: {this.Request.AddressLine1}");
            writer.WriteLine($"Address Line 2: {this.Request.AddressLine2}");
            writer.WriteLine($"Suburb: {this.Request.Suburb}");
            writer.WriteLine($"State: {this.Request.State}");
            writer.WriteLine($"Postcode: {this.Request.Postcode}");
            writer.WriteLine($"Country: {this.Request.Country}");
            writer.WriteLine($"Comment: {this.Request.Comment}");
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.Heading2(EmailHtml.Anchor("Pickup Request", this.Link))));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.EmptyParagraph()));

            switch (this.Request.Type)
            {
                case PickupRequestType.GreaterSydney:
                    wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Type of practice:"), "Greater Sydney Practices" }));
                    break;
                case PickupRequestType.Australia:
                    wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Type of practice:"), "Australia Wide Practices" }));
                    break;
                case PickupRequestType.NewZealand:
                    wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Type of practice:"), "New Zealand Practices" }));
                    break;
                case PickupRequestType.Queensland:
                    wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Type of practice:"), "Queensland Practices" }));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Created By:"), this.Request.CreatedBy?.UserName }));

            if (this.Request.EmployeeId.HasValue)
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Employee:"), $"{this.Request.Employee.FirstName} {this.Request.Employee.LastName}" }));
            }

            if (this.Request.ClientId.HasValue)
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Client:"), this.Request.Client.Name }));
            }

            if (this.Request.CorporateId.HasValue)
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Corporate:"), this.Request.Corporate.Name }));
            }

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.EmptyParagraph()));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Practice Name:"), this.Request.PracticeName }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Number of handpieces:"), this.Request.HandpiecesCount.ToString() }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Contact Person:"), this.Request.ContactPerson }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Email:"), this.Request.Email }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Phone:"), this.Request.Phone }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Address Line 1:"), this.Request.AddressLine1 }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Address Line 2:"), this.Request.AddressLine2 }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Suburb:"), this.Request.Suburb }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("State:"), this.Request.State }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Postcode:"), this.Request.Postcode }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Country:"), this.Request.Country }));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(content: new Object[] { EmailHtml.Bold("Comment:"), this.Request.Comment }));

            return wrapper;
        }
    }
}
