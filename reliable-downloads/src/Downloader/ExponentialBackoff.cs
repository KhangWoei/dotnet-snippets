namespace Downloader;

internal sealed class ExponentialBackoff(int retries = 3, TimeSpan? initialDelay = null)
{
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken)
    {
        var delay = initialDelay?? TimeSpan.FromSeconds(1);
        var exceptions = new List<Exception>();

        for (var attempt = 0; attempt < retries; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                await Task.Delay(delay, cancellationToken);
                delay += delay;
            }
        }

        throw new MaxRetriesExceeded(exceptions);
    }
}

public sealed class MaxRetriesExceeded(IEnumerable<Exception> innerExceptions)
    : AggregateException("Operation failed after maximum retries.", innerExceptions);