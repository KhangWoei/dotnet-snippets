namespace CascadingConfiguration;

public sealed class ChildConfiguration(bool? enabled, bool? disabled) {
    public bool? Enabled { get; } = enabled;

    public bool? Disabled { get; } = disabled;
}
