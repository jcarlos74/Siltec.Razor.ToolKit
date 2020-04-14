using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace Siltec.WebComponents.TagHelpers.DataGrid
{
    public class DataGridContext
    {
        public ModelExplorer ModelExplorer { get; set; }
        public List<ModelExpression> ModelColumnsProperties { get; set; }
        public List<DataGridColumnProperties> ColumnsProperties { get; set; }
        public List<ActionData> ActionDataSet { get; set; }
    }
}
