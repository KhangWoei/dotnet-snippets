using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinnhubClient;

public static class ClientSetup
{
    public static IHttpClientBuilder UseClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FinnhubOptions>(configuration.GetSection(nameof(FinnhubOptions)));
        
        return services.AddHttpClient<FinnhubClient.Client>();
    }
}