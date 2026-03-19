namespace Downloader;

internal sealed class RetryingDownloader(IDownloader inner, ExponentialBackoff backoff) : IDownloader
{
    public Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken)
        => backoff.ExecuteAsync(() => inner.DownloadAsync(uri, cancellationToken), cancellationToken);
}
