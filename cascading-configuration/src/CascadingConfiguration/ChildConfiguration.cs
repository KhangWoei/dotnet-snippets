namespace CascadingConfiguration;

public sealed class ChildConfiguration(string name, bool? enabled = null, bool? disabled = null)
{
    public string Name { get; } = name;

    public bool? Enabled { get; } = enabled;

    public bool? Disabled { get; } = disabled;

    public ChildConfiguration Combine(ChildConfiguration? other)
    {
        if (other is null)
        {
            return this;
        }
        
        var name = string.IsNullOrEmpty(other.Name) ? Name : other.Name;
        var enabled = other.Enabled ?? Enabled;
        var disabled = other.Disabled ?? Disabled;
        
        return new ChildConfiguration(name, enabled, disabled);
    }
    
    public ChildConfiguration Difference(ChildConfiguration other)
    {
        return this;
    }
}
