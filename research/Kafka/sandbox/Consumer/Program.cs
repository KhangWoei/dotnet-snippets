using Confluent.Kafka;

namespace Consumer;

public static class Consumer
{
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    public static void Main(string[] args)
    {
        var configuration = new ConsumerConfig
        {
            AutoOffsetReset = AutoOffsetReset.Earliest,
            BootstrapServers = "localhost:99092",
            GroupId = "test-consumer-group"
        };

        var consumerBuilder = new ConsumerBuilder<Null, string>(configuration);

        using var consumer = consumerBuilder.Build();

        consumer.Subscribe("test-topic");

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            CancellationTokenSource.Cancel();
        };

        while (!CancellationTokenSource.IsCancellationRequested)
        {
            try
            {
                var message = consumer.Consume(CancellationTokenSource.Token); //polling is handled by the consume method, this is blocking
                Console.WriteLine(
                    $"Consumed message '{message.Message.Value}' at: '{message.TopicPartitionOffset}'");
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occured: {e.Error.Reason}");
            }
        }
        
        consumer.Close();
    }
}