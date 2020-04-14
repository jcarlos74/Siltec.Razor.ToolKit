using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Siltec.WebComponents.TagHelpers.BreadCrumbs
{
    [HtmlTargetElement("smart-breadcrumb")]
    public class BreadCrumbTagHelper : TagHelper
    {
        /// <summary>
        /// Icone a ser exibido no primeiro nivel(Área) do BreadCrumb 
        /// </summary>
        [HtmlAttributeName("area-glyphicon")]
        public string AreaGlyphIcon { set; get; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected HttpRequest Request => ViewContext.HttpContext.Request;

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
           
            var currentFullUrl = Request.GetEncodedUrl();
            var currentRouteUrl = getCurrentRouteUrl();

            string area = (ViewContext.RouteData.Values["area"] ?? "").ToString();

            var tagUl = new TagBuilder("ul");

            if (!string.IsNullOrEmpty(area))
            {

                tagUl.AddCssClass("breadcrumb");

                //Monta a tag da Área - 1º Nivel do breadcrumb
                var tagLiArea = new TagBuilder("li");

                if (!string.IsNullOrWhiteSpace(AreaGlyphIcon))
                {
                    tagLiArea.InnerHtml.AppendHtml($"<i class='{AreaGlyphIcon}'></i> ");
                }

                tagLiArea.InnerHtml.AppendHtml($"<a href='#'>");

                tagLiArea.InnerHtml.AppendHtml($"{area}"); //Nome da área

                tagLiArea.InnerHtml.AppendHtml("</a>");

                tagUl.InnerHtml.AppendHtml(tagLiArea);

                var nestedItens = currentRouteUrl.Replace("/", ";").Split(';');

                var qtdItens = nestedItens.Count();

                for (int i = 0; i < qtdItens ; i++)
                {
                    if (!string.IsNullOrEmpty(nestedItens[i]))
                    {
                        var tagLi = new TagBuilder("li");
                                                
                        if ( i == qtdItens -1)
                        {
                            tagLi.AddCssClass("active");
                            tagLi.InnerHtml.AppendHtml($"{nestedItens[i]}"); //Descrição do item
                        }
                        else
                        {
                            tagLi.InnerHtml.AppendHtml($"<a href='#'>");

                            tagLi.InnerHtml.AppendHtml($"{nestedItens[i]}"); //Descrição do item

                            tagLi.InnerHtml.AppendHtml("</a>");
                        }

                        tagUl.InnerHtml.AppendHtml(tagLi);
                    }
                }
            }

            //Div principal
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "page-breadcrumbs" );

            output.Content.AppendHtml(tagUl);
        }

        private string getCurrentRouteUrl()
        {
            var routeValues = ViewContext.ActionDescriptor.RouteValues;

            if (routeValues.TryGetValue("action", out var action))
            {
                var urlHelper = ViewContext.HttpContext.Items.Values.OfType<IUrlHelper>().FirstOrDefault();

                if (urlHelper == null)
                {
                    throw new NullReferenceException("Falha ao localizar o IUrlHelper do ViewContext.HttpContext.");
                }

                return urlHelper.Action(action);
            }

            if (routeValues.TryGetValue("page", out var page))
            {
                return page;
            }

            return string.Empty;
        }
    }
}
