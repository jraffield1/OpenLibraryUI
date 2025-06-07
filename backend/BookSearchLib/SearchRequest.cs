namespace BookSearchLib.Models;

public class SearchRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Subject { get; set; }
    public int Limit { get; set; } = 10;
    public int Offset { get; set; } = 0;
}