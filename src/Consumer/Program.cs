using Confluent.Kafka;
using Consumer;
using Consumer.Errors;
using FinnhubClient;
using FinnhubClient.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;

var applicationBuilder = Host.CreateApplicationBuilder();
applicationBuilder.Services.Configure<ConsumerOptions>(
    applicationBuilder.Configuration.GetSection(nameof(ConsumerOptions)));
applicationBuilder.Services.Configure<DatabaseOptions>(
    applicationBuilder.Configuration.GetSection(nameof(DatabaseOptions)));

using var host = applicationBuilder.Build();

var databaseOptions = host.Services.GetRequiredService<IOptions<DatabaseOptions>>().Value;
var datasourceBuilder = new NpgsqlDataSourceBuilder(databaseOptions.ConnectionString);
await using var dataSource = datasourceBuilder.Build();

var options = host.Services.GetRequiredService<IOptions<ConsumerOptions>>().Value;
var configuration = new ConsumerConfig
{
    BootstrapServers = options.BootstrapServers,
    GroupId = "database-write-consumer",
    EnableAutoOffsetStore = false,
    EnableAutoCommit = true,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnablePartitionEof = false,
};

using var consumer = new ConsumerBuilder<Ignore, Quote>(configuration)
    .SetValueDeserializer(new QuoteDeserializer())
    .Build();

consumer.Subscribe("test-topic");

var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cancellationTokenSource.Cancel();
};

var cancellationToken = cancellationTokenSource.Token;

try
{
    var symbolCache = new Dictionary<string, int>();

    while (true)
    {
        try
        {
            var result = consumer.Consume(cancellationToken);

            if (result is not null)
            {
                var quote = result.Message.Value!;

                var symbolId = await GetSymbolId(dataSource, symbolCache, quote.Symbol, cancellationToken);
                
                await CreateQuote(dataSource, quote.Timestamp, symbolId, quote.Price, cancellationToken);
            }
        }
        catch (ConsumeException exception)
        {
            Console.WriteLine(exception);
        }
        catch (UnknownSymbolException exception)
        {
            Console.WriteLine(exception);
        }
    }
}
finally
{
    consumer.Close();
}

async Task<int> GetSymbolId(NpgsqlDataSource source, Dictionary<string, int> cache, string symbol, CancellationToken ct)
{
    if (cache.TryGetValue(symbol, out var symbolId))
    {
        return symbolId;
    }

    await using var connection = source.CreateConnection();
    await connection.OpenAsync();

    await using var command = connection.CreateCommand();
    command.CommandText = "SELECT id FROM core.symbols WHERE symbol = @symbol;";
    command.Parameters.AddWithValue("symbol", symbol);

    var result = await command.ExecuteScalarAsync(ct) ?? throw new UnknownSymbolException(symbol);

    cache[symbol] = Convert.ToInt32(result);
    return cache[symbol];
}

async Task CreateQuote(NpgsqlDataSource source, DateTime time, int symbolId, decimal price, CancellationToken ct)
{
    await using var connection = source.CreateConnection();
    await connection.OpenAsync();
    
    await using var command = connection.CreateCommand();
    command.CommandText = $"INSERT INTO quote.quotes (timestamp, symbol_id, price) VALUES (@time, @symbolId, @price)";
    command.Parameters.AddWithValue("time", time);
    command.Parameters.AddWithValue("symbolId", symbolId);
    command.Parameters.AddWithValue("price", price);
    
    await command.ExecuteNonQueryAsync(ct);
    await connection.CloseAsync();
}