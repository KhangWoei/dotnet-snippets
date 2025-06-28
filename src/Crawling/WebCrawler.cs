using Crawling.CrawlSource;
using Crawling.Frontier;
using Crawling.Harvesting;
using Crawling.LinkVisiting;
using MediatR;

namespace Crawling;

public class WebCrawler(IMediator mediator, ICrawlSourceFactory factory, ILinkVisitor linkVisitor)
{
    public async Task Crawl(Configuration configuration, CancellationToken cancellationToken = default)
    {
        var seenSeeds = new HashSet<string>();
        var seeds = new Queue<string>();
        seeds.Enqueue(configuration.Seed);
        var currentWidth = 0;
        
        while (seeds.Count > 0 && currentWidth < configuration.Width)
        {
            var currentSeed = seeds.Dequeue();
            var source = await factory.Create(currentSeed, configuration.Depth, cancellationToken);

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
                            await mediator.Publish(new UriDiscoveredNotification(link), cancellationToken);
                            var newSeed = link.GetLeftPart(UriPartial.Authority);
                            
                            if (!seenSeeds.Add(newSeed))
                            {
                                seeds.Enqueue(link.GetLeftPart(UriPartial.Authority));
                            }
                        }
                        
                    }
                }

                Thread.Sleep(source.Robot.DelayMs);
            }

            currentWidth++;
        }
    }
}