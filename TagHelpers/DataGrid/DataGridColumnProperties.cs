using Newtonsoft.Json;

namespace Siltec.WebComponents.TagHelpers.DataGrid
{
    public class DataGridColumnProperties
    {
        /// <summary>
        /// O nome a ser renderizado no cabeçalho da coluna. Se não especificado e o campo for especificado, o nome do campo será usado como o nome do cabeçalho.
        /// </summary>
        public string caption { get; set; }

        /// <summary>
        /// Nome do campo no banco de dados
        /// </summary>
        public string dataField { get; set; }

        public bool allowallowSearch { get; set; }
               
        //public bool sortable { get; set; }


        //public string headerClass { get; set; }

        /// <summary>
        /// Tipo de dado
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dataType { get; set; }

        /// <summary>
        /// Largura da coluna no grid
        /// </summary>
        public int width { get; set; } = 0;

        /// <summary>
        /// Controla a serelização da propriedade width, somente será serializada se for maior que -1
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializewidth()
        {
            return (width > 0);
        }

        //public int minWidth { get; set; }

        //public int maxWidth { get; set; }

        ///// <summary>
        ///// Tipo de dado do Filtro
        ///// </summary>
        //public string filter { get; set; }

        public bool visible { get; set; } = true;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string alignment { get; set; }

        ///// <summary>
        ///// Columna editável
        ///// </summary>
        //public bool editable { get; set; }

        ///// <summary>
        ///// Coluna redimensionavel
        ///// </summary>
        //public bool resizable { get; set; }

        /// <summary>
        /// Posição da coluna a ser congelada - você pode fixar colunas definindo o atributo pinned na definição de coluna como 'left'ou 'right'.
        /// </summary>
        [JsonProperty("fixed")]
        public bool Fixed { get; set; }

        public bool ShouldSerializeFixed()
        {
            return (Fixed == true);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string fixedPosition { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public bool checkboxSelection { get; set; }


        //public string colSpan { get; set; }

        //public string valueFormatter { get; set; }

        //public string cellStyle { get; set; }

        //public string toolTip { get; set; }

        //public string cellRenderer { get; set; }
    }
}
