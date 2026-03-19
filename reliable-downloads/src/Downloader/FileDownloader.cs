namespace Downloader;

public class FileDownloader(DownloaderFactory downloaderFactory)
{
    public async Task Download(string source, string destination, CancellationToken cancellationToken)
    {
        if (Uri.TryCreate(source, UriKind.Absolute, out var uri))
        {
            var downloader = await downloaderFactory.CreateAsync(uri, cancellationToken);
            var bytes = await downloader.DownloadAsync(uri, cancellationToken);
            await File.WriteAllBytesAsync(destination, bytes, cancellationToken);
        }
        else
        {
            throw new UriFormatException($"{source} is not a valid URI.");
        }
    }
}
