using Crawling.Robots;
using TrieData;

namespace Crawling.CrawlSource;

public interface ICrawlSource
{
    string Source { get; }
    PriorityQueue<Uri, int> Queue { get; }
    
    ITrie Seen { get; }
    
    IRobot Robot { get; }
    
    int Depth { get; }

    HttpClient CreateClient();
    
    bool CanVisit(Uri uriToVisit);
}