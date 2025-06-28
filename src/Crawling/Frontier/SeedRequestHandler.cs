using Crawling.CrawlSource;
using MediatR;

namespace Crawling.Frontier;

internal sealed class SeedRequestHandler(ISeedQueue<ICrawlSource> queue) : IRequestHandler<SeedRequest, ICrawlSource>
{
    public async Task<ICrawlSource> Handle(SeedRequest request, CancellationToken cancellationToken) => await queue.DequeueAsync(cancellationToken);
}