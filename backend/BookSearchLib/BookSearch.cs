using System.Text.Json;
using BookSearchLib.Models;
using Microsoft.Extensions.Logging;

namespace BookSearchLib
{
    public class BookSearcher
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookSearcher> _logger;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30); 

        public BookSearcher(HttpClient httpClient, ILogger<BookSearcher> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SearchResponse?> SearchAsync(SearchRequest request, CancellationToken cancellationToken = default)
        {
            // create a CTS that cancels after the timeout, linked to the incoming token
            using var ctsTimeout = new CancellationTokenSource(_timeout);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ctsTimeout.Token, cancellationToken);

            var token = linkedCts.Token;
            var searchUrl = BuildSearchUrl(request);

            _logger.LogInformation("Searching OpenLibrary: {Url}", searchUrl);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(searchUrl, token);
            }
            catch (OperationCanceledException) when (ctsTimeout.IsCancellationRequested)
            {
                _logger.LogError("Search timed out after {Timeout}s", _timeout.TotalSeconds);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HTTP error during search for {Query}", request.Title);
                throw;
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Search returned non-success status {StatusCode} for {Url}",
                    response.StatusCode, searchUrl);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync(token);
            var searchResult = JsonSerializer.Deserialize<SearchResponse>(json);

            if (searchResult?.Docs == null)
            {
                _logger.LogWarning("No documents found in search response");
                return searchResult;
            }

            _logger.LogInformation("Fetched {Count} docs; fetching descriptions…",searchResult.Docs.Count);
            var enrichTasks = searchResult.Docs
                .Where(d => !string.IsNullOrEmpty(d.Key))
                .Select(d => EnrichWithDescriptionAsync(d, token));

            await Task.WhenAll(enrichTasks).ConfigureAwait(false);
            return searchResult;
        }

        private async Task EnrichWithDescriptionAsync(SearchResult book, CancellationToken token)
        {
            try
            {
                var key = book.Key!;
                var workUrl = BuildWorkUrl(key);

                _logger.LogDebug("Fetching work data for '{Key}' at {Url}", key, workUrl);

                var resp = await _httpClient.GetAsync(workUrl, token);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Work fetch for {Key} returned {StatusCode}", key, resp.StatusCode);
                    return;
                }

                var workJson = await resp.Content.ReadAsStringAsync(token);
                var workData = JsonSerializer.Deserialize<WorkResponse>(workJson);

                book.Description = workData?.Description?.ToString();
                _logger.LogDebug("Set description for '{Title}' (Key: {Key})", book.Title, key);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Fetch for book '{Title}' was canceled", book.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enriching book '{Title}' (Key: {Key})", book.Title, book.Key);
                book.Description = null;
            }
        }

        private static string BuildSearchUrl(SearchRequest request)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(request.Title))
                queryParams.Add($"title={Uri.EscapeDataString(request.Title)}");

            if (!string.IsNullOrWhiteSpace(request.Author))
                queryParams.Add($"author={Uri.EscapeDataString(request.Author)}");

            if (!string.IsNullOrWhiteSpace(request.Subject))
                queryParams.Add($"subject={Uri.EscapeDataString(request.Subject)}");

            queryParams.Add($"limit={request.Limit}");
            queryParams.Add($"offset={request.Offset}");

            return $"https://openlibrary.org/search.json?{string.Join("&", queryParams)}";
        }

        private static string BuildWorkUrl(string key)
        {
            var normalizedKey = key.StartsWith("/works/") ? key : $"/works/{key}";
            return $"https://openlibrary.org{normalizedKey}.json";
        }
    }
}