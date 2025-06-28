using Crawling.Robots;

namespace Crawling.CrawlSource;

internal sealed class CrawlSourceFactory(IHttpClientFactory httpClientFactory) : ICrawlSourceFactory
{
    public async Task<ICrawlSource> Create(string seed, int depth, CancellationToken cancellationToken)
    {
        var uriBuilder = new UriBuilder(seed)
        {
            Fragment = string.Empty,
            Query = string.Empty,
            Path = string.Empty
        };
        
        var client = httpClientFactory.CreateClient(uriBuilder.Host);

        var robots = await RobotsHandler.GetDisallowedSites(client, uriBuilder.Uri, cancellationToken);
        
        //TODO - Would be better to return a Task<Func<ICrawlSource, CancellationToken>> 
        return new CrawlSource(httpClientFactory, uriBuilder.Uri, robots, depth);
    }
}