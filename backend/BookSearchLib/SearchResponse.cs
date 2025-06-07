using System.Text.Json.Serialization;

namespace BookSearchLib.Models;

// Open Library Search API response model https://openlibrary.org/dev/docs/api/search
public class SearchResult
{
    [JsonPropertyName("author_key")]
    public List<string>? AuthorKey { get; set; }

    [JsonPropertyName("author_name")]
    public List<string>? AuthorName { get; set; }

    [JsonPropertyName("ebook_access")]
    public string? EbookAccess { get; set; }

    [JsonPropertyName("edition_count")]
    public int EditionCount { get; set; }

    [JsonPropertyName("first_publish_year")]
    public int? FirstPublishYear { get; set; }

    [JsonPropertyName("has_fulltext")]
    public bool HasFulltext { get; set; }

    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("language")]
    public List<string>? Language { get; set; }

    [JsonPropertyName("public_scan_b")]
    public bool PublicScanB { get; set; }

    [JsonPropertyName("subtitle")]
    public string? Subtitle { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("cover_i")]
    public int? CoverID { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

}

public class SearchResponse
{
     [JsonPropertyName("numFound")]
    public int NumFound { get; set; }

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("docs")]
    public List<SearchResult> Docs { get; set; } = new();
}