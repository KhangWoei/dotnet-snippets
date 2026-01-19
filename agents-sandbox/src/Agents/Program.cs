using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AgentSandbox.Agents;

internal static class Program 
{
    public static int Main(string[] args) {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.Configure<AgentOptions>(
            builder.Configuration.GetSection(nameof(AgentOptions)));

        using var host = builder.Build();
        
        var agentOptions = host.Services.GetRequiredService<IOptions<AgentOptions>>().Value;

        Console.WriteLine(agentOptions.Claude);
        return 0;
    }
}
