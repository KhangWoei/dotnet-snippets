namespace Downloader;

public sealed class DownloaderFactory(IHttpClientFactory clientFactory)
{
    public async Task<IDownloader> CreateAsync(Uri uri, CancellationToken cancellationToken)
    {
        var client = clientFactory.CreateClient();
        var head = new HttpRequestMessage(HttpMethod.Head, uri);
        var response = await client.SendAsync(head, cancellationToken);

        var acceptsRange = response.Headers.AcceptRanges.Contains("bytes");
        var totalBytes = response.Content.Headers.ContentLength;

        if (acceptsRange && totalBytes is not null)
        {
            return new ChunkedDownloader((long) totalBytes);
        }

        return new WholeDownloader();
    }
}