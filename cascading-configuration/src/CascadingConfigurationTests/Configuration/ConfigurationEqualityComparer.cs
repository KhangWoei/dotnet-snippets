using CascadingConfigurationTests.Configuration.Child;

namespace CascadingConfigurationTests.Configuration;

internal sealed class ConfigurationEqualityComparer : IEqualityComparer<CascadingConfiguration.Configurations.Configuration>
{
    public static ConfigurationEqualityComparer Instance = new ();
    
    public bool Equals(CascadingConfiguration.Configurations.Configuration? left, CascadingConfiguration.Configurations.Configuration? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Name == right.Name
               && left.Childs.SequenceEqual(right.Childs, ChildConfigurationEqualityComparer.Instance);
    }

    public int GetHashCode(CascadingConfiguration.Configurations.Configuration configuration)
    {
        var childHashCode = configuration.Childs.Aggregate(0, (acc, current) =>  HashCode.Combine(acc, ChildConfigurationEqualityComparer.Instance.GetHashCode(current)));
        
        return HashCode.Combine(configuration.Name.GetHashCode(), childHashCode);
    }
}
