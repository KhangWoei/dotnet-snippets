using MediatR;

namespace Crawling.Frontier;

internal record UriDiscoveredNotification(Uri Seed, int Depth) : INotification
{
    public UriDiscoveredNotification(string seed, int depth) : this(new UriBuilder(seed).Uri, depth) { }
}