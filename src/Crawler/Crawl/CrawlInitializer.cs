using Crawling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawl.Crawl;

internal static class CrawlInitializer
{
    public static async Task<int> Run(string seed, int depth, int width, CancellationToken cancellationToken)
    {
        var configuration = new Configuration(seed, depth, width);
        
        var builder = Host.CreateApplicationBuilder();
        builder.Services.UserCrawler(configuration);

        var host = builder.Build();
        await host.RunAsync(cancellationToken);
        
        return 0;
    }
}