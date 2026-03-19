using Downloader;
using DownloaderTests.Helpers;

namespace DownloaderTests;

[TestFixture]
public class DownloaderFactoryTests
{
    [Test]
    public async Task CreateAsync_WithAcceptRangesAndContentLength_ReturnsChunkedDownloader()
    {
        var response = new HttpResponseMessage();
        response.Headers.Add("Accept-Ranges", "bytes");
        response.Content.Headers.ContentLength = 1000;

        var factory = new DownloaderFactory(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)));

        var result = await factory.CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ChunkedDownloader>());
    }

    [Test]
    public async Task CreateAsync_WithNoAcceptRanges_ReturnsWholeDownloader()
    {
        var response = new HttpResponseMessage();
        response.Content.Headers.ContentLength = 1000;

        var factory = new DownloaderFactory(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)));

        var result = await factory.CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<WholeDownloader>());
    }

    [Test]
    public async Task CreateAsync_WithNoContentLength_ReturnsWholeDownloader()
    {
        var response = new HttpResponseMessage();
        response.Headers.Add("Accept-Ranges", "bytes");
        response.Content.Headers.ContentLength = null;

        var factory = new DownloaderFactory(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)));

        var result = await factory.CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<WholeDownloader>());
    }

    [Test]
    public async Task CreateAsync_WithNeitherFlag_ReturnsWholeDownloader()
    {
        var response = new HttpResponseMessage();

        var factory = new DownloaderFactory(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)));

        var result = await factory.CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<WholeDownloader>());
    }
}
