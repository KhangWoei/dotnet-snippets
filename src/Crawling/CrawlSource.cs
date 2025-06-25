using Crawling.Robots;
using Crawling.TrieTree;

namespace Crawling;

internal sealed class CrawlSource
{
    private CrawlSource(Uri uri, int depth, Robot robot)
    {
        Depth = depth;
        Robot = robot;
        Seen = Trie.Create(uri);
        Queue = new PriorityQueue<Uri, int>();
        Queue.Enqueue(uri, -1);
    }
    
    public static async Task<CrawlSource> Create(string seed, int depth, CancellationToken cancellationToken)
    {
        var uri = new Uri(new Uri(seed).GetLeftPart(UriPartial.Authority));
        var robots = await RobotsHandler.GetDisallowedSites(uri, cancellationToken);

        return new CrawlSource(uri, depth, robots);
    }

    public int Depth { get; }
    
    public Robot Robot { get; }
    
    public Trie Seen { get; }
    
    public PriorityQueue<Uri, int> Queue { get; }
    
    public bool CanVisit(Uri uri) => !Seen.Contains(uri) && !Robot.Disallowed.Contains(uri);
}