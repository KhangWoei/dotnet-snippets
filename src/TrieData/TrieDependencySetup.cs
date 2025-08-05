using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TrieData.Nodes;
using TrieData.Tries;

namespace TrieData;

public static class TrieDataDependencySetup
{
    public static void UserTrieData(this IServiceCollection services, NpgsqlConnectionStringBuilder connectionStringBuilder)
    {
        services.AddNpgsqlDataSource(connectionStringBuilder.ToString());

        services.AddScoped<ITriesReadRepository, TriesReadRepository>();
        services.AddScoped<ITriesWriteRepository, TriesWriteRepository>();

        services.AddScoped<INodesReadRepository, NodesReadRepository>();
        services.AddScoped<INodesWriteRepository, NodesWriteRepository>();
        
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(TrieDataDependencySetup).Assembly));
    }
}