using System.Collections.Concurrent;
using Crawling.CrawlSource;
using Crawling.Frontier;
using MediatR;

namespace Crawling;

public sealed class WebCrawler(IMediator mediator, Configuration configuration, ICrawler crawler) : IDisposable
{
    private sealed record CrawlTask(Task Task, CancellationTokenSource CancellationTokenSource);
    
    private readonly SemaphoreSlim _semaphore = new(4, 4);
    private readonly ConcurrentDictionary<Guid, CrawlTask> _tasks = new();
        
    public async Task Start(CancellationToken cancellationToken)
    {
        
        await mediator.Publish(new UriDiscoveredNotification(configuration.Seed, configuration.Depth), cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            var queuedTask = await mediator.Send(new SeedRequest(), cancellationToken);

            if (queuedTask is null)
            {
                if (_tasks.Values.Any(t => !t.Task.IsCompleted))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                    continue;
                }

                break;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                var taskId = Guid.NewGuid();
                var cancellationTokenSource = new CancellationTokenSource();
                var task = ProcessCrawlTask(queuedTask, () => _semaphore.Release(), cancellationTokenSource.Token);
                var crawlTask = new CrawlTask(task, cancellationTokenSource);

                _tasks.TryAdd(taskId, crawlTask);

                foreach (var completedTasks in _tasks.Where(t => t.Value.Task.IsCompleted).Select(k => k.Key))
                {
                    if (_tasks.TryRemove(completedTasks, out var t))
                    {
                        t.CancellationTokenSource.Dispose();
                    }
                }
            }
            catch (OperationCanceledException)
            {
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
        foreach (var task in _tasks.Values)
        {
            if (!task.Task.IsCompleted)
            {
                task.CancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
            }
        }

        try
        {
            Task.WhenAll(_tasks.Values.Select(t => t.Task)).GetAwaiter().GetResult();
        }
        catch (AggregateException) { }

        foreach (var task in _tasks.Values)
        {
            try
            {
                task.CancellationTokenSource.Dispose();
            } catch (ObjectDisposedException) { }
        }

        _semaphore.Dispose();
    }
}
