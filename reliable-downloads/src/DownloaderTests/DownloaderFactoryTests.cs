using Downloader;
using DownloaderTests.Helpers;

namespace DownloaderTests;

[TestFixture]
public class DownloaderFactoryTests
{
    private static DownloaderFactory CreateFactory(HttpResponseMessage response)
        => new(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)), new ExponentialBackoff());

    [Test]
    public async Task CreateAsync_WithAcceptRangesAndContentLength_ReturnsChunkedDownloader()
    {
        var response = new HttpResponseMessage();
        response.Headers.Add("Accept-Ranges", "bytes");
        response.Content.Headers.ContentLength = 1000;

        var result = await CreateFactory(response).CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ChunkedDownloader>());
    }

    [Test]
    public async Task CreateAsync_WithNoAcceptRanges_ReturnsRetryingDownloader()
    {
        var response = new HttpResponseMessage();
        response.Content.Headers.ContentLength = 1000;

        var result = await CreateFactory(response).CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<RetryingDownloader>());
    }

    [Test]
    public async Task CreateAsync_WithNoContentLength_ReturnsRetryingDownloader()
    {
        var response = new HttpResponseMessage();
        response.Headers.Add("Accept-Ranges", "bytes");
        response.Content.Headers.ContentLength = null;

        var result = await CreateFactory(response).CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<RetryingDownloader>());
    }

    [Test]
    public async Task CreateAsync_WithNeitherFlag_ReturnsRetryingDownloader()
    {
        var response = new HttpResponseMessage();

        var result = await CreateFactory(response).CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<RetryingDownloader>());
    }
}
