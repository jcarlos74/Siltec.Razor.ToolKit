using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Siltec.WebComponents.TagHelpers.PageHeader
{
    [HtmlTargetElement("smart-page-header")]
    public class PageHeader : TagHelper
    {
        /// <summary>
        /// Título do Form
        /// </summary>
        [HtmlAttributeName("header-title")]
        public string HeaderTitle { set; get; }

        /// <summary>
        /// Se True indica que o botão toolge sidebar será exibido - Defatul true
        /// </summary>
        [HtmlAttributeName("show-button-toogle-sidebar")]
        public bool ShowButtonToogleSideBar { get; set; } = true;

        /// <summary>
        ///  Se True indica que o botão refresh será exibido - Defatul true
        /// </summary>
        [HtmlAttributeName("show-button-refresh")]
        public bool ShowButtonRefresh { get; set; } = true;

        /// <summary>
        ///  Se True indica que o botão fullscreen será exibido - Defatul true
        /// </summary>
        [HtmlAttributeName("show-button-fullscreen")]
        public bool ShowButtonFullScreen { get; set; } = true;

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //div principal
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "page-header position-relative");

            var sb = new StringBuilder();

            var divHeaderTitle = new TagBuilder("div");

            divHeaderTitle.AddCssClass("header-title");
            divHeaderTitle.InnerHtml.AppendHtml("<h1>");
            divHeaderTitle.InnerHtml.AppendHtml($"{HeaderTitle}");
            divHeaderTitle.InnerHtml.AppendHtml("</h1>");

            //sb.Append(TagHelpersUtils.HtmlContentToString(divHeaderTitle));

            //Header Buttons
            var divHeaderButtons = new TagBuilder("div");

            divHeaderButtons.AddCssClass("header-buttons");

            if (ShowButtonToogleSideBar)
            {
                divHeaderButtons.InnerHtml.AppendHtml("<a class='sidebar-toggler' href='#'>");
                divHeaderButtons.InnerHtml.AppendHtml("<i class='fa fa-arrows-h'></i> ");
                divHeaderButtons.InnerHtml.AppendHtml("</a>");
            }

            if (ShowButtonRefresh)
            {
                divHeaderButtons.InnerHtml.AppendHtml("<a class='refresh' id='refresh-toggler' href='#'>");
                divHeaderButtons.InnerHtml.AppendHtml("<i class='glyphicon glyphicon-refresh'></i> ");
                divHeaderButtons.InnerHtml.AppendHtml("</a>");
            }

            if (ShowButtonFullScreen)
            {
                divHeaderButtons.InnerHtml.AppendHtml("<a class='fullscreen' id='fullscreen-toggler' href='#'>");
                divHeaderButtons.InnerHtml.AppendHtml("<i class='glyphicon glyphicon-fullscreen'></i> ");
                divHeaderButtons.InnerHtml.AppendHtml("</a>");
            }

            //sb.Append(TagHelpersUtils.HtmlContentToString(divHeaderButtons.InnerHtml));

            output.Content.AppendHtml(divHeaderTitle);
            output.Content.AppendHtml(divHeaderButtons);
        }


    }
}
