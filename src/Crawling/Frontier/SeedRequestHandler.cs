using Crawling.CrawlSource;
using MediatR;

namespace Crawling.Frontier;

internal sealed class SeedRequestHandler(ISeedQueue<Task<ICrawlSource>> queue) : IRequestHandler<SeedRequest, Task<ICrawlSource>>
{
    public async Task<Task<ICrawlSource>> Handle(SeedRequest request, CancellationToken cancellationToken) => await queue.DequeueAsync(cancellationToken);
}