namespace Crawling.Frontier;

public interface ISeedQueue<T>
{
    Task EnqueueAsync(T item, CancellationToken cancellationToken);

    Task<T> DequeueAsync(CancellationToken cancellationToken);

    int GetCount();
}