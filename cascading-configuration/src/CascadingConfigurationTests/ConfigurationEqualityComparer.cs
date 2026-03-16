using CascadingConfiguration;

namespace CascadingConfigurationTests;

internal sealed class ConfigurationEqualityComparer : IEqualityComparer<Configuration>
{
    public static ConfigurationEqualityComparer Instance = new ();
    
    public bool Equals(Configuration? left, Configuration? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return
            left.Name == right.Name
            && ChildConfigurationEqualityComparer.Instance.Equals(left.Child, right.Child)
            && left.Strings.All(s => right.Strings.Contains(s));
    }

    public int GetHashCode(Configuration configuration)
    {
        var childHashCode = configuration.Child is null ? 0 : ChildConfigurationEqualityComparer.Instance.GetHashCode(configuration.Child);
        var stringsHashCode = configuration.Strings.Aggregate(0, HashCode.Combine);
        
        return HashCode.Combine(configuration.Name.GetHashCode(), 
            childHashCode,
            stringsHashCode);
    }
}
