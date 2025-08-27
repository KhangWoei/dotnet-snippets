using System.Net.Http.Headers;

namespace Producer;

public class Finnhub
{
    private const string _baseAddress = "https://api.finnhub.io/api/v1";
    private readonly HttpClient _client;

    private Finnhub(HttpClient client)
    {
        _client = client;
    }

    public static Finnhub Create(string apiKey)
    {
        var client = new HttpClient()
        {
            BaseAddress = new Uri(_baseAddress),
        };

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", apiKey);

        return new Finnhub(client);
    }
}
