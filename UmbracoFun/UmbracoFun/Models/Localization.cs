using System.Text.Json.Serialization;

namespace UmbracoFun.Models;

public class Localization
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("games")]
    public List<Game> Games { get; set; } = new List<Game>();
}