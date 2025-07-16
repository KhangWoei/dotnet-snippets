using Microsoft.Extensions.Hosting;

namespace Crawling;

public class WebCrawlerService(IHostApplicationLifetime lifetime, WebCrawler webCrawler) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await webCrawler.Start(cancellationToken);
            lifetime.StopApplication();
        }
        catch (OperationCanceledException)
        {
            //ignored
        }
    }
}