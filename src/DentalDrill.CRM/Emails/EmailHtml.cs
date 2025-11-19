using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Emails
{
    public static class EmailHtml
    {
        public static IHtmlContent Anchor(IHtmlContent content, String link, String style = null, String align = null)
        {
            var tag = new TagBuilder("a");
            tag.Attributes.Add("href", link);
            if (!String.IsNullOrEmpty(style))
            {
                tag.Attributes.Add("style", style);
            }

            if (!String.IsNullOrEmpty(align))
            {
                tag.Attributes.Add("align", align);
            }

            tag.InnerHtml.AppendHtml(content);

            return tag;
        }

        public static IHtmlContent Anchor(String text, String link, String style = null, String align = null)
        {
            var tag = new TagBuilder("a");
            tag.Attributes.Add("href", link);
            if (!String.IsNullOrEmpty(style))
            {
                tag.Attributes.Add("style", style);
            }

            if (!String.IsNullOrEmpty(align))
            {
                tag.Attributes.Add("align", align);
            }

            tag.InnerHtml.Append(text);

            return tag;
        }

        public static IHtmlContent Image(String src, String alt, Int32? width = null, Int32? height = null, String align = null, String style = null)
        {
            var tag = new TagBuilder("img");
            tag.Attributes.Add("src", src);
            tag.Attributes.Add("alt", alt);
            if (width.HasValue)
            {
                tag.Attributes.Add("width", width.Value.ToString());
            }

            if (height.HasValue)
            {
                tag.Attributes.Add("height", height.Value.ToString());
            }

            if (!String.IsNullOrEmpty(align))
            {
                tag.Attributes.Add("align", align);
            }

            if (!String.IsNullOrEmpty(style))
            {
                tag.Attributes.Add("style", style);
            }

            return tag;
        }

        public static IHtmlContent Span(IHtmlContent content) => EmailHtml.SimpleTag("span", content, null);

        public static IHtmlContent Span(String text) => EmailHtml.SimpleTag("span", text, null);

        public static IHtmlContent Span(IHtmlContent content, String style) => EmailHtml.SimpleTag("span", content, style);

        public static IHtmlContent Span(String text, String style) => EmailHtml.SimpleTag("span", text, style);

        public static IHtmlContent Italic(IHtmlContent content) => EmailHtml.SimpleTag("i", content, null);

        public static IHtmlContent Italic(String text) => EmailHtml.SimpleTag("i", text, null);

        public static IHtmlContent Italic(IHtmlContent content, String style) => EmailHtml.SimpleTag("i", content, style);

        public static IHtmlContent Italic(String text, String style) => EmailHtml.SimpleTag("i", text, style);

        public static IHtmlContent Bold(IHtmlContent content) => EmailHtml.SimpleTag("b", content, null);

        public static IHtmlContent Bold(String text) => EmailHtml.SimpleTag("b", text, null);

        public static IHtmlContent Bold(IHtmlContent content, String style) => EmailHtml.SimpleTag("b", content, style);

        public static IHtmlContent Bold(String text, String style) => EmailHtml.SimpleTag("b", text, style);

        public static IHtmlContent Underline(String text) => EmailHtml.SimpleTag("span", text, $"text-decoration: underline;");

        public static IHtmlContent Underline(String text, String style) => EmailHtml.SimpleTag("span", text, $"text-decoration: underline;{style}");

        public static IHtmlContent Underline(IHtmlContent content) => EmailHtml.SimpleTag("span", content, $"text-decoration: underline;");

        public static IHtmlContent Underline(IHtmlContent content, String style) => EmailHtml.SimpleTag("span", content, $"text-decoration: underline;{style}");

        public static IHtmlContent Sub(IHtmlContent content) => EmailHtml.SimpleTag("sub", content, null);

        public static IHtmlContent Sub(String text) => EmailHtml.SimpleTag("sub", text, null);

        public static IHtmlContent Sub(IHtmlContent content, String style) => EmailHtml.SimpleTag("sub", content, style);

        public static IHtmlContent Sub(String text, String style) => EmailHtml.SimpleTag("sub", text, style);

        public static IHtmlContent Sup(IHtmlContent content) => EmailHtml.SimpleTag("sup", content, null);

        public static IHtmlContent Sup(String text) => EmailHtml.SimpleTag("sup", text, null);

        public static IHtmlContent Sup(IHtmlContent content, String style) => EmailHtml.SimpleTag("sup", content, style);

        public static IHtmlContent Sup(String text, String style) => EmailHtml.SimpleTag("sup", text, style);

        public static IHtmlContent Paragraph(IHtmlContent content) => EmailHtml.SimpleTag("p", content, null);

        public static IHtmlContent Paragraph(String text) => EmailHtml.SimpleTag("p", text, null);

        public static IHtmlContent Paragraph(IHtmlContent content, String style) => EmailHtml.SimpleTag("p", content, style);

        public static IHtmlContent Paragraph(String text, String style) => EmailHtml.SimpleTag("p", text, style);

        public static IHtmlContent Paragraph(params Object[] content) => EmailHtml.SimpleTag("p", content);

        public static IHtmlContent ParagraphStyled(String style, params Object[] content) => EmailHtml.SimpleTag("p", content, style);

        public static IHtmlContent Heading1(IHtmlContent content) => EmailHtml.SimpleTag("h1", content, null);

        public static IHtmlContent Heading1(String text) => EmailHtml.SimpleTag("h1", text, null);

        public static IHtmlContent Heading1(IHtmlContent content, String style) => EmailHtml.SimpleTag("h1", content, style);

        public static IHtmlContent Heading1(String text, String style) => EmailHtml.SimpleTag("h1", text, style);

        public static IHtmlContent Heading1(params Object[] content) => EmailHtml.SimpleTag("h1", content);

        public static IHtmlContent Heading2(IHtmlContent content) => EmailHtml.SimpleTag("h2", content, null);

        public static IHtmlContent Heading2(String text) => EmailHtml.SimpleTag("h2", text, null);

        public static IHtmlContent Heading2(IHtmlContent content, String style) => EmailHtml.SimpleTag("h2", content, style);

        public static IHtmlContent Heading2(String text, String style) => EmailHtml.SimpleTag("h2", text, style);

        public static IHtmlContent Heading2(params Object[] content) => EmailHtml.SimpleTag("h2", content);

        public static IHtmlContent Div(IHtmlContent content) => EmailHtml.SimpleTag("div", content, null);

        public static IHtmlContent Div(String text) => EmailHtml.SimpleTag("div", text, null);

        public static IHtmlContent Div(IHtmlContent content, String style) => EmailHtml.SimpleTag("div", content, style);

        public static IHtmlContent Div(String text, String style) => EmailHtml.SimpleTag("div", text, style);

        public static IHtmlContent Div(params Object[] content) => EmailHtml.SimpleTag("div", content);

        public static IHtmlContent Div(Object[] content, String style) => EmailHtml.SimpleTag("div", content, style);

        public static IHtmlContent Center(IHtmlContent content) => EmailHtml.SimpleTag("center", content, null);

        public static IHtmlContent Center(String text) => EmailHtml.SimpleTag("center", text, null);

        public static IHtmlContent Center(IHtmlContent content, String style) => EmailHtml.SimpleTag("center", content, style);

        public static IHtmlContent Center(String text, String style) => EmailHtml.SimpleTag("center", text, style);

        public static IHtmlContent Center(params Object[] content) => EmailHtml.SimpleTag("center", content);

        public static IHtmlContent CenterWithStyle(String style, params Object[] content) => EmailHtml.SimpleTag("center", content, style);

        public static IHtmlContent LineBreak()
        {
            return new TagBuilder("br") { TagRenderMode = TagRenderMode.SelfClosing };
        }

        public static IHtmlContent UnorderedList(params IHtmlContent[] items)
        {
            var tag = new TagBuilder("ul");
            foreach (var item in items)
            {
                var listTag = new TagBuilder("li");
                listTag.InnerHtml.AppendHtml(item);
                tag.InnerHtml.AppendHtml(listTag);
            }

            return tag;
        }

        public static IHtmlContent UnorderedList(params String[] items)
        {
            var tag = new TagBuilder("ul");
            foreach (var item in items)
            {
                var listTag = new TagBuilder("li");
                listTag.InnerHtml.Append(item);
                tag.InnerHtml.AppendHtml(listTag);
            }

            return tag;
        }

        public static IHtmlContent OrderedList(params IHtmlContent[] items)
        {
            var tag = new TagBuilder("ol");
            foreach (var item in items)
            {
                var listTag = new TagBuilder("li");
                listTag.InnerHtml.AppendHtml(item);
                tag.InnerHtml.AppendHtml(listTag);
            }

            return tag;
        }

        public static IHtmlContent OrderedList(params String[] items)
        {
            var tag = new TagBuilder("ol");
            foreach (var item in items)
            {
                var listTag = new TagBuilder("li");
                listTag.InnerHtml.Append(item);
                tag.InnerHtml.AppendHtml(listTag);
            }

            return tag;
        }

        public static IHtmlContent EmptyParagraph()
        {
            var tag = new TagBuilder("p");
            tag.InnerHtml.AppendHtml("&nbsp;");
            return tag;
        }

        public static IHtmlContent Centered(params Object[] content)
        {
            var table = new TagBuilder("table");
            table.Attributes.Add("width", "100%");
            table.Attributes.Add("align", "center");
            table.Attributes.Add("border", "0");
            table.Attributes.Add("cellpadding", "0");
            table.Attributes.Add("cellspacing", "0");
            var tbody = new TagBuilder("tbody");

            foreach (var contentItem in content)
            {
                if (contentItem is IHtmlContent htmlContent)
                {
                    var tr = new TagBuilder("tr");
                    var td = new TagBuilder("td");
                    td.Attributes.Add("align", "center");
                    td.InnerHtml.AppendHtml(htmlContent);
                    tr.InnerHtml.AppendHtml(td);
                    tbody.InnerHtml.AppendHtml(tr);
                }
                else if (contentItem is String stringContent)
                {
                    var tr = new TagBuilder("tr");
                    var td = new TagBuilder("td");
                    td.Attributes.Add("align", "center");
                    td.InnerHtml.Append(stringContent);
                    tr.InnerHtml.AppendHtml(td);
                    tbody.InnerHtml.AppendHtml(tr);
                }
            }

            table.InnerHtml.AppendHtml(tbody);
            return table;
        }

        public static IHtmlContent CenteredBox(String style, Int32 borderSize, params Object[] content)
        {
            return EmailHtml.CenteredBox(style, borderSize, contentAlign: "center", content);
        }

        public static IHtmlContent CenteredBox(String style, Int32 borderSize, String contentAlign = "center", params Object[] content)
        {
            var table = new TagBuilder("table");
            table.Attributes.Add("width", "100%");
            table.Attributes.Add("style", style);
            table.Attributes.Add("align", "center");
            table.Attributes.Add("border", borderSize.ToString());
            table.Attributes.Add("cellpadding", "10");
            table.Attributes.Add("cellspacing", "0");
            var tbody = new TagBuilder("tbody");

            foreach (var contentItem in content)
            {
                if (contentItem is IHtmlContent htmlContent)
                {
                    var tr = new TagBuilder("tr");
                    var td = new TagBuilder("td");
                    td.Attributes.Add("align", contentAlign);
                    td.InnerHtml.AppendHtml(htmlContent);
                    tr.InnerHtml.AppendHtml(td);
                    tbody.InnerHtml.AppendHtml(tr);
                }
                else if (contentItem is String stringContent)
                {
                    var tr = new TagBuilder("tr");
                    var td = new TagBuilder("td");
                    td.Attributes.Add("align", contentAlign);
                    td.InnerHtml.Append(stringContent);
                    tr.InnerHtml.AppendHtml(td);
                    tbody.InnerHtml.AppendHtml(tr);
                }
            }

            table.InnerHtml.AppendHtml(tbody);
            return table;
        }

        public static IHtmlContent SimpleTable(params Object[] content)
        {
            var table = new TagBuilder("table");
            var tbody = new TagBuilder("tbody");
            var tr = new TagBuilder("tr");

            foreach (var contentItem in content)
            {
                if (contentItem is IHtmlContent htmlContent)
                {
                    var tag = new TagBuilder("td");
                    tag.InnerHtml.AppendHtml(htmlContent);
                    tr.InnerHtml.AppendHtml(tag);
                }
                else if (contentItem is String stringContent)
                {
                    var tag = new TagBuilder("td");
                    tag.InnerHtml.Append(stringContent);
                    tr.InnerHtml.AppendHtml(tag);
                }
            }

            tbody.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(tbody);
            return table;
        }

        public static IHtmlContent SimpleAlignedTable(String tableAlign = "center", String style = "", params Object[] content)
        {
            var table = new TagBuilder("table");
            table.Attributes.Add("align", tableAlign);
            if (!String.IsNullOrEmpty(style))
            {
                table.Attributes.Add("style", style);
            }

            var tbody = new TagBuilder("tbody");
            var tr = new TagBuilder("tr");

            foreach (var contentItem in content)
            {
                if (contentItem is IHtmlContent htmlContent)
                {
                    var tag = new TagBuilder("td");
                    tag.InnerHtml.AppendHtml(htmlContent);
                    tr.InnerHtml.AppendHtml(tag);
                }
                else if (contentItem is String stringContent)
                {
                    var tag = new TagBuilder("td");
                    tag.InnerHtml.Append(stringContent);
                    tr.InnerHtml.AppendHtml(tag);
                }
            }

            tbody.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(tbody);
            return table;
        }

        private static IHtmlContent SimpleTag(String tagName, IHtmlContent content, String style)
        {
            var tag = new TagBuilder(tagName);
            if (!String.IsNullOrEmpty(style))
            {
                tag.Attributes.Add("style", style);
            }

            tag.InnerHtml.AppendHtml(content);
            return tag;
        }

        private static IHtmlContent SimpleTag(String tagName, String text, String style)
        {
            var tag = new TagBuilder(tagName);
            if (!String.IsNullOrEmpty(style))
            {
                tag.Attributes.Add("style", style);
            }

            tag.InnerHtml.Append(text);
            return tag;
        }

        private static IHtmlContent SimpleTag(String tagName, Object[] content, String style = null)
        {
            var tag = new TagBuilder(tagName);
            if (!String.IsNullOrEmpty(style))
            {
                tag.Attributes.Add("style", style);
            }

            foreach (var contentItem in content)
            {
                if (contentItem is IHtmlContent htmlContent)
                {
                    tag.InnerHtml.AppendHtml(htmlContent);
                }
                else if (contentItem is String stringContent)
                {
                    tag.InnerHtml.Append(stringContent);
                }
            }

            return tag;
        }
    }
}
