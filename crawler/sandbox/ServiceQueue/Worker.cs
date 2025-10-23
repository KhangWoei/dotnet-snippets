namespace ServiceQueue;

public class Worker(IServiceQueue queue, ILogger<Worker> logger): BackgroundService
{
    /*
     * initialCount - number of resource access available immediately,
     *                number of tasks that can run without waiting
     * maximumCount - max number of resource we have at any time
     *
     * usually initialCount == maximumCount, if initialCount < maximumCount indicates a ramping behaviour
     *                
     */
    private readonly SemaphoreSlim _semaphore = new(4, 4);
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("""
                              {Name} is running.
                              Tap W to add a work item to the background queue.
                              """, nameof(Worker));
        
        return ProcessQueuedTaskAsync(stoppingToken);
    }

    private async Task ProcessQueuedTaskAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var work = await queue.DequeueAsync(stoppingToken);
               
                /*
                 * Basically asks the semaphore if a permit is available, if no it will block
                 * until one is available
                 */
                await _semaphore.WaitAsync(stoppingToken);
                
                var taskId = new Guid().ToString();
                _ = Execute(taskId, work, stoppingToken); 
            }
            catch (OperationCanceledException) {}
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }

    private async Task Execute(string taskId, Func<CancellationToken, ValueTask> task, CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting work item {ID}", taskId);
        await task(stoppingToken);
        
        /*
         * Releases permit
         */
        _semaphore.Release();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Name} is stopping.", nameof(Worker));
        
        await base.StopAsync(cancellationToken);
    }
}
