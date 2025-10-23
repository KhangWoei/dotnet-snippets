using System.Text.Json;
using Confluent.Kafka;

namespace FinnhubClient.Kafka;

public class QuoteSerializer : ISerializer<Quote>
{
    public byte[] Serialize(Quote quote, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(quote);
    }
}