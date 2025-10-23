using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Crawl;

internal static class LoggingInitializer
{
    public static ILogger Initialize(DirectoryInfo outputDirectory, LogEventLevel logLevel)
    {
        var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd-HH_mm_ss");


        var structuredLogFile = Path.Combine(outputDirectory.FullName, $"crawler.structured-{dateTime}.log");
        var readableLogFile = Path.Combine(outputDirectory.FullName, $"crawler.readable-{dateTime}.log");

        var loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(new CompactJsonFormatter(), structuredLogFile, shared: true)
            .WriteTo.File(readableLogFile, shared: true)
            .MinimumLevel.ControlledBy(new LoggingLevelSwitch { MinimumLevel = logLevel});

        return loggerConfiguration.CreateLogger();
    }
}