﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Siltec.WebComponents.DataTables
{
    [ModelBinder(BinderType = typeof(DataTablesModelBinder))]
    public class DataTablesRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public SearchInfo Search { get; set; }
        public List<SortInfo> Order { get; set; }
        public List<ColumnInfo> Columns { get; set; }
        public string Error { get; set; } = "";
    }

    public class SearchInfo
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class SortInfo
    {
        public int Column { get; set; }
        public bool Descending { get; set; }
    }

    public class ColumnInfo
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchInfo Search { get; set; }
    }
}
