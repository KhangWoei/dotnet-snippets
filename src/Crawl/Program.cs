using Crawler;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace Crawl;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var seedOption = new Option<string>(name: "--seed", description: "Website to start crawling from.")
        {
            IsRequired = true
        };

        var depthOption = new Option<int>(name: "--depth", description: "Depth of the crawled site.")
        {
            IsRequired = false
        };
        
        depthOption.SetDefaultValue(3);

        var services = new ServiceCollection();
        services.UserCrawler();

        var provider = services.BuildServiceProvider();

        var rootCommand = new RootCommand("Web crawler.");
        rootCommand.AddOption(seedOption);
        rootCommand.AddOption(depthOption);

        rootCommand.SetHandler(async context =>
        {
            var crawler = provider.GetRequiredService<WebCrawler>();
            var seed = context.ParseResult.GetValueForOption(seedOption)!;
            var depth = context.ParseResult.GetValueForOption(depthOption);
            
            await crawler.Crawl(seed, depth, context.GetCancellationToken());
        });

        var builder = new CommandLineBuilder(rootCommand)
            .UseHelp()
            .UseDefaults()
            .Build();

        return await builder.InvokeAsync(args);
    }
}
