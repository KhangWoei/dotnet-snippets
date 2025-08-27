using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Producer.Client;

public class Finnhub
{
    private const string BaseAddress = "https://api.finnhub.io/api/v1/";
    private readonly HttpClient _client;

    private Finnhub(HttpClient client)
    {
        _client = client;
    }

    public static Finnhub Create(string apiKey)
    {
        var client = new HttpClient()
        {
            BaseAddress = new Uri(BaseAddress),
        };

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Finnhub-Token", apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return new Finnhub(client);
    }

    public async Task<string> GetQuoteAsync(string symbol, CancellationToken cancellationToken)
    {
        return await _client.GetStringAsync($"quote?symbol{symbol}", cancellationToken);
    }
}