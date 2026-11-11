using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoFun.Models;

public class Publisher : PublishedContentModel
{
    public Publisher(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public string Title => this.Value<string>("title");
}
