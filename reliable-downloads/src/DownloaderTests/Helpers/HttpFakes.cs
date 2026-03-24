namespace DownloaderTests.Helpers;

public sealed class FakeHttpClientFactory(HttpMessageHandler handler) : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        return new HttpClient(handler);
    }
}