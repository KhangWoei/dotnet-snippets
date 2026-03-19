namespace Downloader;

public interface IDownloader
{
    Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken);
}