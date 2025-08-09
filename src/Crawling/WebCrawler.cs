using System.Collections.Concurrent;
using Crawling.CrawlSource;
using Crawling.Frontier;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrieData;
using ILogger = Serilog.ILogger;

namespace Crawling;

public sealed class WebCrawler(IServiceProvider serviceProvider, IMediator mediator, Configuration configuration, ICrawler crawler, ILogger logger) : IDisposable
{
    private sealed record CrawlTask(Task Task, CancellationTokenSource CancellationTokenSource);
    
    private readonly SemaphoreSlim _semaphore = new(4, 4);
    private readonly ConcurrentDictionary<Guid, CrawlTask> _tasks = new();
        
    public async Task Start(CancellationToken cancellationToken)
    {
        var loggerWithContext = logger.ForContext<WebCrawler>();
        
        loggerWithContext.Information("Queuing initial seeds");
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
                
                loggerWithContext.Information("Crawl complete");
                break;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                var taskId = Guid.NewGuid();
                var cancellationTokenSource = new CancellationTokenSource();
                
                var task = ProcessCrawlTask(queuedTask, () => _semaphore.Release(), loggerWithContext, cancellationTokenSource.Token);
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
                loggerWithContext.Debug("Operation canceled");
            }
        }
    }

    private async Task ProcessCrawlTask(Task<ICrawlSource> source, Action onComplete, ILogger loggerWithContext, CancellationToken cancellationToken)
    {
        try
        {
            var trie = await crawler.Crawl(await source, configuration.Depth, cancellationToken);
            
            await WriteToFile(trie, configuration.Output, loggerWithContext, cancellationToken);

            // TODO: Fix this - i need some logging
            if (serviceProvider.GetService(typeof(IRequestHandler<>).MakeGenericType(typeof(CreateTrieCommandRequest))) is not null)
            {
                loggerWithContext.Information("Sending {request}", nameof(CreateTrieCommandRequest));
                await mediator.Send(new CreateTrieCommandRequest(trie), cancellationToken);
            }
        }
        finally
        {
            loggerWithContext.Information("");
            onComplete.Invoke();
        }
    }

    // TODO: Could be improved by using a StreamWriter
    private static async Task WriteToFile(Trie trie, DirectoryInfo directory, ILogger logger, CancellationToken cancellationToken)
    {
        logger.Information("Creating dot file for {name}", trie.Name);
        var dotNotation = trie.ToDotNotation();
        var filePath = Path.Combine(directory.FullName, $"{trie.Name}.dot");
        await File.WriteAllTextAsync(filePath, dotNotation, cancellationToken);
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
