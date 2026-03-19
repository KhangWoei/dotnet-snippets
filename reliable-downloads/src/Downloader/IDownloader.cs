namespace Downloader;

public interface IDownloader
{
    byte[] Download(Uri source);
}