namespace CascadingConfiguration;

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

    public Configuration Difference(Configuration other) {
        return this;
    }
}
