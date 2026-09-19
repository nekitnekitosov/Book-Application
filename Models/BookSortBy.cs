using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Book
{
    [JsonConverter(typeof(StringEnumConverter))]
        public enum BookSortBy
    {
        Name,
        Year
    }
}