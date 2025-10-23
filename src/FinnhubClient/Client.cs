using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace FinnhubClient;

public class Client
{
    private const string BaseAddress = "https://api.finnhub.io/api/v1/";
    private readonly HttpClient _client;

    public Client(HttpClient client, IOptions<FinnhubOptions> options) : this(client, options.Value.ApiKey)
    {
    }

    private Client(HttpClient client, string apiKey)
    {
        client.BaseAddress = new Uri(BaseAddress);
        client.DefaultRequestHeaders.Add("X-Finnhub-Token", apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _client = client;
    }

    public async Task<Quote> GetQuoteAsync(string symbol, CancellationToken cancellationToken)
    {
        var result = await _client.GetStreamAsync($"quote?symbol={symbol}", cancellationToken);
        var rawQuote = await JsonSerializer.DeserializeAsync<RawQuote>(result, cancellationToken: cancellationToken);

        return rawQuote is null
            ? throw new NullReferenceException() //TODO: throw a better error.
            : new Quote(symbol, rawQuote.Price, DateTime.UtcNow);
    }
    
    private class RawQuote
    {
        [JsonPropertyName("c")] public decimal Price { get; set; }
    }
}