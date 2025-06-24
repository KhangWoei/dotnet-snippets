namespace ServiceQueue;

public class Worker(IServiceQueue queue, ILogger<Worker> logger): BackgroundService
{
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

                _ = work(stoppingToken);
            }
            catch (OperationCanceledException) {}
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Name} is stopping.", nameof(Worker));
        
        await base.StopAsync(cancellationToken);
    }
}
