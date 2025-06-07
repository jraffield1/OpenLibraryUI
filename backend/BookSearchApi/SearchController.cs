using Microsoft.AspNetCore.Mvc;
using BookSearchLib;
using BookSearchLib.Models;

namespace BookSearchApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly BookSearcher _searcher;

    public SearchController(BookSearcher searcher)
    {
        _searcher = searcher;
    }

    [HttpGet]
    public async Task<ActionResult<SearchResponse>> Get([FromQuery] SearchRequest request)
    {
        var result = await _searcher.SearchAsync(request);
        return Ok(result);
    }
}
