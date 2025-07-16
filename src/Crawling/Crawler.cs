using Crawling.CrawlSource;
using Crawling.Frontier;
using Crawling.Harvesting;
using Crawling.LinkVisiting;
using MediatR;

namespace Crawling;

internal sealed class Crawler(IMediator mediator, ILinkVisitor linkVisitor) : ICrawler
{
    public async Task Crawl(ICrawlSource source, int depth, CancellationToken cancellationToken)
    {
        while (source.Queue.TryDequeue(out var current, out var currentDepth) && currentDepth < source.Depth)
        {
            var client = source.CreateClient();

            var html = await linkVisitor.VisitAsync(client, current, cancellationToken);
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
                    else
                    {
                        _ = mediator.Publish(new UriDiscoveredNotification(link, depth),
                            cancellationToken);
                    }
                }
            }

            await Task.Delay(source.Robot.DelayMs, cancellationToken);
        }
    }
}