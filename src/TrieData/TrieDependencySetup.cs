using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace TrieData;

public static class TrieDataDependencySetup
{
    public static void UserTrieData(this IServiceCollection services, NpgsqlConnectionStringBuilder connectionStringBuilder)
    {
        services.AddNpgsqlDataSource(connectionStringBuilder.ToString());
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(TrieDataDependencySetup).Assembly));
    }
}