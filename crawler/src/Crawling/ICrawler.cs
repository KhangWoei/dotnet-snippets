using Crawling.CrawlSource;
using TrieData;

namespace Crawling;

public interface ICrawler
{
    Task<Trie> Crawl(ICrawlSource source, int depth, CancellationToken cancellationToken);
}