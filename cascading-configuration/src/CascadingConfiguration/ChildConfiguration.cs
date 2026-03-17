namespace CascadingConfiguration;

public sealed class ChildConfiguration(bool? enabled, bool? disabled) {
    public bool? Enabled { get; } = enabled;

    public bool? Disabled { get; } = disabled;

    public ChildConfiguration Combine(ChildConfiguration other)
    {
        return this;
    }
    
    public ChildConfiguration Difference(ChildConfiguration other)
    {
        return this;
    }
}
