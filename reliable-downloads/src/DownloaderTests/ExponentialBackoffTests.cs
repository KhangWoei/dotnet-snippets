using Downloader;

namespace DownloaderTests;

[TestFixture]
public class ExponentialBackoffTests
{
    [Test]
    public async Task ExecuteAsync_WhenOperationSucceeds_ReturnsResult()
    {
        var backoff = Create();

        var result = await backoff.ExecuteAsync(() => Task.FromResult(42), CancellationToken.None);

        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public async Task ExecuteAsync_WhenOperationFailsThenSucceeds_ReturnsResult()
    {
        var backoff = Create();
        var callCount = 0;

        Task<int> Operation() => ++callCount < 3
            ? throw new InvalidOperationException($"Attempt {callCount}")
            : Task.FromResult(42);

        var result = await backoff.ExecuteAsync(Operation, CancellationToken.None);

        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public void ExecuteAsync_WhenAllRetriesExhausted_ThrowsMaxRetriesExceeded()
    {
        var backoff = Create();

        Task<int> Operation() => throw new InvalidOperationException("always fails");

        Assert.ThrowsAsync<MaxRetriesExceeded>(() => backoff.ExecuteAsync(Operation, CancellationToken.None));
    }

    [Test]
    public void ExecuteAsync_WhenAllRetriesExhausted_CollectsAllExceptions()
    {
        var backoff = Create();
        var callCount = 0;

        Task<int> Operation() => throw new InvalidOperationException($"Attempt {++callCount}");

        var ex = Assert.ThrowsAsync<MaxRetriesExceeded>(() => backoff.ExecuteAsync(Operation, CancellationToken.None));

        Assert.That(ex!.InnerExceptions, Has.Count.EqualTo(3));
    }

    [Test]
    public void ExecuteAsync_WithDefaultRetries_AttemptsThreeTimes()
    {
        var backoff = Create();
        var callCount = 0;

        Task<int> Operation() => throw new InvalidOperationException($"Attempt {++callCount}");

        Assert.ThrowsAsync<MaxRetriesExceeded>(() => backoff.ExecuteAsync(Operation, CancellationToken.None));

        Assert.That(callCount, Is.EqualTo(3));
    }


    [Test]
    public void ExecuteAsync_WithCustomRetries_RespectsRetryCount()
    {
        var backoff = Create(5);
        var callCount = 0;

        Task<int> Operation() => throw new InvalidOperationException($"Attempt {++callCount}");

        Assert.ThrowsAsync<MaxRetriesExceeded>(() => backoff.ExecuteAsync(Operation, CancellationToken.None));

        Assert.That(callCount, Is.EqualTo(5));
    }
    
    
    
    
    private static ExponentialBackoff Create(int retries = 3, TimeSpan? delay = null) => new(retries, delay ?? TimeSpan.FromMilliseconds(1));
}
