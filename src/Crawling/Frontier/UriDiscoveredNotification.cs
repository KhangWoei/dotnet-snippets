using MediatR;

namespace Crawling.Frontier;

internal sealed record UriDiscoveredNotification(Uri Seed, int Depth) : INotification
{
    public UriDiscoveredNotification(string seed, int depth) : this(new UriBuilder(seed).Uri, depth) { }
}