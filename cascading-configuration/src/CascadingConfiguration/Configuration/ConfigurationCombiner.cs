using CascadingConfiguration.Configuration.Combination;

namespace CascadingConfiguration.Configuration;

internal sealed class ConfigurationCombiner : ICombiner<Configuration>
{
    public Configuration Combine(Configuration baseConfiguration, Configuration otherConfiguration)
    {
        var name = string.IsNullOrEmpty(otherConfiguration.Name) ? baseConfiguration.Name : otherConfiguration.Name;

        var baseChildsLookup = baseConfiguration.Childs.ToDictionary(k => k.Name, v => v);

        foreach (var otherChild in otherConfiguration.Childs)
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
}