using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoFun.Models;

namespace UmbracoFun.Controllers;

// Hijacks requests for content of the "game" document type.
public class GameController : RenderController
{
    private readonly IUmbracoMapper _umbracoMapper;

    public GameController(
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoMapper umbracoMapper)
        : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _umbracoMapper = umbracoMapper;
    }

    public override IActionResult Index()
    {
        if (CurrentPage is null)
        {
            return NotFound();
        }

        Game? game = _umbracoMapper.Map<Game>(CurrentPage);
        return CurrentTemplate(game);
    }
}
