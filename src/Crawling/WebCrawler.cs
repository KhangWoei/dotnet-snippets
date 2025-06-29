using Crawling.Frontier;
using Crawling.Harvesting;
using Crawling.LinkVisiting;
using MediatR;

namespace Crawling;

public class WebCrawler(IMediator mediator, ILinkVisitor linkVisitor)
{
    public async Task Crawl(Configuration configuration, CancellationToken cancellationToken = default)
    {
        var seenSeeds = new HashSet<string>();
        await mediator.Publish(new UriDiscoveredNotification(configuration.Seed, configuration.Depth), cancellationToken);
        var currentWidth = 0;

        // Matches the return of medaitor.Send(seedRequest) to { } and if so assign to source;
        // { } is a property or object pattern, it matches any object that has accessible properties
        while (await mediator.Send(new SeedRequest(), cancellationToken) is { } queuedSource && currentWidth++ < configuration.Width)
        {
            var source = await queuedSource;
            
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
                            var newSeed = link.GetLeftPart(UriPartial.Authority);
                            
                            if (seenSeeds.Add(newSeed))
                            {
                                _ =  mediator.Publish(new UriDiscoveredNotification(newSeed, configuration.Depth), cancellationToken);
                            }
                        }
                    }
                }

                await Task.Delay(source.Robot.DelayMs, cancellationToken);
            }
        }
    }
}