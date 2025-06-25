using Crawling;
using Microsoft.Extensions.DependencyInjection;

namespace Crawl.Crawl;

internal static class CrawlInitializer
{
    public static async Task<int> Run(string seed, int depth, int width, CancellationToken cancellationToken)
    {
        var services = new ServiceCollection();
        services.UserCrawler();
        services.AddTransient<CrawlRunner>();
        
        var provider = services.BuildServiceProvider();

        var runner = provider.GetRequiredService<CrawlRunner>();
        await runner.Run(seed, depth, width, cancellationToken);

        return 0;
    }
}