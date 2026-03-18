namespace CascadingConfiguration;

internal sealed class ChildConfigurationCombiner : ICombiner<ChildConfiguration>
{
    public ChildConfiguration Combine(ChildConfiguration baseObject, ChildConfiguration other)
    {
        var name = string.IsNullOrEmpty(other.Name) ? baseObject.Name : other.Name;
        var enabled = other.Enabled ?? baseObject.Enabled;
        var disabled = other.Disabled ?? baseObject.Disabled;
        
        return new ChildConfiguration(name, enabled, disabled);
    }
}
