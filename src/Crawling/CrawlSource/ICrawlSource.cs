using Crawling.Robots;
using Crawling.TrieTree;

namespace Crawling.CrawlSource;

public interface ICrawlSource
{
    PriorityQueue<Uri, int> Queue { get; }
    
    ITrie Seen { get; }
    
    IRobot Robot { get; }
    
    int Depth { get; }

    bool CanVisit(Uri uriToVisit);
}