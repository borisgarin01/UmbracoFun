using System.Text.Json.Serialization;

namespace UmbracoFun.Models;

public class Developer
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("createTimeStamp")]
    public DateTime CreateTimeStamp { get; set; }

    [JsonPropertyName("games")]
    public List<Game> Games { get; init; } = new();
}
