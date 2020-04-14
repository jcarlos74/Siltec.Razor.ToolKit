using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Siltec.WebComponents.TagHelpers.DataTables
{
    public class DataTableContext
    {
        public ModelExplorer ModelExplorer { get; set; }
        public List<ModelExpression> ModelColumnsProperties { get; set; }
        public List<DataTableColumnsProperties> ColumnsProperties { get; set; }
        public List<ActionData> ActionDataSet { get; set; }
    }
}
