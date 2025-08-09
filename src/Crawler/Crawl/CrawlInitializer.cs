using Crawling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawl.Crawl;

internal static class CrawlInitializer
{
    public static async Task<int> Run(
        string seed,
        int depth, 
        int width,
        DirectoryInfo outputDirectory, 
        string? connectionString, 
        bool asBackgroundService, 
        CancellationToken cancellationToken)
    {
        var configuration = new Configuration(seed, depth, width, outputDirectory);
        
        if (asBackgroundService)
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Services.UserCrawler(configuration, connectionString);
            builder.Services.AddHostedService<WebCrawlerService>();

            var host = builder.Build();
            await host.RunAsync(cancellationToken);
        }
        else
        {
            var services = new ServiceCollection();
            services.UserCrawler(configuration, connectionString);

            var provider = services.BuildServiceProvider();
            var crawler = provider.GetRequiredService<WebCrawler>();

            await crawler.Start(cancellationToken);
        }

        return 0;
    }
}