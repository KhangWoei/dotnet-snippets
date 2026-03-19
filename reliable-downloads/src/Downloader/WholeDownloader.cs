namespace Downloader;

internal sealed class WholeDownloader(IHttpClientFactory clientFactory) : IDownloader
{
    public async Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken)
    {
        var client = clientFactory.CreateClient();
        return await client.GetByteArrayAsync(uri, cancellationToken);
    }
}
