using CascadingConfiguration.Configurations.Child;
using CascadingConfiguration.Configurations.Diffing;

namespace CascadingConfiguration.Configurations;

internal sealed class ConfigurationDiffer : IDiffer<Configurations.Configuration, ConfigurationDifference>
{
    public ConfigurationDifference Difference(Configurations.Configuration diffBase, Configurations.Configuration other)
    {
        var nameChange = FieldChangeCalculator<string>.Calculate(nameof(Configurations.Configuration.Name), diffBase.Name, other.Name);

        var additions = new List<ChildConfiguration>();
        var deletions = new List<ChildConfiguration>();
        var updates = new List<ChildConfigurationDifference>();

        var baseChildLookup = diffBase.Childs.ToDictionary(k => k.Name, v => v);
        var otherChildLookup = other.Childs.ToDictionary(k => k.Name, v => v);
        var children = baseChildLookup.Keys.Union(otherChildLookup.Keys);
        
        foreach (var child in children)
        {
            var baseChild = baseChildLookup.GetValueOrDefault(child);
            var otherChild = otherChildLookup.GetValueOrDefault(child);

            if (baseChild is not null && otherChild is not null)
            {
                var update = baseChild.Difference(otherChild);
                updates.Add(update);
            } else if (baseChild is null && otherChild is not null)
            {
                additions.Add(otherChild);
            }
            else if (baseChild is not null && otherChild is null)
            {
                deletions.Add(baseChild);
            }
        }

        var childChanges = new ChildConfigurationCollectionDifference(additions, deletions, updates);
        
        return new ConfigurationDifference(nameChange, childChanges);
    }
}