using System.Text.Json;
using Confluent.Kafka;
using Consumer;
using FinnhubClient;
using FinnhubClient.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var applicationBuilder = Host.CreateApplicationBuilder();
applicationBuilder.Services.Configure<ConsumerOptions>(applicationBuilder.Configuration.GetSection(nameof(ConsumerOptions)));

using var host = applicationBuilder.Build();

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

try
{
    while (true)
    {
        try
        {
            var result = consumer.Consume(cancellationTokenSource.Token);
            Console.WriteLine(result.Message.Value);
        }
        catch (ConsumeException exception)
        {
            Console.WriteLine(exception);
        }
    }
}
finally
{
    consumer.Close();
}