using Crawling.CrawlSource;
using Crawling.Frontier;
using Crawling.LinkVisiting;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Crawling;

public static class CrawlerDependencies
{
    public static void UserCrawler(this IServiceCollection services, Configuration configuration)
    {
        
        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Crawler>());
        services.AddHttpClient();

        services.AddSingleton(configuration);

        services.AddSingleton<WebCrawler>();
        services.AddSingleton<ICrawlSourceFactory, CrawlSourceFactory>();
        services.AddSingleton<ISeedQueue<Task<ICrawlSource>>, SeedQueue>();

        services.AddTransient<ICrawler, Crawler>();
        services.AddTransient<ILinkVisitor, LinkVisitor>();
        services.AddTransient<IVisitationPolicy, VisitationPolicy>();
    }
}
