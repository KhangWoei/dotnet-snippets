namespace Downloader;

internal sealed class WholeDownloader(IHttpClientFactory clientFactory) : IDownloader
{
    public async Task<byte[]> Download(Uri source, CancellationToken cancellationToken)
    {
        var client = clientFactory.CreateClient();
        return await client.GetByteArrayAsync(source, cancellationToken);
    }

    public async Task DownloadToStream(Uri source, Stream stream, CancellationToken cancellationToken)
    {
        var client = clientFactory.CreateClient();
        var downloadStream = await client.GetStreamAsync(source, cancellationToken);

        await downloadStream.CopyToAsync(stream, cancellationToken);
    }
}