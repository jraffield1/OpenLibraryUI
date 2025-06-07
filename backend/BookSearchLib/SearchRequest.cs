namespace BookSearchLib.Models;

public class SearchRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Subject { get; set; }

    public bool HasAnyQuery =>
        !string.IsNullOrWhiteSpace(Title) ||
        !string.IsNullOrWhiteSpace(Author) ||
        !string.IsNullOrWhiteSpace(Subject);
}