using Crawler.Harvesting;
using Crawler.LinkVisiting;
using Crawler.Robots;

namespace Crawler;

public class WebCrawler(ILinkVisitor linkVisitor)
{
    private const int MaxDepth = 1;

    public async Task Crawl(string seed, CancellationToken cancellationToken = default)
    {
        var source = await CrawlSource.Create(seed, cancellationToken);

        var visitQueue = new Queue<Uri>();
        visitQueue.Enqueue(source.Base);
        
        var depth = 0;
        while (visitQueue.Count > 0)
        {
            var width = visitQueue.Count;

            for (var i = 0; i < width; i++)
            {
                var current = visitQueue.Dequeue();
                source.Seen.Add(current);

                var html = await linkVisitor.VisitAsync(current, cancellationToken);
                if (!string.IsNullOrEmpty(html))
                {
                    foreach (var links in LinkHarvester.Harvest(source.Base, html))
                    {
                        if (!source.Seen.Contains(links) && !source.Robot.Disallowed.Contains(links))
                        {
                            visitQueue.Enqueue(links);
                        }
                    }

                }

                Thread.Sleep(source.Robot.DelayMs);
            }

            if (depth == MaxDepth)
            {
                break;
            }

            depth++;
        }
    }
}
