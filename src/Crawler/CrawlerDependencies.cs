using Microsoft.Extensions.DependencyInjection;

namespace Crawler;

public static class CrawlerDependencies
{
    public static void UserCrawler(this IServiceCollection services)
    {
        services.AddTransient<WebCrawler>();
    }
}
