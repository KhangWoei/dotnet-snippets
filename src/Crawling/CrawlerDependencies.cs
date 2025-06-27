using Crawling.CrawlSource;
using Crawling.LinkVisiting;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Crawling;

public static class CrawlerDependencies
{
    public static void UserCrawler(this IServiceCollection services)
    {
        services.AddTransient<WebCrawler>(provider =>
                new WebCrawler(
                    provider.GetRequiredService<IMediator>(),
                    provider.GetRequiredService<ICrawlSourceFactory>(),
                    provider.GetRequiredService<ILinkVisitor>()));

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<WebCrawler>());
        services.AddHttpClient();
        services.AddSingleton<ICrawlSourceFactory, CrawlSourceFactory>();
        services.AddTransient<ILinkVisitor, LinkVisitor>();
    }
}
