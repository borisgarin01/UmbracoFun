using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoFun.Models;

namespace UmbracoFun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : UmbracoController
{
    private readonly IPublishedContentQuery _query;
    private readonly IPublishedValueFallback _fallback;

    public GamesController(IPublishedContentQuery query, IPublishedValueFallback fallback)
    {
        _query = query;
        _fallback = fallback;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var gameNodes = _query.ContentAtRoot()
            .SelectMany(x => x.DescendantsOrSelf())
            .Where(x => x.ContentType.Alias == "game");

        var games = gameNodes
            .Select(x => new Game(x, _fallback))
            .ToList();

        return Ok(games.Select(g => new
        {
            g.Id,
            g.Name,
            g.ReleaseDate,
            Image = g.ImageSource,
            Publishers = g.Publishers.Select(p => new
            {
                p.Id,
                p.Name
            }),
            Developers = g.Developers.Select(d => new
            {
                d.Id,
                d.Name
            }),
            Platforms = g.Platforms.Select(p => new
            {
                p.Id,
                p.Name
            }),
            Description = g.Description,
            Localization = new { Id = g.Localization.Id, Name = g.Localization.Name }
        }));
    }
}