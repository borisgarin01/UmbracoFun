using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoFun.Models;

[PublishedModel("genre")]
public class Genre : PublishedContentModel
{
    public Genre(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public string Title => this.Value<string>("title");
}
