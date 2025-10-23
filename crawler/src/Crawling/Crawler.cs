using Crawling.CrawlSource;
using Crawling.Frontier;
using Crawling.Harvesting;
using Crawling.LinkVisiting;
using MediatR;
using TrieData;
using ILogger = Serilog.ILogger;

namespace Crawling;

internal sealed class Crawler(IMediator mediator, ILinkVisitor linkVisitor, ILogger logger) : ICrawler
{
    public async Task<Trie> Crawl(ICrawlSource source, int depth, CancellationToken cancellationToken)
    {
        var loggerWithContext = logger.ForContext("seed", source.Source);
        loggerWithContext.Information("Crawl starting");

        while (source.Queue.TryDequeue(out var current, out var currentDepth) && currentDepth < source.Depth)
        {
            var html = await linkVisitor.VisitAsync(current, cancellationToken);
            if (!string.IsNullOrEmpty(html))
            {
                loggerWithContext.Debug("Harvesting {link}", current);
                foreach (var link in LinkHarvester.Harvest(html))
                {
                    if (!source.CanVisit(link))
                    {
                        loggerWithContext.Debug("Dropping {link}", link);
                        continue;
                    }

                    if (!source.Source.IsBaseOf(link))
                    {
                        // This uses the configuration's depth not the current depth as we are crawling a different site entirely
                        loggerWithContext.Debug("Relegating {link}", link);
                        _ = mediator.Publish(new UriDiscoveredNotification(link, depth), cancellationToken);
                        continue;
                    }

                    if (source.Seen.TryInsert(link))
                    {
                        loggerWithContext.Debug("Queuing {link}", link);
                        source.Queue.Enqueue(link, currentDepth + 1);
                    }
                }
            }
            
            await Task.Delay(source.Robot.DelayMs, cancellationToken);
        }

        loggerWithContext.Information("Crawl completed");
        return source.Seen;
    }
}