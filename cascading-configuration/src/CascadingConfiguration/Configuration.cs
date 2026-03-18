namespace CascadingConfiguration;

public sealed class Configuration(string name, ChildConfiguration? child, string[]? strings) {

    public string Name { get; } = name;
    
    public ChildConfiguration? Child { get; } = child;

    public string[] Strings { get; } = strings ?? [];

    public Configuration Combine(Configuration? other)
    {
        if (other is null)
        {
            return this;
        }
        
        var name = string.IsNullOrEmpty(other.Name) ? Name : other.Name;
        var child = other.Child is null
            ? Child
            : other.Child.Combine(Child);
        
        return new Configuration(name, child, Strings);
    }

    public Configuration Difference(Configuration other) {
        return this;
    }
}

