using System.CommandLine;
using AgentSandbox.Agents.Claude;

namespace AgentSandbox.Agents;

public class AgentsCommand : RootCommand
{
    public AgentsCommand(ClaudeCommand claudeCommand) : base("Command line tool to mess around with agents in dotnet")
    {
        Add(claudeCommand);
    }
}
