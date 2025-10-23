using Crawling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

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
        var logger = LoggingInitializer.Initialize(outputDirectory, LogEventLevel.Verbose);
        
        if (asBackgroundService)
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Services.UserCrawler(configuration, connectionString);
            builder.Services.AddHostedService<WebCrawlerService>();
            builder.Services.AddTransient<ILogger>(_ => logger);

            var host = builder.Build();
            await host.RunAsync(cancellationToken);
        }
        else
        {
            var services = new ServiceCollection();
            services.UserCrawler(configuration, connectionString);
            services.AddTransient<ILogger>(_ => logger);

            var provider = services.BuildServiceProvider();
            var crawler = provider.GetRequiredService<WebCrawler>();

            await crawler.Start(cancellationToken);
        }

        return 0;
    }
}