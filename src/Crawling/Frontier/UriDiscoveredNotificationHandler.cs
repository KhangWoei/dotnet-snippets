using Crawling.CrawlSource;
using MediatR;

namespace Crawling.Frontier;

internal class UriDiscoveredNotificationHandler(IVisitationPolicy policy, ICrawlSourceFactory factory, ISeedQueue<Task<ICrawlSource>> seedQueue) : INotificationHandler<UriDiscoveredNotification>
{
    // TODO - wrap this with a pre processor to check if the link we are trying to queue is
    // 1. Is a new seed
    // 2. Is the response type we want to handle
    public async Task Handle(UriDiscoveredNotification notification, CancellationToken cancellationToken)
    {
        if (await policy.ShouldVisit(notification.Seed, cancellationToken))
        {
            await seedQueue.EnqueueAsync(factory.Create(notification.Seed, notification.Depth, cancellationToken), cancellationToken);
        }
    }
}