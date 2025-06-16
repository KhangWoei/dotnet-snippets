using Crawler.LinkVisiting;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler;

public static class CrawlerDependencies
{
    public static void UserCrawler(this IServiceCollection services)
    {
        services.AddTransient<WebCrawler>(provider => 
                new WebCrawler(provider.GetRequiredService<ILinkVisitor>()));

        services.AddTransient<HttpClient>();
        services.AddTransient<ILinkVisitor, LinkVisitor>();
    }
}
