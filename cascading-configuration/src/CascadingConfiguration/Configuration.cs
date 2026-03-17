namespace CascadingConfiguration;

public sealed class Configuration(string name, ChildConfiguration? child, string[]? strings) {

    public string Name { get; } = name;
    
    public ChildConfiguration? Child { get; } = child;

    public string[] Strings { get; } = strings ?? [];

    public Configuration Combine(Configuration other) {
        return this;
    }

    public Configuration Difference(Configuration other) {
        return this;
    }
}

