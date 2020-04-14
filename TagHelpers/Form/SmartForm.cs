using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siltec.WebComponents.TagHelpers.Form
{

    public enum ToolBarType
    {
        Pesquisa,
        Cadastro
    }

    [HtmlTargetElement("smart-form")]
   // [RestrictChildren("smart-form-toolbar", "smart-form-body")]
    public class SmartFormTagHelper : TagHelper
    {
        // <summary>
        /// Id do SmartForm
        /// </summary>
        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var smartFormContext = new SmartFormContext();

            context.Items.Add(typeof(SmartFormTagHelper), smartFormContext);

            await output.GetChildContentAsync();


            var template = $@"<div class='widget' id='{this.Id}' >";

            output.Content.AppendHtml(template);

            //ToolBar
            if (smartFormContext.ToolBar != null)
            {
                foreach (var item in smartFormContext.ToolBar)
                {
                    output.Content.AppendHtml(item);
                }
               // output.Content.AppendHtml(smartFormContext.ToolBar);
            }

            //Body
            if (smartFormContext.Body != null)
            {
                output.Content.AppendHtml(smartFormContext.Body);
            }

            output.Content.AppendHtml("</div></div>");
        }
    }

    [HtmlTargetElement("smart-form-toolbar", ParentTag = "smart-form")]
    public class SmartFormToolBar : TagHelper
    {
        public string Id { get; set; }

        [HtmlAttributeName("toolbar-type")]
        public ToolBarType TipoToolBar { get; set; }

        [HtmlAttributeName("caption-button-new")]
        public string CaptionButtonNew { get; set; }

        [HtmlAttributeName("toolbar-visible")]
        public bool Visible { get; set; } = true;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var toolBarContent = new DefaultTagHelperContent();

            
            if ( TipoToolBar == ToolBarType.Pesquisa )
            {
                var div = this.Visible ? $@"<div class='widget-header' id='{Id}'>" : $@"<div class='widget-header' id='{Id}'style ='display: none;'>";

                // oninput='onFilterTextBoxChanged()'

                var template =
                    $@"{div}
                          <div class='widget-buttons pull-left'>
                                <span class='input-icon'>
                                   <input type = 'text' class='form-control input-xs' style='margin-top: 5px;' id = 'txtPesquisa'  placeholder='Pesquisar'> 
                                      <i class='glyphicon glyphicon-search blue'  style='margin-top: 5px;'></i>
                                   </input>
                                </span>
                           </div>
                           <div class='widget-buttons buttons-bordered pull-left'>
                               <button id='btnSmartFormNovoRegistro' title='Incluir novo registro' class='btn btn-default btn-xs'>
                                  <i class='glyphicon glyphicon-plus green'></i> {CaptionButtonNew}
                               </button>
                               <button id='btnSmartFormAtualizar' title='Atualizar' class='btn btn-default btn-xs'><i class='glyphicon glyphicon-refresh blue'></i></button>
                               <button id='btnSmartFormExportExcel' title='Exportar para Excel' class='btn btn-default btn-xs'><i class='fa fa-file-excel-o green'></i></button>
                               <button id='btnSmartFormExportPdf' title='Exportar para PDF' class='btn btn-default btn-xs'><i class='fa fa-file-pdf-o red'></i></button>
                            </div>
                           <div class='widget-buttons'>
                             <a href = '#' data-toggle='config'>
                                 <i class='fa fa-cog'></i>
                             </a>
                             <a href = '#' data-toggle='maximize'>
                                 <i class='fa fa-expand'></i>
                             </a>
                             <a href = '#' data-toggle='collapse'>
                                  <i class='fa fa-minus'></i>
                             </a>
                         </div>
                      </div>";

                toolBarContent.AppendHtml(template);
            }

            if (TipoToolBar == ToolBarType.Cadastro)
            {
                var div = this.Visible ? $@"<div class='widget-header' id='{Id}'>" : $@"<div class='widget-header' id='{Id}'style ='display: none;'>";

                var template =
                    $@"{div}
                           <div class='widget-buttons pull-left'>
                               <button id='btnSmartFormVoltar' title='Voltar' class='btn btn-default btn-xs'><i class='glyphicon glyphicon-arrow-left'></i></button>
                           </div>
                           <div class='widget-buttons buttons-bordered pull-left'>
                              
                               <button id='btnSmartFormSalvar' class='btn btn-default btn-xs'><i class='glyphicon glyphicon-ok green'></i>  Salvar</button>
                               <button id='btnSmartFormExcluir' class='btn btn-default btn-xs'><i class='glyphicon glyphicon-trash red'></i>  Excluir</button>
                            </div>
                           <div class='widget-buttons'>
                             <a href = '#' title='Configurações' data-toggle='config'>
                                 <i class='fa fa-cog'></i>
                             </a>
                             <a href = '#' title='Maximizar' data-toggle='maximize'>
                                 <i class='fa fa-expand'></i>
                             </a>
                             <a href = '#' title='Minimizar' data-toggle='collapse'>
                                  <i class='fa fa-minus'></i>
                             </a>
                         </div>
                      </div>";

                toolBarContent.AppendHtml(template);
            }

            toolBarContent.AppendHtml(childContent);

            var smarFormContext = (SmartFormContext)context.Items[typeof(SmartFormTagHelper)];

            smarFormContext.ToolBar.Add(toolBarContent);

            output.SuppressOutput();

            
        }
    }


    [HtmlTargetElement("smart-form-body", ParentTag = "smart-form")]
    public class SmartFormBody : TagHelper
    {
        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var bodyContent = new DefaultTagHelperContent();

            var template = $@"<div class='widget-body'>";

            bodyContent.AppendHtml(template);

            bodyContent.AppendHtml(childContent);

            var smarFormContext = (SmartFormContext)context.Items[typeof(SmartFormTagHelper)];

            smarFormContext.Body = bodyContent;

            output.SuppressOutput();
        }
               

    }

    public class SmartFormContext
    {
        public SmartFormContext()
        {
            ToolBar = new List<IHtmlContent>();
        }

        public List<IHtmlContent> ToolBar { get; set; }
        public IHtmlContent Body { get; set; }
    }
}
