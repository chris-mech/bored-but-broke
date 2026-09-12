using System.Text.Json.Serialization;

namespace BoredButBrokeFE.Components.Models
{
    public class Hour
    {
        [JsonPropertyName("day")]
        public int Day { get; set; } = 0;
        [JsonPropertyName("start")]
        public string Start { get; set; } = string.Empty;
        [JsonPropertyName("end")]
        public string End { get; set; } = string.Empty;
    }
}
