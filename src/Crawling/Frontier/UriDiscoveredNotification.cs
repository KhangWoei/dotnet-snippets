using MediatR;

namespace Crawling.Frontier;

internal record UriDiscoveredNotification(string Seed, int Depth) : INotification;