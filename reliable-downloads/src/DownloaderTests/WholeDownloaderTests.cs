using Downloader;
using DownloaderTests.Helpers;

namespace DownloaderTests;

[TestFixture]
public class WholeDownloaderTests
{
    [Test]
    public async Task DownloadAsync_ReturnsFileBytes()
    {
        var expected = new byte[] { 1, 2, 3, 4, 5 };
        var response = new HttpResponseMessage { Content = new ByteArrayContent(expected) };
        var downloader = new WholeDownloader(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)));

        var result = await downloader.DownloadAsync(new Uri("http://example.com/file"), CancellationToken.None);

        Assert.That(result, Is.EqualTo(expected));
    }
}
