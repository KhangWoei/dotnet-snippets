using Crawler.Harvesting;
using Crawler.LinkVisiting;
using Crawler.Robots;

namespace Crawler;

public class WebCrawler(ILinkVisitor linkVisitor)
{
    public async Task Crawl(string seed, int depth, CancellationToken cancellationToken = default)
    {
        var source = await CrawlSource.Create(seed, depth, cancellationToken);

        while (source.Queue.TryDequeue(out var current, out var currentDepth) && currentDepth < source.Depth)
        {
            var html = await linkVisitor.VisitAsync(current, cancellationToken);
            if (!string.IsNullOrEmpty(html))
            {
                foreach (var link in LinkHarvester.Harvest(html))
                {
                    if (!source.CanVisit(link))
                    {
                        continue;
                    }

                    if (source.Seen.TryInsert(link))
                    {
                        source.Queue.Enqueue(link, currentDepth + 1);
                    }
                    // else send a new seed or base uri request and have the orchestration service handle it
                }
            }

            Thread.Sleep(source.Robot.DelayMs);
        }
    }
}