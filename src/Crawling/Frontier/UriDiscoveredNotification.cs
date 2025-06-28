using MediatR;

namespace Crawling.Frontier;

internal record UriDiscoveredNotification(Uri Uri) : INotification;