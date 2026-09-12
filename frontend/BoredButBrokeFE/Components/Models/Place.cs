using System.ComponentModel;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace BoredButBrokeFE.Components.Models
{
    public class Place
    {
        [JsonPropertyName("placeId")]
        public string PlaceId { get; set; } = string.Empty;
        [JsonPropertyName("placeName")]
        public string PlaceName { get; set; } = string.Empty;
        [JsonPropertyName("location")]
        public Location Location { get; set; } = new Location();
        [JsonPropertyName("categories")]
        public List<Category> Categories { get; set; } = new List<Category>();
        [JsonPropertyName("price")]
        public Price Price { get; set; } = Price.Cheap;
        [JsonPropertyName("coordinates")]
        public Coordinates Coordinates { get; set; } = new Coordinates();
        [JsonPropertyName("openingHours")]
        public List<OpeningHours> OpeningHours { get; set; } = new List<OpeningHours>();
        [JsonPropertyName("rating")]
        public double Rating { get; set; } = 1;
        [JsonPropertyName("placeUrl")]
        public string PlaceUrl { get; set; } = string.Empty;
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
