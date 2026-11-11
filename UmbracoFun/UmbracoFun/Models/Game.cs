using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoFun.Models;

[PublishedModel("game")]
public class Game : PublishedContentModel
{
    // 1. Declare a private field to store the fallback
    private readonly IPublishedValueFallback _publishedValueFallback;

    public Game(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
        : base(content, publishedValueFallback)
    {
        // 2. Assign a value to the field
        _publishedValueFallback = publishedValueFallback;
    }

    public string Title => this.Value<string>("title");
    public string Description => this.Value<string>("description");

    // 3. Use the private field to construct Genre
    public IEnumerable<Genre> Genres =>
        this.Value<IEnumerable<IPublishedContent>>("genres")?
            .Select(x => new Genre(x, _publishedValueFallback))
        ?? Enumerable.Empty<Genre>();

    public IEnumerable<Developer> Developers => this.Value<IEnumerable<IPublishedContent>>("developers")?
        .Select(x => new Developer(x, _publishedValueFallback))
        ?? Enumerable.Empty<Developer>();

    public IEnumerable<Publisher> Publishers => this.Value<IEnumerable<IPublishedContent>>("publishers")?
        .Select(x => new Publisher(x, _publishedValueFallback))
        ?? Enumerable.Empty<Publisher>();

    public IEnumerable<Platform> Platforms => this.Value<IEnumerable<IPublishedContent>>("platforms")?
        .Select(x => new Platform(x, _publishedValueFallback))
        ?? Enumerable.Empty<Platform>();

    public Localization Localization => this.Value<IEnumerable<IPublishedContent>>("localizations")?
        .Select(x => new Localization(x, _publishedValueFallback)).Single();

    public string ImageSource => this.Value<MediaWithCrops>("image")?.MediaUrl();
    public DateTime? ReleaseDate => this.Value<DateTime?>("releaseDate");
}