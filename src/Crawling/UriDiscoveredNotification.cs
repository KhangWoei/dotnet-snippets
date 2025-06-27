using MediatR;

namespace Crawling;

internal record UriDiscoveredNotification(Uri Uri) : INotification;