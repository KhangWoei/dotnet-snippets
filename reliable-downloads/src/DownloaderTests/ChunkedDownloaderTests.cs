using Downloader;
using DownloaderTests.Helpers;

namespace DownloaderTests;

[TestFixture]
public class ChunkedDownloaderTests
{
    [Test]
    public async Task DownloadAsync_ReturnsAssembledBytes()
    {
        var expected = Enumerable.Range(0, 100).Select(i => (byte)i).ToArray();
        var downloader = new ChunkedDownloader(
            new FakeHttpClientFactory(new RangeAwareHttpMessageHandler(expected)),
            expected.Length,
            new ExponentialBackoff(maxRetries: 0));

        var result = await downloader.DownloadAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task DownloadAsync_MultipleChunks_ReassemblesInOrder()
    {
        // 3 MB of data forces 3 chunks (each 1 MB)
        var expected = Enumerable.Range(0, 3 * 1024 * 1024).Select(i => (byte)(i % 256)).ToArray();
        var downloader = new ChunkedDownloader(
            new FakeHttpClientFactory(new RangeAwareHttpMessageHandler(expected)),
            expected.Length,
            new ExponentialBackoff(maxRetries: 0));

        var result = await downloader.DownloadAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task DownloadAsync_ChunkFailsOnce_RetriesAndReturnsCorrectBytes()
    {
        var expected = Enumerable.Range(0, 100).Select(i => (byte)i).ToArray();
        var inner = new RangeAwareHttpMessageHandler(expected);
        var flaky = new FlakyHttpMessageHandler(failuresBeforeSuccess: 1, inner);
        var downloader = new ChunkedDownloader(
            new FakeHttpClientFactory(flaky),
            expected.Length,
            new ExponentialBackoff(maxRetries: 1, initialDelay: TimeSpan.Zero));

        var result = await downloader.DownloadAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.EqualTo(expected));
    }
}
