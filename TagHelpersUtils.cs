using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.WebEncoders.Testing;
using System;
using System.IO;
using System.Text.Encodings.Web;

namespace Siltec.WebComponents
{
    public static class TagHelpersUtils
    {
        public static string HtmlContentToString(IHtmlContent content, HtmlEncoder encoder = null)
        {
            if (encoder == null)
            {
                encoder = new HtmlTestEncoder();
            }

            using (var writer = new StringWriter())
            {
                content.WriteTo(writer, encoder);
                return writer.ToString();
            }
        }
    }
}
