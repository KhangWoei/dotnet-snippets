namespace Downloader;

public interface IDownloader
{
    Task<byte[]> Download(Uri source, CancellationToken cancellationToken);
    Task DownloadToStream(Uri source, Stream stream, CancellationToken cancellationToken);
}