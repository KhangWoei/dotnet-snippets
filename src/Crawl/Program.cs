using Crawler;
using System.CommandLine;

namespace Crawl;

public class Program
{
    public static int Main(string[] args)
    {
        var seed = new Option<string>(name: "--seed", description: "Website to start crawling from.");

        var rootCommand = new RootCommand("Web crawler.");

        rootCommand.AddOption(seed);

        rootCommand.SetHandler(async (seed) =>
        {
            var crawler = new WebCrawler();
            await crawler.Crawl(seed);
        }, seed);

        return rootCommand.Invoke(args);
    }
}
