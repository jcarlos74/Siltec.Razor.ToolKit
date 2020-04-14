using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Siltec.WebComponents.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading.Tasks;

namespace Siltec.WebComponents.TagHelpers.DataGrid
{
    [HtmlTargetElement("data-grid")]
    public class DataGridTagHelper : TagHelper
    {
        #region Atributos da Tag Principal

        private const string IdAttributeName = "id";
        private const string AjaxUrlAttributeName = "ajax-url";
        private const string ThemaAttributeName = "thema";
        private const string HeightAttributeName = "height";
        private const string WidthAttributeName = "width";
        private const string EnableColResizeAttributeName = "enableColResize";
        private const string EnableFilterAttributeName = "enableFilter";
        private const string RowSelectionAttibuteName = "rowSelection";
        private const string PaginationAttibuteName = "pagination";
        private const string EditableAttibuteName = "true";
        private const string CheckBoxSelectionAttributeName = "checkboxSelection";
        private const string SuppresRowClickSelectionAttributeName = "suppressRowClickSelection";
        private const string KeyDataSourceName = "keyDataSource";

        private const string ShowButtonEditName = "show-button-edit";
        private const string ButtonEditHintName = "button-edit-hint";
        private const string ButtonEditIconName = "button-edit-icon";
        private const string ButtonEditCssName = "button-edit-css";
        private const string ButtonEditName = "button-edit-icon";
        private const string ButtonEditFunctionName = "button-edit-function-name";

        private const string ShowButtonDeleteName = "show-button-delete";
        private const string ButtonDeleteHintName = "button-delete-hint";
        private const string ButtonDeleteCssName = "button-delete-css";
        private const string ButtonDeleteIconName = "button-edit-icon";
        private const string ButtonDeleteFunctionName = "button-edit-function-name";

        // private const string FieldNameAttributeName = "field";

        private const string ModelTypeName = "model-type";
        private const string JsonDataName = "json-data";

        #endregion

        private readonly IModelMetadataProvider _modelMetadataProvider;


        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(KeyDataSourceName)]
        public string KeyDataSource { get; set; }

        [HtmlAttributeName(ThemaAttributeName)]
        public string Thema { get; set; }

        [HtmlAttributeName(HeightAttributeName)]
        public string Heigh { get; set; } = "100px";

        [HtmlAttributeName(WidthAttributeName)]
        public string Width { get; set; }

        [HtmlAttributeName(EnableColResizeAttributeName)]
        public bool EnableColResize { get; set; } = true;

        [HtmlAttributeName(EnableFilterAttributeName)]
        public bool EnableFilter { get; set; }

        [HtmlAttributeName(RowSelectionAttibuteName)]
        public string RowSelection { get; set; } 

        [HtmlAttributeName(PaginationAttibuteName)]
        public bool Pagination { get; set; }

        [HtmlAttributeName(EditableAttibuteName)]
        public bool Editable { get; set; }

        [HtmlAttributeName(CheckBoxSelectionAttributeName)]
        public bool CheckBoxSelection { get; set; }

        [HtmlAttributeName(SuppresRowClickSelectionAttributeName)]
        public bool SuppresRowClickSelection { get; set; }

        [HtmlAttributeName(AjaxUrlAttributeName)]
        public string AjaxUrl { get; set; }

        [HtmlAttributeName(ModelTypeName)]
        public Type ModelType { get; set; }

        [HtmlAttributeName("asp-model")]
        public ModelExpression Model { get; set; }

        [HtmlAttributeName(ShowButtonEditName)]
        public bool ShowButtonEdit { get; set; } = true;


        [HtmlAttributeName(ShowButtonDeleteName)]
        public bool ShowButtonDelete { get; set; } = true;

        [HtmlAttributeName(ButtonEditHintName)]
        public string ButtonEditHint { get; set; } = "Editar";

        [HtmlAttributeName(ButtonEditIconName)]
        public string ButtonEditIcon { get; set; } = "edit";

        [HtmlAttributeName(ButtonEditCssName)]
        public string ButtonEditCss { get; set; }

        [HtmlAttributeName(ButtonEditFunctionName)]
        public string ButtonEditFunction { get; set; } = "EditarRegistro(e);";

        [HtmlAttributeName(ButtonDeleteHintName)]
        public string ButtonDeletetHint { get; set; } = "Excluir";

        [HtmlAttributeName(ButtonDeleteIconName)]
        public string ButtonDeleteIcon { get; set; } = "trash";

        [HtmlAttributeName(ButtonDeleteCssName)]
        public string ButtonDeleteCss { get; set; } = "button-red";

        [HtmlAttributeName(ButtonDeleteFunctionName)]
        public string ButtonDeleteFunction { get; set; } = "ExcluirCancelarRegistro(e);";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null || output == null) throw new ArgumentNullException();

            Contract.EndContractBlock();

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("id", this.Id);

            var classeCss = string.IsNullOrEmpty( this.Thema) ? string.Empty : this.Thema + " " ;

            classeCss += "dt-grid";

            output.Attributes.SetAttribute("class", classeCss);

            //var altura = this.Heigh.Replace(";", string.Empty);
            //var largura = this.Width.Replace(";", string.Empty);

            //var estiloDivContainer = $"height:{altura};width:{largura};";

            //output.Attributes.SetAttribute("style", estiloDivContainer);

            output.Attributes.Add("data-ajax-url", AjaxUrl);
            output.Attributes.Add("data-enabled-col-resize", EnableColResize);
            output.Attributes.Add("data-key-datasource", KeyDataSource);
            output.Attributes.Add("data-show-button-edit", ShowButtonEdit);
            output.Attributes.Add("data-show-button-delete", ShowButtonDelete);

            output.Attributes.Add("data-button-edit-hint", ButtonEditHint);
            output.Attributes.Add("data-button-edit-icon", ButtonEditIcon);
            output.Attributes.Add("data-button-edit-function-name", ButtonEditFunction);

            output.Attributes.Add("data-button-delete-hint", ButtonDeletetHint);
            output.Attributes.Add("data-button-delete-icon", ButtonDeleteIcon);
            output.Attributes.Add("data-button-delete-function-name", ButtonDeleteFunction);
            output.Attributes.Add("data-button-delete-css", ButtonDeleteCss);

            var tableContext = await GetTableContextAsync(context, output);

            //tableContext.m = new List<ModelExpression>();
            if (tableContext.ModelExplorer?.Properties != null)
            {
                tableContext.ColumnsProperties = new List<DataGridColumnProperties>();

                foreach (var columnProperty in tableContext.ModelExplorer.Properties)
                {
                    var columnName = columnProperty.Metadata.PropertyName;

                    var gridColumn = columnProperty.Metadata.ContainerType.GetProperty(columnName).GetCustomAttribute(typeof(GridColumnAttribute)) as GridColumnAttribute;

                    if (gridColumn != null)
                    {
                        var column = new DataGridColumnProperties();

                        column.dataField = gridColumn.FieldName;
                        column.caption = gridColumn.HeaderText;

                        if (gridColumn.Width > -1)
                            column.width = gridColumn.Width;

                        column.visible = gridColumn.Visible;

                        tableContext.ColumnsProperties.Add(column);
                    }
                }
            }

            if (tableContext.ColumnsProperties?.Count > 0)
            {
                string jsonResult = JsonConvert.SerializeObject(tableContext.ColumnsProperties);

                output.Attributes.Add("data-columns", jsonResult);
            }
        }

        private string GetFilterType(enumFilterType filterType)
        {
            if (filterType != enumFilterType.Nenhum)
            {
                switch (filterType)
                {
                    case enumFilterType.Text:
                        return "agTextColumnFilter";
                    case enumFilterType.Number:
                        return "agNumberColumnFilter";
                    case enumFilterType.Date:
                        return "agDateColumnFilter";
                    default:
                        return string.Empty;
                }
            }

            return string.Empty;
        }

        private string GetFreezePostion(enumFreezePosition freezePosition)
        {
            if (freezePosition != enumFreezePosition.Nenhum)
            {
                return freezePosition.ToString().ToLower();
            }

            return string.Empty;

        }

        private async Task<DataGridContext> GetTableContextAsync(TagHelperContext context, TagHelperOutput output)
        {
            Contract.Requires(context != null && output != null);

            var tableContext = new DataGridContext { ModelExplorer = Model.ModelExplorer };  

            context.Items.Add(typeof(DataGridTagHelper), tableContext);

            await output.GetChildContentAsync();

            return tableContext;
        }

        private ModelExplorer GetModelExplorer(Type modelType)
        {
            if (this.Model != null)
            {
                return _modelMetadataProvider.GetModelExplorerForType(this.Model.GetType(), Model); /// ModelType, null);
            }
            return null;
        }
    }
}
