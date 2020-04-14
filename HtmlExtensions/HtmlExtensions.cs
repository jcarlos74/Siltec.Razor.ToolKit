using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Siltec.WebComponents.HtmlExtensions
{
    public static class HtmlExtensions
    {
        public static IHtmlContent DescriptionFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var modelExplorer = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider);
            if (modelExplorer == null) throw new InvalidOperationException($"Falha ao obter o model explorer para {ExpressionHelper.GetExpressionText(expression)}");

            return new HtmlString(modelExplorer.Metadata.Description);
        }
    }
}
