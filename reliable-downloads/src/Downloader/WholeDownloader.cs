namespace Downloader;

internal sealed class WholeDownloader(IHttpClientFactory clientFactory) : IDownloader
{
    public async Task<byte[]> Download(Uri source, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DownloadToStream(Uri source, Stream stream, CancellationToken cancellationToken)
    }
}