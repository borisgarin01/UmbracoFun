using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace UmbracoFun.Models;

public sealed class Game
{
    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("image")]
    public required string Image { get; set; }

    [JsonPropertyName("usersScore")]
    public required float UsersScore { get; set; }

    [JsonPropertyName("usersReviewsCount")]
    public required int UsersReviewsCount { get; set; }

    [JsonPropertyName("criticsScore")]
    public required float CriticsScore { get; set; }

    [JsonPropertyName("criticsReviewsCount")]
    public required int CriticsReviewsCount { get; set; }

    [JsonPropertyName("waitingCount")]
    public int WaitingCount { get; set; }

    [JsonPropertyName("notWaitingCount")]
    public int NotWaitingCount { get; set; }

    [JsonPropertyName("publishers")]
    public required List<Publisher> Publishers { get; set; } = new();

    [JsonPropertyName("releaseDate")]
    public required DateOnly? ReleaseDate { get; set; }

    /// <summary>
    /// Игра считается вышедшей с дня релиза. Без даты релиза - еще не вышла.
    /// До выхода можно только голосовать "жду / не жду", оценки и отзывы запрещены.
    /// </summary>
    [JsonIgnore]
    public bool IsReleased => ReleaseDate.HasValue && ReleaseDate.Value <= DateOnly.FromDateTime(DateTime.Today);

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("trailer")]
    public required string Trailer { get; set; }

    [JsonPropertyName("platforms")]
    public required List<Platform> Platforms { get; set; } = new();

    [JsonPropertyName("genres")]
    public required List<Genre> Genres { get; set; } = new();

    [JsonPropertyName("developers")]
    public required List<Developer> Developers { get; set; } = new();

    [JsonPropertyName("gameScreenshots")]
    public required List<GameScreenshot> Screenshots { get; set; } = new();

    [JsonPropertyName("gamePlayersReviews")]
    public required List<GameReview> GamesPlayersReviews { get; set; } = new();

    [JsonPropertyName("localization")]
    public required Localization Localization { get; set; }

    [JsonPropertyName("localizationId")]
    public long LocalizationId { get; set; }

    [JsonPropertyName("links")]
    public List<GamesCollection> GameCollections { get; set; } = new();
}
