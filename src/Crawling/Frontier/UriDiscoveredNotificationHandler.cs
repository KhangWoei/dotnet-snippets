using Crawling.CrawlSource;
using MediatR;

namespace Crawling.Frontier;

internal class UriDiscoveredNotificationHandler(ICrawlSourceFactory factory, ISeedQueue<ICrawlSource> seedQueue) : INotificationHandler<UriDiscoveredNotification>
{
    // TODO - wrap this with a pre processor to check if the link we are trying to queue is
    // 1. Is a new seed
    // 2. Is the response type we want to handle
    public async Task Handle(UriDiscoveredNotification notification, CancellationToken cancellationToken)
    {
        // TODO - this is wrong, this or the factory or something else should to check if we've seen the URI, and we shouldn't need to pass the depth, maybe need an overload also to accept a URI
        var crawlSource = await factory.Create(notification.Seed, notification.Depth, cancellationToken);
        await seedQueue.EnqueueAsync(crawlSource, cancellationToken);
    }
}