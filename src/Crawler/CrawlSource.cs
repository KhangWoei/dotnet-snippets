using Crawler.Robots;

namespace Crawler;

internal sealed class CrawlSource
{
    private CrawlSource(Uri uri, int depth, Robot robot)
    {
        Base = uri;
        Depth = depth;
        Robot = robot;
        Seen = [];
    }
    
    public Uri Base { get; init; }
    
    public int Depth { get; init; }
    
    public Robot Robot { get; init; }
    
    // TODO -  Kinda want to use a trie tree, would make it easier to reconstruct the output or map of the site it just crawled
    public HashSet<Uri> Seen { get; init; }

    public static async Task<CrawlSource> Create(string seed, int depth, CancellationToken cancellationToken)
    {
        var uri = new Uri(new Uri(seed).GetLeftPart(UriPartial.Authority));
        var robots = await RobotsHandler.GetDisallowedSites(uri, cancellationToken);

        return new CrawlSource(uri, depth, robots);
    }
}