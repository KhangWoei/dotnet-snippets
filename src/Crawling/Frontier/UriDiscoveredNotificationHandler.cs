using System.Collections.Concurrent;
using Crawling.CrawlSource;
using MediatR;

namespace Crawling.Frontier;

internal sealed class UriDiscoveredNotificationHandler(IVisitationPolicy policy, ICrawlSourceFactory factory, ISeedQueue<Task<ICrawlSource>> seedQueue, Configuration configuration) : INotificationHandler<UriDiscoveredNotification>
{
    // TODO - Wrap this in its own type
    // TODO - uri normalization and figure out how to ensure we don't requeue the same base uris
    private static ConcurrentDictionary<Uri, byte> _seen = [];
    private static int _currentWidth;
    
    public async Task Handle(UriDiscoveredNotification notification, CancellationToken cancellationToken)
    {
        if (_seen.TryAdd(notification.Seed, 1) 
            && await policy.ShouldVisit(notification.Seed, cancellationToken) 
            && Interlocked.Increment(ref _currentWidth) <= configuration.Width  )
        {
            var createCrawlSourceTask = factory.Create(notification.Seed.ToString(), notification.Depth, cancellationToken);
            await seedQueue.EnqueueAsync(createCrawlSourceTask, cancellationToken);

        }
    }
}