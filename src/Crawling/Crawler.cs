using Crawling.CrawlSource;
using Crawling.Frontier;
using Crawling.Harvesting;
using Crawling.LinkVisiting;
using MediatR;
using TrieData;

namespace Crawling;

internal sealed class Crawler(IMediator mediator, ILinkVisitor linkVisitor) : ICrawler
{
    public async Task<Trie> Crawl(ICrawlSource source, int depth, CancellationToken cancellationToken)
    {
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

                    if (!source.Source.IsBaseOf(link))
                    {
                        // This uses the configuration's depth not the current depth as we are crawling a different site entirely
                        logger.Debug("Relegating {link}", link);
                        _ = mediator.Publish(new UriDiscoveredNotification(link, depth), cancellationToken);
                        continue;
                    }

                    if (source.Seen.TryInsert(link))
                    {
                        source.Queue.Enqueue(link, currentDepth + 1);
                    }
                    else
                    {
                        // This uses the configuration's depth not the current depth as we are crawling a different site entirely
                        _ = mediator.Publish(new UriDiscoveredNotification(link, depth), cancellationToken);
                    }
                }
            }

            await Task.Delay(source.Robot.DelayMs, cancellationToken);
        }

        return source.Seen;
    }
}