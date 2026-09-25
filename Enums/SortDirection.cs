using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Book
{
    [JsonConverter(typeof(StringEnumConverter))]
         public enum SortDirection
    {        
        Asc, // по возрастанию
        Desc // по убыванию
    }
}