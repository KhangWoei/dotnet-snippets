using Downloader;

namespace DownloaderTests.Helpers;

internal class FakeHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(response);
}

internal class RangeAwareHttpMessageHandler(byte[] data) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var range = request.Headers.Range?.Ranges.FirstOrDefault();
        if (range is not null)
        {
            var from = (int)(range.From ?? 0);
            var to = (int)(range.To ?? data.Length - 1);
            return Task.FromResult(new HttpResponseMessage
            {
                Content = new ByteArrayContent(data[from..(to + 1)])
            });
        }
        return Task.FromResult(new HttpResponseMessage { Content = new ByteArrayContent(data) });
    }
}

internal class FakeHttpClientFactory(HttpMessageHandler handler) : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => new(handler);
}

internal class FakeDownloader(Func<byte[]> handler) : IDownloader
{
    public Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken)
        => Task.FromResult(handler());
}

internal class FlakyHttpMessageHandler(int failuresBeforeSuccess, HttpMessageHandler inner) : HttpMessageHandler
{
    private int _attempts;

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (++_attempts <= failuresBeforeSuccess)
            throw new HttpRequestException("Temporary failure");
        var invoker = new HttpMessageInvoker(inner, disposeHandler: false);
        return invoker.SendAsync(request, cancellationToken);
    }
}

internal class FailingDownloader(int failureCount, byte[] successResult) : IDownloader
{
    private int _attempts;

    public Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken)
    {
        if (++_attempts <= failureCount)
            throw new HttpRequestException("Temporary failure");
        return Task.FromResult(successResult);
    }
}
