using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoFun.Models;

[PublishedModel("platform")]
public class Platform : PublishedContentModel
{
    public Platform(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public string Title => this.Value<string>("title");
}
