using System.CommandLine;

namespace Crawl.Crawl;

internal sealed class CrawlCommand : RootCommand
{
    public CrawlCommand() : base("Crawl the web from given seed(s)")
    {
        Add(Seed);
        Add(Depth);
        Add(Width);

        SetAction((result, cancellationToken) => CrawlInitializer.Run(
            result.GetRequiredValue(Seed),
            result.GetValue(Depth), 
            result.GetValue(Width),
            cancellationToken));
    }

    private static readonly Option<string> Seed = new(name: "--seed")
    {
        Description = "Website to start crawling from.",
        Required = true,
    };

    private static readonly Option<int> Depth = new(name: "--depth")
    {
        Description = "Depth of the crawled site.",
        DefaultValueFactory = _ => 2,
        Required = false
    };

    private static readonly Option<int> Width = new(name: "--width")
    {
        Description = "Width of the crawler.",
        DefaultValueFactory = _ => 2,
        Required = false
    };
}