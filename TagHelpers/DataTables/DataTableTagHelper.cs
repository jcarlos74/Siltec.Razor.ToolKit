using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Siltec.WebComponents.ComponentModel;

namespace Siltec.WebComponents.TagHelpers.DataTables
{
    [HtmlTargetElement("data-table")]
    [RestrictChildren("data-table-columns")]
    public class DataTableTagHelper : TagHelper
    {
        private const string DataUrlName = "url-data";
        private const string ProcessingName = "processing";
        private const string ServerSideName = "server-side";
        private const string SearchDelayName = "search-delay";
        private const string QueryIdsName = "query-ids";
        private const string ModelTypeName = "model-type";
        private const string TableIdName = "table-id";
        private const string JsonDataName = "json-data";

        private readonly IModelMetadataProvider _modelMetadataProvider;
        //private readonly IStringLocalizer _localizer;

        [HtmlAttributeName(DataUrlName)]
        public string DataUrl { get; set; }

        [HtmlAttributeName(ProcessingName)]
        public bool Processing { get; set; } = true;

        [HtmlAttributeName(ServerSideName)]
        public bool ServerSide { get; set; } = true;

        [HtmlAttributeName(SearchDelayName)]
        public int SearchDelay { get; set; } = 1000;

        [HtmlAttributeName(QueryIdsName)]
        public string QueryIds { get; set; }

        [HtmlAttributeName(ModelTypeName)]
        public Type ModelType { get; set; }

        [HtmlAttributeName(TableIdName)]
        public string TableId { get; set; } = "dataTable";

        [HtmlAttributeName(JsonDataName)]
        public string JsonData { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null || output == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            output.TagName = "table";
            output.TagMode = TagMode.StartTagAndEndTag;

            var tableContext = await GetTableContextAsync(context, output);

            var classeCss = this.Class;

            classeCss += " dt-table";

            output.Attributes.Add("id", this.TableId);
            output.Attributes.Add("class", classeCss);
            output.Attributes.Add("cellspacing", "0");
            output.Attributes.Add("width", "100%");

            output.Attributes.Add("data-id", this.TableId);
            output.Attributes.Add("data-url", this.DataUrl);
            output.Attributes.Add("data-server-side", this.ServerSide.ToString().ToLower());


            if (tableContext.ModelExplorer?.Properties != null)
            {
                //    output.Content.AppendHtml("<thead>");
                //    output.Content.AppendHtml("<tr>");


                if (tableContext.ColumnsProperties == null)
                {
                    tableContext.ColumnsProperties = new List<DataTableColumnsProperties>();
                }

                foreach (var columnProperty in tableContext.ModelExplorer.Properties)
                {
                    var columnName = columnProperty.Metadata.PropertyName;

                    var gridColumn = columnProperty.Metadata.ContainerType.GetProperty(columnName).GetCustomAttribute(typeof(GridColumnAttribute)) as GridColumnAttribute;

                    //if (gridColumn != null)
                    //{
                    //    output.Content.AppendHtml($"<th>{gridColumn.HeaderText}</th>");
                    //}

                    //Alterar para propriedades do DataTable
                    tableContext.ColumnsProperties.Add(new DataTableColumnsProperties()
                    {
                        data = gridColumn.FieldName,
                        name = gridColumn.FieldName,
                        title = gridColumn.HeaderText,
                        //width = gridColumn.Width,
                        visible = gridColumn.Visible.ToString().ToLower(),
                        render = RenderValue(columnProperty.Metadata.ModelType)
                    });
                }

                string jsonResult = JsonConvert.SerializeObject(tableContext.ColumnsProperties,Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore,StringEscapeHandling = StringEscapeHandling.EscapeHtml });

                output.Attributes.Add("data-columns", jsonResult);

               // output.Content.AppendHtml("</tr>");
              //  output.Content.AppendHtml("</thead>");
            }

         //   TableHeader(output, tableContext);
         //criar    TableBody(output, tableContext);

            
         //   AppendTableHtml(sb, tableContext, context);

            //sb.AppendLine("<script type=\"text/javascript\">");

            //var initializeFunctionName = "initializeFunction";
            //AppendInitializeFunction(sb, tableContext, initializeFunctionName);

            //sb.AppendLine($"{initializeFunctionName}();");
            //sb.AppendLine("</script>");

          

           // output.Content.AppendHtml(sb.ToString());
        }

        private string RenderValue(Type modelType)
        {
            var sbFormataDados = new StringBuilder();

            if (modelType == typeof(DateTime) || modelType == typeof(DateTime?))
            {
                sbFormataDados.AppendLine(" function(data){");
                sbFormataDados.AppendLine("if (data == null) return data;");
                sbFormataDados.AppendLine("var dt = new Date(data);");
                sbFormataDados.AppendLine("return dt.toLocaleString();");
                sbFormataDados.AppendLine("}");
            }
            else if (modelType == typeof(decimal) || modelType == typeof(decimal?))
            {
                sbFormataDados.AppendLine(" function(data){");
                sbFormataDados.AppendLine("return data.toLocaleString();");
                sbFormataDados.AppendLine("}");
            }
            else
            {
                sbFormataDados.AppendLine(" $.fn.dataTable.render.text()");
            }

            return sbFormataDados.ToString();
        }

        private void TableHeader(TagHelperOutput output, DataTableContext tableContext)
        {
            output.Content.AppendHtml("<thead>");
            output.Content.AppendHtml("<tr>");

            //if (tableContext.ColumnsProperties != null)
            //{
            //    foreach (var columnProperty in tableContext.ColumnsProperties)
            //    {
            //        output.Content.AppendHtml($"<th>{columnProperty.Metadata?.DisplayName}</th>");
            //    }
            //}

            output.Content.AppendHtml("</tr>");
            output.Content.AppendHtml("</thead>");
        }

        //private void TableBody(TagHelperOutput output, DataTableContext tableContext)
        //{
        //    output.Content.AppendHtml("<tbody>");

        //    foreach (var item in Items)
        //    {
        //        output.Content.AppendHtml("<tr>");
        //        foreach (var prop in props)
        //        {
        //            var value = GetPropertyValue(prop, item);
        //            output.Content.AppendHtml($"<td>{value}</td>");
        //        }
        //        output.Content.AppendHtml("</tr>");
        //    }

        //    output.Content.AppendHtml("</tbody>");
        //}
        private void AppendInitializeFunction(StringBuilder sb, DataTableContext tableContext, string initializeFunctionName)
        {
            sb.AppendLine("function " + initializeFunctionName + "(){");
            sb.AppendLine($"$('#{TableId}').DataTable({{");

            var localizationUrl = "http://cdn.datatables.net/plug-ins/1.10.13/i18n/Portuguese-Brasil.json"; // _localizer["LocalizationUrl"];

            if (!string.IsNullOrEmpty(localizationUrl))
            {
                sb.AppendLine($"language: {{url: \"{localizationUrl}\"}},");
            }

            if (ServerSide)
            {
                AppendServerSideProcessingData(sb);

            }
            else if (!string.IsNullOrEmpty(JsonData))
            {
                AppendData(sb);
            }
            else
            {
                throw new NotImplementedException();
            }

            //AppendColumns(sb, tableContext);

            sb.AppendLine("});");
            sb.AppendLine("}");
        }


        public DataTableTagHelper(IModelMetadataProvider modelMetadataProvider)//, IStringLocalizer<DataTableTagHelper> localizer)
        {
            //if (modelMetadataProvider == null || localizer == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _modelMetadataProvider = modelMetadataProvider;
            //_localizer = localizer;
        }


        private async Task<DataTableContext> GetTableContextAsync(TagHelperContext context, TagHelperOutput output)
        {
            Contract.Requires(context != null && output != null);

            var tableContext = new DataTableContext { ModelExplorer = GetModelExplorer(ModelType) };
            context.Items.Add(typeof(DataTableTagHelper), tableContext);

            await output.GetChildContentAsync();

            return tableContext;
        }

        private void AppendTableHtml(StringBuilder sb, DataTableContext tableContext, TagHelperContext context)
        {
            Contract.Requires(sb != null && tableContext != null && context != null);

            var classes = "";
            if (context.AllAttributes.TryGetAttribute("class", out TagHelperAttribute classTag))
            {
                classes = classTag.Value.ToString();
            }

            classes += " dt-table";

            sb.AppendLine($"<table id=\"{TableId}\" class=\"{classes}\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            if (tableContext.ColumnsProperties != null)
            {
                //foreach (var columnProperty in tableContext.ColumnsProperties)
                //{
                //    sb.AppendLine($"<th>{columnProperty.Metadata?.DisplayName}</th>");
                //}
            }
            if (tableContext.ActionDataSet != null && tableContext.ActionDataSet.Any())
            {
                sb.AppendLine("<th></th>");
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("</table>");
        }

        private void AppendServerSideProcessingData(StringBuilder sb)
        {
            Contract.Requires(sb != null);

            sb.AppendLine("serverSide: true,");
            sb.AppendLine($"processing: {Processing.ToString().ToLower()},");
            sb.AppendLine($"searchDelay: {SearchDelay},");
            sb.AppendLine("ajax: {");
            sb.AppendLine("contentType: \"json\",");
            sb.AppendLine($"url:\"{DataUrl}\",");
            sb.AppendLine("data: function (params) {");
            sb.AppendLine("return {");
            sb.AppendLine("draw: params.draw,");
            sb.AppendLine("start: params.start,");
            sb.AppendLine("pageSize: params.length,");
            sb.AppendLine("term: params.search.value,");
            sb.AppendLine("orderField: params.columns[params.order[0].column].data,");
            sb.AppendLine("orderDirection: params.order[0].dir,");
            if (QueryIds != null)
            {
                sb.AppendLine($"queryIds: \"{QueryIds}\",");
            }
            sb.AppendLine("};},},");
        }

        private void AppendData(StringBuilder sb)
        {
            Contract.Requires(sb != null);

            sb.AppendLine("searching: false,");
            sb.AppendLine("paging: false,");
            sb.AppendLine("ordering: false,");
            sb.AppendLine("info: false,");
            sb.AppendLine($"aaData: {JsonData},");
        }

        //private void AppendColumns(StringBuilder sb, DataTableContext tableContext)
        //{
        //    Contract.Requires(sb != null && tableContext != null);

        //    sb.AppendLine("columns: [");
        //    if (tableContext.ColumnsProperties != null)
        //    {
        //        foreach (var columnProperty in tableContext.ColumnsProperties)
        //        {
        //            var columnName = Char.ToLowerInvariant(columnProperty.Name[0]) + columnProperty.Name.Substring(1);
        //            if (columnProperty.Metadata.ModelType == typeof(DateTime) || columnProperty.Metadata.ModelType == typeof(DateTime?))
        //            {
        //                sb.AppendLine($"{{ \"data\": \"{columnName}\",");
        //                sb.AppendLine("\"render\": function(data){");
        //                sb.AppendLine("if (data == null) return data;");
        //                sb.AppendLine("var d = new Date(data);");
        //                sb.AppendLine("return d.toLocaleString();");
        //                sb.AppendLine("}},");
        //            }
        //            else if (columnProperty.Metadata.ModelType == typeof(decimal) || columnProperty.Metadata.ModelType == typeof(decimal?))
        //            {
        //                sb.AppendLine($"{{ \"data\": \"{columnName}\",");
        //                sb.AppendLine("\"render\": function(data){");
        //                sb.AppendLine("return data.toLocaleString();");
        //                sb.AppendLine("}},");
        //            }
        //            else
        //            {
        //                sb.AppendLine($"{{ \"data\": \"{columnName}\",");
        //                sb.AppendLine("render: $.fn.dataTable.render.text()},");
        //            }
        //        }
        //    }
        //    if (tableContext.ActionDataSet != null && tableContext.ActionDataSet.Any())
        //    {
        //        sb.AppendLine(GetColumnActionContent(tableContext.ActionDataSet));
        //    }
        //    sb.AppendLine("],");
        //}
        
        private string GetColumnActionContent(List<ActionData> actionDataSet)
        {
            Contract.Requires(actionDataSet != null);

            var sb = new StringBuilder();
            sb.AppendLine("{ \"sortable\" : false,");
            sb.AppendLine("\"render\" : function(data, type, row){");
            sb.AppendLine("var action = ''");

            foreach (var actionData in actionDataSet)
            {
                if (actionData != null)
                {
                    if (!string.IsNullOrEmpty(actionData.CanExecuteProperty))
                    {
                        sb.Append($"if(row['{actionData.CanExecuteProperty}'] == true){{ action = action + '");
                    }
                    else
                    {
                        sb.Append("{ action = action + '");
                    }

                    if (actionData.ActionUrl.Contains("id"))
                    {
                        var url = actionData.ActionUrl.Replace("id", "' + row['id'] + '");
                        sb.Append($"<a href={url}>{actionData.ActionTitle}</a>");
                    }
                    else
                    {
                        sb.Append($"<a href={actionData.ActionUrl}/' + row['id'] + '>{actionData.ActionTitle}</a>");
                    }

                    if (actionDataSet.IndexOf(actionData) < actionDataSet.Count - 1)
                    {
                        sb.Append(" | ");
                    }

                    sb.Append("';}");
                }
            }
            sb.Append("return action;");
            sb.Append("}}");
            return sb.ToString();
        }

        private ModelExplorer GetModelExplorer(Type modelType)
        {
            if (modelType != null)
            {
                return _modelMetadataProvider.GetModelExplorerForType(ModelType, null);
            }
            return null;
        }
    }
}


