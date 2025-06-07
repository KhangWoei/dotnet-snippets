using Crawler;
namespace Crawl;

public class Program
{
    public static async Task Main(string[] args)
    {
        var seed = "https://www.bbc.co.uk/news";

        var crawler = new WebCrawler();
        await crawler.Crawl(seed);
    }
}
