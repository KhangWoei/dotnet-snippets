using Crawling.CrawlSource;

namespace Crawling.Frontier;

public interface ISeedQueue<T>
{
    ValueTask EnqueueAsync(T item, CancellationToken cancellationToken);

    Task<ICrawlSource>? Dequeue();

    int GetCount();
}