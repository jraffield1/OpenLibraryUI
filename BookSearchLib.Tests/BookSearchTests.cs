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
    public async Task Subject_Search()
    {
        var httpClient = new HttpClient();
        var searcher = new BookSearcher(httpClient);
        var query = new SearchRequest { Subject = "Science Fiction" };

        var result = await searcher.SearchAsync(query);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Docs);
    }

    [Fact]
    public async Task Pagination_Limit_And_Offset()
    {
        var httpClient = new HttpClient();
        var searcher = new BookSearcher(httpClient);

        // First page
        var first = await searcher.SearchAsync(new SearchRequest { Title = "Dune", Limit = 5, Offset = 0 });
        // Second page
        var second = await searcher.SearchAsync(new SearchRequest { Title = "Dune", Limit = 5, Offset = 5 });

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(5, first.Docs.Count);
        Assert.Equal(5, second.Docs.Count);
        // Make sure pages don’t overlap exactly
        Assert.DoesNotContain(first.Docs.First().Key, second.Docs.Select(d => d.Key));
    }

    [Fact]
    public async Task NoResults_Returns_EmptyDocs()
    {
        var httpClient = new HttpClient();
        var searcher = new BookSearcher(httpClient);
        var query = new SearchRequest { Title = "asdasdasdasdasd" };  // hopefully gibberish

        var result = await searcher.SearchAsync(query);

        Assert.NotNull(result);
        Assert.Empty(result.Docs);
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