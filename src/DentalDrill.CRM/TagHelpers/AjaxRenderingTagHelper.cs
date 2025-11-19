using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DentalDrill.CRM.TagHelpers
{
    public enum AjaxRenderingMode
    {
        Never = 0,
        NormalOnly = 1,
        AjaxOnly = 2,
        Always = 3,
    }

    [HtmlTargetElement(Attributes = "ajax-rendering")]
    public class AjaxRenderingTagHelper : ITagHelper, ITagHelperComponent
    {
        public Int32 Order => 0;

        [HtmlAttributeName("ajax-rendering")]
        public AjaxRenderingMode Mode { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public void Init(TagHelperContext context)
        {
        }

        public Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("ajax-rendering");

            switch (this.Mode)
            {
                case AjaxRenderingMode.Never:
                    output.SuppressOutput();
                    return Task.CompletedTask;
                case AjaxRenderingMode.NormalOnly:
                    if (this.ViewContext.HttpContext.IsAjaxRequest())
                    {
                        output.SuppressOutput();
                    }

                    return Task.CompletedTask;
                case AjaxRenderingMode.AjaxOnly:
                    if (!this.ViewContext.HttpContext.IsAjaxRequest())
                    {
                        output.SuppressOutput();
                    }

                    return Task.CompletedTask;
                case AjaxRenderingMode.Always:
                    return Task.CompletedTask;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
