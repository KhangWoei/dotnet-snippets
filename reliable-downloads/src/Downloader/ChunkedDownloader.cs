namespace Downloader;

internal sealed class ChunkedDownloader(long total) : IDownloader
{
    private readonly long _total = total;

    public Task<byte[]> Download(Uri source, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DownloadToStream(Uri source, Stream stream, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}