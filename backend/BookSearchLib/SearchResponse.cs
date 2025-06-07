namespace BookSearchLib.Models;

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
}

public class SearchResponse
{
    public int Start { get; set; }
    public int NumFound { get; set; }
    public List<SearchResult> Docs { get; set; } = new();
}