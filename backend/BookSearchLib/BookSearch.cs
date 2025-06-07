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

    private static string ConstructQueryString(SearchRequest req)
    {
        var sb = new StringBuilder("https://openlibrary.org/search.json?");

        // Given the query info provided construct the multiparameter query string
        if (!string.IsNullOrWhiteSpace(req.Title))
            sb.Append($"title={Uri.EscapeDataString(req.Title)}&");
        if (!string.IsNullOrWhiteSpace(req.Author))
            sb.Append($"author={Uri.EscapeDataString(req.Author)}&");
        if (!string.IsNullOrWhiteSpace(req.Subject))
            sb.Append($"subject={Uri.EscapeDataString(req.Subject)}&");

        var url = sb.ToString().TrimEnd('&');

        return url;
    }

    public async Task<SearchResponse?> SearchAsync(SearchRequest req)
    {
        string query = ConstructQueryString(req);
        
        var response = await _httpClient.GetAsync(query);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<SearchResponse>(json);
    }
}