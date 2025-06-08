using Crawler;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace Crawl;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var seed = new Option<string>(name: "--seed", description: "Website to start crawling from.");

        var rootCommand = new RootCommand("Web crawler.");
        rootCommand.AddOption(seed);

        rootCommand.SetHandler(async (seed) =>
        {
            var crawler = new WebCrawler();
            await crawler.Crawl(seed);
        }, seed);

        var builder = new CommandLineBuilder(rootCommand)
            .UseHelp()
            .UseDefaults()
            .Build();

        return await builder.InvokeAsync(args);
    }
}
