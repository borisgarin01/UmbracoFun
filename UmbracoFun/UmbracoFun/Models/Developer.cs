using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoFun.Models;

[PublishedModel("developer")]
public class Developer : PublishedContentModel
{
    public Developer(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public string Title => this.Value<string>("title");
}
