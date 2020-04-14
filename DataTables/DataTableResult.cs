using Newtonsoft.Json;
using System.Collections.Generic;

namespace Siltec.WebComponents.DataTables
{
    public class DataTableResult<T>
    {
        [JsonProperty(PropertyName="draw")]
        public int Draw { get; set; }

        [JsonProperty(PropertyName = "recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty(PropertyName = "recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<T> Data { get; set; }

    }
}
