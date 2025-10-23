using Crawling.Robots;
using TrieData;

namespace Crawling.CrawlSource;

public interface ICrawlSource
{
    Uri Source { get; }
    PriorityQueue<Uri, int> Queue { get; }
    
    Trie Seen { get; }
    
    IRobot Robot { get; }
    
    int Depth { get; }

    HttpClient CreateClient();
    
    bool CanVisit(Uri uriToVisit);
}