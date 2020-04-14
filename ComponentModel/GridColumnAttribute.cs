using System;

namespace Siltec.WebComponents.ComponentModel
{
    [AttributeUsage(System.AttributeTargets.All)]
    public class GridColumnAttribute : Attribute
    {
        /// <summary>
        /// Nome do campo no banco de dados
        /// </summary>
        public string FieldName { get; set; }

        public GridColumnAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }

        /// <summary>
        /// Titulo (cabeçalho) da coluna no grid
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        ///  Este parâmetro pode ser usado para definir a largura de uma coluna e pode assumir qualquer valor de CSS (3em, 20px etc.).
        /// </summary>
        public int Width { get; set; }


        public string Aignment { get; set; }

        //public int MinWidth { get; set; }

        //public int MaxWidth { get; set; }

        /// <summary>
        /// Indica se a coluna será ocultada
        /// </summary>
        public bool Visible { get; set; } = true;


        /// <summary>
        /// Indica se a columna é ordenavel
        /// </summary>
        public bool Sortable { get; set; } = true;

        /// <summary>
        /// Indica se a columna é editável
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Indica se a coluna é redimensionavel
        /// </summary>
        public bool Resizable { get; set; } = true;


        public bool Fixed { get; set; } = true;

        /// <summary>
        /// Posição da coluna a ser congelada - você pode fixar colunas definindo o atributo pinned na definição de coluna como 'left'ou 'right'.
        /// Pinned
        /// </summary>
        public enumFreezePosition FreezePosition { get; set; } = enumFreezePosition.Nenhum;

        /// <summary>
        /// Tipo de dado do Filtro
        /// </summary>
        //public enumFilterType FilterType { get; set; } = enumFilterType.Nenhum;

        /// <summary>
        /// Função para format e renderizar na celula
        /// </summary>
        //public string ValueFormatter { get; set; }

        /// <summary>
        /// Alinhamneto do tento na celula: left, center, right, justified
        /// </summary>
        //public string cellStyle { get; set; }

        //public string ToolTip { get; set; }

        //public string Link { get; set; }
    }
}
