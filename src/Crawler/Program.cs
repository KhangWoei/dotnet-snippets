using System.CommandLine;
using Crawl.Crawl;

namespace Crawl;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var command = new CrawlCommand();
        var configuration = new CommandLineConfiguration(command);

        try
        {
            return await configuration.Parse(args).InvokeAsync();
        }
        catch (Exception)
        {
            return 1;
        }
    }
}