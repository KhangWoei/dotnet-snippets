using System.CommandLine;
using Microsoft.Extensions.Options;

namespace AgentSandbox.Agents.Claude;

public sealed class ClaudeCommand : Command
{
    private readonly ClaudeOptions _options;
    
    public ClaudeCommand(IOptions<ClaudeOptions> options) : base("claude", "Interact with Claude AI")
    {
        _options = options.Value;
    }
}
