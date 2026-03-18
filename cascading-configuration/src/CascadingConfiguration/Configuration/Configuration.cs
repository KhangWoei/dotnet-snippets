using CascadingConfiguration.Configuration.Child;
using CascadingConfiguration.Configuration.Combination;
using CascadingConfiguration.Configuration.Diffing;

namespace CascadingConfiguration.Configuration;

public sealed class Configuration(string name, ChildConfiguration[]? childs = null) {

    public string Name { get; } = name;

    public ChildConfiguration[] Childs { get; } = childs ?? [];

    public Configuration Combine(Configuration? other, ICombiner<Configuration>? combiner = null)
    {
        if (other is null)
        {
            return this;
        }

        combiner ??= new ConfigurationCombiner();
        return combiner.Combine(this, other);
    }

    public ConfigurationDifference Difference(Configuration other, IDiffer<Configuration, ConfigurationDifference>? differ = null)
    {
        differ ??= new ConfigurationDiffer();

        return differ.Difference(this, other);
    }
}
