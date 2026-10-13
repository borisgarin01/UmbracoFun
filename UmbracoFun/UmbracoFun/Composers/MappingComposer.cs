using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Mapping;
using UmbracoFun.MappingDefinitions;

namespace UmbracoFun.Composers;

public class MappingComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
            .Add<GameMappingDefinition>()
            .Add<GamesCollectionMappingDefinition>();
    }
}
