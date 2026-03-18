namespace CascadingConfiguration;

public sealed class Configuration(string name, ChildConfiguration[]? childs = null) {

    public string Name { get; } = name;

    public ChildConfiguration[] Childs { get; } = childs ?? [];

    public Configuration Combine(Configuration? other)
    {
        if (other is null)
        {
            return this;
        }
        
        var name = string.IsNullOrEmpty(other.Name) ? Name : other.Name;

        var baseChildsLookup = Childs.ToDictionary(k => k.Name, v => v);

        foreach (var otherChild in other.Childs)
        {
            if (baseChildsLookup.TryGetValue(otherChild.Name, out var baseChild))
            {
                baseChildsLookup[otherChild.Name] = baseChild.Combine(otherChild);
            }
            else
            {
                baseChildsLookup[otherChild.Name] = otherChild;
            }
        }
        
        return new Configuration(name, baseChildsLookup.Values.ToArray());
    }

    public Configuration Difference(Configuration other) {
        return this;
    }
}
