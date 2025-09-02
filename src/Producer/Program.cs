using Confluent.Kafka;
using Confluent.Kafka.Admin;
using FinnhubClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Producer;

var applicationBuilder = Host.CreateApplicationBuilder();
applicationBuilder.Services.UseClient(applicationBuilder.Configuration);
applicationBuilder.Services.Configure<ProducerOptions>(applicationBuilder.Configuration.GetSection(nameof(ProducerOptions)));
applicationBuilder.Services.Configure<QuoteOptions>(applicationBuilder.Configuration.GetSection(nameof(QuoteOptions)));

using var host = applicationBuilder.Build();
var client = host.Services.GetRequiredService<Client>();
var quotes = host.Services.GetRequiredService<IOptions<QuoteOptions>>().Value;

var options = host.Services.GetRequiredService<IOptions<ProducerOptions>>().Value;

var adminConfiguration = new AdminClientConfig
{
    BootstrapServers = options.BootstrapServers,
};

try
{
    using var admin = new AdminClientBuilder(adminConfiguration).Build();
    await admin.CreateTopicsAsync([
        new TopicSpecification
        {
            Name = "quotes",
        }
    ]);
}
catch (CreateTopicsException exception)
{
    if (exception.Results
        .Select(r => r.Error.Code)
        .Any(e => e != ErrorCode.TopicAlreadyExists && e != ErrorCode.NoError))
    {
        throw;
    }
}


var producerConfiguration = new ProducerConfig { BootstrapServers = options.BootstrapServers };
var producerBuilder = new ProducerBuilder<Null, string>(producerConfiguration);

using var producer = producerBuilder.Build();

try
{
    foreach (var symbol in quotes.Symbols)
    {
        var result = await client.GetQuoteAsync(symbol, CancellationToken.None);
        var message = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = result });
        Console.WriteLine($"Delivered '{message.Value} to {message.TopicPartitionOffset}'");
    }
}
catch (Exception exception)
{
    Console.WriteLine(exception);
}
finally
{
    producer.Flush();
}