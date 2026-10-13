using System.Text.Json.Serialization;

namespace UmbracoFun.Models;

public sealed class Publisher
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public List<Game> Games { get; set; } = new List<Game>();
}