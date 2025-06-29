using Crawling.CrawlSource;
using Crawling.Frontier;
using Crawling.LinkVisiting;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Crawling;

public static class CrawlerDependencies
{
    public static void UserCrawler(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<WebCrawler>());
        services.AddHttpClient();
        
        services.AddSingleton<ICrawlSourceFactory, CrawlSourceFactory>();
        services.AddSingleton<ISeedQueue<Task<ICrawlSource>>, SeedQueue>();
        
        services.AddTransient<WebCrawler>(provider =>
                new WebCrawler(
                    provider.GetRequiredService<IMediator>(),
                    provider.GetRequiredService<ILinkVisitor>()));
        services.AddTransient<ILinkVisitor, LinkVisitor>();
        services.AddTransient<IVisitationPolicy, VisitationPolicy>();
    }
}
