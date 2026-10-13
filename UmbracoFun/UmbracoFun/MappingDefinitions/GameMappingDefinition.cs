using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UmbracoFun.Models;

namespace UmbracoFun.MappingDefinitions;

/// <summary>
/// Maps Umbraco content of the "game" document type (and the content/media it picks) to the <see cref="Game"/> domain model.
/// Related entities only get their own data: back-references (e.g. <see cref="Genre.Games"/>) are left empty
/// so that mapping a game never recurses into the graph of games around it.
/// </summary>
public class GameMappingDefinition : IMapDefinition
{
    private readonly IPublishedUrlProvider _publishedUrlProvider;

    public GameMappingDefinition(IPublishedUrlProvider publishedUrlProvider)
    {
        _publishedUrlProvider = publishedUrlProvider;
    }

    public void DefineMaps(IUmbracoMapper mapper)
    {
        mapper.Define<IPublishedContent, Game>(NewGame, Map);
        mapper.Define<IPublishedContent, Publisher>((source, _) => new Publisher { Id = source.Id, Name = source.Name });
        mapper.Define<IPublishedContent, Developer>((source, _) => new Developer { Id = source.Id, Name = source.Name, CreateTimeStamp = source.CreateDate });
        mapper.Define<IPublishedContent, Genre>((source, _) => new Genre { Id = source.Id, Name = source.Name });
        mapper.Define<IPublishedContent, Platform>((source, _) => new Platform { Id = source.Id, Name = source.Name });
        mapper.Define<IPublishedContent, Localization>((source, _) => new Localization { Name = source.Name });
        mapper.Define<IPublishedContent, GameScreenshot>((source, _) => new GameScreenshot { ImageUrl = MediaUrl(source) });
    }

    // Umbraco.Code.MapAll -GameCollections -GamesPlayersReviews
    private Game NewGame(IPublishedContent source, MapperContext context)
    {
        IPublishedContent? localization = source.Value<IPublishedContent>(GameAliases.Localization);

        return new Game
        {
            Id = source.Id,
            Title = source.Value<string>(GameAliases.Title) is { Length: > 0 } title ? title : source.Name,
            Image = MediaUrl(source.Value<IPublishedContent>(GameAliases.Image)),
            Description = source.Value<string>(GameAliases.Description) ?? string.Empty,
            Trailer = source.Value<string>(GameAliases.Trailer) ?? string.Empty,
            ReleaseDate = ToDateOnly(source.Value(GameAliases.ReleaseDate)),
            UsersScore = source.Value<float>(GameAliases.UsersScore),
            UsersReviewsCount = source.Value<int>(GameAliases.UsersReviewsCount),
            CriticsScore = source.Value<float>(GameAliases.CriticsScore),
            CriticsReviewsCount = source.Value<int>(GameAliases.CriticsReviewsCount),
            Publishers = MapPicked<Publisher>(source, GameAliases.Publishers, context),
            Developers = MapPicked<Developer>(source, GameAliases.Developers, context),
            Genres = MapPicked<Genre>(source, GameAliases.Genres, context),
            Platforms = MapPicked<Platform>(source, GameAliases.Platforms, context),
            Screenshots = MapPicked<GameScreenshot>(source, GameAliases.Screenshots, context),
            Localization = localization is null ? new Localization { Name = string.Empty } : context.Map<Localization>(localization)!,
            LocalizationId = localization?.Id ?? 0,

            // Player reviews belong to application users and are not stored in Umbraco.
            GamesPlayersReviews = new List<GameReview>(),
        };
    }

    private static void Map(IPublishedContent source, Game target, MapperContext context)
    {
        target.WaitingCount = source.Value<int>(GameAliases.WaitingCount);
        target.NotWaitingCount = source.Value<int>(GameAliases.NotWaitingCount);
    }

    private static List<T> MapPicked<T>(IPublishedContent source, string alias, MapperContext context)
    {
        IEnumerable<IPublishedContent> picked = source.Value<IEnumerable<IPublishedContent>>(alias) ?? [];
        return context.MapEnumerable<IPublishedContent, T>(picked.Where(x => x is not null));
    }

    private string MediaUrl(IPublishedContent? media) =>
        media is null ? string.Empty : _publishedUrlProvider.GetMediaUrl(media);

    // Date pickers return different CLR types depending on the editor/configuration.
    private static DateOnly? ToDateOnly(object? value) => value switch
    {
        DateOnly date => date,
        DateTime dateTime when dateTime != default => DateOnly.FromDateTime(dateTime),
        DateTimeOffset dateTimeOffset when dateTimeOffset != default => DateOnly.FromDateTime(dateTimeOffset.Date),
        _ => null,
    };

    /// <summary>
    /// Property aliases on the "game" document type.
    /// </summary>
    public static class GameAliases
    {
        public const string Title = "title";
        public const string Image = "image";
        public const string Description = "description";
        public const string Trailer = "trailer";
        public const string ReleaseDate = "releaseDate";
        public const string UsersScore = "usersScore";
        public const string UsersReviewsCount = "usersReviewsCount";
        public const string CriticsScore = "criticsScore";
        public const string CriticsReviewsCount = "criticsReviewsCount";
        public const string WaitingCount = "waitingCount";
        public const string NotWaitingCount = "notWaitingCount";
        public const string Publishers = "publishers";
        public const string Developers = "developers";
        public const string Genres = "genres";
        public const string Platforms = "platforms";
        public const string Screenshots = "screenshots";
        public const string Localization = "localization";
    }
}
