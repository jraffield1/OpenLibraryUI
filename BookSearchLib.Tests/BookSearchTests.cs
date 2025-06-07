using BookSearchLib;
using BookSearchLib.Models;
using Xunit;

namespace BookSearchLib.Tests;

public class BookSearcherLiveTests
{
    [Fact]
    public async Task Title_Search()
    {
        var httpClient = new HttpClient();
        var searcher = new BookSearcher(httpClient);
        var query = new SearchRequest { Title = "Dune" };

        var result = await searcher.SearchAsync(query);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Docs);
        Assert.Contains(result.Docs, doc =>
            doc.Title.Contains("Dune", System.StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Combined_Title_Author_Search()
    {
        var httpClient = new HttpClient();
        var searcher = new BookSearcher(httpClient);
        var query = new SearchRequest { Title = "Foundation", Author = "Asimov" };

        var result = await searcher.SearchAsync(query);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Docs);
        Assert.Contains(result.Docs, doc =>
            doc.Title.Contains("Foundation", System.StringComparison.OrdinalIgnoreCase)
            && doc.AuthorName != null
            && doc.AuthorName.Any(name => name.Contains("Asimov", System.StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public async Task Description_Exists()
    {
        var httpClient = new HttpClient();
        var searcher = new BookSearcher(httpClient);
        var query = new SearchRequest { Title = "Dune" };

        var result = await searcher.SearchAsync(query);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Docs);
        Assert.True(result.Docs.First().Description?.Length > 0);
    }
}