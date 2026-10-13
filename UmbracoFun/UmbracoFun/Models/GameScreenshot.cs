using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UmbracoFun.Models;

public class GameScreenshot
{
    [Required]
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; }
}