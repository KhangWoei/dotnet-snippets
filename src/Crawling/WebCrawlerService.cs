using System.Collections.Concurrent;
using Crawling.CrawlSource;
using Crawling.Frontier;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Crawling;

public class WebCrawlerService(IHostApplicationLifetime lifetime, IMediator mediator, Configuration configuration, Crawler crawler) : BackgroundService
{
    private readonly ConcurrentDictionary<Guid, Task> _tasks = new();
    private readonly SemaphoreSlim _semaphore = new(4, 4);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await ProcessQueuedTasksAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            //ignored
        }
    }

    private async Task ProcessQueuedTasksAsync(CancellationToken cancellationToken)
    {
        await mediator.Publish(new UriDiscoveredNotification(configuration.Seed, configuration.Depth),
            cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            var queuedTask = await mediator.Send(new SeedRequest(), cancellationToken);

            if (queuedTask is null)
            {
                if (_tasks.Values.Any(t => !t.IsCompleted))
                {
                    continue;
                }

                break;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                var taskId = Guid.NewGuid();
                var crawlTask = ProcessCrawlTask(queuedTask, cancellationToken);
                _tasks.TryAdd(taskId, crawlTask);

                foreach (var completedTasks in _tasks.Where(task => task.Value.IsCompleted).Select(k => k.Key))
                {
                    _tasks.TryRemove(completedTasks, out _);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        lifetime.StopApplication();
    }
    
    private async Task ProcessCrawlTask(Task<ICrawlSource> source, CancellationToken cancellationToken)
    {
        try
        {
            await crawler.Crawl(await source, configuration.Depth, cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        var runningTasks = _tasks.Values.Where(t => !t.IsCompleted).ToList();
        if (runningTasks.Count != 0)
        {
            await Task.WhenAll(runningTasks).WaitAsync(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }

    public override void Dispose()
    {
        _semaphore.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}