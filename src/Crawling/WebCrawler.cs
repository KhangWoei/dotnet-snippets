using System.Collections.Concurrent;
using Crawling.CrawlSource;
using Crawling.Frontier;
using MediatR;

namespace Crawling;

public sealed class WebCrawler(IMediator mediator, Configuration configuration, Crawler crawler) : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(4, 4);
    public async Task Start(CancellationToken cancellationToken)
    {
        ConcurrentDictionary<Guid, Task> tasks = new();
        
        await mediator.Publish(new UriDiscoveredNotification(configuration.Seed, configuration.Depth),
            cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            var queuedTask = await mediator.Send(new SeedRequest(), cancellationToken);

            if (queuedTask is null)
            {
                if (tasks.Values.Any(t => !t.IsCompleted))
                {
                    continue;
                }

                break;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                var taskId = Guid.NewGuid();
                var crawlTask = ProcessCrawlTask(queuedTask, () => _semaphore.Release(), cancellationToken);
                tasks.TryAdd(taskId, crawlTask);

                foreach (var completedTasks in tasks.Where(task => task.Value.IsCompleted).Select(k => k.Key))
                {
                    tasks.TryRemove(completedTasks, out _);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        if (cancellationToken.IsCancellationRequested)
        {
            var runningTasks = tasks.Values.Where(t => !t.IsCompleted).ToList();
            if (runningTasks.Count != 0)
            {
                await Task.WhenAll(runningTasks).WaitAsync(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }

    private async Task ProcessCrawlTask(Task<ICrawlSource> source, Action onComplete, CancellationToken cancellationToken)
    {
        try
        {
            await crawler.Crawl(await source, configuration.Depth, cancellationToken);
        }
        finally
        {
            onComplete.Invoke();
        }
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}