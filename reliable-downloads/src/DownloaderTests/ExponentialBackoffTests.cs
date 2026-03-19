using Downloader;

namespace DownloaderTests;

[TestFixture]
public class ExponentialBackoffTests
{
    [Test]
    public async Task ExecuteAsync_SucceedsOnFirstAttempt_ReturnsResult()
    {
        var backoff = new ExponentialBackoff(maxRetries: 3);

        var result = await backoff.ExecuteAsync(() => Task.FromResult(42), CancellationToken.None);

        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public async Task ExecuteAsync_FailsThenSucceeds_ReturnsResult()
    {
        var attempts = 0;
        var backoff = new ExponentialBackoff(maxRetries: 3, initialDelay: TimeSpan.FromMilliseconds(1));

        var result = await backoff.ExecuteAsync(() =>
        {
            if (++attempts < 2) throw new HttpRequestException("temporary");
            return Task.FromResult(42);
        }, CancellationToken.None);

        Assert.That(result, Is.EqualTo(42));
        Assert.That(attempts, Is.EqualTo(2));
    }

    [Test]
    public void ExecuteAsync_ExceedsMaxRetries_Throws()
    {
        var backoff = new ExponentialBackoff(maxRetries: 2, initialDelay: TimeSpan.FromMilliseconds(1));

        Assert.ThrowsAsync<HttpRequestException>(() =>
            backoff.ExecuteAsync<int>(() => throw new HttpRequestException("always fails"), CancellationToken.None));
    }
}
