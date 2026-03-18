namespace CascadingConfiguration;

public sealed class ChildConfiguration(bool? enabled, bool? disabled) {
    public bool? Enabled { get; } = enabled;

    public bool? Disabled { get; } = disabled;

    public ChildConfiguration Combine(ChildConfiguration? other)
    {
        if (other is null)
        {
            return this;
        }

        var enabled = other.Enabled ?? Enabled;
        var disabled = other.Disabled ?? Disabled;
        
        return new ChildConfiguration(enabled, disabled);
    }
    
    public ChildConfiguration Difference(ChildConfiguration other)
    {
        return this;
    }
}
