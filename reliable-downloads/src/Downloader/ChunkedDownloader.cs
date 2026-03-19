namespace Downloader;

internal sealed class ChunkedDownloader(long total) : IDownloader
{
    private readonly long _total = total;
    
    public byte[] Download(Uri source)
    {
        throw new NotImplementedException();
    }
}