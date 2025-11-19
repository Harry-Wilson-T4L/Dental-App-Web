using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DentalDrill.CRM.TagHelpers
{
    [HtmlTargetElement("handpiece-status-indicator")]
    public class HandpieceStatusIndicatorTagHelper : ITagHelper, ITagHelperComponent
    {
        public Int32 Order => 0;

        [HtmlAttributeName("status")]
        public HandpieceStatus Status { get; set; }

        [HtmlAttributeName("description")]
        public Boolean IncludeDescription { get; set; }

        public void Init(TagHelperContext context)
        {
        }

        public Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("status");
            output.Attributes.RemoveAll("description");

            var visualizationNumber = this.Status.ToInternalVisualisationNumber();
            var danger = false;
            if (visualizationNumber < 0)
            {
                visualizationNumber = -visualizationNumber;
                danger = true;
            }

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.Clear();
            output.AddClass("handpiece-status-indicator", HtmlEncoder.Default);
            output.Attributes.Add("data-max", "7");
            output.Attributes.Add("data-value", visualizationNumber.ToString());
            output.Attributes.Add("data-danger", danger.ToString());

            output.Content.AppendHtml(this.CreateProgress());
            output.Content.AppendHtml(this.CreatePoints());

            if (this.IncludeDescription)
            {
                output.PostElement.AppendHtml(this.CreateDescription());
            }

            return Task.CompletedTask;
        }

        private IHtmlContent CreateProgress()
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("progress handpiece-status-indicator__progress");
            tag.InnerHtml.AppendHtml(this.CreateProgressBar());
            return tag;
        }

        private IHtmlContent CreateProgressBar()
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("progress-bar");
            return tag;
        }

        private IHtmlContent CreatePoints()
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("handpiece-status-indicator__points");
            tag.InnerHtml.AppendHtml(this.CreatePoint());
            tag.InnerHtml.AppendHtml(this.CreatePoint());
            tag.InnerHtml.AppendHtml(this.CreatePoint());
            tag.InnerHtml.AppendHtml(this.CreatePoint());
            tag.InnerHtml.AppendHtml(this.CreatePoint());
            tag.InnerHtml.AppendHtml(this.CreatePoint());
            tag.InnerHtml.AppendHtml(this.CreatePoint());
            return tag;
        }

        private IHtmlContent CreatePoint()
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("handpiece-status-indicator__points__point");
            return tag;
        }

        private IHtmlContent CreateDescription()
        {
            var description = this.Status.ToInternalStatusDescription();
            var tag = new TagBuilder("h4");
            tag.AddCssClass("handpiece-status-description");
            tag.InnerHtml.Append(description);
            return tag;
        }
    }
}
