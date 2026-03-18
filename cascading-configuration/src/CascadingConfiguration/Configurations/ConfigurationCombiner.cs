using CascadingConfiguration.Configurations.Combination;

namespace CascadingConfiguration.Configurations;

internal sealed class ConfigurationCombiner : ICombiner<Configurations.Configuration>
{
    public Configurations.Configuration Combine(Configurations.Configuration baseConfiguration, Configurations.Configuration otherConfiguration)
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
        
        return new Configurations.Configuration(name, baseChildsLookup.Values.ToArray());
    }
}