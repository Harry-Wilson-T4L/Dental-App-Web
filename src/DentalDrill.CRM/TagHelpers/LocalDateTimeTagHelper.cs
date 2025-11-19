using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DentalDrill.CRM.TagHelpers
{
    [HtmlTargetElement("local-date-time")]
    public class LocalDateTimeTagHelper : ITagHelper, ITagHelperComponent
    {
        public Int32 Order => 0;

        [HtmlAttributeName("value")]
        public DateTime Value { get; set; }

        [HtmlAttributeName("format")]
        public String Format { get; set; }

        public void Init(TagHelperContext context)
        {
        }

        public Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var id = $"DateTimeObject_{Guid.NewGuid():N}";

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.RemoveAll("id");
            output.Attributes.RemoveAll("value");
            output.Attributes.RemoveAll("format");

            output.Attributes.Add("id", id);
            output.Content.AppendHtml(this.CreateNoScript());
            output.Content.AppendHtml(this.CreateScript(id));

            return Task.CompletedTask;
        }

        private IHtmlContent CreateNoScript()
        {
            var noScript = new TagBuilder("noscript");
            noScript.InnerHtml.Append(this.Value.ToString(this.Format));
            return noScript;
        }

        private IHtmlContent CreateScript(String elementId)
        {
            var script = new TagBuilder("script");
            script.Attributes.Add("type", "text/javascript");

            var formattedDate = $"{this.Value:s}Z";

            var scriptText = $"(function () {{ document.getElementById(\"{elementId}\").innerText = kendo.toString(new Date(\"{formattedDate}\"), \"{this.Format}\"); }})();";
            script.InnerHtml.AppendHtml(scriptText);

            return script;
        }
    }
}
