using Examine;
using Examine.Search;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Umbraco.Cms.Api.Delivery.Querying.Filters;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoFun.Models;
using UmbracoFun.Models.RequestsModels.Games;
using UmbracoFun.Models.ResponsesModels;
using static OpenIddict.Client.WebIntegration.OpenIddictClientWebIntegrationConstants.Osu;

namespace UmbracoFun.Controllers;

[ApiController]
[Route("api/games/[controller]")]
public class GamesController : UmbracoController
{
    private readonly IPublishedContentQuery _query;
    private readonly IPublishedValueFallback _fallback;
    private readonly IExamineManager _examineManager;
    private readonly UmbracoHelper _umbracoHelper;

    public GamesController(IPublishedContentQuery query, IPublishedValueFallback fallback, IExamineManager examineManager, UmbracoHelper umbracoHelper)
    {
        _query = query;
        _fallback = fallback;
        _examineManager = examineManager;
        _umbracoHelper = umbracoHelper;
    }

    [HttpGet]
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

    [HttpGet("First/{offset:int}/{limit:int}")]
    [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
    public IActionResult First(int offset, int limit)
    {
        var gameNodes = _query.ContentAtRoot()
            .SelectMany(x => x.DescendantsOrSelf())
            .Where(x => x.ContentType.Alias == "game")
            .Skip(offset)
            .Take(limit);

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

    [HttpGet("byParameters")]
    public IActionResult GetGames([FromQuery] GameFilterRequest filter)
    {
        IEnumerable<string> ids = Array.Empty<string>();
        if (!string.IsNullOrEmpty("The") && _examineManager.TryGetIndex("ExternalIndex", out IIndex? index))
        {
            ids = index
                .Searcher
                .CreateQuery("content")
                .NodeTypeAlias("game")
                .And()
                .Field("title", "The")
                .Execute()
                .Select(x => x.Id);
        }

        return Ok();
    }
}