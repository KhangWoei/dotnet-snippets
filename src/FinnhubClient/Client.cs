using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace FinnhubClient;

public class Client
{
    private const string BaseAddress = "https://api.finnhub.io/api/v1/";
    private readonly HttpClient _client;

    public Client(HttpClient client, IOptions<FinnhubOptions> options) : this(client, options.Value.ApiKey) { }
    
    private Client(HttpClient client, string apiKey)
    {
        client.BaseAddress = new Uri(BaseAddress);
        client.DefaultRequestHeaders.Add("X-Finnhub-Token",  apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        _client = client;
    }


    public async Task<string> GetQuoteAsync(string symbol, CancellationToken cancellationToken) => 
        await _client.GetStringAsync($"quote?symbol{symbol}", cancellationToken);
}

public class FinnhubOptions
{
    public string ApiKey { get; init; }
}
