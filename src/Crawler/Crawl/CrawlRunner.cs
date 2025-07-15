using Crawling;

namespace Crawl.Crawl;

internal class CrawlRunner(Crawler crawler)
{
    public async Task Run(string seed, int depth, int width, CancellationToken cancellationToken)
    {
        var configuration = new Configuration(seed, depth, width);

        await crawler.Crawl(configuration, cancellationToken);
    }
}