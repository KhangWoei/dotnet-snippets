namespace Crawler;

public class WebCrawler
{
    public async Task Crawl(string seed, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Visiting {seed}");

        var client = new HttpClient();
        var uri = new Uri(seed);
        var html = await client.GetStringAsync(uri, cancellationToken);

        Console.WriteLine($"""
                            Website: {seed}
                            Content: {html}
                           """);
    }
}
