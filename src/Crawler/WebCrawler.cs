using Crawler.Harvesting;
using Crawler.LinkVisiting;
using Crawler.Robots;

namespace Crawler;

public class WebCrawler(ILinkVisitor linkVisitor)
{
    public async Task Crawl(string seed, int depth, CancellationToken cancellationToken = default)
    {
        var source = await CrawlSource.Create(seed, depth, cancellationToken);

        var visitQueue = new Queue<Uri>();
        visitQueue.Enqueue(source.Base);
        
        var currentDepth = 0;
        while (visitQueue.Count > 0 && currentDepth < source.Depth)
        {
            var width = visitQueue.Count;

            for (var i = 0; i < width; i++)
            {
                var current = visitQueue.Dequeue();
                source.Seen.Add(current);

                var html = await linkVisitor.VisitAsync(current, cancellationToken);
                if (!string.IsNullOrEmpty(html))
                {
                    foreach (var link in LinkHarvester.Harvest(html))
                    {
                        // TODO - handle links that are not relative to the current source
                        if (source.Base.IsBaseOf(link))
                        {
                            if (source.CanVisit(link))
                            {
                                visitQueue.Enqueue(link);
                            }
                        }
                        // else send a new seed or base uri request and have the orchestration service handle it
                    }
                }

                Thread.Sleep(source.Robot.DelayMs);
            }
            
            currentDepth++;
        }
    }
}
