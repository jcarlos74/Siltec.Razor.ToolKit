using System;
using System.Collections.Generic;
using System.Text;

namespace Siltec.WebComponents.TagHelpers.DataTables
{
    public class DataTableColumnsProperties
    {
        public string name { get; set; }

        public string title { get; set; }

        /// <summary>
        /// Essa propriedade pode ser usada para ler e gravar dados de e para qualquer propriedade da fonte de dados
        /// </summary>
        public string data { get; set; }

        public string className { get; set; }

        public string render { get; set; }
        /// <summary>
        /// Muitas vezes, você pode querer ter conteúdo estático em uma coluna, por exemplo, botões simples de edição e / ou exclusão, que possuem eventos atribuídos a eles. Esta opção está disponível para esses casos de uso - criando conteúdo estático para uma coluna. Se você deseja criar conteúdo dinâmico (ou seja, com base em outros dados na linha), a columns.renderopção deve ser usada.
        /// Além disso, essa opção pode ser útil ao carregar dados JSON, pois o valor configurado aqui será usado se o valor da célula do JSON for nulo(por exemplo, você pode definir uma sequência padrão de Not available.
        /// </summary>
        public string defaultContent { get; set; }

        /// <summary>
        /// ativa ou desativa a ordenção da coluna para o usuario final
        /// </summary>
        public string orderable { get; set; }


        public string visible { get; set; }

        public string type { get; set; }

        /// <summary>
        /// Usando esse parâmetro, você pode definir se DataTables deve incluir esta coluna nos dados filtráveis ​​da tabela. Você pode usar esta opção para desativar a pesquisa em colunas geradas, como os botões 'Editar' e 'Excluir', por exemplo.
        /// </summary>
        public string searchable { get; set; }

        /// <summary>
        /// Este parâmetro pode ser usado para definir a largura de uma coluna e pode assumir qualquer valor de CSS (3em, 20px etc.).
        /// </summary>
        public string width { get; set; }
    }
}
