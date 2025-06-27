using MediatR;

namespace Crawling;

internal class UriDiscoveredNotificationHandler : INotificationHandler<UriDiscoveredNotification>
{
    public Task Handle(UriDiscoveredNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Found an external uri {notification.Uri}");
        return Task.CompletedTask;
    }
}