using System.Threading.Channels;
using Crawling.CrawlSource;

namespace Crawling.Frontier;

public class SeedQueue : ISeedQueue<ICrawlSource>
{
    private readonly Channel<ICrawlSource> _channel;

    public SeedQueue()
    {
        var options = new BoundedChannelOptions(100);
        _channel = Channel.CreateBounded<ICrawlSource>(options);
    }

    public async Task EnqueueAsync(ICrawlSource item, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(item, cancellationToken);
    }

    public async Task<ICrawlSource> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }

    public int GetCount() => _channel.Reader.Count;
}