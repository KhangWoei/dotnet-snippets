using Crawler.LinkVisiting;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler;

public static class CrawlerDependencies
{
    public static void UserCrawler(this IServiceCollection services)
    {
        services.AddTransient<WebCrawler>(provider =>
                new WebCrawler(provider.GetRequiredService<ILinkVisitor>()));
        
        services.AddSingleton<HttpClient>(_ => new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(30),
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(10)
        })
        {
            DefaultRequestHeaders = { { "accept", "text/html" } },
            Timeout = TimeSpan.FromSeconds(15)
        });
        services.AddTransient<ILinkVisitor, LinkVisitor>();
    }
}
