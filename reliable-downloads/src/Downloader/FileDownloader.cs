namespace Downloader;

public class FileDownloader(DownloaderFactory downloaderFactory)
{
    public async Task Download(string source, string destination, CancellationToken cancellationToken)
    {
        if (Uri.TryCreate(source, UriKind.Absolute, out var uri))
        {
            var downloader = await downloaderFactory.CreateAsync(uri, cancellationToken);
            
            // probably need to validate that the destination is valid
        }
        else
        {
            throw new UriFormatException($"{uri} is not a valid URI.");
        }
    }
}