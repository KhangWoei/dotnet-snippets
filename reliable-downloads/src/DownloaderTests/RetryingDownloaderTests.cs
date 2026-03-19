using Downloader;
using DownloaderTests.Helpers;

namespace DownloaderTests;

[TestFixture]
public class RetryingDownloaderTests
{
    [Test]
    public async Task DownloadAsync_DelegatesToInnerDownloader()
    {
        var expected = new byte[] { 1, 2, 3 };
        var inner = new FakeDownloader(() => expected);
        var downloader = new RetryingDownloader(inner, new ExponentialBackoff(maxRetries: 0));

        var result = await downloader.DownloadAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task DownloadAsync_RetriesOnFailure()
    {
        var expected = new byte[] { 1, 2, 3 };
        var inner = new FailingDownloader(failureCount: 1, successResult: expected);
        var downloader = new RetryingDownloader(inner, new ExponentialBackoff(maxRetries: 2, initialDelay: TimeSpan.FromMilliseconds(1)));

        var result = await downloader.DownloadAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.EqualTo(expected));
    }
}
