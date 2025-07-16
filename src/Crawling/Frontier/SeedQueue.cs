using System.Threading.Channels;
using Crawling.CrawlSource;

namespace Crawling.Frontier;

internal sealed class SeedQueue : ISeedQueue<Task<ICrawlSource>>
{
    private readonly Channel<Task<ICrawlSource>> _channel;

    public SeedQueue()
    {
        var options = new BoundedChannelOptions(100);
        _channel = Channel.CreateBounded<Task<ICrawlSource>>(options);
    }

    public async ValueTask EnqueueAsync(Task<ICrawlSource> item, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(item, cancellationToken);
    }

    public Task<ICrawlSource>? Dequeue()
    {
        _channel.Reader.TryRead(out var result);
        
        return result;
    }

    public int GetCount() => _channel.Reader.Count;
}