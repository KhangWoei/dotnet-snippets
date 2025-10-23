using Confluent.Kafka;

namespace Producer;

public static class Program
{
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    public static async Task Main()
    {
        var configuration = new ProducerConfig { BootstrapServers = "localhost:9092" };

        var producerBuilder = new ProducerBuilder<Null, string>(configuration);
        using var producer = producerBuilder.Build();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            CancellationTokenSource.CancelAsync();
        };

        while (!CancellationTokenSource.Token.IsCancellationRequested)
        {
            Console.WriteLine("Input:");
            var input = Console.ReadLine() ?? string.Empty;

            try
            {
                var message = await producer.ProduceAsync("test-topic", new Message<Null, string>() { Value = input });
                Console.WriteLine($"Delivered '{message.Value}' to {message.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }

        Console.WriteLine("Shutting down...");
        producer.Flush();
    }
}