using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoFun.Models;

[PublishedModel("localization")]
public class Localization : PublishedContentModel
{
    public Localization(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public string Title => this.Value<string>("title");
}
