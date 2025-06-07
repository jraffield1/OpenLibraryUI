using System.Net.Http;
using System.Text;
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

    private static string ConstructSearchURL(SearchRequest request)
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

        var queryString = string.Join("&", queryParams);
        return $"https://openlibrary.org/search.json?{queryString}";
    }

    private static string ConstructWorksURL(SearchResult book)
    {
        var workKey = book.Key.StartsWith("/works/") ? book.Key : $"/works/{book.Key}";
        return $"https://openlibrary.org{workKey}.json";
    }

    public async Task<SearchResponse?> SearchAsync(SearchRequest request)
    {
        var response = await _httpClient.GetAsync(ConstructSearchURL(request));
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var queryResponse = JsonSerializer.Deserialize<SearchResponse>(json);

        if (queryResponse?.Docs == null) return queryResponse;

        var fetchTasks = queryResponse.Docs.Select(async book =>
        {
            if (string.IsNullOrEmpty(book.Key)) return;

            try
            {
                var worksResponse = await _httpClient.GetAsync(ConstructWorksURL(book));
                if (!worksResponse.IsSuccessStatusCode) return;

                var workJson = await worksResponse.Content.ReadAsStringAsync();

                var worksData = JsonSerializer.Deserialize<WorkResponse>(workJson);

                book.Description = worksData?.Description?.ToString();
            }
            catch
            {
                book.Description = null;
            }
        });

        await Task.WhenAll(fetchTasks);
        return queryResponse;
    }

}