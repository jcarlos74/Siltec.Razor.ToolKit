using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Siltec.WebComponents.DataTables
{
    /// <summary>
    /// Analisa (liga) os parâmetros de entrada HTTP das solicitações do jQuery DataTable  para o modelo de classe DataTablesRequest.
    /// </summary>
    public class DataTablesModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var Values = bindingContext.ValueProvider;

            var allValues = bindingContext.HttpContext.Request.Query;

            if (Values.GetValue("draw") == ValueProviderResult.None)
            {
                throw new ArgumentNullException("draw", "Argumento de DataTables incorreto ou ausente.");
            }

            var draw = Convert.ToInt32(Values.GetValue("draw").ToString());
            var start = Convert.ToInt32(Values.GetValue("start").ToString());
            var length = Convert.ToInt32(Values.GetValue("length").ToString());

            // parses search[value]
            // parses search[regex]
            var search = new SearchInfo
            {
                Value = Values.GetValue("search[value]").ToString(),
                Regex = Convert.ToBoolean(Values.GetValue("search[regex]").ToString())
            };

            // parses columns[n][data]
            // parses columns[n][name]
            // parses columns[n][orderable]
            // parses columns[n][searchable]
            // parses columns[n][search][value]
            // parses columns[n][search][regex]
            var columns = new List<ColumnInfo>();
            for (var i = 0; true; i++)
            {
                var keyData = $@"columns[{i}][data]";
                var keyName = $@"columns[{i}][name]";
                var keyOrderable = $@"columns[{i}][orderable]";
                var keySearchable = $@"columns[{i}][searchable]";
                var keyValue = $@"columns[{i}][search][value]";
                var keyRegex = $@"columns[{i}][search][regex]";

                if (!Values.ContainsPrefix(keyData) ||
                    string.IsNullOrWhiteSpace(Values.GetValue(keyData).ToString()))
                    break;

                columns.Add(new ColumnInfo
                {
                    Data = Values.GetValue(keyData).ToString(),
                    Name = Values.GetValue(keyName).ToString(),
                    Orderable = Boolean.TryParse(Values.GetValue(keyOrderable).ToString(), out var orderable) ? orderable : false,
                    Searchable = Boolean.TryParse(Values.GetValue(keySearchable).ToString(), out var searchable) ? searchable : false,
                    Search = new SearchInfo
                    {
                        Value = Values.GetValue(keyValue).ToString(),
                        Regex = Boolean.TryParse(Values.GetValue(keyRegex).ToString(), out var regex) ? regex : false
                    }
                });
            }

            // parses order[n][column]
            // parses order[n][dir]
            var order = new List<SortInfo>();
            for (var i = 0; true; i++)
            {
                var keyColumn = $@"order[{i}][column]";
                var keyDir = $@"order[{i}][dir]";

                if (!Values.ContainsPrefix(keyColumn) ||
                    string.IsNullOrWhiteSpace(Values.GetValue(keyColumn).ToString()) ||
                    Int32.TryParse(Values.GetValue(keyColumn).ToString(), out int column) == false)
                {
                    break;
                }
                if (columns[column].Orderable)
                {
                    order.Add(new SortInfo
                    {
                        Column = column,
                        Descending = Values.GetValue(keyDir).ToString().ToLower() == "desc"
                    });
                }
            }

            // bind all the DataTablesRequest data together
            bindingContext.Result = ModelBindingResult.Success(new DataTablesRequest
            {
                Draw = draw,
                Start = start,
                Length = length,
                Search = search,
                Order = order,
                Columns = columns,
            });

            return Task.CompletedTask;

        }
    }
}
