using Crawling.CrawlSource;

namespace Crawling;

public interface ICrawler
{
    Task Crawl(ICrawlSource source, int depth, CancellationToken cancellationToken);
}