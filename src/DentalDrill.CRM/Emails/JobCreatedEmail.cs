using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class JobCreatedEmail : BaseEmployeeEmail
    {
        public Client Client { get; set; }

        public Job Job { get; set; }

        protected override Boolean IsTransactionalEmail => true;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress(Encoding.UTF8, this.Client.PrincipalDentist, this.Client.Email));
        }

        protected override String GetSubject()
        {
            return "Handpieces Received - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine("Hi {0},", this.Client.PrincipalDentist);
            writer.WriteLine("Just confirming that your handpieces have been received and are in line for assessment. We shall endeavour to get your estimate to you as soon as possible.");
            //// writer.WriteLine("Please be advised that the technical assessment requires the handpieces to be dissembled if the equipment does not pass the prescribed test. Furthermore, due to regulatory requirements, should you decide not to proceed with quoted repairs the faulty handpiece will be returned disassembled. Please contact us immediately if you do not wish to go ahead with the assessment.");
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph($"Hi {this.Client.PrincipalDentist},"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph($"Just confirming that your handpieces have been received and are in line for assessment. We shall endeavour to get your estimate to you as soon as possible."));
            //// wrapper.InnerHtml.AppendHtml(EmailHtml.Paragraph($"Please be advised that the technical assessment requires the handpieces to be dissembled if the equipment does not pass the prescribed test. Furthermore, due to regulatory requirements, should you decide not to proceed with quoted repairs the faulty handpiece will be returned disassembled. Please contact us immediately if you do not wish to go ahead with the assessment."));
            return wrapper;
        }
    }
}
