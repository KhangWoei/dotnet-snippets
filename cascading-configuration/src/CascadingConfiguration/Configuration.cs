namespace CascadingConfiguration;

public sealed class Configuration(string name) {

    public string Name { get; } = name;

    public Configuration Combine(Configuration? other)
    {
        if (other is null)
        {
            return this;
        }
        
        var name = string.IsNullOrEmpty(other.Name) ? Name : other.Name;
        
        return new Configuration(name);
    }

    public Configuration Difference(Configuration other) {
        return this;
    }
}
