using System.Text.Json.Serialization;

namespace UmbracoFun.Models
{
    public class Genre
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("games")]
        public List<Game> Games { get; set; } = new();
    }
}