namespace CascadingConfiguration.IO.Models;

internal sealed class ConfigurationData
{
    public string? Name { get; init; }

    public ChildConfigurationData[]? Childs { get; init; }
}