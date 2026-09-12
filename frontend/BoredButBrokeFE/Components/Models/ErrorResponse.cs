using System.Text.Json.Serialization;

namespace BoredButBrokeFE.Components.Models
{
    public record ErrorResponse
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("detail")]
        public string? Detail { get; set; }
    }
}
