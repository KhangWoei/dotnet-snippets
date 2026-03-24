using Downloader;
using DownloaderTests.Helpers;

namespace DownloaderTests;

[TestFixture]
public class DownloaderFactoryTests
{
    [Test]
    public async Task CreateAsync_WithAcceptRangesAndContentLength_ReturnsChunkedDownloader()
    {
        var response = new HttpResponseMessageBuilder().WithAcceptRange().WithContentLength(1000).Build();

        var factory = new DownloaderFactory(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)));

        var result = await factory.CreateAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ChunkedDownloader>());
    }

    [Test]
    public async Task CreateAsync_WithNoAcceptRanges_ReturnsWholeDownloader()
    {
        var response = new HttpResponseMessageBuilder().WithContentLength(1000).Build();

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
