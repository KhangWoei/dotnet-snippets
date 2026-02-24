using AgentSandbox.Agents.Claude;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AgentSandbox.Agents;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.Configure<ClaudeOptions>(builder.Configuration.GetSection(nameof(ClaudeOptions)));
        builder.Services.AddTransient<ClaudeCommand>();
        builder.Services.AddTransient<AgentsCommand>();

        var application = builder.Build();
        using var scope = application.Services.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<AgentsCommand>();
        var result = command.Parse(args);
        return await result.InvokeAsync();
    }
}
