using System.Text.Json.Serialization;

namespace BookSearchLib.Models;

// Open Library Search API response model https://openlibrary.org/dev/docs/api/search
public class SearchResult
{
    public List<string>? AuthorKey { get; set; }
    public List<string>? AuthorName { get; set; }
    public string? EbookAccess { get; set; }
    public int EditionCount { get; set; }
    public int? FirstPublishYear { get; set; }
    public bool HasFulltext { get; set; }
    public string? Key { get; set; }
    public List<string>? Language { get; set; }
    public bool PublicScanB { get; set; }
    public string? Subtitle { get; set; }
    public string Title { get; set; } = "";
    [JsonPropertyName("cover_i")]
    public int? CoverID { get; set; }
    public string? Description { get; set; }

}

public class SearchResponse
{
    [JsonPropertyName("numFound")]
    public int NumFound { get; set; }
    public int Start { get; set; }
    public List<SearchResult> Docs { get; set; } = new();
}