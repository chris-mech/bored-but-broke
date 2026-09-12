using System.Text.Json.Serialization;

namespace BoredButBrokeFE.Components.Models
{
    public class OpeningHours
    {
        [JsonPropertyName("hours")]
        public List<Hour> Hours { get; set; } = new List<Hour>();
        [JsonPropertyName("hoursType")]
        public string HoursType { get; set; } = string.Empty;
        [JsonPropertyName("isOpenNow")]
        public bool IsOpenNow { get; set; } = false;
    }
}
