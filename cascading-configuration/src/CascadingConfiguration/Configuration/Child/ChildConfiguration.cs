using CascadingConfiguration.Configuration.Combination;

namespace CascadingConfiguration.Configuration.Child;

public sealed class ChildConfiguration(string name, bool? enabled = null, bool? disabled = null)
{
    public string Name { get; } = name;

    public bool? Enabled { get; } = enabled;

    public bool? Disabled { get; } = disabled;

    public ChildConfiguration Combine(ChildConfiguration? other, ICombiner<ChildConfiguration>? combiner = null)
    {
        if (other is null)
        {
            return this;
        }

        combiner ??= new ChildConfigurationCombiner();
        
        return combiner.Combine(this, other);
    }
    
    public ChildConfiguration Difference(ChildConfiguration other)
    {
        return this;
    }
}