using Downloader;
using DownloaderTests.Helpers;

namespace DownloaderTests;

[TestFixture]
public class WholeDownloaderTests
{
    [Test]
    public async Task Download_ReturnsFileBytes()
    {
        var expected = new byte[] { 1, 2, 3, 4, 5};
        var response = new HttpResponseMessageBuilder()
            .WithContent(new ByteArrayContent(expected))
            .Build();

        var result = await Create(response).Download(new Uri("http://fake.com"), CancellationToken.None);
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task DownloadToStream_WritesToStream()
    {
        const string expected = "result";
        var response = new HttpResponseMessageBuilder()
            .WithContent(new StringContent(expected))
            .Build();
        
        var stream = new MemoryStream();
        await Create(response).DownloadToStream(new Uri("http://fake.com"), stream, CancellationToken.None);

        stream.Seek(0, SeekOrigin.Begin);
        var result = await new StreamReader(stream).ReadToEndAsync();
        
        Assert.That(result, Is.EqualTo(expected));
    }

    private static WholeDownloader Create(HttpResponseMessage response) => new(new FakeHttpClientFactory(new FakeHttpMessageHandler(response)));
}
