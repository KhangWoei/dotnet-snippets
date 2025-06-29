using System.Threading.Channels;
using Crawling.CrawlSource;

namespace Crawling.Frontier;

public class SeedQueue : ISeedQueue<Task<ICrawlSource>>
{
    private readonly Channel<Task<ICrawlSource>> _channel;

    public SeedQueue()
    {
        var options = new BoundedChannelOptions(100);
        _channel = Channel.CreateBounded<Task<ICrawlSource>>(options);
    }

    public async Task EnqueueAsync(Task<ICrawlSource> item, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(item, cancellationToken);
    }

    public async Task<Task<ICrawlSource>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }

    public int GetCount() => _channel.Reader.Count;
}