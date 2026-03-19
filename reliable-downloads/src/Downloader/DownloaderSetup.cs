using Microsoft.Extensions.DependencyInjection;

namespace Downloader;

public static class DownloaderSetup
{
    public static IServiceCollection AddDownloader(this IServiceCollection services)
    {
        return services.AddHttpClient()
            .AddTransient<FileDownloader>()
            .AddTransient<DownloaderFactory>()
            .AddSingleton<ExponentialBackoff>();
    }
}