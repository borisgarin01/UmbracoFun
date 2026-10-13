using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UmbracoFun.Models;

namespace UmbracoFun.MappingDefinitions;

/// <summary>
/// Maps Umbraco content of the "gamesCollection" document type to <see cref="GamesCollection"/>.
/// Games are mapped through <see cref="GameMappingDefinition"/>.
/// </summary>
public class GamesCollectionMappingDefinition : IMapDefinition
{
    private readonly IPublishedUrlProvider _publishedUrlProvider;

    public GamesCollectionMappingDefinition(IPublishedUrlProvider publishedUrlProvider)
    {
        _publishedUrlProvider = publishedUrlProvider;
    }

    public void DefineMaps(IUmbracoMapper mapper)
    {
        mapper.Define<IPublishedContent, GamesCollection>((_, _) => new GamesCollection(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IPublishedContent source, GamesCollection target, MapperContext context)
    {
        IPublishedContent? image = source.Value<IPublishedContent>(GamesCollectionAliases.Image);
        IEnumerable<IPublishedContent> games = source.Value<IEnumerable<IPublishedContent>>(GamesCollectionAliases.Games) ?? [];

        target.Name = source.Value<string>(GamesCollectionAliases.Title) is { Length: > 0 } title ? title : source.Name;
        target.Description = source.Value<string>(GamesCollectionAliases.Description) ?? string.Empty;
        target.ImageSource = image is null ? string.Empty : _publishedUrlProvider.GetMediaUrl(image);
        target.GamesCollectionItems = context.MapEnumerable<IPublishedContent, Game>(games.Where(x => x is not null))
            .Select(game => new GamesCollectionItem { Game = game, GamesCollection = target })
            .ToList();
    }

    /// <summary>
    /// Property aliases on the "gamesCollection" document type.
    /// </summary>
    public static class GamesCollectionAliases
    {
        public const string Title = "title";
        public const string Description = "description";
        public const string Image = "image";
        public const string Games = "games";
    }
}
