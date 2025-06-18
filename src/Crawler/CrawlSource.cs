using Crawler.Robots;

namespace Crawler;

internal sealed class CrawlSource
{
    private CrawlSource(Uri uri, int depth, Robot robot)
    {
        Base = uri;
        Depth = depth;
        Robot = robot;
        Seen = [uri];
        Queue = new PriorityQueue<Uri, int>();
        Queue.Enqueue(uri, -1);
    }
    
    public static async Task<CrawlSource> Create(string seed, int depth, CancellationToken cancellationToken)
    {
        var uri = new Uri(new Uri(seed).GetLeftPart(UriPartial.Authority));
        var robots = await RobotsHandler.GetDisallowedSites(uri, cancellationToken);

        return new CrawlSource(uri, depth, robots);
    }
    
    public Uri Base { get; }
    
    public int Depth { get; }
    
    public Robot Robot { get; }
    
    // TODO -  Kinda want to use a trie tree, would make it easier to reconstruct the output or map of the site it just crawled
    public HashSet<Uri> Seen { get; }
    
    public PriorityQueue<Uri, int> Queue { get; }
    
    public bool CanVisit(Uri uri) => !Seen.Contains(uri) && !Robot.Disallowed.Contains(uri);
}