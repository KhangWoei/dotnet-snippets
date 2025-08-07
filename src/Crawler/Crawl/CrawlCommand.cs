using System.CommandLine;
using System.CommandLine.Parsing;
using Npgsql;

namespace Crawl.Crawl;

internal sealed class CrawlCommand : RootCommand
{
    private const string CrawlSeed = "CRAWL_SEED";
    private const string ConnectionStringVariable = "CRAWL_DB_CONNECTION";

    public CrawlCommand() : base("Crawl the web from given seed(s)")
    {
        Add(Seed);
        Add(Depth);
        Add(Width);
        Add(ConnectionString);
        Add(AsService);

        Validators.Add(result => Validate(Seed, ConnectionString, result));
        
        SetAction((result, cancellationToken) => CrawlInitializer.Run(
            result.GetRequiredValue(Seed)!,
            result.GetValue(Depth), 
            result.GetValue(Width),
            result.GetValue(AsService), 
            result.GetValue(ConnectionString),
            cancellationToken));
    }

    private static void Validate(Option<string?> seedOption, Option<string?> connectionOption, CommandResult result)
    {
        var seedResult = result.Children.SingleOrDefault(r => r is OptionResult o && o.Option == seedOption);

        if (seedResult is null || seedResult.Tokens.Count == 0 || string.IsNullOrEmpty(seedResult.Tokens[0].Value))
        {
            result.AddError($"Crawl seed must must be provided via `--seed` or in the {CrawlSeed} environment variable.");
        }

        var connectionStringResult = result.Children.SingleOrDefault(r => r is OptionResult o && o.Option == connectionOption);

        if (connectionStringResult is not null && connectionStringResult.Tokens.Count != 0 && !string.IsNullOrEmpty(connectionStringResult.Tokens[0].Value))
        {
            var connectionString = connectionStringResult.Tokens[0].Value;
            
            try
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString).ToString();
            }
            catch (ArgumentException)
            {
                result.AddError($"{connectionString} is not a valid PostgreSQL connection string.");
            }
            
        }
    }

    private static readonly Option<string?> Seed = new(name: "--seed")
    {
        Description = $"Website to start crawling from. Must either be provided or set in the environment variable {CrawlSeed}.",
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable(CrawlSeed),
        Required = false
    };

    private static readonly Option<int> Depth = new(name: "--depth")
    {
        Description = "Depth of the crawled site.",
        DefaultValueFactory = _ => 10,
        Required = false
    };

    private static readonly Option<int> Width = new(name: "--width")
    {
        Description = "Width of the crawler.",
        DefaultValueFactory = _ => 50,
        Required = false
    };

    private static readonly Option<string?> ConnectionString = new(name: "--connection-string")
    {
        Description = $"Website to start crawling from. Must either be provided or set in the environment variable {CrawlSeed}.",
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable(ConnectionStringVariable),
        Required = false
    };
    
    private static readonly Option<bool> AsService = new(name: "--as-service")
    {
        Description = "Run the crawler as a background service.",
        DefaultValueFactory = _ => false,
        Required = false
    };
}