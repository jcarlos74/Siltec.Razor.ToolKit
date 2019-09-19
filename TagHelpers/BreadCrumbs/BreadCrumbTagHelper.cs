using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web.Mvc;
using System.Web;

namespace Siltec.Razor.ToolKit.TagHelpers.BreadCrumbs
{
    [HtmlTargetElement("breadcrumb")]
    public class BreadCrumbTagHelper : TagHelper
    {
        /// <summary>
        /// Icone a ser exibido no primeiro nivel(Área) do BreadCrumb 
        /// </summary>
        [HtmlAttributeName("area-glyphicon")]
        public string AreaGlyphIcon { set; get; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        //protected HttpRequest Request => ViewContext.HttpContext.Request;

    }
}
