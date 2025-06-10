using Microsoft.AspNetCore.Mvc;
using BookSearchLib;
using BookSearchLib.Models;

namespace BookSearchApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly BookSearcher _searcher;
        private readonly ILogger<SearchController> _logger;

        public SearchController(BookSearcher searcher, ILogger<SearchController> logger)
        {
            _searcher = searcher;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponse>> Get([FromQuery] SearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received search request: {@Request}", request);

            try
            {
                var result = await _searcher.SearchAsync(request, cancellationToken);

                if (result == null || result.Docs == null || !result.Docs.Any())
                {
                    _logger.LogWarning("No results found for request: {@Request}", request);
                    return NotFound();
                }

                _logger.LogInformation("Search completed: {Count} items for {@Request}",result.Docs.Count, request);

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Search canceled by client for request: {@Request}", request);
                return BadRequest("Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error during search for {@Request}", request);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
