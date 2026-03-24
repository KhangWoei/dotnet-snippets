namespace DownloaderTests.Helpers;

public class FakeHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) => await Task.FromResult(response);
}