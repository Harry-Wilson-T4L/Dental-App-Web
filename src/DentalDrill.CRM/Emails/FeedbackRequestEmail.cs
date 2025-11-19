using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Emails.Helpers;
using DentalDrill.CRM.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public class FeedbackRequestEmail : BaseEmail
    {
        private readonly FeedbackFormDomainModel feedbackForm;
        private readonly String recipient;
        private readonly String rootUrl;

        public FeedbackRequestEmail(FeedbackFormDomainModel feedbackForm, String recipient, String rootUrl)
        {
            this.feedbackForm = feedbackForm;
            this.recipient = recipient;
            this.rootUrl = rootUrl;
        }

        protected override Boolean IsTransactionalEmail => false;

        protected override void FillRecipients(InternetAddressList to, InternetAddressList cc, InternetAddressList bcc)
        {
            to.Add(new MailboxAddress((String)null, this.recipient));
        }

        protected override String GetSubject()
        {
            return "Feedback - Dental Drill Solutions";
        }

        protected override void RenderTextContent(TextWriter writer)
        {
            writer.WriteLine($"We value your feedback");
            writer.WriteLine();
            writer.WriteLine($"We would like your feedback to improve our service");
            writer.WriteLine($"As a Thank You for your recent custom and feedback we will apply a further 5% Discount Off your next handpiece repair bill!*");
            writer.WriteLine($"*Discount is not applicable to new parts service/sales");
            writer.WriteLine($"");
            writer.WriteLine($"Open Form: {this.GetFormUrl()}");
            writer.WriteLine($"");
            writer.WriteLine($"Please click on the links below to share your personal experience with others");
            writer.WriteLine($"Review us on Google: https://www.google.com/search?client=firefox-b-d&q=dental+drill+solutions");
            writer.WriteLine($"Review us on Facebook: https://www.facebook.com/dentaldrillsolutions/reviews/?ref=page_internal");
        }

        protected override IHtmlContent RenderHtmlContent(IDictionary<String, MimeEntity> resources)
        {
            var wrapper = new TagBuilder("div");

            wrapper.InnerHtml.AppendHtml(this.RenderHeader(resources));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div("We would like your feedback to improve our service", "margin-top: 8px; margin-bottom: 16px; font-weight: bold; text-align: center;"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(
                new Object[]
                {
                    "As a Thank You for your recent custom and feedback we will apply a further ",
                    EmailHtml.Span("5% Discount Off", "color: red;"),
                    " your next handpiece repair bill!",
                    EmailHtml.Sub("*", "color: red;"),
                },
                "margin-top: 8px; margin-bottom: 16px; font-weight: bold; text-align: center;"));
            wrapper.InnerHtml.AppendHtml(this.RenderQuestions(resources));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div("*Discount is not applicable to new parts service/sales", "margin-bottom: 16px; color: red;"));
            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(
                EmailHtml.Div(
                    EmailHtml.Anchor("Open Form", this.GetFormUrl(), "color: #ffffff; line-height: 36px; text-decoration: none;"),
                    "height: 36px; width: 100px; border-radius: 4px; background-color: #007bff; margin-left: auto; margin-right: auto;"),
                "text-align: center;"));

            wrapper.InnerHtml.AppendHtml(EmailHtml.Div(
                text: "Please click on the picture links below to share your personal experience with others",
                style: "text-align: center; margin-top: 0.5em; margin-bottom: 0.5em"));

            if (resources.TryGetValue("review-facebook.jpg", out var reviewFacebook) &&
                resources.TryGetValue("review-google.jpg", out var reviewGoogle))
            {
                wrapper.InnerHtml.AppendHtml(EmailHtml.Div(
                    content: new Object[]
                    {
                        EmailHtml.Anchor(
                            content: EmailHtml.Image(src: $"cid:{reviewGoogle.ContentId}", alt: "Review us on Google", width: 250),
                            link: "https://www.google.com/search?client=firefox-b-d&q=dental+drill+solutions"),
                        EmailHtml.Span(new HtmlString(" &nbsp; ")),
                        EmailHtml.Anchor(
                            content: EmailHtml.Image(src: $"cid:{reviewFacebook.ContentId}", alt: "Review us on Facebook", width: 250),
                            link: "https://www.facebook.com/dentaldrillsolutions/reviews/?ref=page_internal"),
                    },
                    style: "text-align: center;"));
            }

            return wrapper;
        }

        protected override IDictionary<String, MimeEntity> AddBodyLinkedResources(AttachmentCollection resources)
        {
            var loader = new ResourceLoader(resources, base.AddBodyLinkedResources(resources));
            loader.LoadImageFromResources("dds-logo-small.jpg");
            loader.LoadImageFromResources("rating1.png");
            loader.LoadImageFromResources("rating2.png");
            loader.LoadImageFromResources("rating3.png");
            loader.LoadImageFromResources("rating4.png");
            loader.LoadImageFromResources("rating5.png");
            loader.LoadImageFromResources("review-google.jpg");
            loader.LoadImageFromResources("review-facebook.jpg");

            return loader.GetResult();
        }

        private IHtmlContent RenderHeader(IDictionary<String, MimeEntity> resources)
        {
            var table = new TagBuilder("table");
            table.Attributes.Add("border", "0");
            table.Attributes.Add("cellpadding", "0");
            table.Attributes.Add("cellspacing", "0");
            table.Attributes.Add("width", "100%");
            table.Attributes.Add("style", "border-bottom: 2px solid #dadada; padding-bottom: 8px;");
            {
                var tableRow = new TagBuilder("tr");

                {
                    var tableCell = new TagBuilder("td");
                    tableCell.InnerHtml.AppendHtml(EmailHtml.Anchor(
                        EmailHtml.Image($"cid:{resources["dds-logo-small.jpg"].ContentId}", "Dental Drill Solutions"),
                        "https://www.dds11.au/"));

                    tableRow.InnerHtml.AppendHtml(tableCell);
                }

                {
                    var tableCell = new TagBuilder("td");
                    tableCell.InnerHtml.AppendHtml(EmailHtml.Heading1("We value your feedback", "font-size: 28px; padding-top: 32px; text-align: right;"));

                    tableRow.InnerHtml.AppendHtml(tableCell);
                }

                table.InnerHtml.AppendHtml(tableRow);
            }

            return table;
        }

        private IHtmlContent RenderQuestions(IDictionary<String, MimeEntity> resources)
        {
            var questions = this.feedbackForm.GetQuestionsAndAnswers();
            var root = new TagBuilder("div");
            for (var i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                var questionDiv = new TagBuilder("div");
                questionDiv.Attributes.Add("style", i == (questions.Count - 1) ? "margin-top: 16px; margin-bottom: 16px;" : "margin-top: 16px; margin-bottom: 16px; border-bottom: 2px solid #dadada;");

                switch (question.Question.Type)
                {
                    case FeedbackFormQuestionType.Rating:
                        questionDiv.InnerHtml.AppendHtml(EmailHtml.Div(question.Question.Name, "font-weight: bold; text-align: center;"));
                        questionDiv.InnerHtml.AppendHtml(EmailHtml.Div(
                            new Object[]
                            {
                                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["rating1.png"].ContentId}", "Rate 1"), this.GetFormUrl(), "margin-left: 8px; margin-right: 8px;"),
                                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["rating2.png"].ContentId}", "Rate 2"), this.GetFormUrl(), "margin-left: 8px; margin-right: 8px;"),
                                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["rating3.png"].ContentId}", "Rate 3"), this.GetFormUrl(), "margin-left: 8px; margin-right: 8px;"),
                                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["rating4.png"].ContentId}", "Rate 4"), this.GetFormUrl(), "margin-left: 8px; margin-right: 8px;"),
                                EmailHtml.Anchor(EmailHtml.Image($"cid:{resources["rating5.png"].ContentId}", "Rate 5"), this.GetFormUrl(), "margin-left: 8px; margin-right: 8px;"),
                            },
                            "margin-top: 16px; margin-bottom: 16px; text-align: center;"));
                        break;

                    case FeedbackFormQuestionType.MultilineText:
                        questionDiv.InnerHtml.AppendHtml(EmailHtml.Div(question.Question.Name, "font-weight: bold; text-align: left; margin-bottom: 8px;"));
                        questionDiv.InnerHtml.AppendHtml(EmailHtml.Div(EmailHtml.Div("", "border: 1px solid #ced4da; height: 100px;")));
                        break;
                }

                root.InnerHtml.AppendHtml(questionDiv);
            }

            return root;
        }

        private String GetFormUrl()
        {
            var builder = new StringBuilder();
            builder.Append(this.rootUrl);
            if (!this.rootUrl.EndsWith("/"))
            {
                builder.Append("/");
            }

            builder.AppendFormat($"Feedback/Form/{this.feedbackForm.Id}");
            return builder.ToString();
        }
    }
}
