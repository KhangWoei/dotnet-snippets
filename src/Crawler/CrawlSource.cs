using Crawler.Robots;

namespace Crawler;

internal sealed class CrawlSource
{
    private CrawlSource(Uri uri, Robot robot)
    {
        Base = uri;
        Robot = robot;
        Seen = [];
    }
    
    public Uri Base { get; init; }
    
    public Robot Robot { get; init; }
    
    public HashSet<Uri> Seen { get; init; }

    public static async Task<CrawlSource> Create(string seed, CancellationToken cancellationToken)
    {
        var uri = new Uri(new Uri(seed).GetLeftPart(UriPartial.Authority));
        var robots = await RobotsHandler.GetDisallowedSites(uri, cancellationToken);

        return new CrawlSource(uri, robots);
    }
}