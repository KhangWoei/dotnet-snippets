using System.Text.Json;
using Confluent.Kafka;

namespace FinnhubClient.Kafka;

public class QuoteDeserializer : IDeserializer<Quote>
{
    public Quote Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<Quote>(data) ?? throw new InvalidOperationException();
    }
}