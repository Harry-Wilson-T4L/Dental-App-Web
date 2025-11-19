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
    [HtmlTargetElement("job-status-indicator")]
    public class JobStatusIndicatorTagHelper : ITagHelper, ITagHelperComponent
    {
        private Int32[] statusConfig;
        private Int32 indicatorIndex;

        public Int32 Order => 0;

        [HtmlAttributeName("entity")]
        public Job Entity { get; set; }

        [HtmlAttributeName("description")]
        public Boolean IncludeDescription { get; set; }

        public void Init(TagHelperContext context)
        {
        }

        public Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("entity");
            output.Attributes.RemoveAll("description");

            var visualizationNumber = this.Entity.Status.ToIndicatorValue();
            var danger = false;
            if (visualizationNumber < 0)
            {
                visualizationNumber = -visualizationNumber;
                danger = true;
            }

            this.indicatorIndex = visualizationNumber;
            this.statusConfig = this.Entity.ComputeStatusConfig().Split(';', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            var overrideConfig = String.Join(" ", this.statusConfig
                .Skip(1)
                .Select((x, i) => new { Index = i + 1, HasOverride = x > 0 && (i + 1) < visualizationNumber })
                .Where(x => x.HasOverride)
                .Select(x => x.Index));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.Clear();
            output.AddClass("handpiece-status-indicator", HtmlEncoder.Default);
            output.Attributes.Add("data-max", "7");
            output.Attributes.Add("data-value", visualizationNumber.ToString());
            output.Attributes.Add("data-danger", danger.ToString());
            if (!String.IsNullOrWhiteSpace(overrideConfig))
            {
                output.Attributes.Add("data-override", overrideConfig);
            }

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
            tag.InnerHtml.AppendHtml(this.CreatePoint(1));
            tag.InnerHtml.AppendHtml(this.CreatePoint(2));
            tag.InnerHtml.AppendHtml(this.CreatePoint(3));
            tag.InnerHtml.AppendHtml(this.CreatePoint(4));
            tag.InnerHtml.AppendHtml(this.CreatePoint(5));
            tag.InnerHtml.AppendHtml(this.CreatePoint(6));
            tag.InnerHtml.AppendHtml(this.CreatePoint(7));
            return tag;
        }

        private IHtmlContent CreatePoint(Int32 index)
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("handpiece-status-indicator__points__point");
            if (index < this.indicatorIndex && this.statusConfig[index] > 0)
            {
                tag.InnerHtml.Append(this.statusConfig[index].ToString());
            }

            return tag;
        }

        private IHtmlContent CreateDescription()
        {
            var description = this.Entity.Status.ToDisplayString();
            var tag = new TagBuilder("h4");
            tag.AddCssClass("handpiece-status-description");
            tag.InnerHtml.Append(description);
            return tag;
        }
    }
}
