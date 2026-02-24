using System.CommandLine;

namespace AgentSandbox.Agents.Claude.Models;

internal sealed class ModelsCommand : Command
{
    public ModelsCommand() : base("models", "Get claude model information")
    {
    }
}
