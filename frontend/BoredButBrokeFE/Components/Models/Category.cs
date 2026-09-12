using System.Text.Json.Serialization;

namespace BoredButBrokeFE.Components.Models
{
    public class Category
    {
        [JsonPropertyName("alias")]
        public string Alias { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
    }
}
