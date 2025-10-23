namespace Crawling.CrawlSource;

public interface ICrawlSourceFactory
{
    Task<ICrawlSource> Create(string seed, int depth, CancellationToken cancellationToken);
}