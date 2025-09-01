using Confluent.Kafka;
using FinnhubClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Producer;

var applicationBuilder = Host.CreateApplicationBuilder();
applicationBuilder.Services.UseClient(applicationBuilder.Configuration);
applicationBuilder.Services.Configure<ProducerOptions>(applicationBuilder.Configuration.GetSection(nameof(ProducerOptions)));

using var host = applicationBuilder.Build();
var client = host.Services.GetRequiredService<Client>();

var options = host.Services.GetRequiredService<IOptions<ProducerOptions>>().Value;
var configuration = new ProducerConfig { BootstrapServers = options.BootstrapServers };
var producerBuilder = new ProducerBuilder<Null, string>(configuration);

using var producer = producerBuilder.Build();

try
{
    var result = await client.GetQuoteAsync("MSFT", CancellationToken.None);
    var message = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = result });
    Console.WriteLine($"Delivered '{message.Value} to {message.TopicPartitionOffset}'");
}
catch (Exception exception)
{
    Console.WriteLine(exception);
}
finally
{
    producer.Flush();
}
