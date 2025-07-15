using System.Collections.Concurrent;
using Crawling.Frontier;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Crawling;

public class WebCrawler(IMediator mediator, IOptions<Configuration> configuration, Crawler crawler) : IHostedService, IDisposable
{
    private readonly ConcurrentDictionary<Guid, Task> _tasks = new();
    private readonly SemaphoreSlim _semaphore = new(4, 4);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = ProcessQueuedTasksAsync(cancellationToken);
        return Task.CompletedTask;
    }

    private async Task ProcessQueuedTasksAsync(CancellationToken cancellationToken)
    {
        await mediator.Publish(new UriDiscoveredNotification(configuration.Value.Seed, configuration.Value.Depth), cancellationToken);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var queuedTask = await mediator.Send(new SeedRequest(), cancellationToken);

            if (queuedTask is null)
            {
                if (_tasks.Values.Any(t => t.Status == TaskStatus.Running))
                {
                    continue;
                }

                break;
            }
            
            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                var taskId = Guid.NewGuid();
                var crawlTaskCompletionSource = new TaskCompletionSource();
                var crawlTask = crawlTaskCompletionSource.Task;
                _tasks.TryAdd(taskId, crawlTask);
                
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var source = await queuedTask;
                        await crawler.Crawl(source, configuration.Value.Depth, cancellationToken);
                        crawlTaskCompletionSource.SetResult();
                    }
                    catch (OperationCanceledException)
                    {
                        crawlTaskCompletionSource.TrySetCanceled();
                    }
                    catch (Exception ex)
                    {
                        crawlTaskCompletionSource.SetException(ex);
                    }
                    finally
                    {
                        _tasks.TryRemove(taskId, out _);
                    }
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var runningTasks = _tasks.Values.Where(t => t.Status == TaskStatus.Running).ToList();
        if (runningTasks.Count != 0)
        {
            await Task.WhenAll(runningTasks).WaitAsync(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}