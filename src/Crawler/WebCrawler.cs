using HtmlAgilityPack;

namespace Crawler;

public class WebCrawler
{
    public async Task Crawl(string seed, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Visiting {seed}");

        var client = new HttpClient();
        var uri = new Uri(seed);

        var baseUri = new Uri(uri.GetLeftPart(UriPartial.Authority));
        var robotsUri = new Uri(baseUri, "/robots.txt");
        var robotsContent = await client.GetStringAsync(robotsUri, cancellationToken);
    
        Console.WriteLine(robots);
            
        var html = await client.GetStringAsync(uri, cancellationToken);
        
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        foreach(var link in doc.DocumentNode.SelectNodes("//a[@href]"))
        {
            var attribute = link.Attributes["href"];
            Console.WriteLine(attribute.Value);
        }
    }
}
