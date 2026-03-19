namespace Downloader;

public sealed class ExponentialBackoff(int maxRetries = 3, TimeSpan? initialDelay = null)
{
    private readonly TimeSpan _initialDelay = initialDelay ?? TimeSpan.FromSeconds(1);

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken)
    {
        var delay = _initialDelay;
        for (var attempt = 0; ; attempt++)
        {
            try
            {
                return await operation();
            }
            catch when (attempt < maxRetries)
            {
                await Task.Delay(delay, cancellationToken);
                delay += delay;
            }
        }
    }
}
