using System.Text.Json;
using BookSearchLib.Models;

namespace BookSearchLib;

public class BookSearcher
{
    private readonly HttpClient _httpClient;

    public BookSearcher(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SearchResponse?> SearchAsync(SearchRequest request)
    {
        // Do basic search on parameters to get list of books
        var searchUrl = BuildSearchUrl(request);
        var response = await _httpClient.GetAsync(searchUrl).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var searchResult = JsonSerializer.Deserialize<SearchResponse>(json);
        if (searchResult?.Docs == null) return searchResult;

        // For each book that was returned try to retrieve its description from the works API
        var fetchTasks = searchResult.Docs
            .Where(book => !string.IsNullOrEmpty(book.Key))
            .Select(book => EnrichWithDescriptionAsync(book));

        await Task.WhenAll(fetchTasks).ConfigureAwait(false);
        return searchResult;
    }

    private async Task EnrichWithDescriptionAsync(SearchResult book)
    {
        try
        {
            if (book.Key == null)
                throw new Exception($"Book {book.Title} key not found");

            var workUrl = BuildWorkUrl(book.Key);
            var response = await _httpClient.GetAsync(workUrl);
            if (!response.IsSuccessStatusCode) return;

            var workJson = await response.Content.ReadAsStringAsync();
            var workData = JsonSerializer.Deserialize<WorkResponse>(workJson);

            book.Description = workData?.Description?.ToString();
        }
        catch(Exception e)
        {
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
