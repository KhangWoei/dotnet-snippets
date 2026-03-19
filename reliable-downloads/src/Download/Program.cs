using System.CommandLine;
using Downloader;
using Microsoft.Extensions.DependencyInjection;

namespace Download;

internal static class Program
{
    private static readonly Option<string> Source = new("--source")
    {
        Description = "Download source.",
        Required = true
    };

    private static readonly Option<string> Destination = new("--destination")
    {
        Description = "Where to write the file.",
        Required = true
    };

    public static async Task<int> Main(string[] args)
    {
        var root = new RootCommand("Tries to download a file.") { Source, Destination };
        root.SetAction(async (context, cancellationToken) =>
        {
            var source = context.GetValue(Source)!;
            var destination = context.GetValue(Destination)!;

            await using var provider = new ServiceCollection()
                .AddDownloader()
                .BuildServiceProvider();

            var downloader = provider.GetRequiredService<FileDownloader>();
            await downloader.Download(source, destination, cancellationToken);
        });
            
        var result = root.Parse(args);
        return await result.InvokeAsync();
    }
}
